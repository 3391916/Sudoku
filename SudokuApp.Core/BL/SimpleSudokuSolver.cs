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
    public class SimpleSudokuSolver : ISudokuSolver
    {
        public List<SudokuCell> Solve(List<SudokuCell> cells)
        {
            return Solve(cells, false).First();
        }

        public List<List<SudokuCell>> FindAllSolutions(List<SudokuCell> cells)
        {
            return Solve(cells, true).Distinct();
        }

        private List<List<SudokuCell>> Solve(List<SudokuCell> cells, bool checkAllCombinations)
        {
            List<List<SudokuCell>> possibleSolutions = new List<List<SudokuCell>>();

            // fix cells with single possible value aproach
            cells = FixCellsWithSinglePossibleValue(cells);

            if (cells.Count(c => !c.Value.HasValue) == 0)
            {
                possibleSolutions.Add(cells);
                return possibleSolutions;
            }
                

            // if there are left not solved cells than we should use brute force
            foreach (var cell in cells.Where(c => !c.Value.HasValue))
            {
                CalculatePossibleValues(cells, new List<SudokuCell>() { cell });

                if (cell.PossibleValues.Count == 0)
                    continue;

                foreach (var v in cell.PossibleValues)
                {
                    cell.Value = v;
                    var result = Solve((List<SudokuCell>)cells.Clone(), checkAllCombinations);

                    if (result != null)
                    {
                        if (!checkAllCombinations)
                            return result;

                        possibleSolutions.AddRange(result);
                    }
                }
            }

            if (possibleSolutions.Any())
                return possibleSolutions;

            return null;
        }

        private List<SudokuCell> FixCellsWithSinglePossibleValue(List<SudokuCell> cells)
        {
            int emptyCellCount = 81;

            while (cells.Count(c => !c.Value.HasValue) < emptyCellCount)
            {
                emptyCellCount = cells.Count(c => !c.Value.HasValue);

                foreach (SudokuBlock block in Enum.GetValues(typeof(SudokuBlock)))
                {
                    int cc = 9;
                    while (cells.Count(c => !c.Value.HasValue && c.Block == block) < cc)
                    {
                        var emptyCellsInBlock = cells.Where(c => !c.Value.HasValue && c.Block == block);
                        cc = emptyCellsInBlock.Count();

                        CalculatePossibleValues(cells, emptyCellsInBlock);

                        CheckHorizontally(emptyCellsInBlock);

                        CalculatePossibleValues(cells, emptyCellsInBlock);

                        CheckVertically(emptyCellsInBlock);
                    }
                }
            }

            return cells;
        }

        private void CheckHorizontally(IEnumerable<SudokuCell> ecs)
        {
            var hcs = ecs.Where(c => c.PossibleValues.Count() == 1);

            foreach (SudokuCell cell in hcs)
            {
                cell.Value = cell.PossibleValues[0];

                foreach (var item in ecs)
                {
                    item.PossibleValues.Remove(cell.Value.Value);
                }
            }
        }

        private void CheckVertically(IEnumerable<SudokuCell> ecs)
        {
            for (int i = 1; i < 10; i++)
            {
                if (ecs.Count(c => c.PossibleValues.Contains(i)) == 1)
                {
                    var cell = ecs.First(c => c.PossibleValues.Contains(i));
                    cell.Value = i;

                    foreach (var item in ecs)
                    {
                        item.PossibleValues.Remove(cell.Value.Value);
                    }
                }
            }
        }

        private void CalculatePossibleValues(IEnumerable<SudokuCell> allCells, IEnumerable<SudokuCell> cells)
        {
            foreach (var cell in cells)
            {
                cell.PossibleValues.Clear();

                for (int i = 1; i < 10; i++)
                {
                    bool isPossible = IsValuePossibleToBeInCell(allCells, cell, i);

                    if (isPossible)
                        cell.PossibleValues.Add(i);
                }
            }
        }

        private bool IsValuePossibleToBeInCell(IEnumerable<SudokuCell> cells, SudokuCell cell, int value)
        {
            if (cell == null || cell.Value.HasValue)
                throw new ArgumentException("cell");

            // value exist in a block
            if (cells.Count(c => c.Block == cell.Block && c.Value.HasValue && c.Value.Value == value) > 0)
                return false;

            // horizontal check
            if (GetValues(cells, cell, Orientation.Horizontal).Contains(value))
                return false;

            // vertical check
            if (GetValues(cells, cell, Orientation.Vertical).Contains(value))
                return false;

            return true;
        }

        private List<int> GetValues(IEnumerable<SudokuCell> cells, SudokuCell cell, Orientation orientation)
        {
            var values =
                cells.Where(
                        c => (orientation == Orientation.Horizontal ? c.Y == cell.Y : c.X == cell.X) && c.Value.HasValue)
                    .Select(c => c.Value.Value).ToList();

            return values;
        }
    }
}
