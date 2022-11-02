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

        public int Length { get => matrix.Length; }

        public Matrix Transposed => Transpose(this);

        public Matrix(int rows, int cols) {

            Rows = rows;
            Cols = cols;

            matrix = new float[Rows * Cols];

            for (int i = 0; i < Rows * Cols; i++) {

                matrix[i] = i;
            }
        }

        public float this[int i] { get => matrix[i]; }

        // Multiply 2 matrices and return the resulting matrix
        public static Matrix MatMul(Matrix m1, Matrix m2) {

            int m = m1.Rows;
            int n = m2.Cols;
            int l = m1.Cols;

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

        public static Matrix Transpose(Matrix mat) {

            int m = mat.Rows;
            int n = mat.Cols;

            float[] transposed = new float[mat.Length];

            for (int i = 0; i < m; i++) {

                for (int j = 0; j < n; j++) {

                    transposed[j + i * n] = mat[i + j * m];
                }
            }

            return new Matrix(n, m) { matrix = transposed };
        }

        // Return a readeable Matrix text representation
        public override string ToString() {

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
