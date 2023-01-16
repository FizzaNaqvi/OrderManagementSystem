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

namespace AdminMenuProject.ViewModel
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        ConnectToClient connect = new ConnectToClient(1);
        ThreadStart thStart;
        Thread th;
        public ObservableCollection<Order> _orders;
        public ObservableCollection<Order> OrderList {
            get {
                return _orders;
            }
            set {
                _orders = value;
            } }
        public event PropertyChangedEventHandler PropertyChanged;

        public OrderViewModel()
        {
            _orders = new ObservableCollection<Order>();
            Deserialize();
            thStart = new ThreadStart(GetOrder);
            th = new Thread(thStart);
            th.Start();

        }

        public void serialize()
        {
            try
            {
                string path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\Desktop\OrderDetails.txt";
              

                XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<Order>));
                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create)))
                {
                    x.Serialize(writer, _orders);
                
                }
                //for client
                 path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\Desktop\OrderDetails1.txt";


                 x = new XmlSerializer(typeof(ObservableCollection<Order>));
                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create)))
                {
                    x.Serialize(writer, _orders);

                }
            }
            catch (Exception ex)
            {

            }

        }
        public ObservableCollection<Order> Deserialize()
        {

            try
            {
                string path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\Desktop\OrderDetails.txt";
                XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<Order>));
                StreamReader reader = new StreamReader(path);
                _orders = x.Deserialize(reader) as ObservableCollection<Order>;
                //connect.SendToClient(_menuItemsList);
                return _orders;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Order>();
            }
        }

        public void GetOrder()
        {
            Order order = new Order();
          
            // connect.pipe.Connect();
            while (true)
            {
                order = connect.GetOrderFromClient();
                if(order!=null)
                {
                    if (OrderList.Count != 0)
                    {
                        var o = OrderList.FirstOrDefault(x => x.Id == order.Id);
                        if(o!=null)
                        {
                            o.status = order.status;
                            o.dateTime = order.dateTime;
                        
                        }
                    }
                    //    order.Id = 1;
                    //else
                    // order.Id = OrderList[OrderList.Count-1].Id+1;
                    if (order.status != "Canceled")
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                        {
                            OrderList.Add(order);
                           
                        // connect.SendOrderId(order.Id);
                    });
                        serialize();
                    }
                    
                }
                Thread.Sleep(500);
            }

        }
        

    }
}
