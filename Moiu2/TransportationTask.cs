using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moiu2
{
    class TransportationTask
    {
        // метод северо-западного угла
        public static Matrix InitialPhase(List<int> a, List<int> b, out List<(int, int)> U_b)
        {
            Matrix X = new Matrix(a.Count, b.Count);
            U_b = new List<(int, int)>();
            for(int i = 0, j = 0; i < a.Count && j < b.Count;)
            {
                var min = Math.Min(a[i], b[j]);
                X[i, j] += min;
                U_b.Add((i, j));
                a[i] -= min;
                b[j] -= min;
                if (a[i] == 0)
                    i++;
                else if (b[j] == 0)
                    j++;
            }
            return X;
        }
        // Основная фаза
        public static Matrix MasterPhase(Matrix X, Matrix c, List<(int, int)> U_b)
        {       
            int iter = 0;
            Random random = new Random();
            while (true)
            {
                GetUV(c, U_b, out List<int> u, out List<int> v);
                (int, int) ij_0 = (-1, -1);
                List<(int, int)> a = new List<(int, int)>();
                for (int i = 0; i < c.N; i++)
                {
                    for (int j = 0; j < c.M; j++)
                    {
                        if (U_b.Contains((i, j)))
                            continue;
                        if (u[i] + v[j] > c[i, j])
                        {
                            a.Add((i, j));
                        }
                    }
                }
                if (a.Count == 0)
                {
                    Console.WriteLine("result");
                    return X;
                }
                ij_0 = a[random.Next(a.Count)];
                U_b.Add(ij_0);
                var cycle = GetCornerPos(c.N, c.M, U_b);
                
                int index = cycle.IndexOf(ij_0);
                GetLists(cycle, ij_0, out List<(int, int)> PosList, out List<(int, int)> NegList);
                
                List<int> app_val = new List<int>();
                for(int i = 0; i < NegList.Count; i++)
                {
                    app_val.Add((int)X[NegList[i].Item1, NegList[i].Item2]);
                }
                var min = app_val.Min();
                var index_delete = NegList[app_val.IndexOf(min)];
                foreach(var p in PosList)
                {
                    X[p.Item1, p.Item2] += min;
                }
                foreach(var n in NegList)
                {
                    X[n.Item1, n.Item2] -= min;
                }
                U_b.Remove(index_delete);
                Console.WriteLine(iter);
                iter++;
                Console.WriteLine(X);
                U_b.Sort((a,b) => Compare(a, b));
            }
        }
        public static Matrix MethodOfPotentials(ref Matrix c, List<int> a, List<int> b)
        {
            var a_sum = a.Sum();
            var b_sum = b.Sum();
            if (a_sum > b_sum)
            {
                b.Add(a_sum - b_sum);
                Matrix c_temp = new Matrix(c.N, c.M + 1);
                for(int i = 0; i < c.N; i++)
                {
                    for(int j = 0; j < c.M; j++)
                    {
                        c_temp[i, j] = c[i, j];
                    }
                }
                c = c_temp;
            }
            else if (b_sum > a_sum)
            {
                a.Add(b_sum - a_sum);
                Matrix c_temp = new Matrix(c.N + 1, c.M);
                for (int i = 0; i < c.N; i++)
                {
                    for (int j = 0; j < c.M; j++)
                    {
                        c_temp[i, j] = c[i, j];
                    }
                }
                c = c_temp;
            }
            Matrix X = InitialPhase(a, b, out List<(int, int)> U_b);
            //Console.WriteLine("X after initial phase:");
            //Console.WriteLine(X);
            return MasterPhase(X, c, U_b);
        }
        private static List<(int,int)> GetCornerPos(int n, int m, List<(int,int)> U_b)
        {
            List<List<(int, int)>> A = new List<List<(int, int)>>();
            for(int i = 0; i < n; i++)
            {
                A.Add(new List<(int,int)>());
                for(int j = 0; j < m; j++)
                {
                    if (U_b.Contains((i, j)))
                        A[i].Add((i,j));
                    else
                        A[i].Add((-1, -1));
                }
            }
            while(true)
            {
                bool isDeleted = false;
                for(int i = 0; i < A.Count; i++)
                {
                    int count = 0;
                    for (int j = 0; j < A[0].Count; j++)
                    {
                        if (A[i][j] != (-1, -1))
                            count++;
                    }
                    if (count <= 1)
                    {
                        isDeleted = true;
                        A.RemoveAt(i);
                    }
                }
                for(int j = 0; j < A[0].Count; j++)
                {
                    int count = 0;
                    for(int i = 0; i < A.Count; i++)
                    {
                        if (A[i][j] != (-1, -1))
                            count++;
                    }
                    if(count <= 1)
                    {
                        for(int i = 0; i < A.Count; i++)
                        {
                            isDeleted = true;
                            A[i].RemoveAt(j);
                        }
                    }
                }
                if(!isDeleted)
                {
                    List<(int, int)> res = new List<(int, int)>();
                    for (int i = 0; i < A.Count; i++)
                    {
                        for (int j = 0; j < A[0].Count; j++)
                        {
                            if(A[i][j] != (-1,-1))
                                res.Add(A[i][j]);
                        }
                    }
                    return res;
                }
            }            
        }
        private static void GetLists(List<(int, int)> cycle, (int,int) inex_insert,
            out List<(int, int)> PosList, out List<(int,int)> NegList) 
        {
            GetCycle(ref cycle);
            PosList = new List<(int, int)>();
            NegList = new List<(int, int)>();
            var mod = cycle.IndexOf(inex_insert) % 2;
            for(int i = 0; i < cycle.Count; i++)
            {
                if (i % 2 == mod)
                    PosList.Add(cycle[i]);
                else
                    NegList.Add(cycle[i]);
            }
        }
        private static void GetCycle(ref List<(int, int)> matrix_cycle)
        {
            List<(int, int)> tmp = new List<(int, int)>() { matrix_cycle[0] };
            List<bool> added = new List<bool>() { true };
            for (int i = 0; i < matrix_cycle.Count - 1; i++)
                added.Add(false);
            while(true)
            {
                for(int i = 1; i < matrix_cycle.Count; i++)
                {
                    if((matrix_cycle[i].Item1 == tmp[tmp.Count -1].Item1
                        || matrix_cycle[i].Item2 == tmp[tmp.Count - 1].Item2)
                        && !tmp.Contains(matrix_cycle[i]))
                    {
                        tmp.Add(matrix_cycle[i]);
                        added[i] = true;
                    }
                }
                if (!added.Contains(false))
                    break;
            }
            matrix_cycle = tmp;
        }
        private static void GetUV(Matrix c, List<(int,int)> U_b, out List<int> u, out List<int> v)
        {
            U_b.Sort((a, b) => Compare(a, b));
            u = Zero(c.N);
            v = Zero(c.M);
            bool[] u_solved = new bool[c.N];
            bool[] v_solved = new bool[c.M];
            u_solved[0] = true;
            while (true)
            {
                for (int i = 0; i < c.N; i++)
                {
                    List<(int, int)> row = (from pos in U_b
                                            where pos.Item1 == i
                                            select pos).ToList<(int, int)>();
                    if (i != 0)
                    {
                        for (int j = 0; j < row.Count; j++)
                        {
                            if (v_solved[row[j].Item2])
                            {
                                u[i] = (int)c[i, row[j].Item2] - v[row[j].Item2];
                                u_solved[i] = true;
                                break;
                            }
                        }
                    }
                    foreach (var pos in row)
                    {
                        if (u_solved[i])
                        {
                            v[pos.Item2] = (int)c[i, pos.Item2] - u[i];
                            v_solved[pos.Item2] = true;
                        }
                    }
                }
                if (!(v_solved.Contains(false) && u_solved.Contains(false)))
                    break;
            }
        }
        private static List<int> Zero(int len)
        {
            var res = new List<int>();
            for(int i = 0; i < len; i++)
            {
                res.Add(0);
            }
            return res;
        }
        private static int Compare((int,int) a, (int, int) b)
        {
            if (a.Item1 > b.Item1)
                return 1;
            if (b.Item1 > a.Item1)
                return -1;
            if (a.Item2 > b.Item2)
                return 1;
            if (b.Item2 > a.Item2)
                return -1;
            return 0;
        }
    }
}
