using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using dk.nsi.seal;
using dk.nsi.seal.dgwstypes;
using Attribute = dk.nsi.seal.dgwstypes.Attribute;
using System.Xml;

namespace fmkclient.net
{
    public class SosiUtil
    {
        
        private SealCard _idCard;
        private readonly X509Certificate2 _userCertificate;

        private readonly String _issuer;
        private readonly String _sosiCareProviderName;
        private readonly String _sosiCareProviderCvr;
        private readonly String _itSystemName;

        private readonly String _userGivenName;
        private readonly String _userSurName;
        private readonly String _userRole;
        private readonly String _userCpr;
        private readonly String _userEmail;
        private readonly String _userAuthCode;

        private readonly String _stsUrl;

        public SosiUtil()
        {
            _issuer = Properties.Settings.Default.IDCardIssuer;
            _sosiCareProviderName = Properties.Settings.Default.SosiCareProviderName;
            _sosiCareProviderCvr = Properties.Settings.Default.SosiCareProviderCvr;
            _itSystemName = Properties.Settings.Default.ITSystemName;
            _stsUrl = Properties.Settings.Default.STSUrl;
            
            _userGivenName = Properties.Settings.Default.UserGivenName;
            _userSurName = Properties.Settings.Default.UserSurName;
            _userRole = Properties.Settings.Default.UserRole;
            _userCpr = Properties.Settings.Default.UserCpr;
            _userEmail = Properties.Settings.Default.UserEmail;
            _userAuthCode = Properties.Settings.Default.UserAuthCode;

            _userCertificate = new X509Certificate2(Properties.Settings.Default.CERTPath, Properties.Settings.Default.CERTPass);
        }

        public SealCard GetIdCard()
        {
            if (!IsIdCardValid(_idCard))
            {
                var rsc = SealCard.Create(MakeAssertionForSts(_userCertificate));
                _idCard = SealUtilities.SignIn(rsc, _issuer, _stsUrl);
            }
            return _idCard;
        }

