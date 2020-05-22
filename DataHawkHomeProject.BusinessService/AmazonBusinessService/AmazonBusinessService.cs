using DataHakHomeProject.Service.AmazonService;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DataHawkHomeProject.BusinessService.AmazonBusinessService
{
    public class AmazonBusinessService : IAmazonBusinessService
    {
        private readonly IAmazonService _amazonService;
        private readonly ILogger<AmazonBusinessService> _logger;
        private readonly CultureInfo _usCultureInfo;

        public List<LoadedReview> _loadedReviews;

        public IEnumerable<LoadedReview> GetLoadedReviews()
        {
            return _loadedReviews;
        }

        public IEnumerable<LoadedReview> GetLoadedReviews(string asin)
        {
            return _loadedReviews.Where(a => a.Asin == asin);
        }


        public AmazonBusinessService(IAmazonService service, ILogger<AmazonBusinessService> logger)
        {
            _amazonService = service;
            _logger = logger;
            _loadedReviews  = new List<LoadedReview>();
            _usCultureInfo = new CultureInfo("de-DE");
        }

        public async Task<IEnumerable<bool>> GetReviewsAsync(string asins)
        {
            var result = new Task<bool>[asins.Split(',').Count() + 1];
            var i = 0;
            foreach (var asin in asins.Split(',').Select(x => x.Trim()))
            {
                result[i++] = GetReviewAsync(asin);
            }

            Task.WaitAll(result);
            return result.Select(x => x.Result);
        }

        public async Task<bool> GetReviewAsync(string asin)
        {
            asin = asin.Trim();
            try
            {
                var reviews = new List<LoadedReview>();
                var page = new Task<string>[5];
                for (var i = 1; i <= 5; ++i)
                    page[i - 1] = _amazonService.GetPageAsync(asin, i);
                Task.WaitAll(page);
                for (var i = 0; i < 5; ++i)
                {
                    var pageDocument = new HtmlDocument();
                    pageDocument.LoadHtml(page[i].Result);
                    var htmlNodes = pageDocument.DocumentNode.SelectNodes("(//div[contains(@class,'a-section celwidget')])").Skip(1);
                    foreach (var htmlNode in htmlNodes)
                    {
                        reviews.Add(
                            new LoadedReview
                            {
                                Asin = asin,
                                Date = DateTime.Parse(string.Join(' ', htmlNode.SelectNodes("(.//span[contains(@class,'review-date')])").FirstOrDefault().InnerText.Split(' ').Skip(6)), _usCultureInfo).ToString("dd/MM/yyyy"),
                                Rating = htmlNode.SelectNodes("(.//i[contains(@class,'review-rating')])")[0].InnerText.Split(' ').FirstOrDefault(),
                                ReviewText = htmlNode.SelectNodes("(.//span[contains(@class,'a-size-base review-text review-text-content')])").FirstOrDefault().InnerText.Trim(),
                                Title = htmlNode.SelectNodes("(.//a[contains(@class,'review-title')])").FirstOrDefault()?.InnerText.Trim()
                            });
                    }
                }
                if (!reviews.Any())
                    return false;
                foreach (var review in reviews)
                {
                    if (!_loadedReviews.Any(a => a.ReviewText == review.ReviewText && asin == a.Asin))
                        _loadedReviews.Add(review);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed getting review for asyn {asin} : {e.Message}");
                return false;
            }

            return true;
        }

    }
}
