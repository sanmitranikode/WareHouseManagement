using Acr.BarCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace WareHouseManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarcodeGeneratePage : ContentPage
    {
        public BarcodeGeneratePage()
        {
            InitializeComponent();
            var btnCreateQR = new Button { Text = "Genrate" };
            var imgCode = new Image();
            var txtBarcode = new EntryCell { Label = "Value " };

            btnCreateQR.Clicked += (sender, e) =>
            {
                try
                {
                    var QRstream = BarCodes.Instance.Create(new BarCodeCreateConfiguration
                    {
                        Format = BarCodeFormat.CODE_128,
                        BarCode = txtBarcode.Text.Trim(),
                        Width = 500,
                        Height = 100
                    }
                    );
                    txtBarcode.LabelColor = Color.White;
                    imgCode.Source = ImageSource.FromStream(() => QRstream);
                }
                catch (Exception ex)
                {
                    txtBarcode.LabelColor = Color.Red;
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    DisplayAlert("Alert", "Enter value that want to be carried in the QR Code", "OK");
                }
            };


            this.Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text="QR CODE GENERATOR",
                        FontSize=25,
                        FontAttributes=FontAttributes.Bold,
                        HorizontalOptions=LayoutOptions.CenterAndExpand,
                    },
                    btnCreateQR,
                    imgCode,
                    new TableView(new TableRoot {
                        new TableSection {
                            txtBarcode
                        }
                    })

                }
            };
        }
    }
}