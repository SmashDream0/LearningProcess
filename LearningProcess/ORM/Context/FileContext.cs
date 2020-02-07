using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LearningProcess.Repository;
using LearningProcess.ORM;
using LearningProcess.ORM.Table;
using Dapper.Contrib.Extensions;
using System.IO;

namespace LearningProcess.ORM.Context
{
    public class FileContext : BaseContext
    {
        public FileContext(string dataBaseName)
            : base(EContextType.File)
        {
            _dbName = $@"Data Source={dataBaseName};Version=3";

            CreateConnection();

            CheckTables();
        }

        private readonly string _dbName;

        private IDbConnection _singleConnection;

        /// <summary>
        /// Исполнить запрос и получить ответ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IEnumerable<T> Query<T>(ORM.Specification.BaseSpecification specification)
        { return _singleConnection.Query<T>(specification.SelectQuery); }

        /// <summary>
        /// Внести значения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public override void Insert<T>(T[] entities)
        {
            foreach (var entity in entities)
            { _singleConnection.Insert(entity); }
        }

        /// <summary>
        /// Удалить значения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public override void Delete(ORM.Specification.BaseSpecification specification)
        { _singleConnection.Execute(specification.DeleteQuery); }

        public override void Update<T>(T entity)
        {
            var result = _singleConnection.Update(entity);
        }

        private void CreateConnection()
        {
            var connection = new SQLiteConnection(_dbName);
            connection.Open();

            _singleConnection = connection;
        }

        private void CheckTables()
        {
            foreach (var table in Tables)
            {
                var checkStr = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{table.NameDB}';";

                var command = _singleConnection.CreateCommand();
                command.CommandText = checkStr;

                bool isExist = false;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var name = reader.GetString(0);

                        isExist = true;
                    }
                }

