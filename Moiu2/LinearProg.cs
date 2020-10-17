using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moiu2
{
    static class LinearProg
    {
        public static Matrix SimplexMethod(Matrix A, Matrix B, Matrix C, List<int> J_b, Matrix X, out Matrix A_b, out Matrix A_b_rev)
        {
            A_b = new Matrix(A.N);
            A_b_rev = null;
            Matrix A_j0 = null;
            int j_0 = 0, iter = 0;

            while (true)
            {
                for (int i = 0; i < J_b.Count; i++)
                {
                    for (int j = 0; j < A.N; j++)
                    {
                        A_b[j, i] = A[j, J_b[i]];
                    }
                }

                if(A_b_rev == null)
                {
                    A_b_rev = A_b.GetReverse();
                }
                else
                {
                    A_b_rev = Matrix.GetReverse(A_b, A_b_rev, A_j0.GetTranstoseMatrix(), J_b.IndexOf(j_0));
                }

                Matrix C_b = new Matrix(1, J_b.Count);
                for(int i = 0; i < J_b.Count; i++)
                {
                    C_b[i] = C[J_b[i]];
                }

                Matrix u_ = C_b * A_b_rev;

                var delta_ = u_ * A - C;

                List<int> J_nb = new List<int>();
                for (int i = 0; i < A.M; i++)
                {
                    if (!J_b.Contains(i))
                        J_nb.Add(i);
                }
                bool isOk = true;
                for(int i = 0; i < J_nb.Count; i++)
                {
                    if (delta_[J_nb[i]] < 0)
                    { 
                        isOk = false;
                        j_0 = J_nb[i];
                        break;
                    }
                }
                if(isOk)
                {
                    Console.WriteLine("Result:");
                    Console.WriteLine(X);
                    return X;
                }
                A_j0 = new Matrix(1, A.N);
                for(int i = 0; i < A.N; i++)
                {
                    A_j0[i] = A[i, j_0];
                }
                var z = A_b_rev * A_j0.GetTranstoseMatrix();

                var tetta = new List<double>();
                for(int i = 0; i < z.N; i++)
                {
                    if (z[i] > 0)
                    {
                        tetta.Add(X[J_b[i]] / z[i]);
                    }
                    else tetta.Add(double.MaxValue);
                }
                var tetta_0 = tetta.Min();
                if(tetta_0 == double.MaxValue)
                {
                    Console.WriteLine("Нет оптимального плана");
                    return null;
                }
                var min_index = tetta.IndexOf(tetta_0);

                J_b[min_index] = j_0;

                J_nb.Clear();
                for (int i = 0; i < A.M; i++)
                {
                    if (!J_b.Contains(i))
                        J_nb.Add(i);
                }
                for(int i = 0; i < J_b.Count; i++)
                {
                    X[J_b[i]] = X[J_b[i]] - tetta_0 * z[i];
                }
                X[j_0] = tetta_0;
                for(int i = 0; i < J_nb.Count; i++)
                {
                    X[J_nb[i]] = 0;
                }
                iter++;
                Console.WriteLine($"iteration {iter}: {X}");                
            }
        }
        public static void InitialPhase(ref Matrix A, ref Matrix b, Matrix c, out List<int> J_b, out Matrix X)
        {
            int N = A.M, M = A.N;
            List<int> J_u = Range(N, N + M);
            J_b = Range(N, N + M);
            X = null;
            Matrix A_b = Matrix.E(M);
            Matrix A_ext = A.Extend(A_b);
            Matrix e = Matrix.GetRow(N + M);
            Matrix X_ext = Matrix.GetRow(N + M);
            
            // ================= 1. =================

            for (int i = 0; i < b.N; i++)
            {
                if(b[i] < 0)
                {
                    for(int j = 0; j < A.M; j++)
                    {
                        A[i, j] *= -1;
                    }
                    b[i] *= -1;
                }
            }

            for (int i = N; i < N + M; i++)
            {
                e[i] = -1;
                X_ext[i] = b[i - N];
            }

            // ================= solve =================
            
            X_ext = SimplexMethod(A_ext, b, e, J_b, X_ext, out A_b, out Matrix A_b_inv);

            // ================= a =================

            for(int i = N; i < N + M; i++)
                if (X_ext[i] != 0)
                    throw new Exception("Множество допустимых планов пусто");

            

            while (true)
            {
                int j_k = -1;
                int k = -1;
                for (int i = 0; i < J_b.Count; i++)
                {
                    if (J_u.Contains(J_b[i]))
                    {
                        j_k = J_b[i];
                        k = J_b.IndexOf(j_k);
                        break;
                    }
                }
                // ================= b =================
                if (j_k == -1)
                {
                    X = Matrix.GetRow(N);
                    for (int i = 0; i < N; i++)
                    {
                        X[0, i] = X_ext[0, i];
                    }
                    return;
                }
                // ================= c =================
                else
                {
                    List<Matrix> L = new List<Matrix>();
                    List<int> J_n = new List<int>();
                    for (int i = 0; i < N; i++)
                    {
                        if (!J_b.Contains(i))
                        {
                            J_n.Add(i);
                            L.Add(A_b_inv * A.GetColumn(i));
                        }
                    }
                    int j_0 = -1;
                    for (int j = 0; j < L.Count; j++)
                    {
                        var ek = Matrix.e(M, k);
                        var alpha = ek * L[j];
                        
                        if (alpha[0] != 0)
                        {
                            j_0 = j;
                            int index = J_b.IndexOf(j_k);
                            J_b[index] = j;
                            Matrix vector = A.GetColumn(j);
                            A_b_inv = Matrix.GetReverse(A_b, A_b_inv, vector, j);
                            A_b.ReplaceVector(vector, index);
                            break;
                        }
                        
                        if (j_0 != -1)
                            break;
                    }
                    // ================= 2)  =================
                    if (j_0 == -1)
                    {
                        int i_0 = j_k - N;
                        A = A.RemooveRow(i_0);
                        b = b.RemooveRow(i_0);
                        J_b.Remove(j_k);
                        A_b = A_b.RemooveRC(j_k - N, k);
                        A_b_inv = A_b_inv.RemooveRC(k, j_k - N);
                        X_ext = X_ext.RemooveCol(j_k);
                        N--;
                    }
                }
            }
        }
        public static Matrix DualSimplexMethod(Matrix A, Matrix b, Matrix c, Matrix y, List<int> J_b)
        {
            Matrix A_b = null, B = null;
            Matrix kappa_b;
            List<int> J_n = new List<int>();
            int iter = 0;
            for(int i = 0; i < A.M ; i++)
            {
                if (!J_b.Contains(i))
                    J_n.Add(i);
            }
            while (true)
            {
                if(A_b == null)
                    A_b = GetA_b(A, J_b);
                if(B == null)
                    B = A_b.GetReverse();

                kappa_b = B * b;
                int j_k = -1, k = 0;
                for (int i = 0; i < kappa_b.N; i++)
                {
                    if (kappa_b[i, 0] < 0)
                    {
                        j_k = J_b[i];
                        k = i;
                        break;
                    }
                }
                if (j_k == -1)
                {
                    var kappa = new Matrix(1, A.M);
                    for(int i = 0; i < J_b.Count; i++)
                    {
                        kappa[0, J_b[i]] = kappa_b[i];
                    }
                    return kappa;
                }
                Matrix B_k = B.GetRow(k);
                List<double> sigma = new List<double>();
                //var A_t = A.GetTranstoseMatrix();
                for (int j = 0; j < J_n.Count; j++)
                {
                    var miu = (B_k * A.GetColumn(J_n[j]))[0, 0];
                    if (miu < 0)
                    {
                        var delta = (y * A.GetColumn(J_n[j]))[0, 0] - c[0, J_n[j]];
                        sigma.Add(-delta / miu);
                    }
                    else
                        sigma.Add(double.MaxValue);
                }
                var sigma_0 = sigma.Min();
                if (sigma_0 == double.MaxValue)
                    throw new Exception("Ограничения прямой задачи несовместны.");
                else
                {
                    var j_0 = J_n[sigma.IndexOf(sigma_0)];
                    J_b[k] = j_0;
                    J_n[J_n.IndexOf(j_0)] = j_k;
                    var vector = A.GetColumn(j_0);
                    B = Matrix.GetReverse(A_b, B, vector, k);
                    A_b.ReplaceVector(vector, k);
                }
                iter++;
            }
        }
        private static Matrix GetA_b(Matrix A, List<int> J_b)
        {
            int m = J_b.Count;
            Matrix res = new Matrix(m);
            for(int i = 0; i < m; i++)
            {
                for(int j = 0; j < m; j++)
                {
                    res[i, j] = A[i, J_b[j]];
                }
            }
            return res;
        }
        private static List<int> Range(int start = 0, int stop = 0, int iter = 1)
        {
            List<int> res = new List<int>();
            for(int i = start; i < stop; i+=iter)
            {
                res.Add(i);
            }
            return res;
        }
        private static List<int> Range(int stop)
        {
            List<int> res = new List<int>();
            for (int i = 0; i < stop; i++)
            {
                res.Add(i);
            }
            return res;
        }
    }
}
