using System.Web.Optimization;

namespace InsuredTraveling
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jQuery/jquery-3.1.0.min.js",
                "~/Scripts/Bootstrap/bootstrap.min.js",
                "~/Scripts/jQuery/jquery.tablesorter.min.js",
                "~/Scripts/jQuery/jquery-ui-1.12.0.min.js",
                "~/Scripts/app/app.js",
                "~/Scripts/app/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/Bootstrap/bootstrap.min.css",
                     "~/Content/sorter/style.css",
                      "~/Content/FontAwesome/font-awesome.min.css",
                      "~/Content/themes/base/jquery-ui.min.css",
                     "~/Content/site.css"));
        }
    }
}
