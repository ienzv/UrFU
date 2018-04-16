using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SystemOfSymbolicMathematics
{
    public class Binder
    {
        public void RegisterParameter(ParameterExpression parameter)
        {
            parameters.Add(parameter.Name, parameter);
        }

        ParameterExpression ResolveParameter(string parameterName)
        {
            if (parameters.TryGetValue(parameterName, out ParameterExpression parameter))
            {
                return parameter;
            }
            return null;
        }

        Dictionary<string, ParameterExpression> parameters
            = new Dictionary<string, ParameterExpression>();

        public Expression Resolve(string identifier)
        {
            return ResolveParameter(identifier);
        }

        public MethodInfo ResolveMethod(string functionName)
        {
            foreach (var methodInfo in typeof(Math).GetMethods())
            {
                if (methodInfo.Name.Equals(functionName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return methodInfo;
                }
            }
            return null;
        }
    }
}
