namespace CodeMechanic.Extensions
{
    public static class CombinatoricsExtensions
    {

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int combinationSize = 1)
        {
           if (source.Count() == 0)
           {
               return Enumerable.Empty<IEnumerable<T>>();
           }

           if (combinationSize > source.Count() /*|| combinationSize == 0*/)
           {
               combinationSize = source.Count();
           }

           if (source.Count() == 1)
           {
               return new[] { source };
           }

           var indexedSource = source
               .Select((x, i) => new
               {
                   Item = x,
                   Index = i
               })
               .ToList();

           return indexedSource
               .SelectMany(x => indexedSource
                   .OrderBy(y => x.Index != y.Index)
                   .Skip(1)
                   .OrderBy(y => y.Index)
                   .Skip(x.Index)
                   .Combinations(combinationSize - 1)
                   .Select(y => new[] { x }.Concat(y).Select(z => z.Item))
               );
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
           if (sequences == null)
           {
               throw new ArgumentNullException(nameof(sequences));
           }

           IEnumerable<IEnumerable<T>> emptyProduct = new IEnumerable<T>[] { Enumerable.Empty<T>() };
           return sequences.Aggregate(
               emptyProduct,
               (accumulator, sequence) =>
                   from accseq in accumulator
                   from item in sequence
                   select accseq.Concat(new[] { item }))
                   .Where(x => x.Any());
        }

       

        private static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }


        private static void Swap(ref char a, ref char b)
        {
           if (a == b) return;

           var temp = a;
           a = b;
           b = temp;
        }

      
    }
}
