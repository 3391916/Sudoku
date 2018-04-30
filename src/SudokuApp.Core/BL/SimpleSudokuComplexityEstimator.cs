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
    public class SimpleSudokuComplexityEstimator : ISudokuComplexityEstimator
    {
        public SudokuDifficultyLevel EstimateComplexityLevel(Sudoku sudoku)
        {
            var numberOfEmptyCells = sudoku.Cells.Count(c => !c.Initial);

            if (numberOfEmptyCells < 50)
                return SudokuDifficultyLevel.Easy;

            if (numberOfEmptyCells < 54)
                return SudokuDifficultyLevel.Medium;

            if (numberOfEmptyCells < 58)
                return SudokuDifficultyLevel.Hard;

            return SudokuDifficultyLevel.Samurai;
        }

        public int GetNumberOfFreeCellsByComplexityLevel(SudokuDifficultyLevel sudokuDifficultyLevel)
        {
            switch (sudokuDifficultyLevel)
            {
                case SudokuDifficultyLevel.Easy:
                    return 49;
                case SudokuDifficultyLevel.Medium:
                    return 53;
                case SudokuDifficultyLevel.Hard:
                    return 57;
                case SudokuDifficultyLevel.Samurai:
                default:
                    return 58;
            }
        }
    }
}
