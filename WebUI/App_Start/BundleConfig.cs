using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebUI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                    .Include("~/Scripts/script.js")  /* не перепутайте порядок */
                    .Include("~/Scripts/bootstrap*")
                    );

            bundles.Add(new StyleBundle("~/Content/css")
                    .Include("~/Content/style.css")  /* не перепутайте порядок */
                    .Include("~/Content/bootstrap*")
                    );

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                     "~/Scripts/jquery-ui-1.*"));

            bundles.Add(new StyleBundle("~/Content/css/jqueryui")
                   .Include("~/Content/jquery-ui-1*"));

            // bootstrap-extensions

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap-tooltip").Include(
                        "~/Scripts/bootstrap-tooltip.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap-typeahead").Include(
                        "~/Scripts/bootstrap-typeahead.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap-alert").Include(
                        "~/Scripts/bootstrap-alert.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jquery.unobtrusive-ajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            // Jcrop

            bundles.Add(new ScriptBundle("~/js/jquery.Jcrop.min.js").Include(
                        "~/js/jquery.Jcrop.min.js"));

            bundles.Add(new StyleBundle("~/css/jquery.Jcrop.min.css")
                   .Include("~/css/jquery.Jcrop.min.css"));

            //jsdelivr for DateTime EditorTemplates

            bundles.Add(new ScriptBundle("~/Content/jsdelivr")
                .Include("~/Content/jsdelivr/modernizr-custom.js")
                .Include("~/Content/jsdelivr/polyfiller.js")
                );
        }
    }
}