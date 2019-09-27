using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Common;

namespace ECommerce.Repository
{
    public abstract class DataRepository<T,M>
    {
        public abstract Result<int> Insert(T item);
        public abstract Result<int> Update(T item);
        public abstract Result<int> Delete(M id);
        public abstract Result<List<T>> List();
        public abstract Result<T> GetObjById(M id);
        public abstract Result<List<T>> GetLatestObj(int Quantity); //son eklenilen urunleri listeleymek icin kullanacagim method
    }
}
