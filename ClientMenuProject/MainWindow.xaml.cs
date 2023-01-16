using ClientMenuProject.Connection;
using ClientMenuProject.ViewModel;
using MenuClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using MenuItem = MenuClassLibrary.MenuItem;

namespace ClientMenuProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ConnectToServer connect = new ConnectToServer();
        NamedPipeClientStream pipe = new NamedPipeClientStream(".", "menuPipe", PipeDirection.In);
        MenuViewModel vm = new MenuViewModel();
        public BillViewModel BillVm = new BillViewModel();
        ThreadStart thStart;
        Thread th;
        int isReset;
        //ObservableCollection<Bill> _selecteditems;
        //public ObservableCollection<Bill> SelectedItems
        //{
        //    get { return _selecteditems; }
        //    set
        //    {
        //        _selecteditems = value;
        //        OnPropertyChanged("SelectedItems");
        //    }
        //}
        //double totalBill = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm;
            BillGrid.DataContext = BillVm;
            isReset = 0;


              }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }

        private void ViewCart_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(quantityLbl.Content) != 0)
            {
                userCart.Visibility = Visibility.Collapsed;
                backToMenuPanel.Visibility = Visibility.Visible;
                subTotal.Content=BillVm.CalculateBill();
                tab.Visibility = Visibility.Collapsed;
                BillGrid.Visibility = Visibility.Visible;
                BillData.Items.Refresh();
                waiting.Visibility = Visibility.Collapsed;
                dateTime.Visibility = Visibility.Collapsed;
                viewBill.Visibility = Visibility.Collapsed;
            }
            else
                MessageBox.Show("Please select item(s).","Warning");
        }

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           TabItem tab1 = tab.SelectedItem as TabItem;

            string option = tab1.Header.ToString();
            vm.FilteredMenuItem.Clear();
            vm.SelectedOptionMenu(option);
        }

        private void DeleteSelection(object sender, MouseButtonEventArgs e)
        {
            Bill obj = ((FrameworkElement)sender).DataContext as Bill;
            subTotal.Content = BillVm.DeleteFromBill(obj);
            quantityLbl.Content = Convert.ToInt32(quantityLbl.Content) -obj.quantity;
            priceLbl.Content = Convert.ToDouble(priceLbl.Content)- obj.totalPrice;
           }

       

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Bill obj = ((FrameworkElement)sender).DataContext as Bill;
            subTotal.Content = BillVm.RemoveFromSelection(obj);
            //var item = _selecteditems.FirstOrDefault(x => x == obj);
            //item.quantity = item.quantity - 1;
            //obj.quantity = item.quantity;
            //if (item.quantity <= 0)
            //    _selecteditems.Remove(obj);
            //else
            //{
            //    item.totalPrice = item.totalPrice - item.Price;
            //    obj.totalPrice = item.totalPrice;
            //}
            quantityLbl.Content = Convert.ToInt32(quantityLbl.Content) - 1;
            priceLbl.Content = Convert.ToDouble(priceLbl.Content) - obj.Price;
            BillData.Items.Refresh();


        }
        private void AddSelection(object sender, MouseButtonEventArgs e)
        {
            Bill obj = ((FrameworkElement)sender).DataContext as Bill;
            subTotal.Content = BillVm.AddInSelection(obj);
            quantityLbl.Content = Convert.ToInt32(quantityLbl.Content) + 1;
            priceLbl.Content = Convert.ToDouble(priceLbl.Content) + obj.Price;
            BillData.Items.Refresh();


        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            if ((options.Visibility == Visibility.Collapsed) && (viewBill.Visibility == Visibility.Collapsed))
            {
                OrderPlacedGrid.Visibility = Visibility.Visible;
                viewBill.Visibility = Visibility.Visible;
                waiting.Visibility = Visibility.Visible;
                dateTime.Visibility = Visibility.Visible;
            }
            else
            {
                userCart.Visibility = Visibility.Visible;
                backToMenuPanel.Visibility = Visibility.Collapsed;
                tab.Visibility = Visibility.Visible;
                BillGrid.Visibility = Visibility.Collapsed;
                waiting.Visibility = Visibility.Collapsed;
                dateTime.Visibility = Visibility.Collapsed;
                viewBill.Visibility = Visibility.Collapsed;

                if (OrderPlacedGrid.Visibility == Visibility.Visible || isReset == 1)
                {
                    BillVm.SelectedItems = new ObservableCollection<Bill>();
                    BillData.ItemsSource = BillVm.SelectedItems;
                    options.Visibility = Visibility.Visible;
                    quantityLbl.Content = 0;
                    priceLbl.Content = 0.00;
                    order.Visibility = Visibility.Visible;
                    isReset = 0;
                    BillVm.order = new Order();
                }
                OrderPlacedGrid.Visibility = Visibility.Collapsed;
            }

        }
    
        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (!subTotal.Content.Equals("0"))
            {
                options.Visibility = Visibility.Collapsed;
                BillGrid.Visibility = Visibility.Collapsed;
                OrderPlacedGrid.Visibility = Visibility.Visible;
                order.Visibility = Visibility.Collapsed;
                 BillVm.ConfirmOrder("Preparing");
                dateTime.Content ="Ordered @"+ DateTime.Now.ToString();
                waiting.Visibility = Visibility.Visible;
                dateTime.Visibility = Visibility.Visible;
                viewBill.Visibility = Visibility.Visible;

                thStart = new ThreadStart(autoReset);
                th = new Thread(thStart);
                th.Start();
            }
            else
            {
                MessageBox.Show("Please select item(s)");
                BackToMenu_Click( sender,  e);
            }
           }
        void AutoReset()
        {
            TimeSpan t = DateTime.Now.Subtract(BillVm.order.dateTime);
            while(t.Minutes<5)
            {
                t = DateTime.Now.Subtract(BillVm.order.dateTime);
            }
            Reset_MouseDown(null,null);
        }
        private void ViewBill_Click(object sender, RoutedEventArgs e)
        {
            BillGrid.Visibility = Visibility.Visible;
            OrderPlacedGrid.Visibility = Visibility.Collapsed;
            waiting.Visibility = Visibility.Collapsed;
            dateTime.Visibility = Visibility.Collapsed;
           viewBill.Visibility = Visibility.Collapsed;
            order.Visibility = Visibility.Collapsed;
          }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

                BillVm.ConfirmOrder("Cancel");
              
          
            }

        private void Reset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isReset = 1;
            BackToMenu_Click(sender, e);
               }

        private void CancelOrder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TimeSpan t = DateTime.Now.Subtract(BillVm.order.dateTime);
            if (t.Seconds < 5)
            {
                BillVm.ConfirmOrder("Canceled");
                MessageBox.Show("Order Canceled successfully", "Message");
                isReset = 1;
                BackToMenu_Click(sender, e);
                th.Abort();
            }
            else
                MessageBox.Show("You cannot cancel this order.", "Warning");

        }

        void autoReset()
        {
            Thread.Sleep(10000);
            isReset = 1;
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                BackToMenu_Click(null, null);
      });
           
        }
    }
}
