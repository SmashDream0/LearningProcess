using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LearningProcess.Specification.Operand;

namespace LearningProcess.Specification
{
    public class Operation<T> : AData
    {
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// <param name="operandA">Операнд A</param>
        /// <param name="condition">Условие</param>
        /// <param name="operandB">Операнд B</param>
        public Operation(AData operandA, EOperation condition, AData operandB)
        {
            _operandA = operandA;
            _operation = condition;
            _operandB = operandB;
        }

        /// <summary>
        /// Конструктор 2
        /// </summary>
        /// <param name="field">Коле</param>
        /// <param name="condition">Условие</param>
        /// <param name="value">Значение</param>
        public Operation(Expression<Func<T, object>> field, EOperation condition, object value)
            : this(new Column<T>(field), condition, new Value(value))
        { }

        private readonly AData _operandA;
        private readonly EOperation _operation;
        private readonly AData _operandB;

        public override string Data => $"{GetOperandA()}{GetOperation()}{GetOperandB()}";

        private string GetOperandA()
        { return _operandA.Data; }
        private string GetOperandB()
        {
            if (_operation == EOperation.Like)
            { return $"%{_operandB.Data}%"; }
            else
            { return _operandB.Data; }
        }
        private string GetOperation()
        {
            switch (_operation)
            {
                case EOperation.Equal:
                    return "=";
                case EOperation.Less:
                    return "<";
                case EOperation.More:
                    return ">";
                case EOperation.Like:
                    return "LIKE";
                default:
                    throw new Exception($"Unknown operation: {_operation}");
            }
        }
    }

    public enum EOperation
    {
        /// <summary>
        /// Равно
        /// </summary>
        Equal,
        /// <summary>
        /// Больше
        /// </summary>
        More,
        /// <summary>
        /// Меньше
        /// </summary>
        Less,
        /// <summary>
        /// Эквивалент Like '%text%'
        /// </summary>
        Like
    };
}