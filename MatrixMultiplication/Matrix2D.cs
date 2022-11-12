namespace MatrixMultiplication {

    internal struct Matrix2D {

        private float[,] matrix;

        public int Rows { get => matrix.GetLength(0); }
        public int Cols { get => matrix.GetLength(1); }

        public int Length { get => Rows * Cols; }

        public float this[int i, int j] { get => matrix[i, j]; }

        public static implicit operator Matrix2D(float[,] m) => new() { matrix = m };

        public Matrix2D(int rows, int cols, bool fill = false, float? toFill = null) {

            matrix = new float[rows, cols];

            if (fill) {

                int k = 0;

                for (int i = 0; i < rows; i++) {

                    for (int j = 0; j < cols; j++) {

                        matrix[i, j] = toFill ?? k++;
                    }
                }
            }
        }

        // Multiply 2 matrices and return the resulting matrix [classic]
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

        public static Matrix2D MatMulP(Matrix2D m1, Matrix2D m2, int tasks) {

            // Store Rows/Cols values of m1 and m2
            int m = m1.Rows;
            int n = m2.Cols;
            int l = m1.Cols;

            // Verify if given matrices are valid
            if (l != m2.Rows) {

                Console.WriteLine("Classic Parallel Matrix Multiplication Failed: Cols of m1 must match Rows of m2!\n");
                return new(m, n);
            }

            // The matrix to be returned
            float[,] newMat = new float[m, n];

            // Half the number of tasks acts as division for partition of m2
            int halfTasks = tasks / 2;

            // Matrix partitions boundaries
            int halfRowsM1 = m / 2;
            int divColsM2 = n / halfTasks;

            // The tasks to initiate
            Task[] partMuls = new Task[tasks];

            // Loop through the number of tasks
            for (int i = 0; i < tasks; i++) {

                // Calculate "switches" that control m1 and m2 indexation
                int m1Switch = -((i % halfTasks) - i) / halfTasks;
                int m2Switch = i % halfTasks;

                // Calculate on which row/col the multiplications should begin for the current partition
                int m1Begin = halfRowsM1 * m1Switch;
                int m2Begin = divColsM2 * m2Switch;

                // Calculate on which row/col the multiplications should end for the current partition
                int m1End = m1Begin + halfRowsM1;
                int m2End = m2Begin + divColsM2;

                // Inititate a task (queue a partition multiplication on the thread pool)
                partMuls[i] = Task.Run(() => {

                    // Standard (double index) matrix multiplication algorithm
                    for (int i = m1Begin; i < m1End; i++) {

                        for (int j = m2Begin; j < m2End; j++) {

                            float sum = 0;

                            for (int k = 0; k < l; k++) {

                                sum += m1[i, k] * m2[k, j];
                            }

                            newMat[i, j] = sum;
                        }
                    }
                });
            }

            // Wait for all tasks to finish
            Task.WaitAll(partMuls);

            //Parallel.For(0, tasks, (i) => {

            //    int m1Switch = -((i % halfTasks) - i) / halfTasks;
            //    int m2Switch = i % halfTasks;

            //    int m1Begin = halfRowsM1 * m1Switch;
            //    int m2Begin = divColsM2 * m2Switch;

            //    int m1End = m1Begin + halfRowsM1;
            //    int m2End = m2Begin + divColsM2;

            //    for (int ix = m1Begin; ix < m1End; ix++) {

            //        for (int j = m2Begin; j < m2End; j++) {

            //            float sum = 0;

            //            for (int k = 0; k < l; k++) {

            //                sum += m1[ix * l + k] * m2[k * n + j];
            //            }

            //            newMat[ix * n + j] = sum;
            //        }
            //    }
            //});

            // Return the newly created matrix
            return newMat;
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
