using Irony.Parsing;

namespace SystemOfSymbolicMathematics
{
    [Language("Expression")]
    public class ExpressionGrammar : Grammar
    {
        public ExpressionGrammar()
        {
            // Terminals
            var number = new NumberLiteral("Number");
            var identifier = new IdentifierTerminal("Identifier");

            // Non-terminals
            var expression = new NonTerminal("Expression");
            var terminal = new NonTerminal("Terminal");
            var binaryExpression = new NonTerminal("BinaryExpression");
            var parenthesisExpression = new NonTerminal("ParentthesisExpression");
            var unaryExpression = new NonTerminal("UnaryExpression");
            var unaryOperator = new NonTerminal("UnaryOperator");
            var binaryOperator = new NonTerminal("BinaryOperator", "operator");
            var propertyAccess = new NonTerminal("PropertyAccess");
            var functionCall = new NonTerminal("FunctionCall");
            var commandSeparatedIdentifierList = new NonTerminal("PointArgumentList");
            var argumentList = new NonTerminal("ArgumentList");

            // BNF rules
            expression.Rule = terminal | unaryExpression | binaryExpression;
            terminal.Rule = number | identifier | parenthesisExpression | functionCall |
                propertyAccess;
            unaryExpression.Rule = unaryOperator + terminal;
            unaryOperator.Rule = ToTerm("-");
            binaryExpression.Rule = expression + binaryOperator + expression;
            binaryOperator.Rule = ToTerm("+") | "-" | "*" | "/" | "^";
            propertyAccess.Rule = identifier + "." + identifier;
            functionCall.Rule = identifier + "(" + argumentList + ")";
            argumentList.Rule = expression | commandSeparatedIdentifierList;
            parenthesisExpression.Rule = "(" + expression + ")";
            commandSeparatedIdentifierList.Rule = MakePlusRule(commandSeparatedIdentifierList, ToTerm(","),
                identifier);
            Root = expression;

            RegisterOperators(1, "+", "-");
            RegisterOperators(2, "*", "/");
            RegisterOperators(3, Associativity.Right, "^");

            MarkPunctuation("(", ")", ".", ",");
            MarkTransient(terminal, expression, binaryOperator, unaryOperator,
                parenthesisExpression, argumentList, commandSeparatedIdentifierList);
        }

        public static ExpressionGrammar Instance = new ExpressionGrammar();
    }
}
