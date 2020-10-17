using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moiu2
{
    class QuadraticProgramming
    {
        public static Matrix Solve(Matrix c, Matrix D, Matrix A, Matrix b, Matrix x, List<int> J_b, List<int> J_be)
        {
            int iter = 1;
            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"iter {iter}:");
                Console.ResetColor();
                Console.WriteLine("J_b: " + Program.Print(J_b));
                Console.WriteLine("J_be: " + Program.Print(J_be));
                Console.WriteLine("x:");
                Console.WriteLine($"{x}");

                var A_b = GetBasis(A, J_b);

                Console.WriteLine("A_b");
                Console.WriteLine($"{A_b}");

                var A_b_inv = A_b.GetReverse();

                Console.WriteLine("A_b_inv");
                Console.WriteLine($"{A_b_inv}");

                //-----------step 1--------------

                var c_x = c + D * x;

                Console.WriteLine("c_x");
                Console.WriteLine($"{c_x}");

                var c_x_b = GetBasisRows(c_x, J_b);

                Console.WriteLine("c_x_b");
                Console.WriteLine($"{c_x_b}");

                var u_x = (-1 * c_x_b.GetTranstoseMatrix()) * A_b_inv;

                Console.WriteLine("u_x");
                Console.WriteLine($"{u_x}");

                var delta_x = u_x * A + c_x.GetTranstoseMatrix();
                
                Console.WriteLine("delta_x");
                Console.WriteLine($"{delta_x}");

                //-----------step 2,3--------------
                int j_0 = -1;
                for(int i = 0; i < delta_x.M; i++)
                {
                    if(!J_be.Contains(i) && delta_x[0, i] < 0)
                    {
                        j_0 = i;
                        break;
                    }
                }
                if(j_0 == -1)
                {
                    return x;
                }
                Console.WriteLine("j_0");
                Console.WriteLine($"{j_0+1}");
                //-----------step 4--------------            
                var l  = new Matrix(delta_x.M, 1);
                l[j_0, 0] = 1;
                var A_be = GetBasis(A, J_be);
                var D_be = GetD_be(D, J_be);
                var H = GetH(D_be, A_be);

                Console.WriteLine("H");
                Console.WriteLine($"{H}");

                if (H.GetDeterminant() == 0)
                    throw new Exception("asd");
                var H_inv = H.GetReverse();

                Console.WriteLine("H_inv");
                Console.WriteLine($"{H_inv}");

                var b_e = new Matrix(J_be.Count + A.N, 1);
                for (int i = 0; i < J_be.Count; i++)
                {
                    b_e[i, 0] = D[J_be[i], j_0];
                }
                for (int i = 0; i < A.N; i++)
                {
                    b_e[i + J_be.Count, 0] = A[i, j_0];
                }

                Console.WriteLine("bb:");
                Console.WriteLine($"{b_e}");

                var x_ = (-1 * H_inv) * b_e;

                Console.WriteLine("x_");
                Console.WriteLine($"{x_}");

                for (int i = 0; i < J_be.Count; i++)
                {
                    l[J_be[i], 0] = x_[i, 0];
                }
                

                Console.WriteLine("l");
                Console.WriteLine($"{l}");

                //-----------step 5-------------- 
                var delta = (l.GetTranstoseMatrix() * D * l)[0, 0];

                Console.WriteLine("delta");
                Console.WriteLine($"{delta}");

                var tetta = ZeroList(J_be.Count + 1);
                if (delta <= 1E-5)
                    tetta[J_be.Count] = double.MaxValue;
                else
                    tetta[J_be.Count] = Math.Abs(delta_x[0, j_0]) / delta;
                for (int i = 0; i < J_be.Count; i++)
                {
                    var j = J_be[i];
                    if (l[j] < 0)
                    {
                        tetta[i] = -x[j, 0] / l[j];
                    }
                    else
                    {
                        tetta[i] = double.MaxValue;
                    }
                }


                Console.WriteLine("tetta:");
                Console.WriteLine($"{Program.Print(tetta)}");

                var tetta_0 = tetta.Min();

                Console.WriteLine("tetta_0:");
                Console.WriteLine($"{tetta_0}");

                if (tetta_0 <= 1E-20)
                    throw new Exception("Целевая функция задачи не ограничена снизу на множестве допустимых планов");


                var index = tetta.IndexOf(tetta_0);
                var j_e = index == J_be.Count ? j_0 : J_be[index];

                Console.WriteLine($"j_e = {j_e+1}");

                //-----------step 6-------------- 
                x = x + (tetta_0 * l); // TODO reverce l


                var J_be_temp = new List<int>(J_be);
                for (int i = 0; i < J_b.Count; i++)
                {
                    var j = J_b[i];
                    if (J_be.Contains(j))
                        J_be_temp.Remove(j);
                }

                if (J_b.Contains(j_e))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    var s = J_b.IndexOf(j_e);
                    Console.WriteLine($"s = {s+1}");
                    bool case4 = true;
                    for (int i = 0; i < J_be_temp.Count; i++)
                    {
                        if ((A_b_inv * A.GetColumn(J_be_temp[i]))[s, 0] != 0)
                        {
                            Console.WriteLine("case3");
                            J_b.RemoveAt(s);
                            J_b.Add(J_be_temp[i]);
                            J_be.Remove(j_e);
                            case4 = false;
                            break;
                        }
                    }
                    if (case4)
                    {
                        Console.WriteLine(case4);
                        J_b[J_b.IndexOf(j_e)] = j_0;
                        J_be[J_be.IndexOf(j_e)] = j_0;
                    }
                    Console.ResetColor();
                }
                else if (J_be_temp.Contains(j_e))
                {
                    J_be.Remove(j_e);
                }
                else if (j_e == j_0)
                {
                    J_be.Add(j_e);
                }
                iter++;
            }
            
        }
        private static Matrix GetBasis(Matrix A, List<int> J_b)
        {
            int m = J_b.Count;
            Matrix res = new Matrix(A.N, m);
            for (int i = 0; i < A.N; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    res[i, j] = A[i, J_b[j]];
                }
            }
            return res;
        }
        private static Matrix GetBasisRows(Matrix A, List<int> J_b)
        {
            int m = J_b.Count;
            Matrix res = new Matrix(m, A.M);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < A.M; j++)
                {
                    res[i, j] = A[J_b[i], j];
                }
            }
            return res;
        }
        private static Matrix GetD_be(Matrix D, List<int> J_be)
        {
            Matrix res = new Matrix(J_be.Count);
            int k = 0;
            foreach(var i in J_be)
            {
                int t = 0;
                foreach(var j in J_be)
                {
                    res[k, t] = D[i, j];
                    t++;
                }
                k++;
            }
            return res;
        }
        private static Matrix GetH(Matrix D_be, Matrix A_be)
        {
            Matrix res = new Matrix(D_be.N + A_be.N, D_be.M + A_be.N);
            for(int i = 0; i < D_be.N; i++)
            {
                for(int j = 0; j < D_be.M; j++)
                {
                    res[i, j] = D_be[i, j];
                }
            }
            for(int i = 0; i < A_be.N; i++)
            {
                for(int j = 0; j < A_be.M; j++)
                {
                    res[i + D_be.N, j] = A_be[i, j];
                }
            }
            for (int i = 0; i < A_be.M; i++)
            {
                for (int j = 0; j < A_be.N; j++)
                {
                    res[i, j + D_be.M] = A_be[j, i];

                }
            }
            return res;
        }
        private static List<double> ZeroList(int count)
        {
            List<double> res = new List<double>();
            for(int i = 0; i < count; i++)
                res.Add(0);
            return res;
        }
    }
}
