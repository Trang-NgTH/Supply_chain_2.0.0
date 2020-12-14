using Olwen_2._0._0.DataModel;
using Olwen_2._0._0.Model;
using Olwen_2._0._0.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olwen_2._0._0.Repositories.Implements
{
    public class StoreDetailRepository : IStoreDetail
    {
        public bool AddListProToStore(IEnumerable<ProductStoreModel> ds, int storeID)
        {
            try
            {
                StoreDetail sd = new StoreDetail();
                using (var db = new DbEntities())
                {
                    foreach (var pro in ds)
                    {
                        sd = new StoreDetail()
                        {
                            ProductID = pro.ProductID,
                            Quantity = pro.Quantity,
                            StoreID = storeID
                        };

                        db.StoreDetails.Add(sd);
                        db.SaveChanges();
                    }

                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddNewPro(StoreDetail sd)
        {
            try
            {
                var db = new DbEntities();
                db.StoreDetails.Add(sd);
                db.SaveChanges();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeletePro(StoreDetail sd)
        {
            try
            {
                var db = new DbEntities();
                var sdo = db.StoreDetails.SingleOrDefault(t => t.ProductID == sd.ProductID && t.StoreID == sd.StoreID);
                db.StoreDetails.Remove(sdo);
                db.SaveChanges();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateQty(StoreDetail sd)
        {
            try
            {
                var db = new DbEntities();
                var sdo = db.StoreDetails.SingleOrDefault(t => t.Product.ProductID == sd.ProductID && t.Store.StoreID == sd.StoreID);
                sdo.Quantity = sd.Quantity;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
