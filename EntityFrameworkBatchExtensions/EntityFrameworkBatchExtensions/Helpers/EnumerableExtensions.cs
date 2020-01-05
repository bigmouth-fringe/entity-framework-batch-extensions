using System;
using System.Collections.Generic;

namespace EntityFrameworkBatchExtensions.Helpers
{
    public static class EnumerableExtensions
    {
        // SOURCE: https://stackoverflow.com/a/20953521/11344051
        public static IEnumerable<IEnumerable<T>> Batches<T>(this IEnumerable<T> enumerable, int chunkSize)
        {
            if (chunkSize < 1) {
                yield return new List<T>();
            }
            using(var e = enumerable.GetEnumerator()) {
                while (e.MoveNext()) {
                    var remaining = chunkSize;
                    var innerMoveNext = new Func<bool>(() => --remaining > 0 && e.MoveNext());

                    yield return e.GetBatch(innerMoveNext);
                    while (innerMoveNext()) {}
                }
            }
        }

        // SOURCE: https://stackoverflow.com/a/20953521/11344051
        private static IEnumerable<T> GetBatch<T>(this IEnumerator<T> e, Func<bool> innerMoveNext)
        {
            do yield return e.Current;
            while (innerMoveNext());
        }
    }
}