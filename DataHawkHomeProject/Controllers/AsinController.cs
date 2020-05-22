using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataHawkHomeProject.BusinessService.AmazonBusinessService;
using DataHawkHomeProject.Controllers.WebDataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataHawkHomeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsinController : ControllerBase
    {
        private readonly IAmazonBusinessService _amazonBusinessService;
        public AsinController(IAmazonBusinessService amazonBusinessService)
        {
            _amazonBusinessService = amazonBusinessService;
        }


        [HttpPost]
        public async Task<IActionResult> PostAsins(AsinDto asins)
        {
            var result = await _amazonBusinessService.GetReviewAsync(asins.Asins);
            if (result)
                return new OkResult();
            else
                return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{asin}")]
        public async Task<IEnumerable<LoadedReview>> GetAsin(string? asin)
        {
            return _amazonBusinessService.GetLoadedReviews(asin);
        }

        [HttpGet]
        public async Task<IEnumerable<LoadedReview>> GetAsins()
        {
            return _amazonBusinessService.GetLoadedReviews();
        }

        [HttpGet("loadedAsinsList")]
        public async Task<IEnumerable<object>> GetListOfLoadedAsins()
        {
            return _amazonBusinessService.GetLoadedReviews().Select(x => new { asinNumber = x.Asin, status = 2, statusDisplay = "Done" }).Distinct();
        }

    }
}