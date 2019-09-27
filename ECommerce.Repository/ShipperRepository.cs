using ECommerce.Common;
using ECommerce.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repository
{
    public class ShipperRepository 
    {
        private static MyECommerceEntities db = Tools.GetConnection();
        public static List<Shipper> List()
        {
            return db.Shippers.ToList();
        }
    }
}
