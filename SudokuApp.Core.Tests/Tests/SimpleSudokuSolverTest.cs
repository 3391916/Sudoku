using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using SudokuApp.Core.BL;
using SudokuApp.Core.Interfaces;
using SudokuApp.Core.Interfaces.FileLoaders;
using SudokuApp.Core.Tests.Helpers;
using SudokuApp.Core.Tests.TestFiles;

namespace SudokuApp.Core.Tests.Tests
{
    [TestFixture]
    public class SimpleSudokuSolverTest : TestBase
    {
        [Test]
        public void SolveTest()
        {
            var fileLoader = Container.Resolve<ISudokuFileLoader>();

            var testFilesPaths = new List<string>()
            {
                FileNameResolver.ResolveFileName(FileNames.Easy),
                FileNameResolver.ResolveFileName(FileNames.Medium),
                FileNameResolver.ResolveFileName(FileNames.Hard),
                FileNameResolver.ResolveFileName(FileNames.Samurai),
            };

            foreach (var testFilesPath in testFilesPaths)
            {
                var sudoku = fileLoader.LoadSudoku(testFilesPath);
                sudoku.Solve();
                Assert.IsTrue(sudoku.Cells.Any(c => !c.Value.HasValue));
            }
        }

        [Test]
        public void FetchAllSolutionsTest()
        {
            var fileLoader = Container.Resolve<ISudokuFileLoader>();
            var sudokuSolver = Container.Resolve<ISudokuSolver>();

            var testFilesPaths = new List<string>()
            {
                FileNameResolver.ResolveFileName(FileNames.Easy),
                FileNameResolver.ResolveFileName(FileNames.Medium),
                //FileNameResolver.ResolveFileName(FileNames.Hard),
                //FileNameResolver.ResolveFileName(FileNames.Samurai),
            };

            foreach (var testFilesPath in testFilesPaths)
            {
                var sudoku = fileLoader.LoadSudoku(testFilesPath);
                var allSolutions = sudokuSolver.FindAllSolutions(sudoku.Cells);
                Assert.IsTrue(allSolutions.Count() == 1);
            }
        }
    }
}
