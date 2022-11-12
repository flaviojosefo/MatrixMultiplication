namespace MatrixMultiplication {

    internal struct Matrix2D {

        private float[,] matrix;

        public int Rows { get => matrix.GetLength(0); }
        public int Cols { get => matrix.GetLength(1); }

        public int Length { get => Rows * Cols; }

        public float this[int i, int j] { get => matrix[i, j]; }

        public static implicit operator Matrix2D(float[,] m) => new Matrix2D { matrix = m };

        public Matrix2D(int rows, int cols) {

            matrix = new float[rows, cols];

            int k = 0;

            for (int i = 0; i < rows; i++) {

                for (int j = 0; j < cols; j++) {

                    matrix[i, j] = k++;
                }
            }
        }

        public static Matrix2D MatMul(Matrix2D m1, Matrix2D m2) {

            // Store Rows/Cols values of m1 and m2
            int m = m1.Rows;
            int n = m2.Cols;
            int l = m1.Cols;
            int p = m2.Rows;

            // Verify if given matrices are valid
            if (l != p) {

                Console.WriteLine("Classic Matrix Multiplication Failed: Cols of m1 must match Rows of m2!\n");
                return new(m, n);
            }

            // The matrix to be returned
            float[,] newMatrix = new float[m, n];

            // Standard (double index) matrix multiplication algorithm
            for (int i = 0; i < m; i++) {

                for (int j = 0; j < n; j++) {

                    float sum = 0;

                    for (int k = 0; k < l; k++) {

                        sum += m1[i, k] * m2[k, j];
                    }

                    newMatrix[i, j] = sum;
                }
            }

            // Return the newly created matrix
            return newMatrix;
        }

        // Return a readeable Matrix text representation
        public override string ToString() {

            // Show the 5 first/last values if the Matrix is too long
            if (Length > 250) {

                int toShow = 5;

                string firstValues = $"Showing first {toShow} indexes: ";
                string lastValues = $"Showing last  {toShow} indexes: ";

                int shiftEnd = Cols - toShow;

                for (int i = 0; i < toShow; i++) {

                    firstValues += $"{matrix[0, i]}" + (i == toShow - 1 ? '\n' : ", ");
                    lastValues += $"{matrix[Rows - 1, shiftEnd + i]}" + (i == toShow - 1 ? '\n' : ", ");
                }

                return firstValues + lastValues;
            }

            string mat = "";

            for (int i = 0; i < Rows; i++) {

                for (int j = 0; j < Cols; j++) {

                    mat += $"{matrix[i, j]} ";
                }

                mat += '\n';
            }

            return mat;
        }
    }
}
