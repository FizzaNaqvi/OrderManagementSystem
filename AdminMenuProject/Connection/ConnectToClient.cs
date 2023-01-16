using MenuClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdminMenuProject.Connection
{
   public class ConnectToClient
    {
        public NamedPipeClientStream pipe = new NamedPipeClientStream(".", "orderPipe", PipeDirection.InOut, PipeOptions.Asynchronous);
        XmlSerializer x = new XmlSerializer(typeof(Order));
        StreamReader reader;

        public ConnectToClient(int i=0)
        {
            if(i==1)
                reader = new StreamReader(pipe);

        }
        public void SendToClient(ObservableCollection<MenuItem> menu)
        {
            
            using (NamedPipeServerStream pipe = new NamedPipeServerStream("menuPipe", PipeDirection.InOut))
            {
                // string path = "";
                
                //string path = @"C:\Users\fnaqvi\OneDrive - TRAFiX, LLC\My Practice Project_V2\ClientMenuProject_V2\ClientMenuProject\ClientMenuProject\bin\Debug\ClientMenuProject.exe";
                //ProcessStartInfo info = new ProcessStartInfo(path);
                //Process.Start(info);
                pipe.WaitForConnection();
                try
                {
                        XmlSerializer x = new XmlSerializer(typeof(ObservableCollection<MenuItem>));
                        using (StreamWriter writer = new StreamWriter(pipe))
                        {
                        //writer.AutoFlush = true;
                            x.Serialize(writer, menu);

                        }
                       }
                    catch (Exception ex)
                    { }
                
            }
        }
        public Order GetOrderFromClient()
        {
            Order order = new Order();
            try
            {
                if (!pipe.IsConnected)
                    pipe.Connect();
                if (pipe.IsConnected)
                {
                    order = x.Deserialize(reader) as Order;
                   return order;
                }
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public void SendOrderId(int Id)
        {

            using (NamedPipeServerStream pipe = new NamedPipeServerStream("idPipe", PipeDirection.InOut))
            {
                  pipe.WaitForConnection();
                try
                {
                    XmlSerializer x = new XmlSerializer(typeof(int));
                    using (StreamWriter writer = new StreamWriter(pipe))
                    {
                        //writer.AutoFlush = true;
                        x.Serialize(writer, Id);

                    }
                }
                catch (Exception ex)
                { }

            }
        }
    }
}
