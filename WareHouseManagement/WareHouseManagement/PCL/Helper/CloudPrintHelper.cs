using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using WareHouseManagement.PCL.Common;
using WareHouseManagement.PCL.Model;

namespace WareHouseManagement.PCL.Helper
{
   public class CloudPrintHelper
    {

      public string  PrintData(PrintPalletModel _model)
        {
           
            string html = "";


            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("WareHouseManagement.Droid.index.html");
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
            try
            {
                using (var sr = new StreamReader(stream))
                {
                    html = sr.ReadToEnd();

                    html = html.Replace("#CustomerName#", _model.CustomerName);
                    html = html.Replace("#Todaydate#", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("#LotNo#", _model.LotNo);
                    html = html.Replace("#TotalProduct#", _model.TotalProducts.ToString());
                    html = html.Replace("#BarcodeURL#", GlobalConstant.BaseUrl+ "/Barcode/" + _model.Tag + ".jpeg");
                    

                }

                return html;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }






          
        }
    }
}
