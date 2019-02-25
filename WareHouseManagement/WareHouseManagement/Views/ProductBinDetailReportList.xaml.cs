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
	public partial class ProductBinDetailReportList : ContentPage
	{
        #region Declaration
        List<BinsDetailByProductModel> _model = new List<BinsDetailByProductModel>();
        #endregion
        public ProductBinDetailReportList(List<BinsDetailByProductModel> model)
		{
			InitializeComponent ();
            _model = model;
            BinList.ItemsSource = _model;
        }
	}
}