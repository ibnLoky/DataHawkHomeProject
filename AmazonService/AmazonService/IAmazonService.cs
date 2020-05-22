using System.Threading.Tasks;

namespace DataHakHomeProject.Service.AmazonService
{
    public interface IAmazonService
    {
        Task<string> GetPageAsync(string asin, int page = 1);
    }
}
