using System.Web.Optimization;

namespace InsuredTraveling
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-3.1.0.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/jquery.tablesorter.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.12.0.min.js"));

            bundles.Add(new ScriptBundle("~/Content/jqueryuicss").Include(
                    "~/Content/themes/base/jquery-ui.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/themes/base/jquery-ui.css",
                     "~/Content/sorter/style.css",
                      "~/Content/FontAwesome/font-awesome.min.css",
                     "~/Content/site.css"));
        }
    }
}
