using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuApp.Core.Entities
{
    public class SudokuCell : ICloneable, IComparable<SudokuCell>
    {
        public SudokuCell()
        {
            PossibleValues = new List<int>(9);
        }

        public SudokuCell(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public int? Value { get; set; }
        public bool Initial { get; set; }

        public SudokuBlock Block { get; set; }

        public List<int> PossibleValues { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            var cell = new SudokuCell();
            cell.X = this.X;
            cell.Y = this.Y;
            cell.Value = this.Value;
            cell.Initial = this.Initial;
            cell.Block = this.Block;
            cell.PossibleValues = this.PossibleValues.Select(pv=>pv).ToList();

            return cell;
        }

        #endregion

        public int CompareTo(SudokuCell other)
        {
            return (this.X == other.X && this.Y == other.Y && this.Value == other.Value && this.Block == other.Block) ? 0 : -1;
        }

        public static SudokuBlock GetBlock(int x, int y)
        {
            if (x < 0 || x > 8 || y < 0 || y > 8)
                throw new ArgumentOutOfRangeException();

            if (x < 3 && y < 3)
                return SudokuBlock.B00;

            if (x > 2 && x < 6 && y < 3)
                return SudokuBlock.B10;

            if (x > 5 && y < 3)
                return SudokuBlock.B20;

            if (x < 3 && y > 2 && y < 6)
                return SudokuBlock.B01;

            if (x > 2 && x < 6 && y > 2 && y < 6)
                return SudokuBlock.B11;

            if (x > 5 && y > 2 && y < 6)
                return SudokuBlock.B21;

            if (x < 3 && y > 5)
                return SudokuBlock.B02;

            if (x > 2 && x < 6 && y > 5)
                return SudokuBlock.B12;

            return SudokuBlock.B22;
        }
    }
}
