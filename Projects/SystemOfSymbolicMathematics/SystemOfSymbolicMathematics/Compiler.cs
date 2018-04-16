using System;
using System.Linq.Expressions;
using Irony.Parsing;

namespace SystemOfSymbolicMathematics
{
    public class Compiler
    {
        public static Func<double, double> CompileFunction(string functionText)
        {
            ParseTree ast = ParserInstance.Parse(functionText);
            ExpressionTreeBuilder builder = new ExpressionTreeBuilder();
            Expression<Func<double, double>> expression = builder.CreateFunction(ast.Root);
            Func<double, double> function = expression.Compile();
            return function;
        }

        static Parser ParserInstance = new Parser(ExpressionGrammar.Instance);
    }
}
