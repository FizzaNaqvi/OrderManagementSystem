using AdminMenuProject.Connection;
using AdminMenuProject.ViewModel;
using MenuClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace AdminMenuProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      //  ConnectToClient connect = new ConnectToClient();
        MenuViewModel vm = new MenuViewModel();
        OrderViewModel orderVm;
        //NamedPipeServerStream pipe = new NamedPipeServerStream("menuPipe", PipeDirection.Out);
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm;
            orderVm = new OrderViewModel();
            orderGrid.DataContext = orderVm;
            //try
            //{
            //    string path = @"C:\Users\fnaqvi\source\repos\ClientMenuProject\ClientMenuProject\bin\Debug\ClientMenuProject.exe";
            //    ProcessStartInfo info = new ProcessStartInfo(path);
            //    Process.Start(info);

            //}
            //catch (Exception ex)
            //{ }
            // BindTypeComboBox();

        }
        public void BindTypeComboBox()
        {
            typeCombo.ItemsSource = null;
            List<string> list = new List<string>();
            list.Add("Starter");
            list.Add("B.B.Q.");
            list.Add("Burgers");
            list.Add("Sandwich");
            list.Add("Broast");
            list.Add("Roll");
            list.Add("Others");
             typeCombo.ItemsSource = list;
            typeCombo.SelectedIndex = tab.SelectedIndex;

        }
        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            TabItem tab1 = tab.SelectedItem as TabItem;
            string option = tab1.Header.ToString();
            typeCombo.SelectedValue = option;
            vm.FilteredMenuItem.Clear();
            vm.SelectedOptionMenu(option);
        }

        private void AddToMenu(object sender, MouseButtonEventArgs e)
        {
            if (validate())
            {
                vm.AddItem();
                typeCombo.SelectedIndex = tab.SelectedIndex;
            }
            else
                MessageBox.Show("Please fill all fields");
             }

        private void Update_Click(object sender, MouseButtonEventArgs e)
        {
            if(validate())
                {
            vm.UpdateItem();
                typeCombo.SelectedIndex = tab.SelectedIndex;
                setControls();
                
            }
            else
                MessageBox.Show("Please fill all fields");
        }

        private void Cancel_Click(object sender, MouseButtonEventArgs e)
        {
            vm.CancelItem();
            setControls();
        }
        void setControls()
        {

            updatePanel.Visibility = Visibility.Collapsed;
            AddBtn.Visibility = Visibility.Visible;
        }

        bool validate()
        {
            if (txtName.Text == "" || txtPrice.Text == "" || txtDesc.Text == "" || typeCombo.SelectedIndex == -1)
                return false;
            return true;
        }

        private void EllipseMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainLbl.Content = "MENU";
            tab.Visibility = Visibility.Visible;
            mainView.Visibility = Visibility.Collapsed;
            addControls.Visibility = Visibility.Visible;
            back.Visibility = Visibility.Visible;
        }

        private void EllipseOrder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainLbl.Content = "TODAY'S ORDERS";
            orderGrid.Visibility = Visibility.Visible;
            mainView.Visibility = Visibility.Collapsed;
            back.Visibility = Visibility.Visible;
          //  orderVm.OrderList.Reverse();
            orderGrid.Items.Refresh();
        }

        private void Back_Click(object sender, MouseButtonEventArgs e)
        {
            //hiding menu
            mainLbl.Content = "ORDER MANAGEMENT SYSTEM";
            tab.Visibility = Visibility.Collapsed;
            mainView.Visibility = Visibility.Visible;
            addControls.Visibility = Visibility.Collapsed;

            orderGrid.Visibility = Visibility.Collapsed;
            //hiding back button
            back.Visibility = Visibility.Hidden;
        }

      

        private void EllipseReport_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        // saving all orders in file on closing
          private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            orderVm.serialize();
        }

        private void Status_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Order o = ((FrameworkElement)sender).DataContext as Order;
            if (o.status == "Preparing" && o.status!="Canceled")
                o.status = "Delivered";
            var updateOrder=orderVm.OrderList.FirstOrDefault(x=>x.Id==o.Id);
            updateOrder.status = o.status;
            orderGrid.Items.Refresh();
            
        }

        private void TxtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }
    }
}
