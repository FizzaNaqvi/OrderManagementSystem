using AdminMenuProject.Connection;
using MenuClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MenuItem = MenuClassLibrary.MenuItem;

namespace AdminMenuProject.ViewModel
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        ConnectToClient connect = new ConnectToClient();
        public MenuItem _menuItem;
        public ObservableCollection<MenuItem> _filteredMenuItem;
        public ObservableCollection<MenuItem> _menuItemsList;

        public ObservableCollection<String> _menuItemTypes;
        public ObservableCollection<MenuItem> FilteredMenuItem
        {
            get { return _filteredMenuItem; }
            set { _filteredMenuItem = value; }
        }

        public ObservableCollection<MenuItem> MenuItemsList
        {
            get { return _menuItemsList; }
            set { _menuItemsList = value; }
        }

        public ObservableCollection<String> MenuItemTypes
        {
            get { return _menuItemTypes; }
            set { _menuItemTypes = value; }
        }

        public MenuItem MenuItem
        {
            get { return _menuItem; }
            set { _menuItem = value; OnPropertyChanged("MenuItem");  }
        }
        public MenuViewModel()
        {
            _menuItemsList = new ObservableCollection<MenuItem>();
            _menuItemsList = Deserialize();
            _filteredMenuItem = new ObservableCollection<MenuItem>();
            _menuItemTypes = new ObservableCollection<String>();
            PopulateMenuItem();
            _menuItem = new MenuItem();

            ThreadStart thStart = new ThreadStart(sendList);
            Thread th = new Thread(thStart);
            th.Start();
           
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void PopulateMenuItem()
        {
            // typeCombo.ItemsSource = null;
            //List<string> list = new List<string>();
            _menuItemTypes.Add("Starter");
            _menuItemTypes.Add("B.B.Q.");
            _menuItemTypes.Add("Burgers");
            _menuItemTypes.Add("Sandwich");
            _menuItemTypes.Add("Broast");
            _menuItemTypes.Add("Roll");
            _menuItemTypes.Add("Others");
          //  typeCombo.ItemsSource = list;
            //typeCombo.SelectedIndex = tab.SelectedIndex;

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }

        public void SelectedOptionMenu(string optionName)
        {
            Deserialize();
            ObservableCollection<MenuItem> m = new ObservableCollection<MenuItem>(MenuItemsList.Where(x => x.Type == optionName));
            FilteredMenuItem.Clear();
            foreach (var item in m)
            {
                FilteredMenuItem.Add(item);
            }
        }

        public void AddItem()
        {
           
            if (_menuItemsList.Count == 0)
                MenuItem.Id = 1;
            else
                MenuItem.Id = _menuItemsList[_menuItemsList.Count - 1].Id+1;
            _menuItemsList.Add(MenuItem);
            serialize();
            if(FilteredMenuItem.Count!=0)
            SelectedOptionMenu(FilteredMenuItem[0].Type);
            else
              SelectedOptionMenu(MenuItem.Type);
           // connect.SendToClient(_menuItemsList);
            MenuItem = new MenuItem();
            
        }

        public void serialize()
        {
            try
            {
                string path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\Desktop\MenuDataNew.txt";
                XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<MenuItem>));
                //StreamWriter writer = new StreamWriter(path);
                //x.Serialize(writer, _menuItemsList);

                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate)))
                {
                    x.Serialize(writer, _menuItemsList);

                }
            }
            catch (Exception ex)
            {

            }
           
        }

        public ObservableCollection<MenuItem> Deserialize()
        {

            try
            {
                string path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\Desktop\MenuDataNew.txt";
                XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<MenuItem>));
                StreamReader reader = new StreamReader(path);
                _menuItemsList = x.Deserialize(reader) as ObservableCollection<MenuItem>;
                //connect.SendToClient(_menuItemsList);
                return _menuItemsList;
            }
            catch(Exception ex)
            {
                return new ObservableCollection<MenuItem>();
            }
        }

        public void DeleteItem()
        {
            MenuItemsList.Remove(MenuItem);
            FilteredMenuItem.Remove(MenuItem);
            serialize();
           // connect.SendToClient(_menuItemsList);
            MenuItem = new MenuItem();

           // connect.SendToClient(_menuItemsList);
        }

        public void UpdateItem()
        {
            var item = MenuItemsList.FirstOrDefault(x => x.Id == MenuItem.Id);
            item.Price = MenuItem.Price;
            item.Name = MenuItem.Name;
            item.Type = MenuItem.Type;
            item.Desc = MenuItem.Desc;

            var item1 = FilteredMenuItem.FirstOrDefault(x => x.Id == MenuItem.Id);
            item1.Price = MenuItem.Price;
            item1.Name = MenuItem.Name;
            item1.Type = MenuItem.Type;
            item1.Desc = MenuItem.Desc;
            serialize();
            if (FilteredMenuItem.Count != 0)
                SelectedOptionMenu(FilteredMenuItem[0].Type);
            else
                SelectedOptionMenu(MenuItem.Type);


            MenuItem = new MenuItem();

            //connect.SendToClient(_menuItemsList);

        }

        public void CancelItem()
        {
            MenuItem = new MenuItem();
        }

       void sendList()
        {
            while (true)
            {
                connect.SendToClient(MenuItemsList);
            }
        }

    }
}
