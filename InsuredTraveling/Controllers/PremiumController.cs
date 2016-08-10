﻿using Authentication.WEB.Models;
using Authentication.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authentication.WEB.Controllers
{
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
        public IHttpActionResult Code(Policy policy)
        {
            RatingEngineService ratingEngineService = new RatingEngineService();

            Premium Premium = new Premium();
            Premium.PremiumAmount = (int)ratingEngineService.vkupnaPremija(policy);
            return Ok(new { PremiumAmount = Premium.PremiumAmount });
        }
    }
}
