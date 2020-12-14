using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olwen_2._0._0.DataModel
{
    public class ProductODModel:ObservableObject
    {
        private int? _productID;
        private int? _orderQty;
        private string _name;
        private double? _unitPrice, _subTotal;
        private int? _storeID;

        public int? ProductID
        {
            get
            {
                return _productID;
            }

            set
            {
                _productID = value;
                Set<int?>(() => this.ProductID, ref _productID, value);
            }
        }

        public int? OrderQty
        {
            get
            {
                return _orderQty;
            }

            set
            {
                _orderQty = value;
                Set<int?>(() => this.OrderQty, ref _orderQty, value);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;

                Set<string>(() => this.Name, ref _name, value);
            }
        }

        public double? UnitPrice
        {
            get
            {
                return _unitPrice;
            }

            set
            {
                _unitPrice = value;
                Set<double?>(() => this.UnitPrice, ref _unitPrice, value);
            }
        }

        public double? SubTotal
        {
            get
            {
                return _unitPrice * _orderQty;
            }

            set
            {
                _subTotal = value;
                Set<double?>(() => this.SubTotal, ref _subTotal, value);
            }
        }

        public int? StoreID
        {
            get
            {
                return _storeID;
            }

            set
            {
                _storeID = value;
                Set<int?>(() => this.StoreID, ref _storeID, value);
            }
        }

        public ProductODModel()
        {

        }
    }
}
