using AdminMenuProject.ViewModel;
using System;
using System.Collections.Generic;
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

namespace AdminMenuProject.UserControls
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
    

        private void edit_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow win = Window.GetWindow(this) as MainWindow;
            if (win != null)
            {
                win.updatePanel.Visibility = Visibility.Visible;
                win.AddBtn.Visibility = Visibility.Collapsed;
            }
            ((MenuViewModel)this.DataContext).MenuItem = ((FrameworkElement)sender).DataContext as MenuItem;
      
           // vm.DeleteItem();
        }

        private void delete_Click(object sender, MouseButtonEventArgs e)
        {
            ((MenuViewModel)this.DataContext).MenuItem = ((FrameworkElement)sender).DataContext as MenuItem;
            ((MenuViewModel)this.DataContext).DeleteItem();

        }
    }
}
