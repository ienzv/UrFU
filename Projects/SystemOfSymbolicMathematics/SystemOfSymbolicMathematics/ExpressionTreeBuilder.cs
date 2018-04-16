using System;
using System.Linq.Expressions;
using System.Reflection;
using Irony.Parsing;

namespace SystemOfSymbolicMathematics
{
    public class ExpressionTreeBuilder
    {
        public ExpressionTreeBuilder()
        {
            Binder = new Binder();
        }

        public Binder Binder { get; set; }

        public Expression<Func<double, double>> CreateFunction(ParseTreeNode root)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(double), "x");
            Binder.RegisterParameter(parameter);
            Expression body = CreateExpression(root);
            var result = Expression.Lambda<Func<double, double>>(body, parameter);
            return result;
        }

        Expression CreateExpression(ParseTreeNode root)
        {
            if (root.Term.Name == "BinaryExpression")
            {
                return CreateBinaryExpression(root);
            }

            if (root.Term.Name == "Identifier")
            {
                return Binder.Resolve(root.Token.Text);
            }

            if (root.Term.Name == "Number")
            {
                return CreateLiteralExpression(Convert.ToDouble(root.Token.Value));
            }

            if (root.Term.Name == "FunctionCall")
            {
                return CreateCallExpression(root);
            }

            return null;
        }

        Expression CreateCallExpression(ParseTreeNode root)
        {
            string functionName = root.ChildNodes[0].Token.Text;
            Expression argument = CreateExpression(root.ChildNodes[1]);
            MethodInfo method = Binder.ResolveMethod(functionName);
            return Expression.Call(method, argument);
        }

        Expression CreateLiteralExpression(double arg)
        {
            return Expression.Constant(arg);
        }

        Expression CreateBinaryExpression(ParseTreeNode node)
        {
            Expression left = CreateExpression(node.ChildNodes[0]);
            Expression right = CreateExpression(node.ChildNodes[2]);

            switch (node.ChildNodes[1].Term.Name)
            {
                case "+":
                    return Expression.Add(left, right);
                case "-":
                    return Expression.Subtract(left, right);
                case "*":
                    return Expression.Multiply(left, right);
                case "/":
                    return Expression.Divide(left, right);
                case "^":
                    return Expression.Power(left, right);
            }
            return null;
        }
    }
}
