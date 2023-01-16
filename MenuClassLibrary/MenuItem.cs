using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuClassLibrary
{
    public class MenuItem:INotifyPropertyChanged
    {
         int itemId;
         string itemName;
         double itemPrice;
         string itemType;
         string itemDesc;

        public int Id { get { return itemId; } set { itemId = value; OnPropertyChanged("Id"); } }
        public string Name { get { return itemName; } set { itemName = value; OnPropertyChanged("Name"); } }
        public string Type
        { get { return itemType; }
            set { itemType = value; OnPropertyChanged("Type"); } }
        public string Desc { get { return itemDesc; } set { itemDesc = value; OnPropertyChanged("Desc"); } }
        public double Price { get { return itemPrice; } set { itemPrice = value; OnPropertyChanged("Price"); } }
       
        public MenuItem()
        {
           // Type = "Starter";
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
    }
}
