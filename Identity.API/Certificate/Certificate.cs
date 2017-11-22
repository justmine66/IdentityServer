using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.IO;

namespace Identity.API.Certificate
{
    static class Certificate
    {
        public static X509Certificate2 Get()
        {
            Assembly assembly = typeof(Certificate).GetTypeInfo().Assembly;
            string certificateName = assembly.GetManifestResourceNames()
                .FirstOrDefault(r => r.Contains("idsrv3test.pfx"));
            using (Stream stream = assembly.GetManifestResourceStream(certificateName))
            {
                return new X509Certificate2(ReadStream(stream), "idsrv3test");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
        }
    }
}
