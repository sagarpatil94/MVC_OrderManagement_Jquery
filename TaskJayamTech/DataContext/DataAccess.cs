using Microsoft.Data.SqlClient;
using System.Data;

namespace TaskJayamTech.DataContext
{
    public class DataAccess: IDataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;
        public DataAccess(IConfiguration configuration)
        {
            _configuration = configuration; 
        }
        public IDbConnection GetConnection { get
            {
                return  new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
        public void CloseConnection()
        {

        }
    }
}