                if (!isExist)
                { AddTable(_singleConnection, table); }
            }
        }

        private static string FormatValue(object value, Type tp)
        {
            if (value == null)
            {
                if (tp == typeof(string))
                { return "''"; }
                else if (tp == typeof(byte[]))
                { return "x''"; }
                else if (tp.IsGenericType && tp.GetGenericTypeDefinition() == typeof(Nullable<>))
                { return "NULL"; }
                else
                { return FormatValue(Activator.CreateInstance(tp), tp); }
            }
            else
            {
                if (typeof(string) == tp)
                { return OperateString(value.ToString()); }
                else if (tp == typeof(byte[]))
                { return OperateByteArray((byte[])value); }
                else if (typeof(int) == tp || typeof(long) == tp || typeof(int?) == tp || typeof(long?) == tp ||
                         typeof(uint) == tp || typeof(ulong) == tp || typeof(uint?) == tp || typeof(ulong?) == tp ||
                         typeof(sbyte) == tp || typeof(sbyte?) == tp ||
                         typeof(byte) == tp || typeof(byte?) == tp ||
                         typeof(double) == tp || typeof(float) == tp || typeof(double?) == tp || typeof(float?) == tp ||
                         typeof(decimal) == tp || typeof(decimal?) == tp)
                { return value.ToString().Replace(',', '.'); }
                else if (typeof(DateTime) == tp)
                { return $"'{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}'"; }
                else if (typeof(DateTime?) == tp)
                { return $"'{((DateTime?)value).Value.ToString("yyyy-MM-dd HH:mm:ss")}'"; }
                else if (typeof(bool) == tp)
                { return ((bool)value ? "1" : "0"); }
                else
                { throw new Exception($"Uncompatible type: {tp}"); }
            }
        }

        private static string OperateString(string value)
        {
            var NewResult = value.ToCharArray();
            char Temp;
            int NewLength = value.Length;

            Array.Resize(ref NewResult, NewResult.Length * 2 + 2);
            Array.Copy(NewResult, 0, NewResult, 1, NewLength);//

            NewResult[0] = '\'';
            NewLength++;

            for (int i = 1; i < NewLength; i++)
            {
                Temp = NewResult[i];

                switch (Temp)
                {
                    case '\'':
                    case '`':
                    case '\"':
                    case '\\':
                    case '|':
                    case ':':
                    case ';':
                    case '?':
                    case '!':
                    case '=':
                    case '&':
                        Array.Copy(NewResult, i, NewResult, i + 1, NewLength - i);//перемещаю остаток массива

                        NewResult[i++] = '\\';//пишу экранирующий символ
                        NewLength++;    //инкрементирую максимальный индекс
                        break;
                        //case '№':
                        //    if (encode == Encoding.GetEncoding(866))
                        //    { NewResult[i] = 'ⁿ'; }
                        break;
                }
            }

            //Array.Resize(ref NewResult, NewLength + 1);
            NewResult[NewLength] = '\'';

            return new string(NewResult, 0, NewLength + 1);
        }

        private static string OperateByteArray(byte[] Bytes)
        {
            var result = new StringBuilder(Bytes.Length * 2);
            result.Append("x'");

            {
                const string hexAlphabet = "0123456789ABCDEF";

                foreach (byte B in Bytes)
                {
                    result.Append(hexAlphabet[(int)(B >> 4)]);
                    result.Append(hexAlphabet[(int)(B & 0xF)]);
                }
            }

            result.Append('\'');

            return result.ToString();
        }

        private static void AddTable(IDbConnection inMemoryConnection, Table.Table table)
        {
            var sb = new StringBuilder();

            sb.Append("CREATE TABLE["); sb.Append(table.NameDB); sb.Append("](");

            Column primaryColumn = null;

            foreach (var column in table.Columns)
            {
                sb.Append('['); sb.Append(column.NameDB); sb.Append(']');

                var typeName = GetSingleColumnType(column);

                sb.Append(typeName);

                if (column.Length > 0)
                { sb.Append('('); sb.Append(column.Length); sb.Append(')'); }

                sb.Append(',');

                if (column.IsPrimary)
                { primaryColumn = column; }
            }

            sb.Length--;

            if (primaryColumn != null)
            { }

            sb.Append(')');

            var command = inMemoryConnection.CreateCommand();
            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        private static string GetSingleColumnType(Column column)
        {
            string typeName;
            if (typeof(string) == column.Type)
            { typeName = "VARCHAR"; }
            else if (typeof(byte[]) == column.Type)
            { typeName = "BLOB"; }
            else if (typeof(int) == column.Type || typeof(long) == column.Type || typeof(int?) == column.Type || typeof(long?) == column.Type)
            { typeName = "INTEGER"; }
            else if (typeof(uint) == column.Type || typeof(ulong) == column.Type || typeof(uint?) == column.Type || typeof(ulong?) == column.Type)
            { typeName = "INTEGER UNSIGNED"; }
            else if (typeof(sbyte) == column.Type || typeof(sbyte?) == column.Type)
            { typeName = "TINYINT"; }
            else if (typeof(byte) == column.Type || typeof(byte?) == column.Type)
            { typeName = "TINYINT UNSIGNED"; }
            else if (typeof(double) == column.Type || typeof(float) == column.Type || typeof(double?) == column.Type || typeof(float?) == column.Type)
            { typeName = "DOUBLE"; }
            else if (typeof(decimal) == column.Type || typeof(decimal?) == column.Type)
            { typeName = "DECIMAL"; }
            else if (typeof(DateTime) == column.Type || typeof(DateTime?) == column.Type)
            { typeName = "DATETIME"; }
            else if (typeof(bool) == column.Type || typeof(bool?) == column.Type)
            { typeName = "TINYINT"; }
            else if (typeof(Enum).IsAssignableFrom(column.Type))
            { typeName = "TINYINT"; }
            else
            { throw new Exception($"Uncompatible type: {column.Type}"); }

            if (column.IsPrimary)
            { typeName += " PRIMARY KEY"; }

            return typeName;
        }
    }
}