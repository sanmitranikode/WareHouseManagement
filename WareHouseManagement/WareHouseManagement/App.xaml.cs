using System;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WareHouseManagement
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }
      
      
        public App()
        {
           
            InitializeComponent();
            bool isLoggedIn = LoadApplicationPropertyAsync<bool>("isLoggedIn");


            if (!isLoggedIn )
            {
                MainPage =new LogInPage();
            }
            else
            {
                MainPage = new NavigationPage(new WareHouseManagement.MainPage());
            }

           // MainPage =new NavigationPage(new MainPage());
        }
        private T LoadApplicationPropertyAsync<T>(string key)
        {
            try
            {
                return (T)Xamarin.Forms.Application.Current.Properties[key];
            }
            catch
            {
                Xamarin.Forms.Application.Current.Properties[key] = false;
                return (T)Xamarin.Forms.Application.Current.Properties[key];
            }
          
        }
      

        protected override void OnStart()
        {
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
