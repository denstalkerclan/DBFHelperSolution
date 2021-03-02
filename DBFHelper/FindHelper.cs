using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFHelper
{
    public interface IObjectPair<TSource1, TSource2, TKey>
    {
        TKey Key { get; set; }
        TSource1 Source1 { get; set; }
        TSource2 Source2 { get; set; }
    }

    public class ObjectPair<TSource1, TSource2, TKey> : IObjectPair<TSource1, TSource2, TKey>
    {
        public TKey Key { get; set; }
        public TSource1 Source1 { get; set; }
        public TSource2 Source2 { get; set; }
    }

    public static class FindHelper
    {
        public static List<TObjectPair> FindEquals<TObjectPair, TSource1, TSource2, TKey>(
            this IEnumerable<TSource1> source1, Func<TSource1, IEnumerable<TKey>> keySelector1,
            IEnumerable<TSource2> source2, Func<TSource2, IEnumerable<TKey>> keySelector2,
            IEqualityComparer<TKey> keyEqualityComparer = null)
                where TObjectPair : IObjectPair<TSource1, TSource2, TKey>, new()
        {
            var sourceList1 = source1.ToDictionary(
                x => x,
                x => (ICollection<TKey>)keySelector1(x).ToList());
            var sourceList2 = source2.ToDictionary(
                x => x,
                x => (ICollection<TKey>)keySelector2(x).ToList());
            var dictSource2 = source2.ToDictionary(keySelector2, keyEqualityComparer);

            return FindEquals<TObjectPair, TSource1, TSource2, TKey>(sourceList1, dictSource2, sourceList2);
        }

        private static void RemoveKeys<TSource, TKey>(TSource source, IEnumerable<TKey> keys, IDictionary<TKey, ICollection<TSource>> dictSource)
        {
            foreach (var key in keys)
            {
                dictSource[key].Remove(source);
                if (dictSource[key].Count == 0)
                {
                    dictSource.Remove(key);
                }
            }
        }

        private static List<TObjectPair> FindEquals<TObjectPair, TSource1, TSource2, TKey>(
            IDictionary<TSource1, ICollection<TKey>> source1,
            IDictionary<TKey, ICollection<TSource2>> dictSource2,
            IDictionary<TSource2, ICollection<TKey>> source2)
                where TObjectPair : IObjectPair<TSource1, TSource2, TKey>, new()
        {
            var objectPairs = new List<TObjectPair>();
            foreach (var source in source1.ToList())
            {
                var key = source.Value.First();
                if (dictSource2.ContainsKey(key) || source.Value.Count == 1)
                {
                    var objectPair = new TObjectPair()
                    {
                        Source1 = source.Key
                    };
                    if (dictSource2.ContainsKey(key))
                    {
                        objectPair.Source2 = dictSource2[key].First();
                        objectPair.Key = key;
                        RemoveKeys(objectPair.Source2, source2[objectPair.Source2], dictSource2);
                    }
                    objectPairs.Add(objectPair);
                    source1.Remove(source);
                    continue;
                }
                source.Value.Remove(key);
            }
            if (source1.Any())
            {
                objectPairs.AddRange(FindEquals<TObjectPair, TSource1, TSource2, TKey>(source1, dictSource2, source2));
            }
            foreach (var source in dictSource2.SelectMany(x => x.Value).Distinct().ToList())
            {
                var objectPair = new TObjectPair()
                {
                    Source2 = source
                };
                objectPairs.Add(objectPair);
                RemoveKeys(source, source2[source], dictSource2);
            }
            return objectPairs;
        }

        private static Dictionary<TKey, ICollection<TSource>> ToDictionary<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TKey>> keySelector,
            IEqualityComparer<TKey> keyEqualityComparer = null)
        {
            var newDict = new Dictionary<TKey, ICollection<TSource>>(keyEqualityComparer ?? EqualityComparer<TKey>.Default);
            foreach (var pos in source)
            {
                foreach (var key in keySelector(pos))
                {
                    if (!newDict.ContainsKey(key))
                    {
                        newDict.Add(key, new HashSet<TSource>());
                    }
                    newDict[key].Add(pos);
                }
            }
            return newDict;
        }
    }
}
