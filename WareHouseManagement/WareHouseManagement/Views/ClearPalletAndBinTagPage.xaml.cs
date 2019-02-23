using Com.Zebra.Rfid.Api3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Model;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClearPalletAndBinTagPage : TabbedPage
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        public List<TagItem> Tags = new List<TagItem>();
       
        

        List<ProductBarcodeResponseModel> _model = new List<ProductBarcodeResponseModel>();
        public List<ProductBarcodeResponseModel> allItems;
        PalletMaintanancedataBindingModel items;
        PalletItemResponseModel _pendingItem = new PalletItemResponseModel();
       

      

        public ClearPalletAndBinTagPage()
		{
			InitializeComponent ();
		}



    }
}