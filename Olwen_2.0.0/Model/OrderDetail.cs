//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Olwen_2._0._0.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public Nullable<int> ProductId { get; set; }
        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public Nullable<int> OrderQty { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<int> StoreID { get; set; }
    
        public virtual OrderHeader OrderHeader { get; set; }
        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
