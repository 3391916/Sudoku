using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using SudokuApp.Core.Entities;
using SudokuApp.ViewModels;
using Binding = System.Windows.Data.Binding;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;

namespace SudokuApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBlock[,] _sudokuTextBlocks;
        private SudokuViewModel _sudokuViewModel;

        public MainWindow()
        {
            _sudokuViewModel = new SudokuViewModel();

            InitializeComponent();
            DrawSudokuGrid();

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                _sudokuTextBlocks[i, j].SetBinding(TextBlock.TextProperty, new Binding($"Board.[{i}][{j}]"));

            this.DataContext = _sudokuViewModel;
        }


        #region Actions Handlers

        private void LoadGameBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openSudokuFileDialog = new OpenFileDialog();
            openSudokuFileDialog.Filter = "Sudoku Files |*.txt";
            openSudokuFileDialog.Title = "Select a Sudoku Game File";

            if (openSudokuFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _sudokuViewModel.LoadSudoku(openSudokuFileDialog.FileName);
            }
        }

        private void SolveGameBtn_Click(object sender, RoutedEventArgs e)
        {
            _sudokuViewModel.Solve();
        }

        private void NewGameBtn_Click(object sender, RoutedEventArgs e)
        {
            _sudokuViewModel.GenerateNew();
        }

        private void CheckIfUniqueSolution_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_sudokuViewModel.IsSolutionUnique()
                ? "Sudoku has only one possible solution."
                : "Sudoku has more than one possible solution.");
        }

        private void DifficultySlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _sudokuViewModel.CurrentSudokuDifficultyLevel = (SudokuDifficultyLevel)e.NewValue;
            DifficultyTextBlock.Text = _sudokuViewModel.CurrentSudokuDifficultyLevel.ToString().ToUpper();
        }

        #endregion Actions Handlers

        #region Rendering Grid helpers

        private void DrawSudokuGrid()
        {
            _sudokuTextBlocks = new TextBlock[9, 9];
            for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j++)
            {
                var border = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = GetThickness(i, j, 0.1, 0.3)
                };
                _sudokuTextBlocks[i, j] = new TextBlock
                {
                    FontSize = 5,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                border.Child = _sudokuTextBlocks[i, j];
                Grid.SetRow(border, i);
                Grid.SetColumn(border, j);
                SudokuGrid.Children.Add(border);
            }
        }

        private Thickness GetThickness(int i, int j, double thin, double thick)
        {
            var top = i % 3 == 0 ? thick : thin;
            var bottom = i % 3 == 2 ? thick : thin;
            var left = j % 3 == 0 ? thick : thin;
            var right = j % 3 == 2 ? thick : thin;
            return new Thickness(left, top, right, bottom);
        }


        #endregion Rendering Grid helpers
    }
}
