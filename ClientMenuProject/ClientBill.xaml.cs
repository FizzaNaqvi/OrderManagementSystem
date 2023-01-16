using MenuClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ClientMenuProject
{
    /// <summary>
    /// Interaction logic for ClientBill.xaml
    /// </summary>
    public partial class ClientBill : Window, INotifyPropertyChanged,INotifyCollectionChanged
    {
        ObservableCollection<Bill> _selecteditems;
       public ObservableCollection<Bill> SelectedItems { get { return _selecteditems;  }
            set { _selecteditems=value;
                OnPropertyChanged("SelectedItems");
            } }
        double totalBill = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }

        public ClientBill()
        {
            InitializeComponent();
            _selecteditems = new ObservableCollection<Bill>();
            this.DataContext = this;
          
        }
        public void GenerateBill(ObservableCollection<Bill> items)
        {
           
           _selecteditems = items;
            CalculateBill();
        }

      
        private void DeleteSelection(object sender, MouseButtonEventArgs e)
        {
            Bill obj = ((FrameworkElement)sender).DataContext as Bill;
            _selecteditems.Remove(obj);
            CalculateBill();

        }

    void CalculateBill()
        {
            totalBill = 0;
            foreach (var item in _selecteditems)
            {
                totalBill += item.totalPrice;
            }
            subTotal.Content = totalBill.ToString();

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Bill obj = ((FrameworkElement)sender).DataContext as Bill;
            var item = _selecteditems.FirstOrDefault(x=>x==obj);
            item.quantity = item.quantity-1;
            obj.quantity = item.quantity;
            if (item.quantity <= 0)
                _selecteditems.Remove(obj);
            else
            {
                item.totalPrice = item.totalPrice - item.Price;
                obj.totalPrice = item.totalPrice;
            }
            CalculateBill();
            
            
        }
        private void AddSelection(object sender, MouseButtonEventArgs e)
        {
            Bill obj = ((FrameworkElement)sender).DataContext as Bill;
            var item = _selecteditems.FirstOrDefault(x=>x==obj);
            item.quantity = item.quantity+1;
            obj.quantity = item.quantity;
           
                item.totalPrice = item.totalPrice+item.Price;
                obj.totalPrice = item.totalPrice;
            
            CalculateBill();
            
            
        }
    }
}
