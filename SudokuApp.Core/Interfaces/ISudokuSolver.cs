using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuApp.Core.Entities;

namespace SudokuApp.Core.Interfaces
{
    public interface ISudokuSolver
    {
        List<SudokuCell> Solve(List<SudokuCell> cells);

        List<List<SudokuCell>> FindAllSolutions(List<SudokuCell> cells);
    }
}
