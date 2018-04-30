using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuApp.Core.Entities;

namespace SudokuApp.Core.Interfaces
{
    public interface ISudokuGenerator
    {
        Sudoku GenerateSudoku(SudokuDifficultyLevel difficultyLevel);
    }
}
