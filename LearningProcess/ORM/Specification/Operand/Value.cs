using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningProcess.ORM.Specification.Operand
{
    public class Value : BaseData
    {
        public Value(object data)
        { _data = ConvertData(data); }

        private string _data;

        public override string Data => _data;

        private static string ConvertData(object data)
        {
            const string empty = "NULL";

            if (data == null)
            { return empty; }

            var valueType = data.GetType();

            var typeCode = Type.GetTypeCode(valueType);

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    if ((bool)data)
                    { return "1"; }
                    else
                    { return "0"; }
                case TypeCode.DateTime:

                    return ((DateTime)data).ToString("yyyy-MM-dd");

                case TypeCode.Char:
                case TypeCode.String:

                    return $"'{data}'";

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:

                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:

                    return data.ToString();

                case TypeCode.Decimal:
                    return ((decimal)data).ToString();
                case TypeCode.Double:
                    return ((double)data).ToString();
                case TypeCode.Single:
                    return ((float)data).ToString();
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return empty;
                default:
                    if (valueType.Equals(typeof(Guid)))
                    { return $"'{data}'"; }
                    throw new Exception($"Unknown type: {data.GetType()}");
            }
        }
    }
}
