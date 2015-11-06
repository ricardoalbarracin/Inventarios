using System.Web;
using System.Web.Optimization;

namespace InventariosImportaciones 
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                         "~/Scripts/metisMenu.js"));

            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                         "~/Scripts/sb-admin-2.js"));
            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                         "~/Scripts/morris.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-1.11.3*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css", "~/Content/bootstrap - theme",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/social/css").Include(
                      "~/Content/social/bootstrap-social.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/Dist/css").Include(
                      "~/Content/Dist/sb-admin-2.css", "~/Content/Dist/timeline.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/metisMenu/css").Include(
                      "~/Content/metisMenu/metisMenu.css", "~/Content/metisMenu/metisMenu.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/morris/css").Include(
                      "~/Content/metisMenu/morris.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/font-awesome/css").Include(
                      "~/Content/font-awesome/ccs/font-awesome.css"
                      ));

            //************
            bundles.Add(new StyleBundle("~/Content/sharpen/css").Include(
                      "~/Content/sharpen/ccs/app.min.css", "~/Content/sharpen/ccs/bootstrap - datepicker.cc", "~/Content/sharpen/ccs/bootstrap - datepicker.min.ccs", "~/Content/sharpen/ccs/bootstrap-datetimepicker-standalone.css",
                      "~/Content/sharpen/ccs/bootstrap-datetimepicker.css", "~/Content/sharpen/ccs/bootstrap-datetimepicker.min.css", "~/Content/sharpen/ccs/chartist.min.css", "~/Content/sharpen/ccs/fontello.css", "~/Content/sharpen/ccs/fullcalendar.css", "~/Content/sharpen/ccs/fullcalendar.min.css",
                      "~/Content/sharpen/ccs/fullcalendar.print.css", "~/Content/sharpen/ccs/select2.css", "~/Content/sharpen/ccs/select2.min.css", "~/Content/sharpen/fonts/material-design-iconic-font/ccs/material-design-iconic-font*"

                      ));

           
            

            /////
            bundles.Add(new ScriptBundle("~/bundles/sharpen").Include(
                        "~/Scripts/app.min.js", "~/Scripts/autosize*", "~/Scripts/bootstrap-datepicker.js", "~/Scripts/Chart*", "~/Scripts/chartist*",  "~/Scripts/jquery-ui*", "~/Scripts/select2*", "~/Scripts/parsley*", "~/Scripts/throttle-debounce.min.js", "~/Scripts/jquery.shuffle*", "~/Scripts/raphael*"));
            //*******


        }
    }
}
