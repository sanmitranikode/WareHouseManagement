
using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.CurrentActivity;
using Plugin.Permissions;

namespace WareHouseManagement.Droid
{
    [Activity(Label = "WareHouseManagement", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

           

            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
            AppCenter.Start("android=e2d55bf9-f053-49d9-89af-d3e4762aa019;",
                     typeof(Analytics), typeof(Crashes));

          
            global::Acr.BarCodes.BarCodes.Init(() => (Activity)Xamarin.Forms.Forms.Context);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}