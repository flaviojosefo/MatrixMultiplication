namespace MatrixMultiplication {

    internal class Program {

        static void Main(string[] args) {

            Matrix mat1 = new(1, 4);
            Matrix mat2 = new Matrix(4, 4).Transposed;

            Matrix mat3 = Matrix.MatMul(mat1, mat2);
            Matrix mat4 = Matrix.Transpose(mat3);

            Console.WriteLine(mat1);
            Console.WriteLine(mat2);
            Console.WriteLine(mat3);
            Console.WriteLine(mat4);

            Console.ReadKey();
        }
    }
}