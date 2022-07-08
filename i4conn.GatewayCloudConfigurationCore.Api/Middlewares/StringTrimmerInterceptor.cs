using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Data.Entity.Infrastructure.Interception;

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Interceptors
{
    public class StringTrimmerInterceptor : Microsoft.EntityFrameworkCore.Diagnostics.DbCommandInterceptor
    {
        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace == DataSpace.SSpace)
            {
                var queryCommand = interceptionContext.Result as DbQueryCommandTree;
                if (queryCommand != null)
                {
                    var newQuery = queryCommand.Query.Accept(new StringTrimmerQueryVisitor());
                    interceptionContext.Result = new DbQueryCommandTree(
                        queryCommand.MetadataWorkspace,
                        queryCommand.DataSpace,
                        newQuery);
                }
            }
        }

        private class StringTrimmerQueryVisitor : DefaultExpressionVisitor
        {
            private static readonly string[] _typesToTrim = { "nvarchar", "varchar", "char", "nchar" };

            public override DbExpression Visit(DbNewInstanceExpression expression)
            {
                var arguments = expression.Arguments.Select(a =>
                {
                    var propertyArg = a as DbPropertyExpression;
                    if (propertyArg != null && _typesToTrim.Contains(propertyArg.Property.TypeUsage.EdmType.Name))
                    {
                        return EdmFunctions.TrimEnd(a);
                    }

                    return a;
                });

                return DbExpressionBuilder.New(expression.ResultType, arguments);
            }
        }
    }
}
