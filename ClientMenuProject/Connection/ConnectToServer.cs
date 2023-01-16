using MenuClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClientMenuProject.Connection
{
   public class ConnectToServer
    {

        public NamedPipeClientStream pipe = new NamedPipeClientStream(".", "menuPipe", PipeDirection.InOut,PipeOptions.Asynchronous);
        XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<MenuItem>));
        StreamReader reader;
        public ConnectToServer(int i=0)
        {
            if(i==1)
            reader = new StreamReader(pipe);
        }

      
        public ObservableCollection<MenuItem> GetFromServer()
        {
            ObservableCollection<MenuItem> menu= new ObservableCollection<MenuItem>();
            //using ()
            //{
            try
            {
                if (!pipe.IsConnected)
                    pipe.Connect();
                //using ()
                //{
                if (pipe.IsConnected)
                    menu = x.Deserialize(reader) as ObservableCollection<MenuItem>;

                //}
                //}
            }
            catch(Exception ex)
            { }
            return menu;
        }

        public void SendOrderToAdmin(Order order)
        {

            using (NamedPipeServerStream pipe = new NamedPipeServerStream("orderPipe", PipeDirection.InOut))
            {
                  pipe.WaitForConnection();
                try
                {
                    XmlSerializer x = new XmlSerializer(typeof(Order));
                    using (StreamWriter writer = new StreamWriter(pipe))
                    {
                        x.Serialize(writer, order);

                    }
                }
                catch (Exception ex)
                { }

            }
        }

        public int GetOrderId()
        {
            int id = 0;
            //using ()
            //{
            try
            {
                using (NamedPipeClientStream pipe1 = new NamedPipeClientStream(".", "idPipe", PipeDirection.InOut, PipeOptions.Asynchronous))
                {
                    if (!pipe1.IsConnected)
                        pipe1.Connect();
                    //using ()
                    //{
                    if (pipe1.IsConnected)
                    {
                        using (StreamReader r = new StreamReader(pipe1))
                        {

                            XmlSerializer x1 = new XmlSerializer(typeof(int));
                            id = Convert.ToInt32(x1.Deserialize(r));
                        }
                    }

                }
                //}
            }
            catch(Exception ex){
              
            }
            //}
            return id;
        }
    }
}
