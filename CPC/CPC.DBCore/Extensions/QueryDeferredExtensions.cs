using System;
using System.Linq;
using System.Linq.Expressions;

namespace CPC.DBCore
{
    public static class QueryDeferredExtensions
    {
        #region Common

        public static QueryDeferred<T> DeferredAggregate<T>(this IQueryable<T> source, Expression<Func<T, T, T>> func) => new QueryDeferred<T>(source,
            Expression.Call(
                    null,
                    InternalExtensions.MethodInfo(Queryable.Aggregate, source, func),
                    new[] { source.Expression, Expression.Quote(func) }
                    ));

        public static QueryDeferred<TAccumulate> DeferredAggregate<TSource, TAccumulate>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func) => new QueryDeferred<TAccumulate>(source,
           Expression.Call(
                   null,
                   InternalExtensions.MethodInfo(Queryable.Aggregate, source, seed, func),
                   new[] { source.Expression, Expression.Constant(seed), Expression.Quote(func) }
                   ));

        public static QueryDeferred<TResult> DeferredAggregate<TSource, TAccumulate, TResult>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector) => new QueryDeferred<TResult>(source,
           Expression.Call(
                   null,
                   InternalExtensions.MethodInfo(Queryable.Aggregate, source, seed, func, selector),
                   new[] { source.Expression, Expression.Constant(seed), Expression.Quote(func), Expression.Quote(selector) }
                   ));

        public static QueryDeferred<bool> DeferredAll<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) => new QueryDeferred<bool>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.All, source, predicate),
                 new[] { source.Expression, Expression.Quote(predicate) }
                 ));

        public static QueryDeferred<bool> DeferredAny<TSource>(this IQueryable<TSource> source) => new QueryDeferred<bool>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.Any, source), source.Expression
                 ));

        public static QueryDeferred<bool> DeferredAny<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) => new QueryDeferred<bool>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.Any, source, predicate),
                 new[] { source.Expression, Expression.Quote(predicate) }
                 ));

        public static QueryDeferred<int> DeferredCount<TSource>(this IQueryable<TSource> source) => new QueryDeferred<int>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.Count, source), source.Expression
                 ));

        public static QueryDeferred<int> DeferredCount<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) => new QueryDeferred<int>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.Count, source, predicate),
                 new[] { source.Expression, Expression.Quote(predicate) }
                 ));

        public static QueryDeferred<TSource> DeferredFirst<TSource>(this IQueryable<TSource> source) => new QueryDeferred<TSource>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.First, source), source.Expression
                 ));

        public static QueryDeferred<TSource> DeferredFirst<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) => new QueryDeferred<TSource>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.First, source, predicate),
                 new[] { source.Expression, Expression.Quote(predicate) }
                 ));

        public static QueryDeferred<TSource> DeferredFirstOrDefault<TSource>(this IQueryable<TSource> source) => new QueryDeferred<TSource>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.FirstOrDefault, source), source.Expression
                 ));

        public static QueryDeferred<TSource> DeferredFirstOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) => new QueryDeferred<TSource>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.FirstOrDefault, source, predicate),
                 new[] { source.Expression, Expression.Quote(predicate) }
                 ));

        public static QueryDeferred<TSource> DeferredLast<TSource>(this IQueryable<TSource> source) => new QueryDeferred<TSource>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.Last, source), source.Expression
                 ));

        public static QueryDeferred<TSource> DeferredLast<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) => new QueryDeferred<TSource>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.Last, source, predicate),
                 new[] { source.Expression, Expression.Quote(predicate) }
                 ));

        public static QueryDeferred<TSource> DeferredLastOrDefault<TSource>(this IQueryable<TSource> source) => new QueryDeferred<TSource>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.LastOrDefault, source), source.Expression
                 ));

        public static QueryDeferred<TSource> DeferredLastOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate) => new QueryDeferred<TSource>(source,
         Expression.Call(
                 null,
                 InternalExtensions.MethodInfo(Queryable.LastOrDefault, source, predicate),
                 new[] { source.Expression, Expression.Quote(predicate) }
                 ));
        #endregion
    }
}
