using ECommerce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerceSample.Models
{
    public class OrderInfoViewModel
    {
        public Order Order { get; set; }
        public Invoice Invoice { get; set; }
        public OrderDetail OrderDetail { get; set; }

    }
}