using InsuredTraveling.Models;
using System;
using System.Net.Mail;
using Authentication.WEB.Services;
using System.Net.Mime;

namespace InsuredTraveling.Helpers
{
    public static class SendSavaEmailHelper
    {
        public static bool SendVaucerEmail(UsePointsModel model, string userEmail, float? userPoints, string sellerEmail)
        {
            try
            {
                var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/SAVAMAK728x90.jpg"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();
                var FacebookLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/facebook@2x.png"));
                FacebookLogo.ContentId = Guid.NewGuid().ToString();
                var TwitterLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/twitter@2x.png"));
                TwitterLogo.ContentId = Guid.NewGuid().ToString();
                              
                string mailBody = string.Format(@"   <div>
                  <div >
                  <a href='https://mk.sava.insure/'> <img style='width: 100%; max-width: 1000px; ' src=""cid:{0}"" /> </a>
                  <p> <b> Почитувани,</b></p> " +
                    "<br /> <br />"
              + "Искористивте: " + model.Points + " поени кај Сава брокерот со број: " + model.IDSeller + " Ви остануваат уште " + userPoints.ToString() + " поени.</div><br />"
              + " <div style='border-top: 1px solid #BBBBBB; max-width: 1000px; width:100%; max-width: 1000px; line-height:1px; height:1px; font-size:1px; '>&nbsp;</div> "
              + @" <div style=' text-align: center;'> <a href='https://www.facebook.com/sava.mk'> <img style='width:32px; max-width:35px' src=""cid:{1}"" /></a> <a href='https://twitter.com/Savamk'><img style='width:32px; max-width:35px' src=""cid:{2}"" /></a> </div>"
              + "<br /> "
             , inlineLogo.ContentId, FacebookLogo.ContentId, TwitterLogo.ContentId);

                var view = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Plain);
                var view2 = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);

                view2.LinkedResources.Add(inlineLogo);
                view2.LinkedResources.Add(FacebookLogo);
                view2.LinkedResources.Add(TwitterLogo);

                SavaMailService mailService = new SavaMailService(userEmail);

                mailService.setSubject("Искористени поени Моја Сава.");

                mailService.setBodyText(userEmail, true);

                mailService.AlternativeViews(view);
                mailService.AlternativeViews(view2);

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
                var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/SAVAMAK728x90.jpg"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();
                var FacebookLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/facebook@2x.png"));
                FacebookLogo.ContentId = Guid.NewGuid().ToString();
                var TwitterLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/twitter@2x.png"));
                TwitterLogo.ContentId = Guid.NewGuid().ToString();
                
                string mailBody = string.Format(@"   <div>
                  <div >
                  <a href='https://mk.sava.insure/'> <img style='width: 100%; max-width: 1000px; ' src=""cid:{0}"" /> </a>
                  <p> <b> Почитувани,</b></p>
                  <br /> Корисникот " + user.FirstName + " " + user.LastName + " со корисничко име " + model.Username + " " +
                 "<br /> <br />"
                 + "искористи: " + model.Points + " поени. На корисникот му остануваат уште " + userPoints.ToString() + " поени.</div> <br /> <br />"
                 + " <div style='border-top: 1px solid #BBBBBB; max-width: 1000px; width:100%; max-width: 1000px; line-height:1px; height:1px; font-size:1px; '>&nbsp;</div> "
                 + @" <div style=' text-align: center;'> <a href='https://www.facebook.com/sava.mk'> <img style='width:32px; max-width:35px' src=""cid:{1}"" /></a> <a href='https://twitter.com/Savamk'><img style='width:32px; max-width:35px' src=""cid:{2}"" /></a> </div>"
                 + "<br /> "
                , inlineLogo.ContentId, FacebookLogo.ContentId,TwitterLogo.ContentId);

