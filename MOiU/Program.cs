using System;

namespace MOiU
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix A = new Matrix(new decimal[,]
            {
                {2,5,7 },
                {6,3,4 },
                {5,-2,-3 }
            });
            Matrix A_reverce = new Matrix(new decimal[,]
            {
                { 1, -1, 1 },
                { -38, 41, -34 },
                { 27, -29, 24  }
            });
            Matrix x = new Matrix(new decimal[,]
            {
                {4 },
                {5 },
                {9 }
            });
            A.replace(x, 0);
            Matrix matrix = new Matrix(A);
            int i = 0;
            Matrix L = A_reverce * x;
            for (int j = 0; j < L.N; j++)
            {
                if (L[j, 0] == 0)
                {
                    Console.WriteLine("Матрица необратима");
                    return;
                }
            }
            Matrix L_1 = new Matrix(L);
            L_1[i, 0] = -1;
            Matrix L_2 = L_1 * (-1 / L[i, 0]);
            Matrix E = Matrix.E(3);
            E.replace(L_2, i);
            Matrix result = E * A_reverce;
            Console.WriteLine(result);
            Console.WriteLine(matrix * result);
        }
    }
}
