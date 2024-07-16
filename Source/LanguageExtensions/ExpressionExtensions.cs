using System.Linq.Expressions;

namespace LanguageExtensions;

public static class ExpressionExtensions
{
    /// <summary>
    /// Composes a logical OR conditions expression ie. ([predicate] OR [predicate] OR ... OR [predicate])
    /// </summary>
    public static IQueryable<TSource> WhereAnyOf<TSource>(
        this IQueryable<TSource> source,
        IEnumerable<Expression<Func<TSource, bool>>> predicates)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicates);

        using var enumerator = predicates.GetEnumerator();

        // skip if no predicates provided
        if (!enumerator.MoveNext())
            return source;

        // Ensure every expression has the same argument
        var parameter = Expression.Parameter(typeof(TSource), "x");

        Expression body = Expression.Invoke(enumerator.Current, parameter);

        while (enumerator.MoveNext())
        {
            // Generate predicate invocation expression
            var invokedExpr = Expression.Invoke(enumerator.Current, parameter);
            // Combine expressions with logical OR
            body = Expression.OrElse(body, invokedExpr);
        }

        return source.Where(Expression.Lambda<Func<TSource, bool>>(body, parameter));
    }

    public static Expression<Func<T, bool>> True<T> () => f => true; 

    public static Expression<Func<T, bool>> False<T> () => f => false; 
    
    public static Expression<Func<T, bool>> Or<T> (this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }
    
    public static Expression<Func<T, bool>> And<T> (this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso (expr1.Body, invokedExpr), expr1.Parameters);
    }
}