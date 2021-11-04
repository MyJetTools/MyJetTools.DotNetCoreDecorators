using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DotNetCoreDecorators
{
    public static class SortedListExtentions
    {

            public static int SearchEqualOrGreater<TValue>(this SortedList<long, TValue> sortedList,  long keyToSearch)
            {

                return sortedList.SearchEqualOrGreater(keyToSearch, (v1, v2) =>
                {
                    if (v1 < v2)
                        return -1;

                    return v1 > v2 ? 1 : 0;
                });

            }


            public static int SearchEqualOrGreater<TValue>(this SortedList<DateTime, TValue> sortedList,
                DateTime keyToSearch)
            {
                return sortedList.SearchEqualOrGreater(keyToSearch, (v1, v2) =>
                {
                    if (v1 < v2)
                        return -1;

                    return v1 > v2 ? 1 : 0;
                });

            }
            
            
            public static int SearchEqualOrGreater<TValue>(this SortedList<int, TValue> sortedList,
                int keyToSearch)
            {
                return sortedList.SearchEqualOrGreater(keyToSearch, (v1, v2) =>
                {
                    if (v1 < v2)
                        return -1;

                    return v1 > v2 ? 1 : 0;
                });

            }
            
            public static int SearchEqualOrGreater<TValue>(this SortedList<double, TValue> sortedList,
                double keyToSearch)
            {
                return sortedList.SearchEqualOrGreater(keyToSearch, (v1, v2) =>
                {
                    if (v1 < v2)
                        return -1;

                    return v1 > v2 ? 1 : 0;
                });

            }
            
            public static int SearchEqualOrGreater<TValue>(this SortedList<decimal, TValue> sortedList,
                decimal keyToSearch)
            {
                return sortedList.SearchEqualOrGreater(keyToSearch, (v1, v2) =>
                {
                    if (v1 < v2)
                        return -1;

                    return v1 > v2 ? 1 : 0;
                });

            }



            public static int SearchEqualOrGreater<TKey, TValue>(this SortedList<TKey, TValue> sortedList,
                TKey keyToSearch,
                Func<TKey, TKey, int> comparator)
            {

                if (sortedList.Count == 0)
                    return -1;
                
                if (comparator(keyToSearch, sortedList.Keys[^1]) > 0)
                    return sortedList.Count;


                var min = 0;
                var max = sortedList.Count - 1;
                var position = max / 2;
                
                while (true)
                {

                    var compareResult = comparator(sortedList.Keys[position], keyToSearch);

                    if (compareResult == 0)
                        return position;

                    if (compareResult < 0)
                        min = position;
                    else
                        max = position;

                    position = min + (max - min) / 2;

                    if (max - min <= 1)
                    {

                        var minCompareResult = comparator(sortedList.Keys[min], keyToSearch);
                        if (minCompareResult == 0)
                            return min;

                        var maxCompareResult = comparator(sortedList.Keys[max], keyToSearch);
                        if (maxCompareResult == 0)
                            return max;

                        return minCompareResult > 0
                            ? min
                            : max;
                    }

                }
            }

        }
    }