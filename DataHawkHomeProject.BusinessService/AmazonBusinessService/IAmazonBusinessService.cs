using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataHawkHomeProject.BusinessService.AmazonBusinessService
{

    public interface IAmazonBusinessService
    {
        Task<bool> GetReviewAsync(string asin);
        Task<IEnumerable<bool>> GetReviewsAsync(string asins);
        IEnumerable<LoadedReview> GetLoadedReviews();
        IEnumerable<LoadedReview> GetLoadedReviews(string asin);
    }
}
