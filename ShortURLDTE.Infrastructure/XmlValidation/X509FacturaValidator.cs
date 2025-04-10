using ShortURLDTE.Domain.Services;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ShortURLDTE.Infrastructure.XmlValidation
{
    public class X509FacturaValidator : IFacturaValidator
    {
        public Task<bool> ValidarAsync(string xmlContent)
        {
            try
            {
                var xmlDoc = new XmlDocument { PreserveWhitespace = true };
                xmlDoc.LoadXml(xmlContent);

                var signedXml = new SignedXml(xmlDoc);
                var node = xmlDoc.GetElementsByTagName("Signature")[0] as XmlElement;
                if (node == null) return Task.FromResult(false);

                signedXml.LoadXml(node);

                var keyInfoData = signedXml.KeyInfo.OfType<KeyInfoX509Data>().FirstOrDefault();

                if (keyInfoData == null || keyInfoData.Certificates.Count == 0)
                    throw new Exception("No se encontró un certificado en la firma.");

                var cert = keyInfoData.Certificates[0] as X509Certificate2;

                if (cert == null)
                    cert = new X509Certificate2((keyInfoData.Certificates[0] as X509Certificate).Export(X509ContentType.Cert));


                return Task.FromResult(signedXml.CheckSignature(cert, true));
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }

}
