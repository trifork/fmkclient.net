//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

// Kaspar Bach Pedersen:
// Dette er ikke den originale fil fra MS da jeg har ændret i den

using System;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// A utility class which helps to retrieve an x509 certificate
/// </summary>
public class CertificateUtil
{
    /// <summary>
    ///  Gets an X.509 certificate given the name, store location and the subject distinguished name of the X.509 certificate.
    /// </summary>
    /// <param name="name">Specifies the name of the X.509 certificate to open.</param>
    /// <param name="location">Specifies the location of the X.509 certificate store.</param>
    /// <param name="subjectName">Subject distinguished name of the certificate to return.</param>
    /// <returns>The specific X.509 certificate.</returns>
    public static X509Certificate2 GetCertificate( StoreName name, StoreLocation location, string cvr, string userGivenName )
    {
        X509Store store = new X509Store( name, location );
        X509Certificate2Collection certificates = null;
        store.Open( OpenFlags.ReadOnly );

        try
        {
            X509Certificate2 result = null;

            // Every time we call store.Certificates property, a new collection will be returned.
            certificates = store.Certificates;
            for ( int i = 0; i < certificates.Count; i++ )
            {
                X509Certificate2 cert = certificates[i];
                if (cert.SubjectName.Name.ToLower().Contains(cvr) && cert.SubjectName.Name.ToLower().Contains(userGivenName.ToLower()))
                {
                    if (result != null)
                    {
                        throw new ApplicationException(string.Format("There is more than one certificate found for cvr {0} and givenName {1}", cvr, userGivenName));
                    }
                    result = new X509Certificate2( cert );
                }
            }

            if ( result == null )
            {
                throw new ApplicationException(string.Format("No certificate was found for cvr {0} and givenName {1}", cvr, userGivenName));
            }
            return result;
        }
        finally
        {
            if ( certificates != null )
            {
                for ( int i = 0; i < certificates.Count; i++ )
                {
                    X509Certificate2 cert = certificates[i];
                    cert.Reset();
                }
            }

            store.Close();
        }
    }
}

