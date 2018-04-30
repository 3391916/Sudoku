using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuApp.Core.Entities;

namespace SudokuApp.Core.Interfaces
{
    public interface ISudokuComplexityEstimator
    {
        SudokuDifficultyLevel EstimateComplexityLevel(Sudoku sudoku);

        int GetNumberOfFreeCellsByComplexityLevel(SudokuDifficultyLevel sudokuDifficultyLevel);
    }
}
