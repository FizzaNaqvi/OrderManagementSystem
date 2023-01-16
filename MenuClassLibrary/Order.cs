using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MenuClassLibrary
{
    [Serializable, XmlRoot("order")]
    public class Order
    {
        public int Id { get; set; }
        public ObservableCollection<Bill> bill {get; set;}
        public DateTime dateTime { get; set; } 
        public string status { get; set; } 
        public double totalBill { get; set; } 
    }
}