                var view = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Plain );               
                var view2 = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);
               
                view2.LinkedResources.Add(inlineLogo);
                view2.LinkedResources.Add(FacebookLogo);
                view2.LinkedResources.Add(TwitterLogo);

                SavaMailService mailService = new SavaMailService(sellerEmail);

                mailService.setSubject("Искористени поени од корисник: " + model.Username);

                mailService.setBodyText(sellerEmail, true);
               
                mailService.AlternativeViews(view);
                mailService.AlternativeViews(view2);

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
                var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/SAVAMAK728x90.jpg"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();
                var FacebookLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/facebook@2x.png"));
                FacebookLogo.ContentId = Guid.NewGuid().ToString();
                var TwitterLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/twitter@2x.png"));
                TwitterLogo.ContentId = Guid.NewGuid().ToString();
                
                string mailBody = string.Format(@"   <div>
                  <div >
                  <a href='https://mk.sava.insure/'> <img style='width: 100%; max-width: 1000px; ' src=""cid:{0}"" /> </a>
                  <p> <b> Почитувани,</b></p>
                   <br />" + name + " " + surname +
                  "<br /> <br />" + "Добредојдовте на" + userRole + "  <br />  <b> Уживајте во поволностите со " + userRole + "</ b> </div><br />"
                + " <div style='border-top: 1px solid #BBBBBB; max-width: 1000px; width:100%; max-width: 1000px; line-height:1px; height:1px; font-size:1px; '>&nbsp;</div> "
                + @" <div style=' text-align: center;'> <a href='https://www.facebook.com/sava.mk'> <img style='width:32px; max-width:35px' src=""cid:{1}"" /></a> <a href='https://twitter.com/Savamk'><img style='width:32px; max-width:35px' src=""cid:{2}"" /></a> </div>"
                + "<br /> "
               , inlineLogo.ContentId, FacebookLogo.ContentId, TwitterLogo.ContentId);


                var view = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Plain);
                var view2 = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);

                view2.LinkedResources.Add(inlineLogo);
                view2.LinkedResources.Add(FacebookLogo);
                view2.LinkedResources.Add(TwitterLogo);

                SavaMailService mailService = new SavaMailService(email);

                mailService.setSubject(userRole + " Успешно креиран корисник.");

                mailService.setBodyText(mailBody, true);

                mailService.AlternativeViews(view);
                mailService.AlternativeViews(view2);
                mailService.sendMail();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool SendEmailPolicyUploaded(string email, string name, string surname)
        {
            try
            {
                var inlineLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/SAVAMAK728x90.jpg"));
                inlineLogo.ContentId = Guid.NewGuid().ToString();
                var FacebookLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/facebook@2x.png"));
                FacebookLogo.ContentId = Guid.NewGuid().ToString();
                var TwitterLogo = new LinkedResource(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/twitter@2x.png"));
                TwitterLogo.ContentId = Guid.NewGuid().ToString();

                string mailBody = string.Format(@"   <div>
                  <div >
                  <a href='https://mk.sava.insure/'> <img style='width: 100%; max-width: 1000px; ' src=""cid:{0}"" /> </a>
                  <p> <b> Почитувани,</b></p>
                   <br />" + name + " " + surname +
                  "<br /> <br />" + "Вашите полиси се ажурирани"   + " <br />  <b> Ве молиме внесете број на полиса во вашата мобилна апликација за да го потврдите вашиот идентитет " + "</ b> </div><br />"
                + " <div style='border-top: 1px solid #BBBBBB; max-width: 1000px; width:100%; max-width: 1000px; line-height:1px; height:1px; font-size:1px; '>&nbsp;</div> "
                + @" <div style=' text-align: center;'> <a href='https://www.facebook.com/sava.mk'> <img style='width:32px; max-width:35px' src=""cid:{1}"" /></a> <a href='https://twitter.com/Savamk'><img style='width:32px; max-width:35px' src=""cid:{2}"" /></a> </div>"
                + "<br /> "
               , inlineLogo.ContentId, FacebookLogo.ContentId, TwitterLogo.ContentId);


                var view = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Plain);
                var view2 = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);

                view2.LinkedResources.Add(inlineLogo);
                view2.LinkedResources.Add(FacebookLogo);
                view2.LinkedResources.Add(TwitterLogo);

                SavaMailService mailService = new SavaMailService(email);

                mailService.setSubject( "Ажурирани полиси");

                mailService.setBodyText(mailBody, true);

                mailService.AlternativeViews(view);
                mailService.AlternativeViews(view2);
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