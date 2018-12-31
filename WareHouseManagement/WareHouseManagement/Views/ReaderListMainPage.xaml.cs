using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderListMainPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }
        private ReaderModel rfidModel;
        public ReaderListMainPage()
        {
            InitializeComponent();
            rfidModel = ReaderModel.readerModel;
        }

        private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            (sender as ListView).SelectedItem = null;

            if (args.SelectedItem != null)
            {
                PageDataViewModel pageData = args.SelectedItem as PageDataViewModel;
                Page page = (Page)Activator.CreateInstance(pageData.Type);
                await Navigation.PushAsync(page, false);
            }
        }

        internal void OnResume()
        {
            rfidModel?.SetTriggerMode();
            Console.WriteLine("OnResume");
        }

        internal void OnSleep()
        {
            rfidModel?.Disconnect();
            Console.WriteLine("OnSleep");
        }
    }
}
