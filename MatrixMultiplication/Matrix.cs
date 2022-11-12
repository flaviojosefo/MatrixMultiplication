namespace MatrixMultiplication {

    internal struct Matrix {

        private float[] matrix;

        public int Rows { get; }
        public int Cols { get; }

        public int Length { get => Rows * Cols; }

        public Matrix Transposed => Transpose(this);

        public Matrix(int rows, int cols, bool fill = false, float? toFill = null) {

            Rows = rows;
            Cols = cols;

            matrix = new float[Rows * Cols];

            // Should the array be filled?
            if (fill) {

                for (int i = 0; i < matrix.Length; i++) {

                    matrix[i] = toFill ?? i; // Fill the array with a specific float, if given
                }
            }
        }

        public float this[int i] { get => matrix[i]; }

        // Multiply 2 matrices and return the resulting matrix [classic]
        public static Matrix MatMulC(Matrix m1, Matrix m2) {

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
            float[] newMat = new float[m * n];

            // Standard (single index) matrix multiplication algorithm
            for (int i = 0; i < m; i++) {

                for (int j = 0; j < n; j++) {

                    float sum = 0;

                    for (int k = 0; k < l; k++) {

                        sum += m1[i * l + k] * m2[k * n + j];
                    }

                    newMat[i * n + j] = sum;
                }
            }

            // Return the newly created matrix
            return new Matrix(m, n) { matrix = newMat };
        }

        // Multiply 2 matrices and return the resulting matrix [transposed]
        public static Matrix MatMulT(Matrix m1, Matrix m2t) {

            // Store Rows/Cols values of m1 and m2t
            int m = m1.Rows;
            int n = m2t.Rows; // -> m2.Cols
            int l = m1.Cols;
            int p = m2t.Cols; // -> m2.Rows

            // Verify if given matrices are valid
            if (l != p) {

                Console.WriteLine("Transposed Matrix Multiplication Failed: Cols of m1 must match Cols of m2t!\n");
                return new(m, n);
            }

            // The matrix to be returned
            float[] newMat = new float[m * n];

            // Transposed (single index) matrix 'multiplication' algorithm
            for (int i = 0; i < m; i++) {

                for (int j = 0; j < n; j++) {

                    float sum = 0;

                    for (int k = 0; k < l; k++) {

                        sum += m1[i * l + k] * m2t[j * l + k];
                    }

                    newMat[i * n + j] = sum;
                }
            }

            // Return the newly created matrix
            return new Matrix(m, n) { matrix = newMat };
        }

        // Multithreaded approach to Classic matrix multiplication
        public static Matrix MatMulPC(Matrix m1, Matrix m2, int tasks) {

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
            float[] newMat = new float[m * n];

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

                    // Standard (single index) matrix multiplication algorithm
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
            return new Matrix(m, n) { matrix = newMat };
        }

        // Multithreaded approach to Transposed matrix 'multiplication'
        public static Matrix MatMulPT(Matrix m1, Matrix m2t, int tasks) {

            // Store Rows/Cols values of m1 and m2t
            int m = m1.Rows;
            int n = m2t.Rows; // -> m2.Cols
            int l = m1.Cols;

            // Verify if given matrices are valid
            if (l != m2t.Cols) {

                Console.WriteLine("Classic Parallel Matrix Multiplication Failed: Cols of m1 must match Rows of m2!\n");
                return new(m, n);
            }

            // The matrix to be returned
            float[] newMat = new float[m * n];

            // Half the number of tasks acts as division for partition of m2t
            int halfTasks = tasks / 2;

            // Matrix partitions boundaries
            int halfRowsM1 = m / 2;
            int divColsM2 = n / halfTasks;

            // The tasks to initiate
            Task[] partMuls = new Task[tasks];

            // Loop through the number of tasks
            for (int i = 0; i < tasks; i++) {

                // Calculate "switches" that control m1 and m2t indexation
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

                    // Transposed (single index) matrix multiplication algorithm
                    for (int i = m1Begin; i < m1End; i++) {

                        for (int j = m2Begin; j < m2End; j++) {

                            float sum = 0;

                            for (int k = 0; k < l; k++) {

                                sum += m1[i * l + k] * m2t[j * l + k];
                            }

                            newMat[i * n + j] = sum;
                        }
                    }
                });
            }

            Task.WaitAll(partMuls);

            // Return the newly created matrix
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

            // Show the 5 first/last values if the Matrix is too long
            if (Length > 100) {

                int toShow = 5;

                string firstValues = $"Showing first {toShow} indexes: ";
                string lastValues =  $"Showing last  {toShow} indexes: ";

                int shiftEnd = Length - toShow;

                for (int i = 0; i < toShow; i++) {

                    firstValues += $"{matrix[i]}" + (i == toShow - 1 ? '\n' : ", ");
                    lastValues += $"{matrix[shiftEnd + i]}" + (i == toShow - 1 ? '\n' : ", ");
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
