using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Autofac;
using SudokuApp.Core.Entities;
using SudokuApp.Core.Interfaces;
using SudokuApp.Core.Interfaces.FileLoaders;

namespace SudokuApp.Core.Tests.Tests
{
    [TestFixture]
    public class SimpleSudokuGeneratorTest : TestBase
    {
        [Test]
        public void GenerateSudokuTest()
        {
            var sudokuGenerator = Container.Resolve<ISudokuGenerator>();

            var levels = new List<SudokuDifficultyLevel>()
            {
                SudokuDifficultyLevel.Easy,
                SudokuDifficultyLevel.Medium,
                SudokuDifficultyLevel.Hard,
                SudokuDifficultyLevel.Samurai
            };

            foreach (var sudokuDifficultyLevel in levels)
            {
                var generatedSudoku = sudokuGenerator.GenerateSudoku(sudokuDifficultyLevel);
                Assert.IsNotNull(generatedSudoku);
            }
        }

        [Test]
        public void GeneratedSudokuComplexityTest()
        {
            var sudokuGenerator = Container.Resolve<ISudokuGenerator>();

            var levels = new Dictionary<SudokuDifficultyLevel, int>()
            {
                { SudokuDifficultyLevel.Easy, 49},
                { SudokuDifficultyLevel.Medium, 53},
                { SudokuDifficultyLevel.Hard, 57},
                { SudokuDifficultyLevel.Samurai, 58},
            };

            foreach (var sudokuDifficultyLevel in levels.Keys)
            {
                var generatedSudoku = sudokuGenerator.GenerateSudoku(sudokuDifficultyLevel);
                Assert.AreEqual(levels[sudokuDifficultyLevel], generatedSudoku.Cells.Count(c => !c.Initial));
            }
        }
    }
}
