namespace OrderService.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
