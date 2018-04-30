using System;
using System.Collections.Generic;
using System.Linq;
using SudokuApp.Core.Entities;

namespace SudokuApp.Core.BL.Helpers
{
    public static class Extensions
    {
        private static readonly Random _randomizer = new Random();

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            var list = new List<T>();

            list.AddRange(listToClone.Select(i => i.Clone()).Cast<T>());

            return list;
        }

        public static int[] Shuffle(this int[] collection)
        {
            var array = collection.ToArray();
            var newArray = new int[array.Length];
            var index = array.Length - 1;
            while (index != -1)
            {
                var position = _randomizer.Next(array.Length);
                if (newArray[position] != 0)
                    continue;
                newArray[position] = array[index];
                index--;
            }
            return newArray;
        }

        public static List<List<SudokuCell>> Distinct(this List<List<SudokuCell>> sudokuSolutions)
        {
            var groupedCollection = new Dictionary<string, List<SudokuCell>>();

            foreach (var sudokuSolution in sudokuSolutions)
            {
                var sudokuStringified = sudokuSolution.CellsToString();
                if(!groupedCollection.ContainsKey(sudokuStringified))
                    groupedCollection.Add(sudokuStringified, sudokuSolution);
            }

            return groupedCollection.Values.ToList();
        }


        private static string CellsToString(this List<SudokuCell> cells)
        {
            var values = new List<int>(81);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var cell = cells.FirstOrDefault(c => c.X == j && c.Y == i && c.Value.HasValue);
                    values.Add(cell != null ? cell.Value.Value : 0);
                }
            }

            return string.Join("", values);
        }
    }
}
