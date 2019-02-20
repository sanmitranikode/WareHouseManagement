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
using WareHouseManagement.PCL.Model;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Newtonsoft.Json;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WareHouseManagement
{
    public partial class App : Application
    {


        SharedPreference _objShared = new SharedPreference();
        public App()
        {

            InitializeComponent();
            
            GlobalConstant.UserName = _objShared.LoadApplicationProperty<string>("UserName");
            GlobalConstant.UserPassword = _objShared.LoadApplicationProperty<string>("UserPassword");

            LogInMethod();


            if (GlobalConstant.UserName == null && GlobalConstant.UserPassword==null)
            {
                MainPage = new LogInPage();
            }
            else
            {
                MainPage = new NavigationPage(new WareHouseManagement.MainPage());
            }

            // MainPage =new NavigationPage(new MainPage());
        }
        private async void LogInMethod()
        {
            UserModel _User = new UserModel
            {
                Username = GlobalConstant.UserName,
                Password = GlobalConstant.UserPassword
            };
            var UserDetail = await new PalletMaintainanceService().GetLoginDetail(_User, PalletMaintainanceServiceUrl.GetUserLoginDetail);
            if (UserDetail.Status == 1)
            {
                var UserData = JsonConvert.DeserializeObject<UserLoginViewModel>(UserDetail.Response.ToString());
                GlobalConstant.AccessToken = UserData.Token;
                GlobalConstant.UserName = UserData.Username;
                GlobalConstant.UserPassword = UserData.Password;
                _objShared.SaveApplicationProperty("AccessToken", GlobalConstant.AccessToken);
                _objShared.SaveApplicationProperty("UserName", GlobalConstant.UserName);
                _objShared.SaveApplicationProperty("UserPassword", GlobalConstant.UserPassword);
            }
            else
            {
                GlobalConstant.AccessToken = null;
                GlobalConstant.UserName = null;
                GlobalConstant.UserPassword = null;
            }
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=e2d55bf9-f053-49d9-89af-d3e4762aa019;",
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
