using System;
using System.Collections.Generic;
using System.Text;

namespace MOiU
{
    class Matrix
    {
        private decimal[,] data;
        public int N
        {
            get
            {
                if (data != null)
                {
                    return data.GetUpperBound(0) + 1;
                }
                return 0;
            }
        }
        public int M                  
        {
            get
            {
                if (data != null)
                {
                    if (N != 0)
                        return data.Length / N;
                }
                return 0;
            }
        }
        public decimal this[int x, int y]
        {
            get
            {
                return data[x, y];
            }
            set
            {
                data[x, y] = value;
            }
        }
        public Matrix(int n, int m)
        {
            data = new decimal[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    data[i, j] = 0;
                }
            }
        }
        public Matrix(int n)
        {
            data = (new Matrix(n, n)).data;
        }
        public Matrix(Matrix matrix)
        {
            data = new decimal[matrix.N, matrix.M];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    data[i, j] = matrix[i, j];
                }
            }
        }
        public Matrix(decimal[,] vs)
        {
            data = vs;
        }
        public void replace(Matrix vector, int pos)
        {
            if (pos < N)
            {
                for (int i = 0; i < vector.N; i++)
                {
                    data[i, pos] = vector[i, 0];
                }
            }
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder("");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    str.Append(data[i, j].ToString() + " ");
                }
                str.Append("\n");
            }
            return str.ToString();
        }
        public static Matrix operator *(Matrix matrix, decimal value)
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

        public static Matrix operator *(Matrix mat1, Matrix mat2)
        {
            var result = new Matrix(mat1.N, mat2.M);

            if (mat1.M == mat2.N)
            {
                for (int i = 0; i < mat1.N; i++)
                {
                    for (int j = 0; j < mat2.M; j++)
                    {
                        decimal t = 0.0m;
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
    }
}
