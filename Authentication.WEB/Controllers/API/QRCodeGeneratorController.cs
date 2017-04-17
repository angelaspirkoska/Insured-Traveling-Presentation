using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace InsuredTraveling.Controllers.API
{
    [System.Web.Http.RoutePrefix("api/QRCodeGenerator")]
    public class QrCodeGeneratorController : ApiController
    {

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("GetQRCodeImage")]
        public IHttpActionResult GetQRCodeImage(QRCodeSavaPolicy model)
        { 
            string barcodeText = model.PolicyNumber + " " + model.SellerNameLastName + " " + model.EmailSeller + " " + model.SSNInsured + " " +
                                 model.SSNHolder + " " + model.ExpireDate + " " + model.Premium;
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
            QrCode qrCode = new QrCode();
            qrEncoder.TryEncode(barcodeText, out qrCode);
            GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(4, QuietZoneModules.Four), Brushes.Black, Brushes.White);

            MemoryStream memoryStream = new MemoryStream();
            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, memoryStream);

            memoryStream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(memoryStream.ToArray());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            
            var response = ResponseMessage(result);
            return response;
            
        }
    }
}