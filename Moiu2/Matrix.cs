using System;
using System.Text;

namespace Moiu2
{
    /// <summary>
    /// The rectangular array or table of numbers, symbols, or expressions,
    /// arranged in rows and columns, which is used to represent a mathematical object or a property of such an object.
    /// </summary>
    public class Matrix : ICloneable
    {
        private double[,] _data;

        /// <summary>
        /// Creates matrix with dimension n n m.
        /// </summary>
        /// <param name="n">Number of matrix rows.</param>
        /// <param name="m">Number of matrix columns.</param>
        public Matrix(int n, int m)
        {
            Data = new double[n, m];
        }

        /// <summary>
        /// Creates square matrix of order n.
        /// </summary>
        /// <param name="n">Order of square matrix.</param>
        public Matrix(int n) : this(n, n) { }

        public Matrix(double[,] vs)
        {
            Data = (double[,])vs.Clone();
        }

        /// <summary>
        /// Number of matrix rows.
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Number of matrix columns.
        /// </summary>
        public int M { get; private set; }

        /// <summary>
        /// Gets element on n x m position.
        /// </summary>
        /// <param name="n">Row position.</param>
        /// <param name="m">Column position.</param>
        /// <returns></returns>
        public double this[int n, int m]
        {
            get => Data[n, m];
            set => Data[n, m] = value;
        }

        /// <summary>
        /// Gets element of vector by on n position.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public double this[int n]
        {
            get => M == 1
                ? Data[n, 0]
                : Data[0, n];
            set
            {
                if (M == 1)
                {
                    Data[n, 0] = value;
                }
                else
                {
                    Data[0, n] = value;
                }
            }
        }

        private double[,] Data
        {
            get => _data;
            set
            {
                _data = value;
                UpdateDataBounds();
            }
        }

        /// <summary>
        /// Gets a difference between matrix objects.
        /// </summary>
        /// <param name="left">Left operand of the difference.</param>
        /// <param name="right">Right operand of the difference.</param>
        /// <returns>New instance of <see cref="Matrix"/> that represents the difference between instances.</returns>
        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (!(left.N == right.N & left.M == right.M))
            {
                throw new ArgumentException(
                    $"Dimension should be equals but was {left.N}x{left.M} and {right.N}x{right.M}.");
            }

            var result = new Matrix(left.N, right.M);

            for (var i = 0; i < left.N; i++)
            {
                for (var j = 0; j < left.M; j++)
                {
                    result[i, j] = left[i, j] - right[i, j];
                }
            }

            return result;

        }

        /// <summary>
        /// Gets the sum between matrix objects.
        /// </summary>
        /// <param name="left">Left operand of the sum.</param>
        /// <param name="right">Right operand of the sum.</param>
        /// <returns>New instance of <see cref="Matrix"/> that represents the sum between instances.</returns>
        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (!(left.N == right.N & left.M == right.M))
            {
                throw new ArgumentException(
                    $"Dimension should be equals but was {left.N}x{left.M} and {right.N}x{right.M}.");
            }

            var result = new Matrix(left.N, right.M);

