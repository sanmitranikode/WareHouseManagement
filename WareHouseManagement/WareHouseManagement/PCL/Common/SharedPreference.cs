using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.PCL.Common
{
    public class SharedPreference
    {
        public async Task SaveApplicationProperty<T>(string key, T value)
        {
            Xamarin.Forms.Application.Current.Properties[key] = value;
            await Xamarin.Forms.Application.Current.SavePropertiesAsync();
        }

        public string LoadApplicationProperty<T>(string key)
        {
            try
            {
                return Xamarin.Forms.Application.Current.Properties[key].ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
