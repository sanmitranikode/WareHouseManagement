using System;
using System.Collections.Generic;
using System.Text;
using WareHouseManagement.Views;

namespace WareHouseManagement.ViewModels
{
    public class PageDataViewModel
    {
        public PageDataViewModel(Type type, string title, string description)
        {
            Type = type;
            Title = title;
            Description = description;
        }

        public Type Type { private set; get; }

        public string Title { private set; get; }

        public string Description { private set; get; }

        static PageDataViewModel()
        {
            All = new List<PageDataViewModel>
            {
                new PageDataViewModel(typeof(ReaderList), "Reader List",
                                      "To select the reader from available reader list"),
                new PageDataViewModel(typeof(InventoryList), "Item inventory",
                                      "Used for Item count or stock count\nPerforms inventory operation"),
                  new PageDataViewModel(typeof(RelativeDistance), "Relative Distance",
                                      "Shows the relative proximity of multiple tags"),
            };
        }

        public static IList<PageDataViewModel> All { private set; get; }
    }
}
