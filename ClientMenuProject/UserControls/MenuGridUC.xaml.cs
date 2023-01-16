using MenuClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MenuItem = MenuClassLibrary.MenuItem;

namespace ClientMenuProject.UserControls
{
    /// <summary>
    /// Interaction logic for MenuGridUC.xaml
    /// </summary>
    public partial class MenuGridUC : UserControl
    {

       
        public MenuGridUC()
        {
            InitializeComponent();
        }

        private void AddToCart(object sender, MouseButtonEventArgs e)
        {
            MenuItem obj = ((FrameworkElement)sender).DataContext as MenuItem;
            Bill bill = new Bill();
            bill.Id = obj.Id;
            bill.Name = obj.Name;
            bill.Price = obj.Price;
            bill.Desc = obj.Desc;
            bill.Type = obj.Type;
            bill.quantity = 1;
            bill.totalPrice = obj.Price;
            MainWindow window = Window.GetWindow(this) as MainWindow;
            window.quantityLbl.Content = Convert.ToInt32(window.quantityLbl.Content) + 1;
            window.priceLbl.Content = Convert.ToDouble(window.priceLbl.Content) + obj.Price;
            if (window.BillVm.SelectedItems.Count != 0)
            {
                var menuItem = window.BillVm.SelectedItems.FirstOrDefault(x=>x.Id==obj.Id);
                if(menuItem != null)
                {
                    menuItem.quantity = menuItem.quantity + 1;
                    menuItem.totalPrice = menuItem.quantity * menuItem.Price;
                }
                else
                    window.BillVm.SelectedItems.Add(bill);
            }
                else
            window.BillVm.SelectedItems.Add(bill);
  
           //   window.selectedItems.Add(obj);
        }
    }
}
