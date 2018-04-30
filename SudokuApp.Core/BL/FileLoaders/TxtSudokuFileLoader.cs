using System;
using System.Collections.Generic;
using System.IO;
using SudokuApp.Core.Entities;
using SudokuApp.Core.Interfaces.FileLoaders;

namespace SudokuApp.Core.BL.FileLoaders
{
    public class TxtSudokuFileLoader : ISudokuFileLoader
    {
        public Sudoku LoadSudoku(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
                throw new ArgumentException("filePath is null or empty");

            var stringFromFile = ReadFile(filePath);
            var validatedAndFormatedString = ValidateAndFormatSourceString(stringFromFile);
            var parsedSudoku = Parse(validatedAndFormatedString);

            return parsedSudoku;
        }

        private string ReadFile(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string ValidateAndFormatSourceString(string sourceStr)
        {
            if(string.IsNullOrEmpty(sourceStr))
                throw new ArgumentNullException("sourceStr is empty");

            sourceStr = sourceStr.Trim().Replace("\r", string.Empty).Replace(" ", string.Empty);

            var lines = sourceStr.Split('\n');

            if(lines.Length!= 9)
                throw new FormatException("source string lines count not equal to 9");

            for (int i=0;i<lines.Length;i++)
            {
                if(lines[0].Length != 9)
                    throw new FormatException($"source string line # {i+1} symbols count not equal to 9");
            }

            return sourceStr;
        }

        private Sudoku Parse(string sudokuString)
        {
            var cells = new List<SudokuCell>(81);

            var lines = sudokuString.Split('\n');

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var cell = new SudokuCell();
                    cell.X = j;
                    cell.Y = i;

                    var charVal = lines[i][j];

                    cell.Value = charVal == '.' 
                        ? (int?)null 
                        : Convert.ToInt32(charVal.ToString());

                    cell.Initial = cell.Value != null;

                    cell.Block = SudokuCell.GetBlock(cell.X, cell.Y);

                    cells.Add(cell);
                }
            }

            return new Sudoku(cells);
        }
    }
}
