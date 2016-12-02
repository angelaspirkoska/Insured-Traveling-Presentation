using System.Web.Optimization;

namespace InsuredTraveling
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-3.1.1.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/jquery.tablesorter.min.js",
                "~/Scripts/jquery.signalR-2.2.1.min.js",
                "~/signalr/hubs",
                "~/Scripts/Chat/chat.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/Content/jqueryuicss").Include(
                    "~/Content/jquery-ui.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/themes/base/jquery-ui.css",
                     "~/Content/sorter/style.css",
                      "~/Content/FontAwesome/font-awesome.min.css",
                     "~/Content/site.css"));
        }
    }
}
