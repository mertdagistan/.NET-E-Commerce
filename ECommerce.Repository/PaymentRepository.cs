using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Entity;
using ECommerce.Common;

namespace ECommerce.Repository
{
    public class PaymentRepository
    {
        private static MyECommerceEntities db = Tools.GetConnection();
        public static List<Payment> List()
        {
            return db.Payments.ToList();
        }
    }
}
