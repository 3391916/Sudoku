using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using SudokuApp.Annotations;
using SudokuApp.Core.BL;
using SudokuApp.Core.Interfaces;
using SudokuApp.Core.Entities;
using SudokuApp.Core.Interfaces.FileLoaders;

namespace SudokuApp.ViewModels
{
    public class SudokuViewModel: INotifyPropertyChanged
    {
        #region Private variables

        private readonly ISudokuFileLoader _sudokuFileLoader;
        private readonly ISudokuGenerator _sudokuGenerator;
        private Sudoku _sudoku;
        private int?[][] _board;

        public int?[][] Board => _board;
        public Sudoku Sudoku => _sudoku;
        public SudokuDifficultyLevel CurrentSudokuDifficultyLevel { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Private variables

        #region Constructors

        public SudokuViewModel() : this(
            IoCContainer.Resolve<ISudokuFileLoader>(),
            IoCContainer.Resolve<ISudokuGenerator>())
        {
            _board = new int?[9][];
            CurrentSudokuDifficultyLevel = SudokuDifficultyLevel.Easy;
        }

        public SudokuViewModel(
            ISudokuFileLoader sudokuFileLoader,
            ISudokuGenerator sudokuGenerator)
        {
            _sudokuFileLoader = sudokuFileLoader;
            _sudokuGenerator = sudokuGenerator;
        }

        #endregion Constructors

        public void LoadSudoku(string filePath)
        {
            try
            {
                _sudoku = _sudokuFileLoader.LoadSudoku(filePath);
                UpdateBoard();
            }
            catch (Exception exception)
            {
                OnErrorOccured(exception);
            }
        }

        public void Solve()
        {
            if (_sudoku != null)
            {
                try
                {
                    _sudoku.Solve();
                    UpdateBoard();
                }
                catch (Exception exception)
                {
                    OnErrorOccured(exception);
                }
            }
        }

        public void GenerateNew()
        {
            try
            {
                _sudoku = _sudokuGenerator.GenerateSudoku(CurrentSudokuDifficultyLevel);
                UpdateBoard();
            }
            catch (Exception exception)
            {
                OnErrorOccured(exception);
            }
        }

        public bool IsSolutionUnique()
        {
            if(_sudoku == null)
                throw new Exception("Can not check Sudoku. Sudoku is not loaded.");

            return _sudoku.IsSolutionUnique();
        }

        #region Assistants

        private void UpdateBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                _board[i] = new int?[9];
                for (int j = 0; j < 9; j++)
                    _board[i][j] = _sudoku == null 
                        ? null
                        : _sudoku.Cells.First(c => c.Y == i && c.X == j).Value;
            }

            OnPropertyChanged("Board");
            OnPropertyChanged("Sudoku");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnErrorOccured(Exception exception)
        {
            //here could be added some logging

            MessageBox.Show("Some error occured during operation");
        }

        #endregion
    }
}
