using Olwen_2._0._0.DataModel;
using Olwen_2._0._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olwen_2._0._0.Repositories.Interfaces
{
    public interface IStoreDetail
    {
        bool UpdateQty(StoreDetail sd);
        bool AddNewPro(StoreDetail sd);
        bool DeletePro(StoreDetail sd);

        bool AddListProToStore(IEnumerable<ProductStoreModel> ds,int storeID);
    }
}
