using System;
using System.Collections.Generic;
using System.Linq;
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
            // New up the Razor template
            var printTemplate = new ListPrintTemplate();

            // Set the model property (ViewModel is a custom property within containing view - FYI)
            printTemplate.Model = ViewModel.ezPrints.ToList();

            // Generate the HTML
            var htmlString = printTemplate.GenerateString();

            // Create a source for the webview
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = htmlString;

            // Create and populate the Xamarin.Forms.WebView
            var browser = new WebView();
            browser.Source = htmlSource;

            var printService = DependencyService.Get<IPrintService>();
            printService.Print(browser);
        }
    }
}