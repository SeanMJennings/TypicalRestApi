using BDD;
using Microsoft.Data.SqlClient;

namespace Integration.Db;

public class TruncateDbSpecification(string connectionString) : Specification
{
    protected override void after_each()
    {
        const string the_command = """
                                   EXEC sp_MSForEachTable @command1='TRUNCATE TABLE ?'
                                   , @whereand = 'AND o.name <> ''SchemaVersions'''
                                   """;
        using var connection = new SqlConnection(connectionString);
        var command = connection.CreateCommand();
        command.CommandText = the_command;
        connection.Open();
        command.ExecuteNonQuery();
    }
}