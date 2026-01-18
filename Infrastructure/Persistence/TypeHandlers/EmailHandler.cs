using System.Data;
using Dapper;
using Domain.Primitives;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence.TypeHandlers;

public class EmailHandler : SqlMapper.TypeHandler<Email>
{
    public static readonly EmailHandler Default = new();

    private EmailHandler()
    {
    }

    public override void SetValue(IDbDataParameter parameter, Email value)
    {
        parameter.Value = value.ToString();

        if (parameter is SqlParameter sqlParameter)
            sqlParameter.SqlDbType = SqlDbType.Text;
    }

    public override Email Parse(object value)
    {
        return value is string email
            ? new Email(email)
            : throw new ArgumentException($"Cannot convert {value.GetType()} to {typeof(Email)}");
    }
}
