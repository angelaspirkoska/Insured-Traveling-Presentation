using Authentication.WEB.Models;
using Authentication.WEB.Services;
using AutoMapper;
using InsuredTraveling;
using InsuredTraveling.App_Start;
using InsuredTraveling.DI;
using InsuredTraveling.Models;
using InsuredTraveling.ViewModels;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Services.Description;

namespace Authentication.WEB.Controllers
{
    [RoutePrefix("api/Premium")]
    public class PremiumController : ApiController
    {

        private IOkSetupService _os;


        public PremiumController(IOkSetupService os)
        {
            _os = os;
        }
        [HttpGet]
        public IHttpActionResult Excel(int value1, int value2)
        {
            var ratingEnginePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/RatingEngineBeta.xlsx");

            RatingService ratingService = new RatingService();
            Premium Premium = new Premium();
            //Calculates premium
            //  Premium.PremiumAmount = ratingService.calculatePremium(ratingEnginePath, value1, value2);

            var quotePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/QuoteDocument.docx");
            var quotePathCopy = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/QuoteDocumentCopy.docx");
            var quotePathPdf = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/QuoteDocumentBeta.pdf");

            //DocumentService documentService = new DocumentService();
            //documentService.generateQuote(quotePath, quotePathCopy, quotePathPdf, Premium.PremiumAmount);

            return Ok(new { PremiumAmount = Premium.PremiumAmount });
        }

        [HttpPost]
        [Route("CalculatePremium")]
        public IHttpActionResult CalculatePremium(CalculatePremiumViewModel data)
        {
            RatingEngineService ratingEngineService = new RatingEngineService();
            Premium Premium = new Premium();

            var policy = new Policy();
            policy.additional_charges = new List<additional_charge>();
            policy = Mapper.Map<CalculatePremiumViewModel, Policy>(data);

            Premium.PremiumAmount = (int)ratingEngineService.totalPremium(policy);
            return Ok(new { PremiumAmount = Premium.PremiumAmount });
        }
    }
}
