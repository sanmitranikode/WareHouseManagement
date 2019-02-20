using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Model;
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
        PalletModel _model { get; set; }
        public EZPrintListPage (PalletModel model)
		{
			InitializeComponent ();
            ViewModel = new EZPrintListViewModel();
            BindingContext = ViewModel;
            _model = model;
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

                    html = html.Replace("#CustomerName#", _model.CustomerName);
                    html = html.Replace("#Todaydate#", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("#LotNo#", _model.LotNo);
                    html = html.Replace("#TotalProduct#", _model.TotalProducts.ToString());
                    html = html.Replace("#BarcodeURL#", "http://3.16.25.240:8025/Barcode/"+ _model.Tag+".jpeg");
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