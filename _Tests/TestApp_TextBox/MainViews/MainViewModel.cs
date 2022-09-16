using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp_TextBox.MainViews
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            TestItems = new ObservableCollection<string>()
            {
                "Apple",
                "BouRHooD",
                "Banana",
                "BananaBourhood",
                "Carrot",
                "Русский",
                "Elderberry",
                "Fruit",
                "Grapes",
                "Honey",
                "Iron",
                "12345",
                "Hello World"
            };
        }
        public ObservableCollection<string> TestItems { get; set; }
    }
}
