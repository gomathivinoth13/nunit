using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SEG.LoyaltyDatabase.Core.Extensions
{
    public static class ExpressionExtensions
    {
        public static string GetExpressionValue(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    var add = expression as BinaryExpression;
                    return GetExpressionValue(add.Left) + " + " + GetExpressionValue(add.Right);
                case ExpressionType.AndAlso:
                    var andAlso = expression as BinaryExpression;
                    return GetExpressionValue(andAlso.Left) + " AND " + GetExpressionValue(andAlso.Right);
                case ExpressionType.OrElse:
                    var orElse = expression as BinaryExpression;
                    return GetExpressionValue(orElse.Left) + " OR " + GetExpressionValue(orElse.Right);
                case ExpressionType.GreaterThan:
                    var greaterThan = expression as BinaryExpression;
                    return GetExpressionValue(greaterThan.Left) + " > " + GetExpressionValue(greaterThan.Right);
                case ExpressionType.GreaterThanOrEqual:
                    var greaterThanEqualTo = expression as BinaryExpression;
                    return GetExpressionValue(greaterThanEqualTo.Left) + " >= " + GetExpressionValue(greaterThanEqualTo.Right);
                case ExpressionType.LessThan:
                    var lessThan = expression as BinaryExpression;
                    return GetExpressionValue(lessThan.Left) + " < " + GetExpressionValue(lessThan.Right);
                case ExpressionType.LessThanOrEqual:
                    var lessThanEqualTo = expression as BinaryExpression;
                    return GetExpressionValue(lessThanEqualTo.Left) + " <= " + GetExpressionValue(lessThanEqualTo.Right);
                case ExpressionType.IsFalse:
                    var isFalse = expression as BinaryExpression;
                    return GetExpressionValue(isFalse.Left) + " = 0";
                case ExpressionType.IsTrue:
                    var isTrue = expression as BinaryExpression;
                    return GetExpressionValue(isTrue.Left) + " = 1";
                case ExpressionType.Constant:
                    var constant = expression as ConstantExpression;
                    if (constant.Type == typeof(string) || constant.Type == typeof(Guid))
                    {
                        return "N'" + constant.Value.ToString().Replace("'", "''") + "'";
                    }
                    else if (constant.Type == typeof(bool))
                    {
                        return (bool)constant.Value ? "1" : "0";
                    }
                    else if (constant.Type.BaseType == typeof(Enum))
                    {
                        var enumVal = Enum.Parse(constant.Type, constant.Value.ToString(), true);
                        var enumInt = Convert.ToInt32(enumVal);
                        return enumInt.ToString();
                    }
                    return constant.Value.ToString();
                case ExpressionType.Equal:
                    var equal = expression as BinaryExpression;
                    return GetExpressionValue(equal.Left) + " = " + GetExpressionValue(equal.Right);
                case ExpressionType.Lambda:
                    var l = expression as LambdaExpression;
                    return GetExpressionValue(l.Body);
                case ExpressionType.MemberAccess:
                    var memberaccess = expression as MemberExpression;
                    return "[" + memberaccess.Member.Name + "]";
                case ExpressionType.Convert:
                    var operand = ((UnaryExpression)expression).Operand;
                    if (operand.NodeType == ExpressionType.Constant)
                    {
                        return GetExpressionValue(operand);
                    }
                    else
                    {
                        var memberExpr = operand as MemberExpression;
                        return GetExpressionValue(memberExpr);
                    }

            }

            throw new NotImplementedException(
              expression.GetType().ToString() + " " +
              expression.NodeType.ToString());
        }
    }

    public class Visitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression memberExpression)
        {
            // Recurse down to see if we can simplify...
            var expression = Visit(memberExpression.Expression);

            if (expression is ConstantExpression)
            {
                object container = ((ConstantExpression)expression).Value;
                var member = memberExpression.Member;
                if (member is FieldInfo fInfo)
                {
                    object value = fInfo.GetValue(container);
                    return Expression.Constant(value);
                }
                if (member is PropertyInfo pInfo)
                {
                    object value = pInfo.GetValue(container, null);
                    return Expression.Constant(value);
                }
            }
            return base.VisitMember(memberExpression);
        }
    }
}
