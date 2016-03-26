using RestaurantManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RestaurantManager.ViewModels
{
    public class OrderViewModel : ViewModel
    {
        private List<MenuItem> _menuItems;
        private MenuItem _selectedMenuItem;

        public OrderViewModel()
        {
            AddMenuItemCommand = new DelegateCommand(AddMenuItem);
            SubmitOrderCommand = new DelegateCommand(SubmitOrder);
            this.CurrentlySelectedMenuItems = new ObservableCollection<MenuItem>();
        }

        protected override void OnDataLoaded()
        {
            this.MenuItems = base.Repository.StandardMenuItems;
        }

        public List<MenuItem> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                NotifyPropertyChanged();
            }
        }
        public MenuItem SelectedMenuItem
        {
            get { return _selectedMenuItem; }
            set
            {
                _selectedMenuItem = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<MenuItem> CurrentlySelectedMenuItems { get; private set; }

        public DelegateCommand AddMenuItemCommand { get; private set; }

        public DelegateCommand SubmitOrderCommand { get; private set; }

        public void AddMenuItem()
        {
            this.CurrentlySelectedMenuItems.Add(this.SelectedMenuItem);
        }

        public void SubmitOrder()
        {
            base.Repository.Orders.Add(
                new Order
                {
                    Items = this.CurrentlySelectedMenuItems.ToList(),
                    Table = base.Repository.Tables.First(),
                    Expedite = false
                }
            );
            this.CurrentlySelectedMenuItems.Clear();

            // Explanation: This is a bad idea with a true MVVM app (seperation of view concerns from logic) but for simplicity sake:
            new Windows.UI.Popups.MessageDialog("Order has been submitted").ShowAsync();
        }
    }
}
