using Authentication.WEB.Models;
using Authentication.WEB.Services;
using InsuredTraveling.Models;
using System.Web.Http;
using System.Web.Services.Description;

namespace Authentication.WEB.Controllers
{
    [RoutePrefix("api/Premium")]
    public class PremiumController : ApiController
    {
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
        [Route("Calculate")]
        public IHttpActionResult Code(Policy policy)
        {
          
            if(!policy.isMobile && policy.IsSamePolicyHolderInsured)
            {
                policy.PolicyHolderName = policy.Name;
                policy.PolicyHolderLastName = policy.LastName;
                policy.PolicyHolderSSN = policy.SSN;
                policy.PolicyHolderEmail = policy.Email;
                policy.PolicyHolderAddress = policy.Address;
                policy.PolicyHolderBirthDate = policy.BirthDate;
                policy.PolicyHolderCity = policy.City;
                policy.PolicyHolderPostalCode = policy.PostalCode;
                policy.PolicyHolderPhoneNumber = policy.PhoneNumber;

              
            }
            if(!policy.isMobile)
            {
                ModelState.Remove("PolicyHolderName");
                ModelState.Remove("PolicyHolderLastName");
                ModelState.Remove("PolicyHolderEmail");
                ModelState.Remove("PolicyHolderAddress");
                ModelState.Remove("PolicyHolderBirthDate");
                ModelState.Remove("PolicyHolderCity");
                ModelState.Remove("PolicyHolderPostalCode");
                ModelState.Remove("PolicyHolderPhoneNumber");
                ModelState.Remove("PolicyHolderSSN");
              //  string[] s = policy.test.ToString().Split('/');

              //  policy.Start_Date = new System.DateTime();
            }
           
            
            if (ModelState.IsValid && policy != null)
            {
                RatingEngineService ratingEngineService = new RatingEngineService();

                Premium Premium = new Premium();
                Premium.PremiumAmount = (int)ratingEngineService.totalPremium(policy);
                return Ok(new { PremiumAmount = Premium.PremiumAmount });
            }else
            {
                return BadRequest("Внесете ги сите полиња!");
            }
        }
    }
}
