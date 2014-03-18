using System.Collections.Generic;
using System.Data;
namespace Vido.Parking.Core.Interfaces
{
  public interface IDatabase
  {
    bool Open(string connectionString);
    void Close();
    DataTable ExecuteReader(string sql);
    int ExecuteNonQuery(string sql);
    string ExecuteScalar(string sql);

    bool Insert(string tableName, Dictionary<string, string> data);
    bool Update(string tableName, Dictionary<string, string> data, string where);
    bool Delete(string tableName, string where);

    bool ClearTable(string table);
    bool ClearDatabase();
  }
}
