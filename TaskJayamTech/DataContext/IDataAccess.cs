using System.Data;

namespace TaskJayamTech.DataContext
{
    public interface IDataAccess
    {
        IDbConnection GetConnection { get; }    
    }
}