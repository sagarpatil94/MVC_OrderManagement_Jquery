using System.ComponentModel.DataAnnotations;

namespace TaskJayamTech.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }
        public int ItemId { get; set; } 

        [Required(ErrorMessage = "Order Date is required")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Order Number is required")]
        public string OrderNumber { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal GST { get; set; }
        public decimal LineTotal { get; set; }
        public decimal OrderTotal { get; set; }
        public string Remark { get; set; }
    }
}