            for (var i = 0; i < left.N; i++)
            {
                for (var j = 0; j < left.M; j++)
                {
                    result[i, j] = left[i, j] + right[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the matrix multiplied by a number.
        /// </summary>
        /// <param name="matrix">The instance of <see cref="Matrix"/>.</param>
        /// <param name="value">The number.</param>
        /// <returns>New instance of <see cref="Matrix"/>
        /// all elements of which are multiplied by number.</returns>
        public static Matrix operator *(Matrix matrix, double value)
        {
            var result = new Matrix(matrix.N, matrix.M);

            for (var i = 0; i < matrix.N; i++)
            {
                for (var j = 0; j < matrix.M; j++)
                {
                    result[i, j] = matrix[i, j] * value;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the matrix multiplied by a number.
        /// </summary>
        /// <param name="matrix">The instance of <see cref="Matrix"/>.</param>
        /// <param name="value">The number.</param>
        /// <returns>New instance of <see cref="Matrix"/>
        /// all elements of which are multiplied by number.</returns>
        public static Matrix operator *(double value, Matrix matrix)
        {            
            return matrix * value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix operator *(Matrix left, Matrix right)
        {
            var result = new Matrix(left.N, right.M);

            if (left.M != right.N)
            {
                throw new ArgumentException(
                    "Rows of right matrix should be equal the number of columns of the left matrix." +
                    $" But was {right.N} and {left.M}");
            }

            for (var i = 0; i < left.N; i++)
            {
                for (var j = 0; j < right.M; j++)
                {
                    var t = 0.0;

                    for (var s = 0; s < left.M; s++)
                    {
                        t += left[i, s] * right[s, j];
                    }

                    result[i, j] = t;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var str = new StringBuilder();

            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    str.Append(Data[i, j].ToString("0.00") + " ");
                }

                str.Append("\n");
            }

            return str.ToString();
        }

        /// <inheritdoc/>
        public object Clone()
        {
            return new Matrix((double[,])Data.Clone());
        }

        /// <summary>
        /// Creates the identity matrix.
        /// The identity matrix of size N x N is the N x N square matrix
        /// with ones on the main diagonal and zeros elsewhere.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>New instance of <see cref="Matrix"/> that represents identity matrix.</returns>
        public static Matrix E(int n)
        {
            var res = new Matrix(n);

            for (var i = 0; i < n; i++)
            {
                res[i, i] = 1;
            }

            return res;
        }

        /// <summary>
        /// Creates a matrix-vector with dimension n and 1 on i position and zeros elsewhere.
        /// </summary>
        /// <param name="n">Number of matrix rows.</param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Matrix E(int n, int i)
        {
            var res = new Matrix(1, n)
            {
                [0, i] = 1
            };

            return res;
        }

        public static Matrix CreateRowVector(int length, double value = 0)
        {
            var res = new Matrix(1, length);

            for(var i = 0; i < res.M; i++)
            {
                res[0, i] = value;
            }

            return res;
        }

        public static Matrix CreateColumnVector(int length, double value = 0)
        {
            var res = new Matrix(length, 1);

            for (var i = 0; i < res.N; i++)
            {
                res[i, 0] = value;
            }

            return res;
        }


        public Matrix GetRow(int i)
        {
            var res = new Matrix(1, M);

            for (var j = 0; j < M; j++)
            {
                res[0, j] = this[i, j];
            }

            return res;
        }


        public Matrix GetTransposeMatrix()
        {
            var res = new Matrix(M, N);

            for (var i = 0; i < M; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    res[i, j] = Data[j, i];
                }
            }

            return res;
        }

        public Matrix GetColumn(int j)
        {
            var res = new Matrix(N, 1);
            
            for(var i = 0; i < N; i++)
            {
                res[i, 0] = this[i, j];
            }

            return res;
        }

        public Matrix RemoveRow(int row)
        {
            var res = new Matrix(N - 1, M);
            var add = 0;
            for (var i = 0; i < N; i++)
            {
                if (i == row)
                {
                    add = -1;
                    continue;
                }
                for (var j = 0; j < M; j++)
                {
                    res[i + add, j] = Data[i, j];
                } 
            }
            return res;
        }

        public Matrix RemoveCol(int col)
        {
            var res = new Matrix(N, M - 1);
            for(var i = 0; i < N; i++)
            {
                var add = 0;
                for(var j = 0; j < M; j++)
                {
                    if(j == col)
                    {
                        add = -1;
                        continue;
                    }
                    res[i, j + add] = _data[i, j];
                }
            }
            return res;
        }

        public Matrix RemoveRC(int row, int col)
        {
            var res = new Matrix(N - 1, M - 1);
            var addR = 0;
            for(var i = 0; i < N; i++)
            {
                var addC = 0;
                if(i == row)
                {
                    addR = -1;
                    continue;
                }
                for(var j = 0; j < M; j++)
                {
                    if(j == col)
                    {
                        addC = -1;
                        continue;
                    }
                    res[i + addR, j + addC] = Data[i, j];
                }
            }
            return res;
        }

        public Matrix Extend(Matrix matrix)
        {
            if (N == matrix.N)
            {
                var res = new Matrix(N, M + matrix.M);

                for (var i = 0; i < N; i++)
                {
                    for (var j = 0; j < M; j++)
                    {
                        res[i, j] = Data[i, j];
                    }
                    for (var j = M; j < M + matrix.M; j++)
                    {
                        res[i, j] = matrix[i, j-M];
                    }
                }
                return res;
            }
            throw new Exception("Cannot extend with matrix");
        }

        public Matrix AppendRow(Matrix matrix)
        {
            Matrix res;
            if(N == 1 && matrix.N == 1)
            {
                res = new Matrix(1, M + matrix.M);
                for (var j = 0; j < M; j++)
                    res[0, j] = Data[0, j];
                for (var j = M; j < M + matrix.M; j++)
                    res[0, j] = matrix[0, j - M];
                return res;
            }
            throw new Exception("cant append");
        }

        public Matrix AppendCol(Matrix matrix)
        {
            Matrix res;
            if(M == 1 && matrix.M == 1)
            {
                res = new Matrix(N + matrix.N, 1);
                for (var i = 0; i < N; i++)
                    res[i, 0] = Data[i, 0];                
                for(var i = N; i < N + matrix.N; i++)
                    res[i, 0] = matrix[i - N, 0];                
                return res;
            }
            throw new Exception("cant append");
        }

        public Matrix ExtendE()
        {
            return Extend(Matrix.E(N));
        }

        public Matrix GetExtendPart()
        {
            var res = new Matrix(N, M / 2);

            for(var i = 0; i < res.N; i++)
            {
                for (var j = 0; j < res.M; j++)
                {
                    res[i, j] = this[i, M / 2 + j];
                }
            }

            return res;
        }

        private void ChangeRows(int a, int b)
        {
            for(var i = 0; i < M; i++)
            {
                var temp = Data[a, i];
                Data[a, i] = Data[b, i];
                Data[b, i] = temp;
            }
        }

        private int FindMaxIndexInCol(int j)
        {
            var res = -1;
            var val = 1E-20;
            for(var i = 0; i < N; i++)
            {
                if(Math.Abs(Data[i,j]) > Math.Abs(val))
                {
                    res = i;
                    val = _data[i, j];
                }
            }
            return res;
        }

        public Matrix GetDiagonolizeUnder()
        {
            var res = new Matrix(this);

            for (var i = 0; i < N - 1; i++)
            {
                for(var j = i + 1; j < N; j++)
                {
                    if(Math.Abs(res[i,i]) < 1.0E-20)
                    {
                        var max = res.FindMaxIndexInCol(i);
                        if (max == -1)
                            throw new Exception("Матрица необратима");
                        else
                        {
                            res.ChangeRows(max, i);
                        }
                    }
                    var temp = res[j, i];                    
                    for (var k = 0; k < M; k++)
                    {
                        res[j, k] -= res[i, k] * temp / res[i, i];
                    }
                }
            }
            return res;
        }

        public Matrix GetDiagonolizeUpper()
        {
            var res = new Matrix(this);

            for (var i = N - 1; i >= 0; i--)
            {
                for(var j = i - 1; j >= 0; j--)
                {
                    if(Math.Abs(res[i,i]) < 1E-20)
                    {
                        var max = res.FindMaxIndexInCol(i);
                        if (max == -1)
                            throw new Exception("Матрица необратима");
                        else
                        {
                            res.ChangeRows(max, i);
                        }
                    }
                    var temp = res[j, i];
                    for (var k = 0; k < M; k++)
                    {
                        res[j, k] -= res[i, k] * temp / res[i, i];
                    }
                }
            }

            return res;
        }

        public double GetDeterminant()
        {
            var diagonolize = GetDiagonolizeUnder().GetDiagonolizeUpper();
            double det = 1;

            for(var i = 0; i < N; i++)
            {
                det *= diagonolize[i, i];
            }
            return det;
        }

        public Matrix GetReverse()
        {
            var diagonolize = ExtendE()
                .GetDiagonolizeUnder()
                .GetDiagonolizeUpper();

            for(var i = 0; i < N; i++)
            {
                var temp = diagonolize[i, i];

                for (var j = 0; j < diagonolize.M; j++)
                {
                    diagonolize[i, j] /= temp;
                }
            }
            return diagonolize.GetExtendPart();
        }

        public void ReplaceVector(Matrix vector, int pos)
        {
            if (pos < N)
            {
                for (var i = 0; i < vector.N; i++)
                {
                    _data[i, pos] = vector[i, 0];
                }
            }
        }

        public static Matrix GetReverse(Matrix A, Matrix A_reverce, Matrix x, int i)
        {
            var matrix = new Matrix(A);

            matrix.ReplaceVector(x, 0);
            var L = A_reverce * x;
            if(L[i]==0)
            {
                throw new Exception("Матрица необратима.");
            }
            var L_1 = new Matrix(L);
            L_1[i, 0] = -1;
            var L_2 = L_1 * (-1 / L[i, 0]);
            var E = Matrix.E(A.N);
            E.ReplaceVector(L_2, i);
            return E * A_reverce;
        }

        private void UpdateDataBounds()
        {
            if (Data != null)
            {
                N = Data.GetUpperBound(0) + 1;

                if (N != 0)
                {
                    M = Data.Length / N;
                }
            }
            else
            {
                N = 0;
                M = 0;
            }
        }
    }
}
