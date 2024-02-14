using Dapper;
using System.Data;
using System.Data.Common;
using TaskJayamTech.DataContext;
using TaskJayamTech.Models;

namespace TaskJayamTech.Repository.OrderService
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDataAccess _dataContext;
        public OrderRepository(IDataAccess dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddOrder(Order order)
        {
            // Define the parameterized INSERT query string
            string query = @"INSERT INTO [dbo].[Orders]
                    ([CustomerId], [OrderDate], [OrderNumber], [ItemId], [Quantity], [Total], [GST], [LineTotal], [OrderTotal], [Remark])
                    VALUES
                    (@CustomerId, @OrderDate, @OrderNumber, @ItemId, @Quantity, @Total, @GST, @LineTotal, @OrderTotal, @Remark)";

            using (IDbConnection _con = _dataContext.GetConnection)
            {   // Create a DynamicParameters object to store the parameter values
                var parameters = new DynamicParameters();

                // Add parameter values dynamically
                parameters.Add("@CustomerId", order.CustomerId);
                parameters.Add("@OrderDate", order.OrderDate);
                parameters.Add("@OrderNumber", order.OrderNumber);
                parameters.Add("@ItemId", order.ItemId);
                parameters.Add("@Quantity", order.Quantity);
                parameters.Add("@Total", order.Total);
                parameters.Add("@GST", order.GST);
                parameters.Add("@LineTotal", order.LineTotal);
                parameters.Add("@OrderTotal", order.OrderTotal);
                parameters.Add("@Remark", order.Remark);
                int afftetedRow = await _con.ExecuteAsync(query,parameters);
                return afftetedRow > 0 ? true : false;    
            }
        }

        public async Task<bool> DeleteOrder(int id)
        {
            using (IDbConnection _con = _dataContext.GetConnection)
            {
                string query = $"Delete [dbo].[Orders] where Id = @Id";
                int afftedRow = await _con.ExecuteAsync(query,new {Id=id});
                return afftedRow > 0 ? true : false;    
            }
        }

        public async Task<bool> EditOrder(Order order)
        {
            string query = $"UPDATE [dbo].[Orders] SET [CustomerId]=@CustomerId,[OrderDate]=@OrderDate,[ItemId] = @ItemId,[Quantity] = @Quantity,[Total] = @Total,[GST] = @GST,[LineTotal] = @LineTotal,[OrderTotal] = @OrderTotal,[Remark] = @Remark WHERE [Id]=@Id";

            using (IDbConnection _con = _dataContext.GetConnection)
            {   // Create a DynamicParameters object to store the parameter values
                var parameters = new DynamicParameters();

                // Add parameter values dynamically
                parameters.Add("@Id", order.Id);
                parameters.Add("@CustomerId", order.CustomerId);
                parameters.Add("@OrderDate", order.OrderDate);
                parameters.Add("@ItemId", order.ItemId);
                parameters.Add("@Quantity", order.Quantity);
                parameters.Add("@Total", order.Total);
                parameters.Add("@GST", order.GST);
                parameters.Add("@LineTotal", order.LineTotal);
                parameters.Add("@OrderTotal", order.OrderTotal);
                parameters.Add("@Remark", order.Remark);
                int afftetedRow = await _con.ExecuteAsync(query, parameters);
                return afftetedRow > 0 ? true : false;
            }
        }

        public async Task<Order> GetByIdOrder(int id)
        {
            string query = $"Select ord.Id,ord.[CustomerId],ord.[OrderDate],ord.[OrderNumber],itm.Price,ord.[ItemId],ord.[Quantity],ord.[Total],ord.[GST],ord.[LineTotal],ord.[OrderTotal],ord.[Remark] from [dbo].[Orders] as ord inner join [dbo].[Items] as itm on ord.itemid = itm.id where ord.id = @Id";
            Order order = new Order();
            using (IDbConnection _con = _dataContext.GetConnection)
            { // Create a DynamicParameters object to store the parameter values
               
                order = await _con.QuerySingleAsync<Order>(query, new {Id =id});
                return order;
            }
        }

        public async Task<MasterTableVM> GetMaster()
        {
            MasterTableVM result  = new MasterTableVM();
            string query = $" select [Id], [Name] from [dbo].[Customers] " +
                            " select [Id], [Name], [Description], [Price] from [dbo].[Items]";
            using (IDbConnection _con = _dataContext.GetConnection)
            {
                using (var multiResult =await _con.QueryMultipleAsync(query))
                {
                    var customer = await multiResult.ReadAsync<Customer>();
                    var item = await multiResult.ReadAsync<Item>();
                    result.customer = customer.ToList();
                    result.items = item.ToList();
                    return result;  
                }
            }
        }

        public async Task<IList<OrderVm>> GetOrder()
        {
            string query = $"select od.Id, c.Name ,ord.name as Item ,ord.price,od.OrderDate,od.OrderNumber,od.Remark,od.Ordertotal from Customers c inner join [dbo].[Orders] od on c.Id = od.CustomerId inner join [dbo].[Items] as ord on od.Id = ord.id";
            using (System.Data.IDbConnection _con = _dataContext.GetConnection)
            {
                try
                {
                    _con.Open();
                   var result = await _con.QueryAsync<OrderVm>(query);
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    _con.Close();
                    throw ex;
                }
               
                
            }
        }
    }
}
