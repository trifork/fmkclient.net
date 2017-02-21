using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fmkclient.net.fmk20150601;
using System.ServiceModel;
using dk.nsi.seal;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace fmkclient.net
{
    public class FmkClient
    {
        private readonly MedicineCardPortTypeClient _fmkClient;
        private readonly SosiUtil _sosiUtil;

        public FmkClient()
        {
            _sosiUtil = new SosiUtil();
            _fmkClient = new MedicineCardPortTypeClient("FmkTest1");
            _fmkClient.Endpoint.Behaviors.Add(new FmkMonitoringEndpointBehaviour());
        }

        private OrgUsingID MakeOrgUsingID()
        {
            return new OrgUsingID
            {
                NameFormat = NameFormat.medcomynumber,
                Value = Properties.Settings.Default.WhitelistYdernummer
            };
        }

        private WhitelistingHeader MakeWhitelistingHeader()
        {
            return new WhitelistingHeader
            {
                Items = new object[] { 
                    Properties.Settings.Default.WhitelistOrgResponsibleName,    
                    Properties.Settings.Default.WhitelistOrgUsingName,
                    MakeOrgUsingID(),
                },
                ItemsElementName = new[] { ItemsChoiceType9.OrgResponsibleName, ItemsChoiceType9.OrgUsingName, ItemsChoiceType9.OrgUsingID, },
                RequestedRole = Properties.Settings.Default.WhitelistRequestedRole,
                SystemName = Properties.Settings.Default.WhitelistSystemName,
                SystemOwnerName = Properties.Settings.Default.WhitelistSystemOwner,
                SystemVersion = Properties.Settings.Default.WhitelistSystemVersion
            };
        }

        public String GetMedicineCard(String cpr)
        {
            SecurityHeaderType sec = _sosiUtil.MakeSecurity();
            Header header = _sosiUtil.MakeHeader();

            GetMedicineCardRequest_2015_06_01 request = new GetMedicineCardRequest_2015_06_01
            {
                GetMedicineCardRequest = new GetMedicineCardRequestType 
                {
                    PersonIdentifier = new PersonIdentifierType() { Value = cpr, source="CPR" },
                    IncludeEffectuations = true, 
                    IncludeNonRelevantPrescriptions = true,
                    IncludePrescriptions = true
                },
                Header = header,
                Security = sec,
                WhitelistingHeader = MakeWhitelistingHeader()
            };

            var responses = new MedicineCardType[1];
            PrescriptionReplicationStatusType prescriptionReplicationStatus;

            var timingList =
                _fmkClient.GetMedicineCard_2015_06_01(sec, header, null, request.WhitelistingHeader, null, request.GetMedicineCardRequest, out prescriptionReplicationStatus, out responses);


            fmkclient.net.fmk20150601.GetMedicineCardResponse_2015_06_01 response = new GetMedicineCardResponse_2015_06_01(timingList, prescriptionReplicationStatus, responses);


            var sww = new StringWriter();
            var writer = XmlWriter.Create(sww);

            var x = new XmlSerializer(response.GetType());
            x.Serialize(writer, response);
            return sww.ToString();
        }
        
        public String GetPrescriptions(String cpr)
        {
            SecurityHeaderType sec = _sosiUtil.MakeSecurity();
            Header header = _sosiUtil.MakeHeader();

            GetPrescriptionRequest_2015_06_01 request = new GetPrescriptionRequest_2015_06_01
            {
                GetPrescriptionRequest = new GetPrescriptionRequestType
                {
                    Item = new PersonIdentifierType() { Value = cpr, source="CPR" },
                    Items = new object[] {
                        new IncludeOpenPrescriptionsType() 
                    },
                    IncludeEffectuations = true
                },
                Header = header,
                Security = sec,
                WhitelistingHeader = MakeWhitelistingHeader()
            };

            PrescriptionReplicationStatusType prescriptionReplicationStatus;
            GetPrescriptionResponseType responseType;

            var timingList =
                _fmkClient.GetPrescription_2015_06_01(sec, header, null, request.WhitelistingHeader, null, request.GetPrescriptionRequest, out prescriptionReplicationStatus, out responseType);


            fmkclient.net.fmk20150601.GetPrescriptionResponse_2015_06_01 response = new GetPrescriptionResponse_2015_06_01(timingList, prescriptionReplicationStatus, responseType);


            var sww = new StringWriter();
            var writer = XmlWriter.Create(sww);

            var x = new XmlSerializer(response.GetType());
            x.Serialize(writer, response);
            return sww.ToString();
        } 
    }
}
