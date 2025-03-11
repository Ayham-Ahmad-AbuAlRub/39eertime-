using System.ComponentModel.DataAnnotations;

namespace _39eertime_.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Distributor { get; set; }
        public int Quantity_Available { get; set; }
        public int Monthly_Amount { get; set; }

        public int Quantity_Required { get; private set; } // Read-only property

        // Method to calculate Quantity_Required
        public void CalculateQuantityRequired()
        {
            Quantity_Required = Monthly_Amount - Quantity_Available;
        }
    }
}