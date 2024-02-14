namespace TaskJayamTech.Models
{
    public class OrderVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public string Remark { get; set; }
        public decimal Ordertotal { get; set; } 
    }
}
