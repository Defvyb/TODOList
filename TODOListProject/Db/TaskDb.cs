using Npgsql;

namespace TODOListProject;
/*
 * Это самопальный класс для работы с БД.
 * без EF фреймворка, просто с помощью Npgsql
 * 
 */
public class TaskDb: ITaskDb
{
    private readonly string _tableName = "Empty";
    NpgsqlConnection _connection;
    //create constructor with npgsql connection
    public TaskDb(NpgsqlConnection connection, string tableName)
    {
        _tableName = tableName;
        _connection = connection;
    }
    
    public bool Add(int id, string name)
    {
        _connection.Open();
        try
        {
            var sql = $"INSERT INTO schema.{_tableName} VALUES ({id}, '{name}')";
            using (var command = new NpgsqlCommand(sql, _connection))
            {
                var result = command.ExecuteNonQuery();
                return result == 1;
            }
        }
        finally
        {
            _connection.Close();
        }
    }
    
    public bool Delete(int id)
    {
        _connection.Open();
        try
        {
            var sql = $"DELETE FROM schema.{_tableName} WHERE id = {id}";
            using (var command = new NpgsqlCommand(sql, _connection))
            {
                var result = command.ExecuteNonQuery();
                return result == 1;
            }
        }
        finally
        {
            _connection.Close();
        }
    }
    
    public Dictionary<int, string> GetList()
    {
        _connection.Open();
        try
        {
            var sql = $"SELECT * FROM schema.{_tableName}";
            using (var command = new NpgsqlCommand(sql, _connection))
            using (var reader = command.ExecuteReader())
            {
                var dictionary = new Dictionary<int, string>();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    dictionary.Add(id, name);
                }
                return dictionary;
            }
        }
        finally
        {
            _connection.Close();
        }
    }
}