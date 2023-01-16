using ClientMenuProject.Connection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MenuClassLibrary;

namespace ClientMenuProject.ViewModel
{
   public class BillViewModel
    {
        double totalBill = 0;
        ObservableCollection<Bill> _selecteditems;
       public Order order;
        int orderId = 0;
        ConnectToServer connect = new ConnectToServer();
        public ObservableCollection<Bill> SelectedItems
        {
            get { return _selecteditems; }
            set
            {
                _selecteditems = value;
            }
        }

        public BillViewModel()
        {
            _selecteditems = new ObservableCollection<Bill>();
            order = new Order();
        }
        public string DeleteFromBill(Bill bill)
        {
          //  Bill obj = ((FrameworkElement)sender).DataContext as Bill;
            //quantityLbl.Content = Convert.ToInt32(quantityLbl.Content) - obj.quantity;
            //priceLbl.Content = Convert.ToDouble(priceLbl.Content) - obj.totalPrice;
            _selecteditems.Remove(bill);
            return CalculateBill();

        }
        public string RemoveFromSelection(Bill bill)
        {
            var item = _selecteditems.FirstOrDefault(x => x == bill);
            if (item != null)
            {
                item.quantity = item.quantity - 1;
                bill.quantity = item.quantity;
                if (item.quantity <= 0)
                    _selecteditems.Remove(bill);
                else
                {
                    item.totalPrice = item.totalPrice - item.Price;
                    bill.totalPrice = item.totalPrice;
                }
            }
            return CalculateBill();
            }

        public string AddInSelection(Bill bill)
        {
            var item = _selecteditems.FirstOrDefault(x => x == bill);
            item.quantity = item.quantity + 1;
            bill.quantity = item.quantity;

            item.totalPrice = item.totalPrice + item.Price;
            bill.totalPrice = item.totalPrice;
            return CalculateBill();
           
        }

        public string CalculateBill()
        {
            totalBill = 0;
            foreach (var item in _selecteditems)
            {
                totalBill += item.totalPrice;
            }
           return totalBill.ToString();

        }
       
        public void ConfirmOrder(string status)
        {
           
            order.bill = SelectedItems;
            if (status != "Canceled")
            {
                order.Id = DeserializeordersForId();
                orderId = order.Id;
            }
            else
            {
                order.Id = orderId;
            }
            order.dateTime = DateTime.Now;
            order.status = status;
            order.totalBill = Convert.ToDouble(CalculateBill());
            connect.SendOrderToAdmin(order);
         
           // serialize();
            //Deserialize();
        }

        public void Deserialize()
        {

            try
            {
                ObservableCollection<Order> orders = new ObservableCollection<Order>();
                string path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\Desktop\Bills.txt";
                XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<Order>));
                StreamReader reader = new StreamReader(path);
                orders = x.Deserialize(reader) as ObservableCollection<Order>;
               
            }
            catch(Exception ex)
            {
               
            }
        }
        public int DeserializeordersForId()
        {

            try
            {
                ObservableCollection<Order> _orders;
                string path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\Desktop\OrderDetails1.txt";
                XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<Order>));
                StreamReader reader = new StreamReader(path);
                _orders = x.Deserialize(reader) as ObservableCollection<Order>;
                reader.Close();
                
                //connect.SendToClient(_menuItemsList);
                return _orders[_orders.Count-1].Id+1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
    }
}
