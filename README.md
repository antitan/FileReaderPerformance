using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
 
namespace Inventory.Common.Infrastructure.Http
{ 
    public class IgnoreSSLValidateDelegatingHandler : DelegatingHandler
    { 
        private readonly ILogger<IgnoreSSLValidateDelegatingHandler> _logger;
 
        public IgnoreSSLValidateDelegatingHandler(ILogger<IgnoreSSLValidateDelegatingHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            System.Threading.CancellationToken cancellationToken)
        {
            HttpMessageHandler handler = this.InnerHandler;
 
            while (handler is DelegatingHandler)
            {
                handler = ((DelegatingHandler)handler).InnerHandler;
            }
 
            if (handler is HttpClientHandler httpClientHandler
                && httpClientHandler.ServerCertificateCustomValidationCallback == null)
            {
                httpClientHandler
                    .ServerCertificateCustomValidationCallback =
                        (message, cert, chain, errors) =>
                        {
                            // It is possible inpect the certificate provided by server
                            _logger.LogDebug($"Requested URI: {message.RequestUri}");
                            _logger.LogDebug($"Effective date: {cert.GetEffectiveDateString()}");
                            _logger.LogDebug($"Exp date: {cert.GetExpirationDateString()}");
                            _logger.LogDebug($"Issuer: {cert.Issuer}");
                            _logger.LogDebug($"Subject: {cert.Subject}");
 
                            // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
                            _logger.LogDebug($"Errors: {errors}");
                            return true;
 
                        };
            }
 
            return base.SendAsync(request, cancellationToken);
        }
    }
}
