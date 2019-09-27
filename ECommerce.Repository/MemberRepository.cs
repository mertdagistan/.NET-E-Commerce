using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Entity;
using ECommerce.Common;

namespace ECommerce.Repository
{
    public class MemberRepository : DataRepository<Member, int>
    {
        private static MyECommerceEntities db = Tools.GetConnection();
        ResultProcess<Member> result = new ResultProcess<Member>();
        public override Result<int> Delete(int id)
        {
            Member m = db.Members.SingleOrDefault(t => t.UserId == id);
            db.Members.Remove(m);
            return result.GetResult(db);
        }

        public override Result<List<Member>> GetLatestObj(int Quantity)
        {
            return result.GetListResult(db.Members.OrderByDescending(t => t.UserId).Take(Quantity).ToList());
        }

        public override Result<Member> GetObjById(int id)
        {
            return result.GetT(db.Members.SingleOrDefault(t => t.UserId == id));
        }

        public override Result<int> Insert(Member item)
        {
            db.Members.Add(item);
            return result.GetResult(db);
        }

        public override Result<List<Member>> List()
        {
            return result.GetListResult(db.Members.ToList());
        }

        public override Result<int> Update(Member item)
        {
            Member m = db.Members.SingleOrDefault(t => t.UserId == item.UserId);
            m.FirstName = item.FirstName;
            m.LastName = item.LastName;
            m.Password = item.Password;
            m.RoleId = item.RoleId;
            m.Email = item.Email;
            m.Address = item.Address;
            return result.GetResult(db);
        }
        public  Result<int> UpdateProfile(Member item)
        {
            Member m = db.Members.SingleOrDefault(t => t.UserId == item.UserId);
            m.FirstName = item.FirstName;
            m.LastName = item.LastName;
            m.Email = item.Email;
            m.Address = item.Address;
            return result.GetResult(db);
        }

        public Result<int> UpdatePw(Member item)
        {
            Member m = db.Members.SingleOrDefault(t => t.UserId == item.UserId);
            m.Password = item.Password;
            return result.GetResult(db);
        }
    }
}
