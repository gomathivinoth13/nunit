﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SEGLoyaltyServiceWeb
{
    public class PassKit
    {
        static readonly Assembly assembly = typeof(PassKit).Assembly;

        public static byte[] GeneratePass(string barcode, string webRootPath, bool isDevEnvironment, string AppleWWDCTHumbPrint, string SEGCertThumbprint)
        {

            MemoryStream icon3x = new MemoryStream();
            MemoryStream logo3x = new MemoryStream();


            assembly.GetManifestResourceStream("SEGLoyaltyServiceWeb.Resources.icons.apple-touch-icon-iphone-60x60.png").CopyTo(icon3x);
            assembly.GetManifestResourceStream("SEGLoyaltyServiceWeb.Resources.SEGRewardsLogo@2x.png").CopyTo(logo3x);

            icon3x.Position = 0;
            logo3x.Position = 0;

            PassSharp.Pass pass = new PassSharp.Pass
            {
                type = PassSharp.PassType.storeCard,
                passTypeIdentifier = "pass.biloholdings.SEGRewards.prod",
                description = "SEGRewards Card",
                organizationName = "SEGrocers",
                serialNumber = barcode,
                teamIdentifier = "2QD87ZRG6U",
                icon3x = new PassSharp.Asset(icon3x.ToArray()),
                logo3x = new PassSharp.Asset(logo3x.ToArray()),
                strip3x = new PassSharp.Asset(logo3x.ToArray())
            };

            pass.AddBarcode(new PassSharp.Fields.Barcode() { altText = barcode, format = PassSharp.Fields.BarcodeFormat.PKBarcodeFormatCode128 });

            return GeneratePass(pass, webRootPath, isDevEnvironment, AppleWWDCTHumbPrint, SEGCertThumbprint);
        }

        public static byte[] GeneratePass(PassSharp.Pass pass, string webRootPath, bool isDevEnvironment, string AppleWWDCTHumbPrint, string SEGCertThumbprint)
        {
            MemoryStream appleMS = new MemoryStream();
            MemoryStream SEGAppsCertificateMS = new MemoryStream();
            X509Certificate2 appleCert = null;
            X509Certificate2 segAppsCert = null;

            //if (isDevEnvironment)
            //{
            assembly.GetManifestResourceStream("SEGLoyaltyServiceWeb.Resources.AppleWWDRCA.cer").CopyTo(appleMS);
            assembly.GetManifestResourceStream("SEGLoyaltyServiceWeb.Resources.SEGAppsCertificate.p12").CopyTo(SEGAppsCertificateMS);
            
            //var appleCertPath = Path.Combine(webRootPath, "Resources", "AppleWWDRCA.cer");
            //File.OpenRead(appleCertPath).CopyTo(appleMS);
            //var segAppsCertPath = Path.Combine(webRootPath, "Resources", "SEGAppsCertificate.p12");
            //File.OpenRead(segAppsCertPath).CopyTo(SEGAppsCertificateMS);

            appleMS.Position = 0;
            SEGAppsCertificateMS.Position = 0;

            if (appleMS.Length < 1) throw new ArgumentException("Invalid Apple Cert");
            if (SEGAppsCertificateMS.Length < 1) throw new ArgumentException("Invalid SEG Cert");
            
            appleCert = new X509Certificate2(appleMS.ToArray(), password: (string)null, keyStorageFlags: X509KeyStorageFlags.Exportable);
            segAppsCert = new X509Certificate2(SEGAppsCertificateMS.ToArray(), "R0ck3tt33r!", X509KeyStorageFlags.Exportable);
            //}
            //else {
            //    X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            //    certStore.Open(OpenFlags.ReadOnly);

            //    //AppleWWDRCA Certificate
            //    X509Certificate2Collection certCollection = certStore
            //        .Certificates
            //        .Find(X509FindType.FindByThumbprint,
            //        AppleWWDCTHumbPrint, // Generated by Azure
            //        false);

            //    //SEG PassKit Certificate
            //    X509Certificate2Collection certCollection2 = certStore
            //        .Certificates
            //        .Find(X509FindType.FindByThumbprint,
            //        SEGCertThumbprint,false
            //        );

            //    if (certCollection.Count > 0)
            //    {
            //        appleCert = certCollection[0];
            //    }
            //    else throw new ArgumentException("Missing AppleWWRDC Cert");

            //    if (certCollection2.Count > 0)
            //    {
            //        segAppsCert = certCollection2[0];
            //    }
            //    else throw new ArithmeticException("Missing SEG Cert");

            //    certStore.Dispose();


            //}
            MemoryStream passStream = new MemoryStream();

            if (segAppsCert == null) throw new ArgumentException("Invalid Apps Cert");
            if (!segAppsCert.HasPrivateKey) throw new ArgumentException("No Private Key");
            PassSharp.PassWriter.WriteToStream(pass, passStream, appleCert, segAppsCert);
            return passStream.ToArray();
        }
    }
}
