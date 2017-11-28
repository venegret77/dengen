using System.Web;
using System.Web.Optimization;
// kek
namespace MashZavod
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery/jquery.min.js",
                      "~/Scripts/bootstrap/js/bootstrap.min.js",
                      "~/Scripts/modernizr/modernizr.js",
                      "~/Scripts/jquery-cookie/jquery.cookie.js",
                      "~/Scripts/perfect-scrollbar/perfect-scrollbar.min.js",
                      "~/Scripts/switchery/switchery.min.js",
                      "~/Scripts/Chart.js/Chart.min.js",
                      "~/Scripts/jquery.sparkline/jquery.sparkline.min.js",
                      "~/Scripts/js/main.js",
                      "~/Scripts/js/index.js"));



        





            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/styles.css",
                      "~/plugins/styles.css",
                      "~/Content/theme-1.css",
                      "~/Content/themify-icons/themify-icons.min.css",
                      "~/Content/fontawesome/css/font-awesome.min.css",

                      "~/Content/animate.css/animate.min.css",
                      "~/Content/perfect-scrollbar/perfect-scrollbar.min.css",
                      "~/Content/switchery/switchery.min.css",


                      "~/Content/bootstrap/css/bootstrap.min.css"));
        }
    }
}
