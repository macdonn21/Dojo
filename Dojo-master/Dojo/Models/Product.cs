//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dojo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.ProductPayments = new HashSet<ProductPayment>();
        }
    
        public string Product_ID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string CostPrice { get; set; }
        public string SellingPrice { get; set; }
        public string QuantityInStock { get; set; }
        public string QuantitySold { get; set; }
        public string Profit { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPayment> ProductPayments { get; set; }
    }
}