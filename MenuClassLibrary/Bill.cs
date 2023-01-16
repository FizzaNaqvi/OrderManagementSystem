using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuClassLibrary
{
    public class Bill : MenuItem
    {

        public int orderId;
        public int quantity { get; set; }
        public double totalPrice { get; set; }
    }
}
