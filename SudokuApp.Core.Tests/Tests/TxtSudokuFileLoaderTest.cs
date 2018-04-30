using System.Collections.Generic;
using Autofac;
using NUnit.Framework;
using SudokuApp.Core.Interfaces.FileLoaders;
using SudokuApp.Core.Tests.Helpers;
using SudokuApp.Core.Tests.TestFiles;

namespace SudokuApp.Core.Tests.Tests
{
    [TestFixture]
    public class TxtSudokuFileLoaderTest : TestBase
    {
        [Test]
        public void TestReadFile()
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
                Assert.IsNotNull(sudoku);
            }
        }
    }
}
