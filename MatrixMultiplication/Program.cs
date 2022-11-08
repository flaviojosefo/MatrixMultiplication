using System.Diagnostics;

namespace MatrixMultiplication {

    internal class Program {

        static void Main(string[] args) {

            int m = 3;
            int n = 5;

            Matrix mat1 = new Matrix(m, n);
            Matrix mat2 = new Matrix(n, m);

            Matrix mat2T = mat2.Transposed;

            Matrix2D mat5 = new Matrix2D(m, n);
            Matrix2D mat6 = new Matrix2D(n, m);

            Stopwatch sw = Stopwatch.StartNew();

            Matrix2D mat7 = Matrix2D.MatMul(mat5, mat6);
            double classicDouble = sw.Elapsed.TotalMilliseconds;

            sw.Restart();

            Matrix mat3 = Matrix.MatMulC(mat1, mat2);
            double classicLinear = sw.Elapsed.TotalMilliseconds;

            sw.Restart();

            Matrix mat4 = Matrix.MatMulT(mat1, mat2T);
            double transposed = sw.Elapsed.TotalMilliseconds;

            sw.Stop();

            //Console.WriteLine(mat1);
            //Console.WriteLine(mat2);

            Console.WriteLine("Calculating...\n");

            Console.Write(mat7);
            Console.WriteLine($"Time (Classic - Double): {classicDouble:0.00} ms\n");

            Console.Write(mat3);
            Console.WriteLine($"Time (Classic - Linear): {classicLinear:0.00} ms\n");

            Console.Write(mat4);
            Console.WriteLine($"Time (Transposed): {transposed:0.00} ms\n");

            string dlSpeed = classicLinear > classicDouble ?
                $"    Double to Linear: Slow down of {classicLinear / classicDouble:0.00}x\n" :
                $"    Double to Linear: Speed up of {classicDouble / classicLinear:0.00}x\n";

            string dtSpeed = transposed > classicDouble ?
                $"Double to Transposed: Slow down of {transposed / classicDouble:0.00}x\n" :
                $"Double to Transposed: Speed up of {classicDouble / transposed:0.00}x\n";

            string ltSpeed = transposed > classicLinear ?
                $"Linear to Transposed: Slow down of {transposed / classicLinear:0.00}x\n" :
                $"Linear to Transposed: Speed up of {classicLinear / transposed:0.00}x\n";

            Console.WriteLine(dlSpeed + dtSpeed + ltSpeed);

            Console.ReadKey();
        }
    }
}