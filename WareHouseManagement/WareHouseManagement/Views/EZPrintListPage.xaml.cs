using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.PrintTemplates;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EZPrintListPage : ContentPage
	{
        EZPrintListViewModel ViewModel;
        public EZPrintListPage ()
		{
			InitializeComponent ();
            ViewModel = new EZPrintListViewModel();
            BindingContext = ViewModel;
        }
        void PrintList(object sender, System.EventArgs e)
        {
            var source = new HtmlWebViewSource();
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();
            //var assembly = typeof(resi).GetTypeInfo().Assembly;
            //Stream stream = assembly.GetManifestResourceStream("JavaScript1.js");
            string html = "";

           
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("WareHouseManagement.Droid.index.html");
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
            try
            {
                using (var sr = new StreamReader(stream))
                {
                    html = sr.ReadToEnd();
                    source.Html = html;
                }


                browser.Source = source;
                Content = browser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        



            var printService = DependencyService.Get<IPrintService>();
            printService.Print(browser);
        }
    }
}