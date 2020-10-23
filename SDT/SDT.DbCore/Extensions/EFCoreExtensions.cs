using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SDT.DbCore
{
    public static class EFCoreExtensions
    {
        #region Common
        public static Func<QueryContext, TResult> SelfCompile<TResult>(Expression expression, QueryCompilationContext queryCompilationContext)
        {
            var expression1 = Expression.Lambda<Func<QueryContext, TResult>>(expression, new ParameterExpression[1]
            {
            QueryCompilationContext.QueryContextParameter
            });
            try
            {
                return expression1.Compile();
            }
            finally
            {
                CoreLoggerExtensions.QueryExecutionPlanned(queryCompilationContext.Logger, new ExpressionPrinter(), expression1);
            }
        }
        #endregion

        #region Command
        public static IRelationalCommand CreateCommand(this IQueryable query, out RelationalQueryContext queryContext)
        {
            var exp = query.ExpressionEx(out queryContext, out var sqlGenerator, out var _);
            return sqlGenerator.GetCommand(exp);
        }

        public static IRelationalCommand CreateCommand(this IQueryable query, Action<IQueryable> action, out RelationalQueryContext queryContext, out object compiledQuery)
        {
            action(query);
            var exp = query.ExpressionEx(out queryContext, out var sqlGenerator, out compiledQuery);
            return sqlGenerator.GetCommand(exp);
        }

        public static IRelationalCommand CreateCommand(this QuerySqlGenerator querySqlGenerator, SelectExpression selectExpression) => querySqlGenerator.GetCommand(selectExpression);

        public static SelectExpression ExpressionEx(this IQueryable query, out RelationalQueryContext queryContext, out QuerySqlGenerator sqlGenerator, out object compiledQuery)
        {
            var queryCompiler = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(query.Provider);

            var database = (RelationalDatabase)queryCompiler.GetType().GetField("_database", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(queryCompiler);

            var queryContextFactory = (IQueryContextFactory)(object)(IQueryContextFactory)queryCompiler.GetType().GetField("_queryContextFactory", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(queryCompiler);

            queryContext = (RelationalQueryContext)queryContextFactory.Create();

            var evaluatableExpressionFilter = (IEvaluatableExpressionFilter)typeof(QueryCompiler).GetField("_evaluatableExpressionFilter", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(queryCompiler);

            var databaseDependencies = (DatabaseDependencies)typeof(Database).GetProperty("Dependencies", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(database);

            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var expression = new ParameterExtractingExpressionVisitor(evaluatableExpressionFilter, queryContext, queryContext.GetType(), queryCompilationContext.Model, queryCompilationContext.Logger, true, false).ExtractParameters(query.Expression);

            var expression1 = expression;

            var queryCompilationContext1 = databaseDependencies.QueryCompilationContextFactory.Create(false);

            var queryTranslationPreprocessorFactory = (IQueryTranslationPreprocessorFactory)queryCompilationContext1.GetType().GetField("_queryTranslationPreprocessorFactory", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(queryCompilationContext1);

            var queryableMethodTranslating = (IQueryableMethodTranslatingExpressionVisitorFactory)queryCompilationContext1.GetType().GetField("_queryableMethodTranslatingExpressionVisitorFactory", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(queryCompilationContext1);

            var queryTranslationPostprocessorFactory = (IQueryTranslationPostprocessorFactory)queryCompilationContext1.GetType().GetField("_queryTranslationPostprocessorFactory", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(queryCompilationContext1);

            var shapedQueryCompiling = (IShapedQueryCompilingExpressionVisitorFactory)queryCompilationContext1.GetType().GetField("_shapedQueryCompilingExpressionVisitorFactory", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(queryCompilationContext1);

            var method = queryCompilationContext1.GetType().GetMethod("InsertRuntimeParameters", BindingFlags.Instance | BindingFlags.NonPublic);


            expression1 = queryTranslationPreprocessorFactory.Create(queryCompilationContext1).Process(expression1);

            expression1 = queryableMethodTranslating.Create(queryCompilationContext1.Model).Visit(expression1);

            var shapedQueryExpression = (ShapedQueryExpression)expression1;

            if (shapedQueryExpression.ResultCardinality != 0)
            {
                shapedQueryExpression.ResultCardinality = 0;
            }

            expression1 = queryTranslationPostprocessorFactory.Create(queryCompilationContext1).Process(expression1);

            expression1 = shapedQueryCompiling.Create(queryCompilationContext1).Visit(expression1);

            expression1 = (Expression)method.Invoke(queryCompilationContext1, new object[1]
            {
                expression1
            });

            dynamic func = typeof(EFCoreExtensions).GetMethod("SelfCompile", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(expression1.Type).Invoke(null, new object[2]
            {
                expression1,
                queryCompilationContext1
            });

            dynamic result = func(queryContext);
            compiledQuery = result;

            var field = result.GetType().GetField("_selectExpression", BindingFlags.Instance | BindingFlags.NonPublic);

            SelectExpression expression2;
            if (field != null)
            {
                expression2 = (SelectExpression)field.GetValue(result);

                var constructorInfo = typeof(RelationalShapedQueryCompilingExpressionVisitor).Assembly.GetType("Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor+ParameterValueBasedSelectExpressionOptimizer").GetConstructors()[0];

                var sqlExpressionFactoryField = result.GetType().GetField("_sqlExpressionFactory", BindingFlags.Instance | BindingFlags.NonPublic);

                object sqlExpressionFactory = sqlExpressionFactoryField.GetValue(result);


                var parameterNameGeneratorFactoryField = expression2.GetType().GetField("_parameterNameGeneratorFactory", BindingFlags.Instance | BindingFlags.NonPublic);
                var parameterNameGeneratorFactory = parameterNameGeneratorFactoryField.GetValue(result);

                var constructor = constructorInfo.Invoke(new object[2]
                {
                    sqlExpressionFactory,parameterNameGeneratorFactory
                });


                expression2 = (SelectExpression)constructor.GetType().GetMethod("Optimize").Invoke(constructor, new object[2]
                {
                    expression2,queryContext.ParameterValues
                });

                var querySqlGeneratorFactoryField = result.GetType().GetField("_querySqlGeneratorFactory", BindingFlags.Instance | BindingFlags.NonPublic);

                var querySqlGeneratorFactory = (IQuerySqlGeneratorFactory)querySqlGeneratorFactoryField.GetValue(result);
                sqlGenerator = querySqlGeneratorFactory.Create();
            }
            else
            {
                var relationalCommandCacheField = result.GetType().GetField("_relationalCommandCache", BindingFlags.Instance | BindingFlags.NonPublic);

                dynamic relationalCommandCache = relationalCommandCacheField.GetValue(result);

                field = relationalCommandCache.GetType().GetField("_selectExpression", BindingFlags.Instance | BindingFlags.NonPublic);

                expression2 = (SelectExpression)field.GetValue(relationalCommandCache);

                var type = typeof(RelationalShapedQueryCompilingExpressionVisitor).Assembly.GetType("Microsoft.EntityFrameworkCore.Query.RelationalShapedQueryCompilingExpressionVisitor+ParameterValueBasedSelectExpressionOptimizer");

                if (type != null)
                {
                    var constructorInfo1 = type.GetConstructors()[0];

                    dynamic sqlExpressionFactoryField = relationalCommandCache.GetType().GetField("_sqlExpressionFactory", BindingFlags.Instance | BindingFlags.NonPublic);

                    var sqlExpressionFactory = sqlExpressionFactoryField.GetValue(relationalCommandCache);

                    dynamic parameterNameGeneratorFactoryFiled = relationalCommandCache.GetType().GetField("_parameterNameGeneratorFactory", BindingFlags.Instance | BindingFlags.NonPublic);

                    var parameterNameGeneratorFactory = parameterNameGeneratorFactoryFiled.GetValue(relationalCommandCache);

                    var useRelationalNulls = RelationalOptionsExtension.Extract(queryCompilationContext.ContextOptions).UseRelationalNulls;

                    var constructor1 = constructorInfo1.Invoke(new object[3]
                    {
                      sqlExpressionFactory,parameterNameGeneratorFactory,useRelationalNulls
                    });

                    expression2 = (((SelectExpression, bool))constructor1.GetType().GetMethod("Optimize").Invoke(constructor1, new object[2]
                      {constructor1,queryContext.ParameterValues})).Item1;
                }
                else
                {
                    dynamic parameterValueBasedSelectExpressionOptimizerField = relationalCommandCache.GetType().GetField("_parameterValueBasedSelectExpressionOptimizer", BindingFlags.Instance | BindingFlags.NonPublic);

                    dynamic parameterValueBasedSelectExpressionOptimizer = parameterValueBasedSelectExpressionOptimizerField.GetValue(relationalCommandCache);

                    dynamic optimize = parameterValueBasedSelectExpressionOptimizer.GetType().GetMethod("Optimize");

                    expression2 = (((SelectExpression, bool))optimize.Invoke(parameterValueBasedSelectExpressionOptimizer, new object[2] { expression2, queryContext.ParameterValues })).Item1;
                }


                dynamic querySqlGeneratorFactoryField = relationalCommandCache.GetType().GetField("_querySqlGeneratorFactory", BindingFlags.Instance | BindingFlags.NonPublic);

                var querySqlGeneratorFactory = (IQuerySqlGeneratorFactory)querySqlGeneratorFactoryField.GetValue(relationalCommandCache);
                sqlGenerator = querySqlGeneratorFactory.Create();
            }
            return expression2;
        }

        /// <summary>A DbContext extension method that creates a new store command.</summary>
        /// <param name="context">The context to act on.</param>
        /// <returns>The new store command from the DbContext.</returns>
        public static DbCommand CreateStoreCommand(this DbContext context)
        {
            DbCommand command;
            if (context.Database.ProviderName == "Devart.Data.Oracle.Entity.EFCore")
            {
                var oracleRelationalDatabaseFacadeExtensions = context.Database.GetDbConnection().GetType().Assembly
                    .GetType("Microsoft.EntityFrameworkCore.OracleRelationalDatabaseFacadeExtensions");
                var getOracleConnectionMethod = oracleRelationalDatabaseFacadeExtensions.GetMethod("GetOracleConnection", BindingFlags.Static | BindingFlags.Public);
                var entityConnection = getOracleConnectionMethod.Invoke(oracleRelationalDatabaseFacadeExtensions, new[] { context.Database });
                command = ((dynamic)entityConnection).CreateCommand();
            }
            else
            {
                var entityConnection = context.Database.GetDbConnection();
                command = entityConnection.CreateCommand();

            }

            var entityTransaction = context.Database.GetService<IRelationalConnection>().CurrentTransaction;
            if (entityTransaction != null)
            {
                command.Transaction = entityTransaction.GetDbTransaction();
            }

            var commandTimeout = context.Database.GetCommandTimeout();
            if (commandTimeout.HasValue)
            {
                command.CommandTimeout = commandTimeout.Value;
            }

            return command;
        }

        #endregion

        #region Context
        /// <summary>An IQueryable extension method that gets database context from the query.</summary>
        /// <param name="query">The query to act on.</param>
        /// <returns>The database context from the query.</returns>
        internal static DbContext GetDbContext(this IQueryable query)
        {
            var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var compiler = (QueryCompiler)compilerField.GetValue(query.Provider);

            var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryContextFactory = (RelationalQueryContextFactory)queryContextFactoryField.GetValue(compiler);

            object stateManagerDynamic;

            var dependenciesField = typeof(RelationalQueryContextFactory).GetField("_dependencies", BindingFlags.NonPublic | BindingFlags.Instance);

            var dependencies = dependenciesField.GetValue(queryContextFactory);

            var stateManagerField = typeof(DbContext).GetTypeFromAssembly("Microsoft.EntityFrameworkCore.Query.QueryContextDependencies").GetProperty("StateManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            stateManagerDynamic = stateManagerField.GetValue(dependencies);

            if (!(stateManagerDynamic is IStateManager stateManager))
            {
                stateManager = ((dynamic)stateManagerDynamic).Value;
            }

            return stateManager.Context;
        }

        public static DbContext GetInMemoryContext<T>(this IQueryable<T> source)
        {
            var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var compiler = (QueryCompiler)compilerField.GetValue(source.Provider);

            var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryContextFactory = queryContextFactoryField.GetValue(compiler);

            object stateManagerDynamic;

            var dependenciesField = queryContextFactory.GetType().GetField("_dependencies", BindingFlags.NonPublic | BindingFlags.Instance);
            var dependencies = dependenciesField.GetValue(queryContextFactory);

            var stateManagerField = typeof(DbContext).GetTypeFromAssembly("Microsoft.EntityFrameworkCore.Query.QueryContextDependencies").GetProperty("StateManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            stateManagerDynamic = stateManagerField.GetValue(dependencies);

            if (!(stateManagerDynamic is IStateManager stateManager))
            {
                stateManager = ((dynamic)stateManagerDynamic).Value;
            }

            return stateManager.Context;
        }

        public static bool IsInMemoryQueryContext<T>(this IQueryable<T> query)
        {
            var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance);
            var compiler = (QueryCompiler)compilerField.GetValue(query.Provider);

            var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance);
            var queryContextFactory = queryContextFactoryField.GetValue(compiler);

            var relationalQueryContextFactory = queryContextFactory as RelationalQueryContextFactory;

            return relationalQueryContextFactory == null && queryContextFactory.GetType().Name == "InMemoryQueryContextFactory";
        }
        #endregion
    }
}
