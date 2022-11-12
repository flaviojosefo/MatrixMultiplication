using System;
using System.Diagnostics;

namespace MatrixMultiplication {

    internal class Program {

        static void Main(string[] args) {

            int m = 2000;
            int n = 2000;
            int l = 2000;

            Matrix mat1 = new Matrix(m, n, true);
            Matrix mat2 = new Matrix(n, l, true);

            Matrix mat2T = mat2.Transposed;

            //Matrix2D mat5 = new Matrix2D(m, n);
            //Matrix2D mat6 = new Matrix2D(n, l);

            Console.WriteLine("Calculating...\n");

            Stopwatch sw = Stopwatch.StartNew();

            //Matrix2D mat7 = Matrix2D.MatMul(mat5, mat6);
            //double classicDouble = sw.Elapsed.TotalMilliseconds;

            //sw.Restart();

            //Matrix mat3 = Matrix.MatMulC(mat1, mat2);
            //double classicLinear = sw.Elapsed.TotalMilliseconds;

            //sw.Restart();

            //Matrix mat4 = Matrix.MatMulT(mat1, mat2T);
            //double transposed = sw.Elapsed.TotalMilliseconds;

            sw.Restart();

            Matrix matPC = Matrix.MatMulPC(mat1, mat2, 8);
            double linearParallel = sw.Elapsed.TotalMilliseconds;

            sw.Restart();

            Matrix matPT = Matrix.MatMulPT(mat1, mat2T, 8);
            double transposedParallel = sw.Elapsed.TotalMilliseconds;

            sw.Stop();

            //Console.WriteLine(mat1);
            //Console.WriteLine(mat2);

            //Console.Write(mat7);
            //Console.WriteLine($"Time (Classic - Double Index): {classicDouble:0.000} ms\n");

            //Console.Write(mat3);
            //Console.WriteLine($"Time (Classic - Linear): {classicLinear:0.000} ms\n");

            //Console.Write(mat4);
            //Console.WriteLine($"Time (Transposed): {transposed:0.000} ms\n");

            Console.Write(matPC);
            Console.WriteLine($"Time (Linear Parallel): {linearParallel:0.000} ms\n");

            Console.Write(matPT);
            Console.WriteLine($"Time (Transposed Parallel): {transposedParallel:0.000} ms\n");

            //string dlSpeed = classicLinear > classicDouble ?
            //    $"    Double to Linear: Slow down of {classicLinear / classicDouble:0.00}x\n" :
            //    $"    Double to Linear: Speed up of {classicDouble / classicLinear:0.00}x\n";

            //string dtSpeed = transposed > classicDouble ?
            //    $"Double to Transposed: Slow down of {transposed / classicDouble:0.00}x\n" :
            //    $"Double to Transposed: Speed up of {classicDouble / transposed:0.00}x\n";

            //string ltSpeed = transposed > classicLinear ?
            //    $"Linear to Transposed: Slow down of {transposed / classicLinear:0.00}x\n" :
            //    $"Linear to Transposed: Speed up of {classicLinear / transposed:0.00}x\n";

            //string lpSpeed = linearParallel > classicLinear ?
            //    $"Linear to Parallel: Slow down of {linearParallel / classicLinear:0.00}x\n" :
            //    $"Linear to Parallel: Speed up of {classicLinear / linearParallel:0.00}x\n";

            string tpSpeed = transposedParallel > linearParallel ?
                $"Classic Parallel to Transposed Parallel: Slow down of {transposedParallel / linearParallel:0.00}x\n" :
                $"Classic Parallel to Transposed Parallel: Speed up of {linearParallel / transposedParallel:0.00}x\n";

            Console.WriteLine(/*dlSpeed + dtSpeed + ltSpeed + lpSpeed + */tpSpeed);

            Console.ReadKey();
        }
    }
}