        public fmkclient.net.fmk20150601.SecurityHeaderType MakeSecurity()
        {
            var assertionDoc = new XmlDocument();
            var assertionElement = assertionDoc.ReadNode(GetIdCard().Xassertion.CreateReader()) as XmlElement;

            var timestampDoc = new XmlDocument();
            var timestampElement = timestampDoc.CreateElement("Timestamp", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            var createdElement = timestampDoc.CreateElement("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            createdElement.InnerText = FiveMinutesAgoUtc().ToLocalTime().ToString("yyyy-MM-ddThh:mm:sszzz");
            timestampElement.AppendChild(createdElement);

            var idAttribute = new XmlDocument().CreateAttribute("id");
            idAttribute.Value = Guid.NewGuid().ToString("D");

            return new fmkclient.net.fmk20150601.SecurityHeaderType
            {
                Any = new [] {
                    timestampElement,
                    assertionElement
                },
                AnyAttr = new [] {
                    idAttribute
                }
            };
        }

        public fmkclient.net.fmk20150601.Header MakeHeader()
        {
            return new fmkclient.net.fmk20150601.Header
            {
                SecurityLevel = 1,
                TimeOut = fmkclient.net.fmk20150601.TimeOut.Item1440,
                TimeOutSpecified = true,
                Linking = new fmkclient.net.fmk20150601.Linking
                {
                    FlowID = Guid.NewGuid().ToString("D"),
                    MessageID = Guid.NewGuid().ToString("D")
                },
                FlowStatus = fmkclient.net.fmk20150601.FlowStatus.flow_running,
                FlowStatusSpecified = true,
                Priority = fmkclient.net.fmk20150601.Priority.RUTINE,
                RequireNonRepudiationReceipt = fmkclient.net.fmk20150601.RequireNonRepudiationReceipt.yes
            };
        }


        private static bool IsIdCardValid(SealCard sc)
        {
            var fiveMinAgo = FiveMinutesAgoUtc();
            // Check if the card is created and valid for atleast five minutes.
            if (sc != null && (sc.ValidTo.CompareTo(fiveMinAgo) < 0))
                return true;
            return false;
        }

        private static DateTime FiveMinutesAgoUtc()
        {
            TimeSpan secsSpan = TimeSpan.FromSeconds(1);
            DateTime fiveMinAgo = DateTime.Now - TimeSpan.FromMinutes(5);
            long roundTics = fiveMinAgo.Ticks % secsSpan.Ticks;
            return new DateTime(fiveMinAgo.Ticks - roundTics).ToUniversalTime();
        }

        static public string EncodeTo64(X509Certificate2 certificate)
        {
            byte[] toEncodeAsBytes = certificate.GetCertHash();
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;

        }

        private Assertion MakeAssertionForSts(X509Certificate2 certificate)
        {
            var vnow = FiveMinutesAgoUtc();
            var ass = new Assertion
            {
                IssueInstant = FiveMinutesAgoUtc(),
                id = "IDCard",
                Version = 2.0m,
                Issuer = _issuer,
                Conditions = new Conditions
                {
                    NotBefore = vnow,
                    NotOnOrAfter = vnow + TimeSpan.FromHours(8)
                },
                Subject = new Subject
                {
                    NameID = new NameID
                    {
                        Format = SubjectIdentifierType.medcomcprnumber,
                        Value = _userCpr
                    },
                    SubjectConfirmation = new SubjectConfirmation
                    {
                        ConfirmationMethod = global::dk.nsi.seal.dgwstypes.ConfirmationMethod.urnoasisnamestcSAML20cmholderofkey,
                        SubjectConfirmationData = new SubjectConfirmationData
                        {
                            Item = new KeyInfo
                            {
                                Item = "OCESSignature"
                            }
                        }
                    }
                },
                AttributeStatement = new[]
                {
                    new AttributeStatement
                    {
                        id = AttributeStatementID.IDCardData,
                        Attribute = new[] 
                        {
                            new Attribute{ Name = AttributeName.sosiIDCardID, AttributeValue = Guid.NewGuid().ToString("D")},
                            new Attribute{ Name = AttributeName.sosiIDCardVersion, AttributeValue = "1.0.1"},
                            new Attribute{ Name = AttributeName.sosiIDCardType, AttributeValue = "user"},
                            new Attribute{ Name = AttributeName.sosiAuthenticationLevel, AttributeValue = "4"},
                            new Attribute{ Name = AttributeName.sosiOCESCertHash, AttributeValue = EncodeTo64(certificate)}
                        }
                    },
                    new AttributeStatement
                    {
                        id = AttributeStatementID.UserLog,
                        Attribute = new[] 
                        {
                            new Attribute{ Name = AttributeName.medcomUserCivilRegistrationNumber, AttributeValue = _userCpr },
                            new Attribute{ Name = AttributeName.medcomUserGivenName, AttributeValue = _userGivenName },
                            new Attribute{ Name = AttributeName.medcomUserSurName, AttributeValue = _userSurName },
                            new Attribute{ Name = AttributeName.medcomUserEmailAddress, AttributeValue = _userEmail },
                            new Attribute{ Name = AttributeName.medcomUserRole, AttributeValue = _userRole },
                            new Attribute{ Name = AttributeName.medcomUserAuthorizationCode, AttributeValue = _userAuthCode }
                        }
                    },
                    new AttributeStatement
                    {
                        id = AttributeStatementID.SystemLog,
                        Attribute = new[] 
                        {
                            new Attribute{ Name = AttributeName.medcomITSystemName, AttributeValue = _itSystemName},
                            new Attribute{ Name = AttributeName.medcomCareProviderID, 
                                AttributeValue = _sosiCareProviderCvr, NameFormatSpecified = true, 
                                NameFormat = SubjectIdentifierType.medcomcvrnumber},
                            new Attribute{ Name = AttributeName.medcomCareProviderName, AttributeValue = _sosiCareProviderName}
                        }
                    }
                }
            };
            return SealUtilities.SignAssertion(ass, certificate);
        }

    }
}
