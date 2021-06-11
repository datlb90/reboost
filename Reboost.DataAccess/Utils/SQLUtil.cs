using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Reboost.DataAccess
{
    public static class SQLUtil
    {
        public static List<T> ExecuteQuery<T>(DbConnection con, string query, Func<DbDataReader, T> map)
        {
            using (var command = con.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                con.Open();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }
        }
    }
}
