using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LearningProcess.Repository;
using Z.Dapper.Plus;

namespace LearningProcess.Context
{
    public class SQLLiteContext : BaseContext
    {
        public SQLLiteContext()
            : base(IContextType.SqlLite)
        {
            CreateConnection();

            var command = _inMemoryConnection.CreateCommand();
            command.CommandText = _startQuerry;
            command.ExecuteNonQuery();
        }

        private IDbConnection _inMemoryConnection;

        private string _dbConnectionString => $"Data Source=:memory:";

        private string _startQuerry = $"CREATE TABLE [Person](" +
                                       "GUID VARCHAR(30)," +
                                       "NAME VARCHAR(30)," +
                                       "BIRTHDAY DATE)";

        private void CreateConnection()
        {
            try
            {
                var connection = new SQLiteConnection(_dbConnectionString);
                connection.Open();

                _inMemoryConnection = connection;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Исполнить запрос и получить ответ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IEnumerable<T> Query<T>(Specification.Specification specification)
        { return _inMemoryConnection.Query<T>(specification.SelectQuery); }

        /// <summary>
        /// Внести значения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public override void Insert<T>(T[] entities)
        { _inMemoryConnection.BulkInsert(entities); }

        /// <summary>
        /// Удалить значения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public override void Delete(Specification.Specification specification)
        { _inMemoryConnection.Execute(specification.DeleteQuery); }
    }
}