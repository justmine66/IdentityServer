using Identity.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging
{
    internal static class LoggingExtensions
    {
        private static Action<ILogger, string, Exception> _tokenSignatureCertificateMissing;

        static LoggingExtensions()
        {
            _tokenSignatureCertificateMissing = LoggerMessage.Define<string>(
                eventId: 1,
                logLevel: LogLevel.Debug,
                formatString: Resources.TokenSignatureCertificateMissing);
        }

        public static void TokenSignatureCertificateMissing(this ILogger logger, string resourceName)
        {
            _tokenSignatureCertificateMissing(logger, resourceName, null);
        }
    }
}
