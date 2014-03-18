namespace Vido.Parking.Core
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SQLite;
  using System.Text;
  using Vido.Parking.Core.Interfaces;

  public class SQLiteDatabase : IDatabase
  {
    private string connectionString;

    /// <summary>
    ///   Default Constructor for SQLiteDatabase Class.
    /// </summary>
    public SQLiteDatabase()
    {
      connectionString = "Data Source=recipes.s3db";
    }

    /// <summary>
    ///   Single Param Constructor for specifying the DB file.
    /// </summary>
    /// <param name="inputFile">The File containing the DB</param>
    public SQLiteDatabase(string inputFile)
    {
      connectionString = string.Format("Data Source={0}", inputFile);
    }

    /// <summary>
    ///   Single Param Constructor for specifying advanced connection options.
    /// </summary>
    /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
    public SQLiteDatabase(Dictionary<string, string> connectionOpts)
    {
      StringBuilder sb = new StringBuilder();
      foreach (KeyValuePair<string, string> row in connectionOpts)
      {
        sb.AppendFormat("{0}={1}; ", row.Key, row.Value);
      }

      connectionString = sb.ToString().Trim();
    }

    public bool Open(string connectionString)
    {
      this.connectionString = connectionString;
      return (true);
    }
    public void Close()
    {
    }

    /// <summary>
    ///  Run a query against the Database.
    /// </summary>
    /// <param name="sql">The SQL to run</param>
    /// <returns>A DataTable containing the result set.</returns>
    public DataTable ExecuteReader(string sql)
    {
      SQLiteConnection connection = new SQLiteConnection(connectionString);
      try
      {
        connection.Open();

        var reader = new SQLiteCommand(connection)
        {
          CommandText = sql
        }.ExecuteReader();

        var dataTable = new DataTable();
        dataTable.Load(reader);

        reader.Close();

        return (dataTable);
      }
      catch (Exception e)
      {
        throw new Exception(e.Message);
      }
      finally
      {
        connection.Close();
      }
    }

    /// <summary>
    ///  Interact with the database for purposes other than a query.
    /// </summary>
    /// <param name="sql">The SQL to be run.</param>
    /// <returns>An Integer containing the number of rows updated.</returns>
    public int ExecuteNonQuery(string sql)
    {
      SQLiteConnection connection = new SQLiteConnection(connectionString);
      try
      {
        connection.Open();

        return (new SQLiteCommand(connection)
        {
          CommandText = sql
        }.ExecuteNonQuery());
      }
      catch
      {
        return (0);
      }
      finally
      {
        connection.Close();
      }
    }

    /// <summary>
    /// Retrieve single items from the DB.
    /// </summary>
    /// <param name="sql">The query to run.</param>
    /// <returns>A string.</returns>
    public string ExecuteScalar(string sql)
    {
      SQLiteConnection connection = new SQLiteConnection(connectionString);
      try
      {
        connection.Open();

        var value = new SQLiteCommand(connection)
        {
          CommandText = sql
        }.ExecuteScalar();

        if (value != null)
        {
          return (value.ToString());
        }

        return (string.Empty);
      }
      catch
      {
        return (string.Empty);
      }
      finally
      {
        connection.Close();
      }
    }

    /// <summary>
    ///  Update rows in the DB.
    /// </summary>
    /// <param name="tableName">The table to update.</param>
    /// <param name="data">A dictionary containing Column names and their new values.</param>
    /// <param name="where">The where clause for the update statement.</param>
    /// <returns>A boolean true or false to signify success or failure.</returns>
    public bool Update(string tableName, Dictionary<string, string> data, string where)
    {
      try
      {
        var sb = new StringBuilder();
        if (data.Count >= 1)
        {
          foreach (var val in data)
          {
            sb.AppendFormat(" {0} = '{1}',", val.Key, val.Value);
          }

          if (sb.Length > 0) --sb.Length; // remove ','
        }

        this.ExecuteNonQuery(string.Format("update {0} set {1} where {2};", tableName, sb.ToString(), where));
      }
      catch
      {
        return (false);
      }

      return (true);
    }

    /// <summary>
    ///   Delete rows from the DB.
    /// </summary>
    /// <param name="tableName">The table from which to delete.</param>
    /// <param name="where">The where clause for the delete.</param>
    /// <returns>A boolean true or false to signify success or failure.</returns>
    public bool Delete(string tableName, string where)
    {
      try
      {
        this.ExecuteNonQuery(string.Format("delete from {0} where {1};", tableName, where));
        return (true);
      }
      catch
      {
        return (false);
      }
    }

    /// <summary>
    ///   Insert into the DB
    /// </summary>
    /// <param name="tableName">The table into which we insert the data.</param>
    /// <param name="data">A dictionary containing the column names and data for the insert.</param>
    /// <returns>A boolean true or false to signify success or failure.</returns>
    public bool Insert(string tableName, Dictionary<string, string> data)
    {
      try
      {
        var sbCol = new StringBuilder();
        var sbVal = new StringBuilder();
        foreach (var val in data)
        {
          sbCol.AppendFormat(" {0},", val.Key);
          sbVal.AppendFormat(" '{0}',", val.Value);
        }

        if (sbCol.Length > 0) --sbCol.Length; // remove ','
        if (sbVal.Length > 0) --sbVal.Length; // remove ','

        this.ExecuteNonQuery(string.Format("insert into {0}({1}) values({2});",
          tableName, sbCol.ToString(), sbVal.ToString()));

        return (true);
      }
      catch
      {
        return (false);
      }

    }

    /// <summary>
    ///   Delete all data from the DB.
    /// </summary>
    /// <returns>A boolean true or false to signify success or failure.</returns>
    public bool ClearDatabase()
    {
      try
      {
        DataTable tables = this.ExecuteReader("select NAME from SQLITE_MASTER where type='table' order by NAME;");
        foreach (DataRow table in tables.Rows)
        {
          this.ClearTable(table["NAME"].ToString());
        }

        return (true);
      }
      catch
      {
        return (false);
      }
    }

    /// <summary>
    ///   Clear all data from a specific table.
    /// </summary>
    /// <param name="table">The name of the table to clear.</param>
    /// <returns>A boolean true or false to signify success or failure.</returns>
    public bool ClearTable(string table)
    {
      try
      {
        this.ExecuteNonQuery(string.Format("delete from {0};", table));
        return (true);
      }
      catch
      {
        return (false);
      }
    }

  }
}
