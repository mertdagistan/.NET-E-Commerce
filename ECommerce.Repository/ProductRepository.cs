using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Common;
using ECommerce.Entity;

namespace ECommerce.Repository
{
    public class ProductRepository : DataRepository<Product, int>
    {
        private static MyECommerceEntities _db = Tools.GetConnection();
        ResultProcess<Product> result = new ResultProcess<Product>();
        public override Result<int> Delete(int id)
        {
            Product silinecek = _db.Products.SingleOrDefault(t => t.ProductId == id);
            _db.Products.Remove(silinecek);
            return result.GetResult(_db);
        }

        public override Result<List<Product>> GetLatestObj(int Quantity)
        {
            return result.GetListResult(_db.Products.OrderByDescending(t => t.ProductId).Take(Quantity).ToList());
        }

        public override Result<Product> GetObjById(int id)
        {
            return result.GetT(_db.Products.SingleOrDefault(t => t.ProductId == id));
        }

        public override Result<int> Insert(Product item)
        {
            _db.Products.Add(item);
            return result.GetResult(_db);
        }

        public override Result<List<Product>> List()
        {

            return result.GetListResult(_db.Products.ToList());

        }

        public override Result<int> Update(Product item)
        {
            Product gunP = _db.Products.SingleOrDefault(t => t.ProductId == item.ProductId);
            gunP.BrandId = item.BrandId;
            gunP.CategoryId = item.CategoryId;
            gunP.Stock = item.Stock;
            gunP.Photo = item.Photo;
            gunP.Price = item.Price;
            gunP.ProductName = item.ProductName;
            return result.GetResult(_db);
        }
    }
}
