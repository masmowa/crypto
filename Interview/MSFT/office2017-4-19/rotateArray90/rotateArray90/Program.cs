using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * Rotate a 2-dimensional array by 90 degrees.

Sample Input:

2   5 
9   16 
3   4 
6   1

Output
6   3   9    2 
1   4   16  5
    Advanced 
        1) rotate 180 degrees
            Output:
             1  6
             4  3
            16  9
             5  2
        2) rotate 260 degrees
          
 */ 
namespace rotateArray90
{
    public class RotateArray
    {
        public RotateArray()
        {
        }
        public int OffsetRow { get; set; }
        public int OffsetCol { get; set; }
        public int ColDir { get; set; }
        public int RowDir { get; set; }

        protected int doRotate(int[,] input, int row, int col)
        {
            int rowNdx = (OffsetRow + (row * RowDir));
            int colNdx = (OffsetCol + (col * ColDir));
            // debug
            //Console.WriteLine("rc({0}, {1})  input[{2},{3}] = {2}", row, col, rowNdx, colNdx, input[rowNdx, colNdx]);
            return input[rowNdx, colNdx];
        }
        public void Run(int[,] input, int rdir, int cdir)
        {
            RowDir = rdir;
            ColDir = cdir;

            OffsetRow = ((rdir < 0) ? input.GetLength(0) - 1 : 0);
            OffsetCol = ((cdir < 0) ? input.GetLength(1) - 1 : 0);

            // todo: is there a way to have only 1 set of nested for loops?

            StringBuilder output = new StringBuilder("");
            if (rdir == cdir)
            {
                // row fixed while visiting all column(s)
                for (int row = 0; row < input.GetLength(0); ++row)
                {
                    for (int col = 0; col < input.GetLength(1); col++)
                    {
                        output.AppendFormat("{0} ", doRotate(input, row, col));
                    }
                    output.AppendFormat("\n");
                }
            }
            else if (rdir != cdir)
            {
                // col fixed while visiting columns
                for (int col = 0; col < input.GetLength(1); col++)
                {
                    for (int row = 0; row < input.GetLength(0); ++row) 
                    {
                        output.AppendFormat("{0} ", doRotate(input, row, col));
                    }
                    output.AppendFormat("\n");
                }

            }
            Console.WriteLine(output.ToString());

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[,] input = new int[,] { 
                { 2, 5 }, 
                { 9, 16 }, 
                { 3, 4 }, 
                { 6, 1 }
            };


            // rotate 90
            RotateArray ra = new RotateArray();
            ra.Run(input, -1, 1);
            // rotate 180
            ra.Run(input, -1, -1);
            // rotate 260
            ra.Run(input, 1, -1);

        }
    }
}
