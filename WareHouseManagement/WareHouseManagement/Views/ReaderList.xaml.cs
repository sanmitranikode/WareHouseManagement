using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderList : ContentPage
    {
        private ReaderListViewModel viewmodel;

        public ReaderList()
        {
            InitializeComponent();
            BindingContext = viewmodel = new ReaderListViewModel();
            Title = "Readers List";
        }

        private void lvMenu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewmodel.ItemSelected(e.Item);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewmodel.UpdateIn();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewmodel.UpdateOut();
        }
    }
}