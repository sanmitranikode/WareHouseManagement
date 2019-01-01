using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WareHouseManagement.ViewModels
{
    class PalletMaintanancedataBindingModel
    {
        private ObservableCollection<List<WRReceivingLogResponseViewModel>> _myObservableCollection { get; set; }

        public PalletMaintanancedataBindingModel()
        {
            MyObservableCollection = _myObservableCollection;
        }

        public ObservableCollection<List<WRReceivingLogResponseViewModel>> MyObservableCollection
        {
            get { return _myObservableCollection; }
            set
            {
                _myObservableCollection = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
