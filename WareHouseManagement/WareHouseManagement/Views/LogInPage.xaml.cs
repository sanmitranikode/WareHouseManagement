﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Helper;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        
        bool isValid =false;
        SharedPreference _objShared = new SharedPreference();

        public LogInPage()
        {
            InitializeComponent();
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try { NavigationPage.SetHasNavigationBar(this, false); } catch { }
           

        }
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            //this.IsBusy = true;


            activity.IsRunning = true;

            btn_login.IsEnabled = false;
            GetUserLoginAsync();

            
        }
        public async void GetUserLoginAsync()
        {
            UserModel _User = new UserModel
            {
                Username = userEmailEntry.Text,
                Password = passwordEntry.Text
            };
            var UserDetail = await new PalletMaintainanceService().GetLoginDetail(_User, PalletMaintainanceServiceUrl.GetUserLoginDetail);
            if (UserDetail.Status == 1)
            {
                var UserData = JsonConvert.DeserializeObject<UserLoginViewModel>(UserDetail.Response.ToString());
                GlobalConstant.AccessToken = UserData.Token;
                _objShared.SaveApplicationProperty("AccessToken", GlobalConstant.AccessToken);

                //Application.Current.Properties["UserEmail"] = userEmailEntry.Text;
                //Application.Current.Properties["Password"] = passwordEntry.Text;







                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
                btn_login.IsEnabled = true;
                activity.IsRunning = false;
            }
            else
            {
                activity.IsRunning = false;
                DisplayAlert("Alert!", " Email Or Password Incorrect", "OK");
                passwordEntry.Text = string.Empty;
                btn_login.IsEnabled = true;

            }
        }
       


    }
}