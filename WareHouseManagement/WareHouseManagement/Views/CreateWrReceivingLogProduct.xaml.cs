using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Service;
using WareHouseManagement.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WareHouseManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateWrReceivingLogProduct : ContentPage
    {
        public List<CustomerViewModel> _Customerlist = new List<CustomerViewModel>();
        public List<VendorModel> _Venderlist = new List<VendorModel>();
        public List<ProductModel> _Productlist = new List<ProductModel>();
        WRReceivingLogModel _wrReceivinglogmodel = new WRReceivingLogModel();
        public CreateWrReceivingLogProduct()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            popupListViewCustomer.IsVisible = false;
        }

        private void CustomerList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (CustomerViewModel)e.SelectedItem;
            if (item != null)
            {
                txt_CustomerId.Text = item.Id.ToString();
                txt_customer.Text = item.CustomerName;
                ((ListView)sender).SelectedItem = null;

            }

            popupListViewCustomer.IsVisible = false;
        }

        private async void Searchcustomer_Tapped(object sender, EventArgs e)
        {
            var getCustomerdata = await new PalletMaintainanceService().GetPalletLog(GetCustomerAndVender.getcustomerlist);
            if (getCustomerdata.Status == 1)
            {
                _Customerlist = JsonConvert.DeserializeObject<List<CustomerViewModel>>(getCustomerdata.Response.ToString());
                CustomerListView.ItemsSource = _Customerlist;
            }

            popupListViewCustomer.IsVisible = true;
        }

        private void Tapancel_Tapped(object sender, EventArgs e)
        {
            popupListViewVender.IsVisible = false;
        }

        private void listVender_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (VendorModel)e.SelectedItem;
            if (item != null)
            {
                txt_Vendor.Text = item.Name;
                ((ListView)sender).SelectedItem = null;
            }

            popupListViewVender.IsVisible = false;

        }

        private async void TapVenderSarch_Tapped(object sender, EventArgs e)
        {
            var getvenderdata = await new PalletMaintainanceService().GetPalletLog(GetCustomerAndVender.getvenderlist);
            if (getvenderdata.Status == 1)
            {
                _Venderlist = JsonConvert.DeserializeObject<List<VendorModel>>(getvenderdata.Response.ToString());
                listVender.ItemsSource = _Venderlist;
            }
            popupListViewVender.IsVisible = true;
        }

        private async void btn_addproduct_Clicked(object sender, EventArgs e)
        {
            if ((txt_customer.Text == ""|| txt_customer.Text == null) || (txt_lotNo.Text==""|| txt_lotNo.Text ==null) ||( txt_Container.Text==""|| txt_Container.Text == null)||( txt_Vendor.Text=="" || txt_Vendor.Text == null))
            {
                DisplayAlert("Message", "Fill Up Properly", "Ok");
            }
            else
            {
                popupAddProductView.IsVisible = true;
            }

           

        }

        private async void txt_product_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_product.Text == "" || txt_product.Text == null)
            {
                listproduct.IsVisible = false;
                txt_Quantity.IsVisible = true;
                txt_Weight.IsVisible = true;
                txt_Cubic.IsVisible = true;
                datepicker_Expiry.IsVisible = true;
                calender.IsVisible = true;
            }
            else
            {
                var getproductdata = await new PalletMaintainanceService().GetPalletLog(GetCustomerAndVender.getproductlist + txt_product.Text);
                if (getproductdata.Status == 1)
                {
                    var Product = JsonConvert.DeserializeObject<List<ProductModel>>(getproductdata.Response.ToString());
                    listproduct.ItemsSource = Product;
                }
                listproduct.IsVisible = true;
                txt_Quantity.IsVisible = false;
                txt_Weight.IsVisible = false;
                txt_Cubic.IsVisible = false;
                datepicker_Expiry.IsVisible = false;
                calender.IsVisible = false;
            }

        }

        private void productViewcancel_Tapped(object sender, EventArgs e)
        {
            popupAddProductView.IsVisible = false;
        }

        private void listproduct_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (ProductModel)e.SelectedItem;
            if (item != null)
            {
                txt_product.Text = item.Name;
                txt_ProductId.Text = item.Id.ToString();
                ((ListView)sender).SelectedItem = null;
            }

            listproduct.IsVisible = false;
            txt_Quantity.IsVisible = true;
            txt_Weight.IsVisible = true;
            txt_Cubic.IsVisible = true;
            datepicker_Expiry.IsVisible = true;
            calender.IsVisible = true;
        }

        private void btn_Viewaddproduct_Clicked(object sender, EventArgs e)
        {
            if((txt_product.Text==""|| txt_product.Text ==null )|| (txt_Quantity.Text==""|| txt_Quantity.Text == null )||( txt_Weight.Text==""|| txt_Weight.Text == null )|| (txt_Cubic.Text==""|| txt_Cubic.Text ==null))
            {
                DisplayAlert("Message", "Fill Up Properly", "Ok");
            }
            else
            {
                _Productlist.Add(new ProductModel
                {
                    Id = Convert.ToInt32(txt_ProductId.Text),
                    Name = txt_product.Text,
                    Quantity = Convert.ToInt32(txt_Quantity.Text),
                    Weight = Convert.ToDecimal(txt_Weight.Text),
                    Cubic = Convert.ToDecimal(txt_Cubic.Text),
                    ExDate = datepicker_Expiry.Date
                });



                ProductGridlist.ItemsSource = null;
                ProductGridlist.ItemsSource = _Productlist;
                cleardataaftersubmit();

            }
            
        }

        private void cleardataaftersubmit()
        {
            txt_product.Text = null;
            txt_Quantity.Text = null;
            txt_Weight.Text = null;
            txt_Cubic.Text = null;
            txt_ProductId.Text = null;
            datepicker_Expiry.Date = DateTime.Now;

        }

        private void DeleteProductitem_Tapped(object sender, EventArgs e)
        {
            var dataproduct = ((TappedEventArgs)e).Parameter;
            var listitems = (from itm in _Productlist where itm.Id == Convert.ToInt32(dataproduct) select itm).FirstOrDefault<ProductModel>();
            //var Postlistitems = (from itm in _wrReceivinglogmodel.wrReceivingProducts where itm.ProductId == Convert.ToInt32(dataproduct) select itm).FirstOrDefault<WRReceivingProducts>();
            //_wrReceivinglogmodel.wrReceivingProducts.Remove(Postlistitems);
            _Productlist.Remove(listitems);
            ProductGridlist.ItemsSource = null;
            ProductGridlist.ItemsSource = _Productlist;

        }

        private void btn_saveproduct_Clicked(object sender, EventArgs e)
        {

            _wrReceivinglogmodel.Id = Convert.ToInt32(txt_CustomerId.Text);
            _wrReceivinglogmodel.LotNo = txt_lotNo.Text;
            _wrReceivinglogmodel.ReceivedDate = txt_rcvingDate.Date;
            _wrReceivinglogmodel.Active = true;
            _wrReceivinglogmodel.ContainerNo = txt_Container.Text;
            _wrReceivinglogmodel.WRReceivingLogStatusId = 10;
            var data = _Productlist.Select(a => new WRReceivingProducts
            {
                ProductId = a.ProductId,
                ProductName = a.Name,
                Quantity = a.Quantity,
                Weight = a.Weight,
                Cubic = a.Cubic,
                ExDate = a.ExDate,
                ProductStatusId = 10

             }).ToList();
                _wrReceivinglogmodel.wrReceivingProducts = data;

        }

        private void btn_ClearProduct_Clicked(object sender, EventArgs e)
        {
            cleardataaftersubmit();
        }

        private void btn_AllClear_Clicked(object sender, EventArgs e)
        {

        }
    }
}