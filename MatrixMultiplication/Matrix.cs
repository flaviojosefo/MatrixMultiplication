using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication {

    internal struct Matrix {

        private float[] matrix;

        public int Rows { get; }
        public int Cols { get; }

        public int Length { get => Rows * Cols; }

        public Matrix Transposed => Transpose(this);

        public Matrix(int rows, int cols) {

            Rows = rows;
            Cols = cols;

            matrix = new float[Rows * Cols];

            for (int i = 0; i < matrix.Length; i++) {

                matrix[i] = i;
            }
        }

        public float this[int i] { get => matrix[i]; }

        // Multiply 2 matrices and return the resulting matrix [classic]
        public static Matrix MatMulC(Matrix m1, Matrix m2) {

            int m = m1.Rows;
            int n = m2.Cols;
            int l = m1.Cols;
            int p = m2.Rows;

            if (l != p) {

                Console.WriteLine("Classic Matrix Multiplication Failed: Cols of m1 must match Rows of m2!\n");
                return new(m, n);
            }

            float[] newMat = new float[m * n];

            for (int i = 0; i < m; i++) {

                for (int j = 0; j < n; j++) {

                    float sum = 0;

                    for (int k = 0; k < l; k++) {

                        sum += m1[i * l + k] * m2[k * n + j];
                    }

                    newMat[i * n + j] = sum;
                }
            }

            return new Matrix(m, n) { matrix = newMat };
        }

        // Multiply 2 matrices and return the resulting matrix [transposed]
        public static Matrix MatMulT(Matrix m1, Matrix m2t) {

            int m = m1.Rows;
            int n = m2t.Rows; // -> m2.Cols
            int l = m1.Cols;
            int p = m2t.Cols; // -> m2.Rows

            if (l != p) {

                Console.WriteLine("Transposed Matrix Multiplication Failed: Cols of m1 must match Cols of m2t!\n");
                return new(m, n);
            }

            float[] newMat = new float[m * n];

            for (int i = 0; i < m; i++) {

                for (int j = 0; j < n; j++) {

                    float sum = 0;

                    for (int k = 0; k < l; k++) {

                        sum += m1[i * l + k] * m2t[j * l + k];
                    }

                    newMat[i * n + j] = sum;
                }
            }

            return new Matrix(m, n) { matrix = newMat };
        }

        public static Matrix Transpose(Matrix mat) {

            int m = mat.Rows;
            int n = mat.Cols;

            float[] transposed = new float[mat.Length];

            for (int i = 0; i < m; i++) {

                for (int j = 0; j < n; j++) {

                    transposed[i + j * m] = mat[j + i * n];
                }
            }

            return new Matrix(n, m) { matrix = transposed };
        }

        public static Matrix Linearise(Matrix2D m) {

            float[] linearMat = new float[m.Length];

            for (int i = 0; i < m.Rows; i++) {

                for (int j = 0; j < m.Cols; j++) {

                    linearMat[j + (i * m.Cols)] = m[i, j];
                }
            }

            return new Matrix(m.Rows, m.Cols) { matrix = linearMat };
        }

        // Return a readeable Matrix text representation
        public override string ToString() {

            if (Length > 250)
                return "Matrix is too big to be drawn!\n";

            string mat = "";

            for (int i = 0; i < Rows; i++) {

                for (int j = 0; j < Cols; j++) {

                    mat += $"{matrix[j + i * Cols]} ";
                }

                mat += '\n';
            }

            return mat;
        }
    }
}
