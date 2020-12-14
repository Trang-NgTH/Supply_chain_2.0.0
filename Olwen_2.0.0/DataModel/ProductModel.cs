using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Olwen_2._0._0.DataModel
{
    public class ProductModel: ObservableObject
    {
        private string _name;
        private int? _quantity;
        private int _productID;
        private decimal? _unitPrice;
        private decimal? _priceOnOrder;
        private string _description;
        private BitmapImage _picture;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                Set<string>(() => this.Name, ref _name, value);

            }
        }

        public int? Quantity
        {
            get
            {
                return _quantity;
            }

            set
            {
                Set<int?>(() => this.Quantity, ref _quantity, value);
            }
        }

        public int ProductID
        {
            get
            {
                return _productID;
            }

            set
            {
                Set<int>(() => this.ProductID, ref _productID, value);
            }
        }

        public decimal? UnitPrice
        {
            get
            {
                return _unitPrice;
            }

            set
            {
                Set<decimal?>(() => this.UnitPrice, ref _unitPrice, value);
            }
        }

        public decimal? PriceOnOrder
        {
            get
            {
                return _priceOnOrder;
            }

            set
            {
                Set<decimal?>(() => this.PriceOnOrder, ref _priceOnOrder, value);
            }
        }

        public BitmapImage Picture
        {
            get
            {
                return _picture;
            }

            set
            {
                Set<BitmapImage>(() => this.Picture, ref _picture, value);
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
                Set<string>(() => this.Description, ref _description, value);
            }
        }

        public ProductModel()
        {

        }
    }
}
