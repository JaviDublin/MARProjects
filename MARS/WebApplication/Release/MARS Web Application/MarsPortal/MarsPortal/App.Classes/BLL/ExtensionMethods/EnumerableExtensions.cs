using System;
using System.Collections.Generic;
using System.Linq;
using App.BLL.DynamicLinq;

namespace App.BLL.ExtensionMethods
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<GroupResult> GroupByMany<TElement>(
            this IEnumerable<TElement> elements, params string[] groupSelectors)
        {
            var selectors = new List<Func<TElement, object>>(groupSelectors.Length);
            selectors.AddRange(groupSelectors.Select(selector => 
                    DynamicLinq.DynamicExpression.ParseLambda(typeof (TElement), typeof (object), selector)).Select(l => (Func<TElement, object>) l.Compile()));
            return elements.GroupByMany(selectors.ToArray());
        }

        public static IEnumerable<GroupResult> GroupByMany<TElement>(
            this IEnumerable<TElement> elements, params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors.Length > 0)
            {
                var selector = groupSelectors.First();
                var nextSelectors = groupSelectors.Skip(1).ToArray(); //reduce the list recursively until zero
                return
                    elements.GroupBy(selector).Select(
                        g => new GroupResult
                        {
                            Key = g.Key,
                            Count = g.Count(),
                            Items = g,
                            SubGroups = g.GroupByMany(nextSelectors)
                        });
            }
            return null;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
    Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            //source.ThrowIfNull("source");
            //selector.ThrowIfNull("selector");
            //comparer.ThrowIfNull("comparer");
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }
    }
}