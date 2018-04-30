using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuApp.Core.BL.Helpers;
using SudokuApp.Core.Interfaces;
using SudokuApp.Core.Entities;

namespace SudokuApp.Core.BL
{
    public class SimpleSudokuGenerator : ISudokuGenerator
    {
        private enum SudokuSymetry
        {
            VerticalHorizontal,
            Diagonal
        }

        private readonly ISudokuComplexityEstimator _sudokuComplexityEstimator;
        private readonly int[] _numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly Random _random = new Random();

        public SimpleSudokuGenerator(ISudokuComplexityEstimator sudokuComplexityEstimator)
        {
            _sudokuComplexityEstimator = sudokuComplexityEstimator;
        }

        public Sudoku GenerateSudoku(SudokuDifficultyLevel difficultyLevel)
        {
            var newSudokuMatrix = GenerateMatrix();
            var nullableSudokuMatrix = new int?[9][];

            for (int i = 0; i < 9; i++)
            {
                if (nullableSudokuMatrix[i] == null)
                    nullableSudokuMatrix[i] = new int?[9];

                for (int j = 0; j < 9; j++)
                    nullableSudokuMatrix[i][j] = newSudokuMatrix[i][j];
            }

            var symetry = SudokuSymetry.VerticalHorizontal;

            if (difficultyLevel == SudokuDifficultyLevel.Hard || difficultyLevel == SudokuDifficultyLevel.Samurai)
                symetry = SudokuSymetry.Diagonal;

            // fetching number of empty cells per level
            int emptyCellsRequiredForLevel = _sudokuComplexityEstimator.GetNumberOfFreeCellsByComplexityLevel(difficultyLevel);
            int currentEmptyCellsCount = 0;

            var generatedEmptyCellsPossibleCoordinates = GenerateSymeticCoordinates(symetry);

            while (currentEmptyCellsCount < emptyCellsRequiredForLevel)
            {
                if(!generatedEmptyCellsPossibleCoordinates.Any())
                    generatedEmptyCellsPossibleCoordinates = GenerateSymeticCoordinates(symetry);

                var coord = generatedEmptyCellsPossibleCoordinates.Pop();

                if (nullableSudokuMatrix[coord.X][coord.Y] == null)
                    continue;

                nullableSudokuMatrix[coord.X][coord.Y] = null;
                currentEmptyCellsCount++;
            }

            var sudokuCells = new List<SudokuCell>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var cell = new SudokuCell();
                    cell.X = j;
                    cell.Y = i;
                    
                    cell.Value = nullableSudokuMatrix[i][j];
                    cell.Initial = nullableSudokuMatrix[i][j] != null;

                    cell.Block = SudokuCell.GetBlock(cell.X, cell.Y);

                    sudokuCells.Add(cell);
                }
            }

            return new Sudoku(sudokuCells);
        }

        /// <summary>
        /// Generate new solved matrix
        /// </summary>
        /// <returns></returns>
        private int[][] GenerateMatrix()
        {
            int[][] resultMatrix = new int[9][];
            var availableNumbers = new List<int>[9];

            for (int x = 0; x < availableNumbers.Length; x++)
                availableNumbers[x] = new List<int>(_numbers);

            // filling first row
            resultMatrix[0] = _numbers.Shuffle();

            for (int i = 0; i < 9; i++)
                availableNumbers[i].Remove(resultMatrix[0][i]);

            int row = 1;
            int rowTries = 0;

            while (row < 9)
            {
                var rowAvailableNumbers = new List<int>(_numbers.Shuffle());

                int[] currentRow = new int[9];
                List<int>[] used = new List<int>[9];

                for (var i = 0; i < used.Length; i++)
                    used[i] = new List<int>(2);

                int pos = 0;
                while (pos < 9)
                {
                    int num = rowAvailableNumbers.FirstOrDefault(x => availableNumbers[pos].Contains(x) 
                                                                      && !used[pos].Contains(x));
                    // if found
                    if (num != 0)
                    {
                        currentRow[pos] = num;
                        used[pos].Add(num);
                        rowAvailableNumbers.Remove(num);
                        pos++;
                    }
                    else
                    {
                        used[pos].Clear();
                        pos--;
                        rowAvailableNumbers.Add(currentRow[pos]);
                        currentRow[pos] = 0;
                    }
                }

                // copy result row to result matrix
                resultMatrix[row] = currentRow;

                for (int i = 0; i < 9; i++)
                    availableNumbers[i].Remove(resultMatrix[row][i]);

                row++;

                if (row % 3 == 0)
                    if (!BoxCheck(row / 3 - 1, resultMatrix))
                    {
                        row--;

                        for (int i = 0; i < resultMatrix[row].Length; i++)
                            availableNumbers[i].Add(resultMatrix[row][i]);

                        rowTries++;

                        if (rowTries > 9)
                        {
                            row--;

                            for (int i = 0; i < resultMatrix[row].Length; i++)
                                availableNumbers[i].Add(resultMatrix[row][i]);

                            rowTries = 0;
                        }
                    }
                    else
                        rowTries = 0;
            }

            return resultMatrix;
        }

        private bool BoxCheck(int i, int[][] matrix)
        {
            for (int j = 0; j < 3; j++)
            {
                var freeNumbers = new List<int>(_numbers);

                for (int x = 0; x < 3; x++)
                    for (int y = 0; y < 3; y++)
                        freeNumbers.Remove(matrix[i * 3 + x][j * 3 + y]);

                if (freeNumbers.Count > 0)
                    return false;
            }
            return true;
        }

        private Stack<Coordinate> GenerateSymeticCoordinates(SudokuSymetry symetry)
        {
            var result = new Stack<Coordinate>();

            if (symetry == SudokuSymetry.VerticalHorizontal)
            {
                int posX = _random.Next(5);
                int posY = _random.Next(5);

                result.Push(new Coordinate(posX, posY));
                result.Push(new Coordinate(8 - posX, posY));
                result.Push(new Coordinate(posX, 8 - posY));
                result.Push(new Coordinate(8 - posX, 8 - posY));
            }
            else
            {
                int posX = _random.Next(5);
                int posY = _random.Next(5);

                result.Push(new Coordinate(posX, posY));
                result.Push(new Coordinate(8 - posX, 8 - posY));

                posX = _random.Next(5);
                posY = _random.Next(5);

                result.Push(new Coordinate(8 - posX, posY));
                result.Push(new Coordinate(posY, 8 - posX));
            }

            return result;
        }
    }
}
