using Olwen_2._0._0.DataModel;
using Olwen_2._0._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Olwen_2._0._0.Repositories.Interfaces
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        Task<IEnumerable<ProductModel>> GetAllProductModelsAsync();

        IEnumerable<ProductModel> GetAllProductModels();

        IEnumerable<ProductModel> GetProductModelsFilter(Func<Product, bool> predicate);

        Task<IEnumerable<ProductStoreModel>> GetAllProductStoreByStoreIDAsync(int storeID);

        IEnumerable<ProductStoreModel> GetAllProductStoreByStoreID(int storeID);

        IEnumerable<ProductStoreModel> GetAllProductStoreRest(int storeID);

        IEnumerable<ProductODModel> GetAllProductODByOrderID(int orderID);

        IEnumerable<Store> GetAllStoreByProductID(int productID);

        IEnumerable<Store> GetAllStoreByNoProductID(int productID);
    }
}
