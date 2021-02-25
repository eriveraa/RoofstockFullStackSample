using System;
using System.ComponentModel.DataAnnotations;

namespace RoofstockFullStackSample.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int YearBuilt { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")] 
        public decimal ListPrice { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C}")] 
        public decimal MonthlyRent { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal GrossYield { 
            get
            {
                try
                {
                    return MonthlyRent * 12 / ListPrice;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            protected set { }
        }
    }
}
