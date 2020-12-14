using Olwen_2._0._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olwen_2._0._0.DependencyInjection
{
    public static class SqlSupport
    {

        public static int? GetQuantityByProductId(int ProductID)
        {
            var query = from c in DbSupport.Ins.Db.StoreDetails
                        where c.ProductID == ProductID
                        group c by c.ProductID into groupSD
                        select groupSD.Sum(t => t.Quantity);

            if (query.FirstOrDefault() == null)
                return 0;
            return query.First();
        }

        public static IEnumerable<GetOrdersByCusID_Result> GetOrderByCusId(this int id)
        {
            return DbSupport.Ins.Db.GetOrdersByCusID(id);
        }

        public static IEnumerable<GetOrdersDetailByOrderID_Result> GetOrdersDetailByOrderID(this int id)
        {
            return DbSupport.Ins.Db.GetOrdersDetailByOrderID(id);
        }
    }
}
