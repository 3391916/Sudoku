using SudokuApp.Core.Entities;

namespace SudokuApp.Core.Interfaces.FileLoaders
{
    public interface ISudokuFileLoader
    {
        Sudoku LoadSudoku(string filePath);
    }
}
