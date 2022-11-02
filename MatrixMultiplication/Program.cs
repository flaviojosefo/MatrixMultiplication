using System.Diagnostics;

namespace MatrixMultiplication {

    internal class Program {

        static void Main(string[] args) {

            Matrix mat1 = new Matrix(3, 5);
            Matrix mat2 = new Matrix(5, 3);

            Matrix mat2T = mat2.Transposed;

            Stopwatch sw = Stopwatch.StartNew();

            Matrix mat3 = Matrix.MatMulC(mat1, mat2);
            double classic = sw.Elapsed.TotalMilliseconds;

            sw.Restart();

            Matrix mat4 = Matrix.MatMulT(mat1, mat2, mat2T);
            double transposed = sw.Elapsed.TotalMilliseconds;

            sw.Stop();

            Console.WriteLine(mat1);
            Console.WriteLine(mat2);

            Console.Write(mat3);
            Console.WriteLine($"Time (classic): {classic:0.00}\n");

            Console.Write(mat4);
            Console.WriteLine($"Time (transposed): {transposed:0.00}\n");

            string speedUp = (transposed > classic ? 
                $"Slow down: {(transposed / classic):0.00}x" : 
                $"Speed up: {(classic / transposed):0.00}x") + '\n';

            Console.WriteLine(speedUp);

            Console.ReadKey();
        }
    }
}