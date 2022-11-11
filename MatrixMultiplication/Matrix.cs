using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static Matrix MatMulP(Matrix m1, Matrix m2) {

            int l = m1.Cols;

            if (l != m2.Rows) {

                Console.WriteLine("Classic Matrix Multiplication Failed: Cols of m1 must match Rows of m2!\n");
                return new(m1.Rows, m2.Cols);
            }

            Task[] tasks = new Task[4];

            int m = m1.Rows;
            int n = m2.Cols;

            float[] newMat = new float[m * n];

            int halfRowsM1 = m / 2;
            int halfColsM2 = n / 2;

            for (int i = 0; i < 4; i++) {

                int m1Switch = -((i % 2) - i) / 2;
                int m2Switch = i % 2;

                int m1Begin = halfRowsM1 * m1Switch;
                int m2Begin = halfColsM2 * m2Switch;

                int m1End = (m1Begin * m1Switch) + halfRowsM1;
                int m2End = (m2Begin * m2Switch) + halfColsM2;

                tasks[i] = Task.Factory.StartNew(() => {

                    for (int i = m1Begin; i < m1End; i++) {

                        for (int j = m2Begin; j < m2End; j++) {

                            float sum = 0;

                            for (int k = 0; k < l; k++) {

                                sum += m1[i * l + k] * m2[k * n + j];
                            }

                            newMat[i * n + j] = sum;
                        }
                    }
                });
            }

            Task.WaitAll(tasks);

            return new Matrix(m, n) { matrix = newMat };
        }

        // Divides a Matrix horizontally in 2 (NOT USED IN PARALLEL MULTIPLICATION)
        public static Matrix[] PartitionHorizontal(Matrix m) {

            int rows = m.Rows / 2;
            int cols = m.Cols;

            int odd = m.Rows % 2;

            Matrix[] matrices = new Matrix[2];

            for (int i = 0; i < 2; i++) {

                rows += odd * i;

                int length = rows * cols;

                float[] newMat = new float[length];

                int shift = (rows - odd) * cols * i; // The amount of indexes the matrix should skip towards

                for (int j = 0; j < length; j++) {

                    newMat[j] = m[shift + j];
                }

                matrices[i] = new Matrix(rows, cols) { matrix = newMat };
            }

            return matrices;
        }

        // Divides a Matrix vertically in 2 (NOT USED IN PARALLEL MULTIPLICATION)
        public static Matrix[] PartitionVertical(Matrix m) {

            int rows = m.Rows;
            int cols = m.Cols / 2;

            int odd = m.Cols % 2;

            Matrix[] matrices = new Matrix[2];

            for (int i = 0; i < 2; i++) {

                cols += odd * i;

                int length = rows * cols;

                float[] newMat = new float[length];

                int shift = (cols - odd) * i;
                int slot = cols + (-((i % cols) - (odd - i)) * odd);

                for (int j = 0; j < length; j++) {

                    int row = -((j % cols) - j) / cols;
                    newMat[j] = m[(j + shift) + (row * slot)];
                }

                matrices[i] = new Matrix(rows, cols) { matrix = newMat };
            }

            return matrices;
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

                    linearMat[j + (i * m.Cols)] = m[i, j]; // x * (y * Width) -> Row Major
                }
            }

            return new Matrix(m.Rows, m.Cols) { matrix = linearMat };
        }

        // Return a readeable Matrix text representation
        public override string ToString() {

            if (Length > 250) {

                int toShow = 5;

                string firstValues = $"Showing first {toShow} indexes: ";
                string lastValues = $"Showing last {toShow} indexes: ";

                int shiftEnd = Length - 1 - toShow;

                for (int i = 0; i < toShow; i++) {

                    firstValues += $"{matrix[i]}" + (i == toShow - 1 ? '\n' : ", ");
                    lastValues += $"{matrix[shiftEnd + i]} " + (i == toShow - 1 ? '\n' : ", ");
                }

                return firstValues + lastValues;
            }

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
