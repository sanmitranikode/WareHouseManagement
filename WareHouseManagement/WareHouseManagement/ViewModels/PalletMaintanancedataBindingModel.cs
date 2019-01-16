using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.PCL.Service;
using Xamarin.Forms;

namespace WareHouseManagement.ViewModels
{
    class PalletMaintanancedataBindingModel: INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ProductBarcodeResponseModel> _items;
        public ObservableCollection<PalletItemResponse> TagData;
        public ObservableCollection<ProductBarcodeResponseModel> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        public PalletMaintanancedataBindingModel(List< ProductBarcodeResponseModel> itemList)
        {
            Items = new ObservableCollection<ProductBarcodeResponseModel>();
            foreach (ProductBarcodeResponseModel itm in itemList)
            {
                Items.Add(itm);
            }
        }


        public PalletMaintanancedataBindingModel(List<PalletItemResponse> itemTagData)
        {
            TagData = new ObservableCollection<PalletItemResponse>();
            foreach (PalletItemResponse itm in itemTagData)
            {
                TagData.Add(itm);
            }
        }



    }

   
}
