using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Identity.API.Certificate
{
    static class Certificates
    {
        public static X509Certificate2 GetX5092()
        {
            Assembly assembly = typeof(Certificates).GetTypeInfo().Assembly;
            string certificateName = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith("TokenSignature.pfx", StringComparison.InvariantCultureIgnoreCase));

            using (Stream stream = assembly.GetManifestResourceStream(certificateName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException(
                        string.Format(CultureInfo.InstalledUICulture,
                        Resources.TokenSignatureCertificateMissing,
                        certificateName));
                }

                return new X509Certificate2(ReadStream(stream), "idsrv3test");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            if (input == null)
            {
                return null;
            }

            var buffer = new byte[16 * 1024];
            using (var memory = new MemoryStream())
            {
                while ((input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memory.Write(buffer, 0, buffer.Length);
                }
                return memory.ToArray();
            }
        }
    }
}
