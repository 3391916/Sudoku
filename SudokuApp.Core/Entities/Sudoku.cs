using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using SudokuApp.Core.BL.Helpers;
using SudokuApp.Core.Interfaces;

namespace SudokuApp.Core.Entities
{
    public class Sudoku
    {
        private readonly ISudokuSolver _sudokuSolver;
        private readonly ISudokuComplexityEstimator _sudokuComplexityEstimator;
        private List<SudokuCell> _cells;
        private SudokuDifficultyLevel _sudokuDifficultyLevel;

        public List<SudokuCell> Cells
        {
            get { return _cells; }
        }

        public SudokuDifficultyLevel DifficultyLevel => _sudokuDifficultyLevel;

        public Sudoku(List<SudokuCell> cells)
        {
            if (cells == null)
                throw new ArgumentNullException("cells can't be equal to null");

            _cells = cells;

            _sudokuSolver = ServiceLocator.Current.GetInstance<ISudokuSolver>();
            _sudokuComplexityEstimator = ServiceLocator.Current.GetInstance<ISudokuComplexityEstimator>();
            _sudokuDifficultyLevel = _sudokuComplexityEstimator.EstimateComplexityLevel(this);
        }

        public void Solve()
        {
            _cells = _sudokuSolver.Solve(_cells);
        }

        public bool IsSolutionUnique()
        {
            var clonedCells = _cells.Clone().ToList();
            // clean already solved part
            foreach (var cell in clonedCells)
            {
                if (cell.Value != null && !cell.Initial)
                    cell.Value = null;
            }
            var possibleSolutions = _sudokuSolver.FindAllSolutions(clonedCells);
            return possibleSolutions.Count == 1;
        }
    }
}
