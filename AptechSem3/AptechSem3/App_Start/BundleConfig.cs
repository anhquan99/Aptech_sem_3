using System.Web;
using System.Web.Optimization;

namespace AptechSem3
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Content/CandidateWeb/assets/js/vendor/modernizr-3.5.0.min.js",
                      "~/Content/CandidateWeb/assets/js/vendor/jquery-1.12.4.min.js",
                      "~/Content/CandidateWeb/assets/js/popper.min.js",
                      "~/Content/CandidateWeb/assets/js/bootstrap.min.js",
                      "~/Content/CandidateWeb/assets/js/jquery.slicknav.min.js",
                      "~/Content/CandidateWeb/assets/js/owl.carousel.min.js",
                      "~/Content/CandidateWeb/assets/js/slick.min.js",
                      "~/Content/CandidateWeb/assets/js/wow.min.js",
                      "~/Content/CandidateWeb/assets/js/animated.headline.js",
                      "~/Content/CandidateWeb/assets/js/jquery.magnific-popup.js",
                      "~/Content/CandidateWeb/assets/js/jquery.nice-select.min.js",
                      "~/Content/CandidateWeb/assets/js/jquery.sticky.js",
                      "~/Content/CandidateWeb/assets/js/contact.js",
                      "~/Content/CandidateWeb/assets/js/jquery.form.js",
                      "~/Content/CandidateWeb/assets/js/jquery.validate.min.js",
                      "~/Content/CandidateWeb/assets/js/mail-Scripts.js",
                      "~/Content/CandidateWeb/assets/js/jquery.ajaxchimp.min.js",
                      "~/Content/CandidateWeb/assets/js/plugins.js",
                      "~/Content/CandidateWeb/assets/js/main.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
