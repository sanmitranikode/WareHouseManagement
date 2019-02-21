using System;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WareHouseManagement
{
    public partial class App : Application
    {


        SharedPreference _objShared = new SharedPreference();
        public App()
        {
           
            InitializeComponent();
            GlobalConstant.AccessToken= _objShared.LoadApplicationProperty<string>("AccessToken");




            if (GlobalConstant.AccessToken==null)
            {
                MainPage =new LogInPage();
            }
            else
            {
                MainPage = new NavigationPage(new WareHouseManagement.MainPage());
            }

           // MainPage =new NavigationPage(new MainPage());
        }
       
      

        protected override void OnStart()
        {
            AppCenter.Start("android=e2d55bf9-f053-49d9-89af-d3e4762aa019;" ,
                  typeof(Analytics), typeof(Crashes));
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
