using InsuredTraveling.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Helpers
{
    public static class SendSavaEmailHelper
    {
        public static void SendVaucerEmail(UsePointsModel model)
        {
            //string fullPath = System.Web.Hosting.HostingEnvironment.MapPath("~/SavaVaucers/" + model.UserEmail + "-" + model.Points + ".pdf");
            var actionResult = new ViewAsPdf("Print", model);
           // var byteArray = actionResult.();

            //var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            //fileStream.Write(byteArray, 0, byteArray.Length);
            //fileStream.Close();
        }
    }
}