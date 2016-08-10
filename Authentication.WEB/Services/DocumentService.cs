using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace Authentication.WEB.Services
{
    class DocumentService
    {
        public void generateQuote(string quotePath, string quotePathCopy, string quotePathPdf, int premium)
        {
            System.IO.File.Delete(quotePathCopy);
            System.IO.File.Copy(quotePath, quotePathCopy);

            Application wordApp = null;
            wordApp = new Application();
            wordApp.Visible = true;
            Document wordDoc = wordApp.Documents.Open(quotePathCopy);
            wordDoc.Activate();

            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            object a = "«ClientName»";
            object b = "Altay Said";
            wordApp.Selection.Find.Execute(ref a, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format,
                ref b, ref replace, ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);

            a = "«PremiumAmount»";
            b = premium;
            wordApp.Selection.Find.Execute(ref a, ref matchCase, ref matchWholeWord,
                ref matchWildCards, ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format,
                ref b, ref replace, ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);

            System.IO.File.Delete(quotePathPdf);
            wordDoc.ExportAsFixedFormat(quotePathPdf, WdExportFormat.wdExportFormatPDF);

            wordDoc.Close();
            wordApp.Quit();

            MailService mailService = new MailService("asaid@optimalreinsurance.com");
            mailService.attach(new System.Net.Mail.Attachment(quotePathPdf));
            mailService.sendMail();
        }
        }
}
