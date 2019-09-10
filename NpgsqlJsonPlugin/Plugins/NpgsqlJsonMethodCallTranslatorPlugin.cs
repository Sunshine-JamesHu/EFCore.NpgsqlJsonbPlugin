using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ronds.Epm.EntityFrameworkCore.NpgsqlJsonPlugin.Plugins
{
    internal class NpgsqlJsonMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
    {
        public NpgsqlJsonMethodCallTranslatorPlugin()
        {

        }
        public virtual IEnumerable<IMethodCallTranslator> Translators { get; } = new IMethodCallTranslator[]
        {
            new JObjectMethodCallTranslator()
        };
    }
    internal class JObjectMethodCallTranslator : IMethodCallTranslator
    {
        public Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.DeclaringType == typeof(Newtonsoft.Json.Linq.Extensions) &&
                methodCallExpression.Method.Name == nameof(Newtonsoft.Json.Linq.JToken.Value) &&
                methodCallExpression.Method.IsGenericMethod &&
                methodCallExpression.Method.GetParameters()[0].ParameterType == typeof(string))
            {
                return new CustomUnaryExpression(methodCallExpression.Arguments.First(), "->>0", typeof(string), postfix: true);
            }

            if ((methodCallExpression.Method.DeclaringType == typeof(Newtonsoft.Json.Linq.JObject) ||
                (methodCallExpression.Method.DeclaringType == typeof(Newtonsoft.Json.Linq.JToken))) &&
                methodCallExpression.Method.Name == "get_Item" &&
                methodCallExpression.Arguments.First().Type == typeof(string) &&
                methodCallExpression.Object != null)
            {
                return new CustomBinaryExpression(methodCallExpression.Object, methodCallExpression.Arguments.First(), "->>", typeof(Newtonsoft.Json.Linq.JToken));
            }

            return null;
        }
    }
}
