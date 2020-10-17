using System;
using System.Collections.Generic;
using System.Text;

namespace Moiu2
{
    class Matrix
    {
        private double[,] data;
        private double[,] Data
        {
            get => data;
            set
            {
                data = value;
                Update();
            }
        }
        public int N { get; private set; }
        public int M { get; private set; }
        private void Update()
        {
            if (Data != null)
            {
                N = Data.GetUpperBound(0) + 1;
                if (N != 0)
                    M = Data.Length / N;
            }
            else
            {
                N = 0;
                M = 0;
            }
        }
        public double this[int x, int y]
        {
            get
            {
                return Data[x, y];
            }
            set
            {
                Data[x, y] = value;
            }
        }
        public double this[int x]
        {
            get
            {
                if (M == 1)
                    return Data[x, 0];
                return Data[0, x];
            }
            set
            {
                if (M == 1)
                    Data[x, 0] = value;
                else Data[0, x] = value;
            }
        }
        public Matrix(int n, int m)
        {
            Data = new double[n, m];
        }
        public Matrix(int n) : this(n, n) { }
        public Matrix(Matrix matrix)
        {
            Data = (double[,])matrix.Data.Clone();
        }
        public Matrix(double[,] vs)
        {
            Data = (double[,])vs.Clone();
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder("");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    str.Append(Data[i, j].ToString("0.00") + " ");
                }
                str.Append("\n");
            }
            return str.ToString();
        }
        public static Matrix operator -(Matrix mat1, Matrix mat2)
        {
            if (mat1.N == mat2.N & mat1.M == mat2.M)
            {
                var result = new Matrix(mat1.N, mat2.M);

                for (int i = 0; i < mat1.N; i++)
                {
                    for (int j = 0; j < mat1.M; j++)
                    {
                        result[i, j] = mat1[i, j] - mat2[i, j];
                    }
                }

                return result;
            }
            else return new Matrix(0, 0);
        }
        public static Matrix operator +(Matrix mat1, Matrix mat2)
        {
            if (mat1.N == mat2.N & mat1.M == mat2.M)
            {
                var result = new Matrix(mat1.N, mat2.M);

                for (int i = 0; i < mat1.N; i++)
                {
                    for (int j = 0; j < mat1.M; j++)
                    {
                        result[i, j] = mat1[i, j] + mat2[i, j];
                    }
                }

                return result;
            }
            else return new Matrix(0, 0);
        }
        public static Matrix operator *(Matrix matrix, double value)
        {
            var result = new Matrix(matrix.N, matrix.M);

            for (int i = 0; i < matrix.N; i++)
            {
                for (int j = 0; j < matrix.M; j++)
                {
                    result[i, j] = matrix[i, j] * value;
                }
            }
            return result;
        }
        public static Matrix operator *(double value, Matrix matrix)
        {            
            return matrix * value;
        }

        public static Matrix operator *(Matrix mat1, Matrix mat2)
        {
            var result = new Matrix(mat1.N, mat2.M);

            if (mat1.M == mat2.N)
            {
                for (int i = 0; i < mat1.N; i++)
                {
                    for (int j = 0; j < mat2.M; j++)
                    {
                        double t = 0.0;
                        for (int s = 0; s < mat1.M; s++)
                        {
                            t += mat1[i, s] * mat2[s, j];
                        }
                        result[i, j] = t;
                    }
                }
                return result;
            }
            return new Matrix(0, 0);
        }
        public static Matrix E(int n)
        {
            Matrix res = new Matrix(n);
            for (int i = 0; i < n; i++)
            {
                res[i, i] = 1;
            }
            return res;
        }
        public static Matrix e(int len, int i)
        {
            Matrix res = new Matrix(1, len);
            res[0, i] = 1;
            return res;
        }
        public static Matrix GetRow(int length, double value = 0)
        {
            Matrix res = new Matrix(1, length);
            for(int i = 0; i < res.M; i++)
            {
                res[0, i] = value;
            }
            return res;
        }
        public static Matrix GetCol(int length, double value = 0)
        {
            Matrix res = new Matrix(length, 1);
            for (int i = 0; i < res.N; i++)
            {
                res[i, 0] = value;
            }
            return res;
        }
        public Matrix GetTranstoseMatrix()
        {
            Matrix res = new Matrix(M, N);

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    res[i, j] = Data[j, i];
                }
            }

            return res;
        }
        public Matrix GetColumn(int j)
        {
            Matrix res = new Matrix(N, 1);
            
            for(int i = 0; i < N; i++)
            {
                res[i, 0] = this[i, j];
            }

            return res;
        }
        public Matrix GetRow(int i)
        {
            Matrix res = new Matrix(1, M);

            for (int j = 0; j < M; j++)
            {
                res[0, j] = this[i, j];
            }

            return res;
        }
        public Matrix RemooveRow(int row)
        {
            Matrix res = new Matrix(N - 1, M);
            int add = 0;
            for (int i = 0; i < N; i++)
            {
                if (i == row)
                {
                    add = -1;
                    continue;
                }
                for (int j = 0; j < M; j++)
                {
                    res[i + add, j] = Data[i, j];
                } 
            }
            return res;
        }
        public Matrix RemooveCol(int col)
        {
            Matrix res = new Matrix(N, M - 1);
            for(int i = 0; i < N; i++)
            {
                int add = 0;
                for(int j = 0; j < M; j++)
                {
                    if(j == col)
                    {
                        add = -1;
                        continue;
                    }
                    res[i, j + add] = data[i, j];
                }
            }
            return res;
        }
        public Matrix RemooveRC(int row, int col)
        {
            Matrix res = new Matrix(N - 1, M - 1);
            int addR = 0;
            for(int i = 0; i < N; i++)
            {
                int addC = 0;
                if(i == row)
                {
                    addR = -1;
                    continue;
                }
                for(int j = 0; j < M; j++)
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
                Matrix res = new Matrix(N, M + matrix.M);

                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        res[i, j] = Data[i, j];
                    }
                    for (int j = M; j < M + matrix.M; j++)
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
                for (int j = 0; j < M; j++)
                    res[0, j] = Data[0, j];
                for (int j = M; j < M + matrix.M; j++)
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
                for (int i = 0; i < N; i++)
                    res[i, 0] = Data[i, 0];                
                for(int i = N; i < N + matrix.N; i++)
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
            Matrix res = new Matrix(N, M / 2);

            for(int i = 0; i < res.N; i++)
            {
                for (int j = 0; j < res.M; j++)
                {
                    res[i, j] = this[i, M / 2 + j];
                }
            }

            return res;
        }
        private void ChangeRows(int a, int b)
        {
            for(int i = 0; i < M; i++)
            {
                var temp = Data[a, i];
                Data[a, i] = Data[b, i];
                Data[b, i] = temp;
            }
        }
        private int FindMaxIndexInCol(int j)
        {
            int res = -1;
            double val = 1E-20;
            for(int i = 0; i < N; i++)
            {
                if(Math.Abs(Data[i,j]) > Math.Abs(val))
                {
                    res = i;
                    val = data[i, j];
                }
            }
            return res;
        }
        public Matrix GetDiagonolizeUnder()
        {
            Matrix res = new Matrix(this);

            for (int i = 0; i < N - 1; i++)
            {
                for(int j = i + 1; j < N; j++)
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
                    double temp = res[j, i];                    
                    for (int k = 0; k < M; k++)
                    {
                        res[j, k] -= res[i, k] * temp / res[i, i];
                    }
                }
            }
            return res;
        }
        public Matrix GetDiagonolizeUpper()
        {
            Matrix res = new Matrix(this);

            for (int i = N - 1; i >= 0; i--)
            {
                for(int j = i - 1; j >= 0; j--)
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
                    double temp = res[j, i];
                    for (int k = 0; k < M; k++)
                    {
                        res[j, k] -= res[i, k] * temp / res[i, i];
                    }
                }
            }

            return res;
        }
        public double GetDeterminant()
        {
            Matrix diagonolize = GetDiagonolizeUnder().GetDiagonolizeUpper();
            double det = 1;

            for(int i = 0; i < N; i++)
            {
                det *= diagonolize[i, i];
            }
            return det;
        }
        public Matrix GetReverse()
        {
            Matrix diagonolize = ExtendE()
                .GetDiagonolizeUnder()
                .GetDiagonolizeUpper();

            for(int i = 0; i < N; i++)
            {
                double temp = diagonolize[i, i];

                for (int j = 0; j < diagonolize.M; j++)
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
                for (int i = 0; i < vector.N; i++)
                {
                    data[i, pos] = vector[i, 0];
                }
            }
        }
        public static Matrix GetReverse(Matrix A, Matrix A_reverce, Matrix x, int i)
        {
            Matrix matrix = new Matrix(A);

            matrix.ReplaceVector(x, 0);
            Matrix L = A_reverce * x;
            if(L[i]==0)
            {
                throw new Exception("Матрица необратима.");
            }
            Matrix L_1 = new Matrix(L);
            L_1[i, 0] = -1;
            Matrix L_2 = L_1 * (-1 / L[i, 0]);
            Matrix E = Matrix.E(A.N);
            E.ReplaceVector(L_2, i);
            return E * A_reverce;
        }
    }
}
