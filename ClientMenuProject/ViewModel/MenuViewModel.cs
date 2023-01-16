using ClientMenuProject.Connection;
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

namespace ClientMenuProject.ViewModel
{
   public class MenuViewModel : INotifyPropertyChanged
    {
        ConnectToServer connect = new ConnectToServer(1);
        ObservableCollection<MenuItem> _filteredMenuItem;
        ObservableCollection<MenuItem> _menuItemsList;
        ThreadStart thStart;
        Thread th;
      
        public ObservableCollection<MenuItem> FilteredMenuItem
        {
            get { return _filteredMenuItem; }
            set { _filteredMenuItem = value; OnPropertyChanged("FilteredMenuItem"); }
        }

        public ObservableCollection<MenuItem> MenuItemsList
        {
            get { return _menuItemsList; }
            set { _menuItemsList = value; OnPropertyChanged("MenuItemsList"); }
        }

        public MenuViewModel()
        {
            _menuItemsList = new ObservableCollection<MenuItem>();
            _filteredMenuItem = new ObservableCollection<MenuItem>();


           thStart = new ThreadStart(GetData);
            th = new Thread(thStart);
            th.Start();
           // MenuItemsList = connect.GetFromServer();
        }
        public event PropertyChangedEventHandler PropertyChanged;



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
            //  MenuItemsList = Deserialize();
           // MenuItemsList = connect.GetFromServer();
            ObservableCollection<MenuItem> m = new ObservableCollection<MenuItem>(MenuItemsList.Where(x => x.Type == optionName));
            FilteredMenuItem.Clear();
            foreach (var item in m)
            {
                FilteredMenuItem.Add(item);
            }
        }

        public ObservableCollection<MenuItem> Deserialize()
        {
          
            try
            {
                string path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\Desktop\MenuDataNew.txt";
                XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<MenuItem>));
                StreamReader reader = new StreamReader(path);
                MenuItemsList = x.Deserialize(reader) as ObservableCollection<MenuItem>;
                return MenuItemsList;
            }
            catch
            {
                return new ObservableCollection<MenuItem>();
            }
        }
        public void GetData()
        {
            
           // connect.pipe.Connect();
            while (true)
            {
                MenuItemsList = connect.GetFromServer();
               Thread.Sleep(5000);
            }
                
        }

    }
}
