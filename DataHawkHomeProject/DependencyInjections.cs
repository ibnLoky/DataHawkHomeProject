using DataHakHomeProject.Service.AmazonService;
using DataHawkHomeProject.BusinessService.AmazonBusinessService;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;

namespace DataHawkHomeProject
{
    public static class DependencyInjections
    {
        public static void AddDependecyInjections(IServiceCollection services)
        {
            services.AddSingleton<IAmazonBusinessService, AmazonBusinessService>();
            services.AddHttpClient<IAmazonService, AmazonService>(client => client.BaseAddress = new System.Uri("http://www.amazon.com/product-reviews/"))
                .ConfigurePrimaryHttpMessageHandler(config => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                });
        }
    }
}
