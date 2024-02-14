using TaskJayamTech.Models;

namespace TaskJayamTech.Repository.OrderService
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(Order order);
        Task<IList<OrderVm>> GetOrder();
        Task<MasterTableVM> GetMaster();  
        Task<bool> DeleteOrder(int id);
        Task<bool> EditOrder(Order order);
        Task<Order> GetByIdOrder(int id);
     
    }
}