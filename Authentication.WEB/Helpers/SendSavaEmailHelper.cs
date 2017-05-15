using InsuredTraveling.Models;
using System;
using System.Net.Mail;
using Authentication.WEB.Services;

namespace InsuredTraveling.Helpers
{
    public static class SendSavaEmailHelper
    {
        public static bool SendVaucerEmail(UsePointsModel model, string userEmail, float? userPoints)
        {
            try
            {
                var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/EmailHeaderSuccess.png"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();

                string mailBody = string.Format(@"   
                     <div style='margin-left:20px'>
                     <img style='width:700px' src=""cid:{0}"" />
                     <p> <b> Почитувани, </b></p>                  
                     <br />
                 <br /> <br />" + "Искористивте: " + model.Points + " поени. Ви остануваат уште "+ userPoints.ToString()+" поени.</div><br />"
                , inlineLogo.ContentId);

                var view = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                view.LinkedResources.Add(inlineLogo);

                MailService mailService = new MailService(userEmail);
                mailService.setSubject("Искористени поени");

                mailService.setBodyText(userEmail, true);
                mailService.AlternativeViews(view);

                mailService.sendMail();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public static bool SendVaucerEmailToSeller(UsePointsModel model, aspnetuser user, float? userPoints, string sellerEmail)
        {
            try
            {
                var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/EmailHeaderSuccess.png"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();

                string mailBody = string.Format(@"   
                     <div style='margin-left:20px'>
                     <img style='width:700px' src=""cid:{0}"" />
                     <p> <b> Почитувани, </b></p>                  
                     <br />
                      Корисникот " + user.FirstName + " " + user.LastName + " со корисничко име " + model.Username + " " +
                 "<br /> <br />" + "искористи: " + model.Points + " поени. Ви остануваат уште " + userPoints.ToString() + " поени.</div><br />"
                , inlineLogo.ContentId);

                var view = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                view.LinkedResources.Add(inlineLogo);

                MailService mailService = new MailService(sellerEmail);
                mailService.setSubject("Искористени поени");

                mailService.setBodyText(sellerEmail, true);
                mailService.AlternativeViews(view);

                mailService.sendMail();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public static bool SendEmailForUserChangeRole(string email, string name, string surname, string userRole)
        {
            try
            {
                var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/EmailHeaderSuccess.png"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();
                string mailBody = string.Format(@"   
                     <div style='margin-left:20px'>
                     <img style='width:700px' src=""cid:{0}"" />
                     <p> <b> Почитувани, </b></p>                  
                     <br />" + name + " " + surname +
                     "<br /> <br />" + "Вие станавте " + userRole + "  <br />  <b>Честитки. </b> </div><br />"
                , inlineLogo.ContentId);

                var view = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                view.LinkedResources.Add(inlineLogo);
                MailService mailService = new MailService(email);
                mailService.setSubject("Промена на корисничи привилегии");
                mailService.setBodyText(email, true);
                mailService.AlternativeViews(view);
                mailService.sendMail();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}