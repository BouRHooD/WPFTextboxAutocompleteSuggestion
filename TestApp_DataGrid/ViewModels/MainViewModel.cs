using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace TestApp_DataGrid.ViewModels
{
    public class ItemVm : VMBase
    {
        private string itemText;
        public string ItemText
        {
            get { return itemText; }
            set
            {
                itemText = value;
                OnPropertyChanged();
            }
        }
    }

    public class MainViewModel : VMBase
    {
        private ObservableCollection<ItemVm> testItems;
        public ObservableCollection<ItemVm> TestItems
        {
            get { return testItems; }
            set
            {
                testItems = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            TestItems = new ObservableCollection<ItemVm>()
            {
                new ItemVm(){ ItemText="Apple" },
                new ItemVm(){ ItemText="BouRHooD" },
                new ItemVm(){ ItemText="Banana" },
                new ItemVm(){ ItemText="BananaBourhood" },
                new ItemVm(){ ItemText="Carrot" },
                new ItemVm(){ ItemText="Русский" },
                new ItemVm(){ ItemText="Elderberry" },
                new ItemVm(){ ItemText="Fruit" },
                new ItemVm(){ ItemText="Grapes" },
                new ItemVm(){ ItemText="Honey" },
                new ItemVm(){ ItemText="Iron" },
                new ItemVm(){ ItemText="12345" },
                new ItemVm(){ ItemText="Hello World" }
            };

            TestItems.CollectionChanged += OnItemsCollectionChanged;
        }

        #region // ----- To remember input values ​​in TextBox
        public IEnumerable<string> Strings => TestItems.Select(i => i.ItemText).Distinct().OrderBy(s => s);
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (ItemVm item in e.NewItems) { item.PropertyChanged += OnItemPropertyChanged; }
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (ItemVm item in e.OldItems) { item.PropertyChanged -= OnItemPropertyChanged; }
                        break;
                    }
            }
            OnPropertyChanged(nameof(Strings));
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemVm.ItemText)) { OnPropertyChanged(nameof(Strings)); }
        }
        #endregion

    }
}
