using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication {

    internal struct Matrix {

        private readonly float[] matrix;

        public int Rows { get; }
        public int Cols { get; }

        public int Length { get => Rows * Cols; }

        public Matrix(int rows, int cols) {

            Rows = rows;
            Cols = cols;

            matrix = new float[Rows * Cols];

            for (int i = 0; i < Rows * Cols; i++) {

                matrix[i] = i;
            }
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
