using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Moiu2
{
    class Program
    {
        static void Main(string[] args)
        {
            //lab2();
            //lab3();
            //lab4();
            //lab5();
            lab6();
        }
        //================================================3==========================================================================
        static void lab3()
        {
            Console.WriteLine("TEST 1");
            Matrix A = new Matrix(new double[,]
            {
                {1,1,1 },
                {2,2,2 }
            });
            Matrix B = new Matrix(new double[,]
            {
                {0 },
                {0 }
            });
            Matrix C = new Matrix(new double[,]
            {
                {1,1 }
            });
            LinearProg.InitialPhase(ref A, ref B, C, out List<int> J_b, out Matrix x);
            Console.WriteLine($"J_b = {Print(J_b)}");
            Console.WriteLine("x1:");
            Console.WriteLine(x);
            

            Console.WriteLine("TEST 2");
            Matrix A1 = new Matrix(new double[,]
            {
                {1, 1, -1, -1, 0, 0 },
                {1, -1, 2, 0, -1, 0 },
                {-2, -8, 3, 0, 0, -1}
            });
            Matrix B1 = new Matrix(new double[,]
            {
                {8 },
                {2 },
                {1 }
            });
            Matrix C1 = new Matrix(new double[,]
            {
                { 1, 1, 0,0,0,0 }
            });
            Console.WriteLine("INITIAL PHASE START");

            LinearProg.InitialPhase(ref A1, ref B1, C1, out List<int> J_b1, out Matrix x1);
            Console.WriteLine($"J_b = {Print(J_b1)}");
            Console.WriteLine("x:");
            Console.WriteLine(x1);
            Console.WriteLine("INITIAL PHASE STOP");
            Matrix X = LinearProg.SimplexMethod(A1, B1, C1, J_b1, x1, out _, out _);
        }
        //=====================================================2=============================================================================
        static void lab2()
        {
            Console.WriteLine("Пример 1.1.1:");
            Matrix A = new Matrix(new double[,]
            {
                {0,1,4,1,0,-3,5,0 },
                {1,-1,0,1,0,0,1,0 },
                {0,7,-1,0,-1,3,8,0 },
                {1,1,1,1,0,3,-3,1 }
            });
            Matrix B = new Matrix(new double[,]
            {
                { 6,10,-2,15 }
            }).GetTranstoseMatrix();
            Matrix C = new Matrix(new double[,]
            {
                {-5,2,3,-4,-6,0,-1,-5 }
            });
            Matrix x = new Matrix(new double[,]
            {
                {4,0,0,6,2,0,0,5}
            });
            List<int> J_b = new List<int> { 1 - 1, 4 - 1, 5 - 1, 8 - 1 };
            LinearProg.SimplexMethod(A, B, C, J_b, x, out _, out _);


            Console.WriteLine("Пример 1.1.2:");

            Matrix A1 = new Matrix(new double[,]
            {
                {0,1,4,1,0,-3,1,0 },
                {1,-1,0,1,0,0,0,0 },
                {0,7,-1,0,-1,3,-1,0 },
                {1,1,1,1,0,3,-1,1 }
            });
            Matrix B1 = new Matrix(new double[,]
            {
                { 6,10,-2,15 }
            }).GetTranstoseMatrix();
            Matrix C1 = new Matrix(new double[,]
            {
                {-5,-2,3,-4,-6,0,1,-5 }
            });
            Matrix x1 = new Matrix(new double[,]
            {
                {10,0,1.5,0,0.5,0,0,3.5}
            });
            List<int> J_b1 = new List<int> { 1 - 1, 3 - 1, 5 - 1, 8 - 1 };
            LinearProg.SimplexMethod(A1, B1, C1, J_b1, x1, out _, out _);
        }
        //========================================================4==========================================================================
        public static void lab4()
        {
            Matrix c = new Matrix(new double[,]
            {
                {-4, -3, -7, 0, 0 }
            });
            Matrix b = new Matrix(new double[,] {
                { -1, -3/2}
            }).GetTranstoseMatrix();
            Matrix A = new Matrix(new double[,]
            {
                {-2, -1, -4, 1, 0 },
                {-2, -2, -2, 0, 1 }
            });
            Matrix y = new Matrix(new double[,] {
                {0, 0 }
            });
            List<int> J_b = new List<int>() { 4 - 1, 5 - 1 };
            Console.WriteLine("TEST 1:");
            Console.WriteLine(LinearProg.DualSimplexMethod(A, b, c, y, J_b));
            Matrix c1 = new Matrix(new double[,]
            {
                {2,2,1,-10,1,4,0,-3 }
            });
            Matrix b1 = new Matrix(new double[,] {
                { -2,4,3}
            }).GetTranstoseMatrix();
            Matrix A1 = new Matrix(new double[,]
            {
                {-2,-1,1,-7,0,0,0,2},
                {4,2,1,0,1,5,-1,-5 },
                {1,1,0,-1,0,3,-1,1 },
            });
            Matrix y1 = new Matrix(new double[,] {
                {1,1,1 }
            });
            List<int> J_b1 = new List<int>() { 1, 4, 6 };
            Console.WriteLine("TEST 2:");
            Console.WriteLine(LinearProg.DualSimplexMethod(A1, b1, c1, y1, J_b1));
            Matrix c2 = new Matrix(new double[,]
            {
                {2,2,1,-10,1,4,0,-3 }
            });
            Matrix b2 = new Matrix(new double[,] {
                { -2,4,3}
            }).GetTranstoseMatrix();
            Matrix A2 = new Matrix(new double[,]
            {
                {-2,-1,1,-7,0,0,0,2},
                {4,2,1,0,1,5,-1,-5 },
                {1,1,0,1,0,3,1,1 },
            });
            Matrix y2 = new Matrix(new double[,] {
                {1,1,1 }
            });
            List<int> J_b2 = new List<int>() { 1, 4, 6 };
            Console.WriteLine("TEST 3:");
            Console.WriteLine(LinearProg.DualSimplexMethod(A2, b2, c2, y2, J_b2));

        }
        //===========================5=======================================================================================================

        public static void lab5()
        {
            List<int> a = new List<int>() { 100, 300, 300 };
            List<int> b = new List<int>() { 300, 200, 200 };
            var c = new Matrix(new double[,]
            {
                {8,4,1, },
                {8,4,3 },
                {9,7,5 }
            });
            Console.WriteLine(TransportationTask.MethodOfPotentials(ref c, a, b));

            //List<int> a = new List<int>() { 20,30,25};
            //List<int> b = new List<int>() { 10,10,10,10,10};
            //var c = new Matrix(new double[,]
            //{
            //    {2,8,-5,7,10 },
            //    {11,5,8,-8,-4 },
            //    {1,3,7,4,2}
            //});
            //Console.WriteLine("result");
            //Console.WriteLine(TransportationTask.MethodOfPotentials(ref c, a, b));


            //List<int> a = new List<int>() { 1 };
            //List<int> b = new List<int>() { 3 };
            //var c = new Matrix(new double[,]
            //{
            //    {1 }
            //});
            //Console.WriteLine("result");
            //Console.WriteLine(TransportationTask.MethodOfPotentials(ref c, a, b));



            //List<int> a = new List<int>() { 30, 25, 20 };
            //List<int> b = new List<int>() { 20, 15, 25, 20 };
            //var c = new Matrix(new double[,]
            //{
            //    { 4, 5, 3, 6 },
            //    { 7, 2, 1, 5 },
            //    { 6, 1, 4, 2 }
            //});
            //Console.WriteLine("result");
            //Console.WriteLine(TransportationTask.MethodOfPotentials(ref c, a, b));

            //List<int> a = new List<int>() { 53, 20, 45, 38 };
            //List<int> b = new List<int>() { 15, 31, 10, 3, 18 };
            //var c = new Matrix(new double[,]
            //{
            //    {3, 0, 3, 1, 6 },
            //    {2, 4, 10, 5, 7},
            //    {-2, 5, 3, 2, 9 },
            //    {1, 3, 5, 1, 9 }
            //});
            //Console.WriteLine("result");
            //Console.WriteLine(TransportationTask.MethodOfPotentials(ref c, a, b));
        }
        //=======================================6===================================================================================
        public static void lab6()
        {
            //var c = new Matrix(new double[,] { { -8, -6, -4, -6 } }).GetTranstoseMatrix();
            //var b = new Matrix(new double[,] { { 2, 3 } }).GetTranstoseMatrix();
            //var D = new Matrix(new double[,]
            //{
            //    {2,1,1,0 },
            //    {1,1,0,0 },
            //    {1,0,1,0 },
            //    {0,0,0,0 }
            //});
            //var A = new Matrix(new double[,]
            //{
            //    {1,0,2,1 },
            //    {0,1,-1,2 }
            //});
            //var x = new Matrix(new double[,] { { 2, 3, 0, 0 } }).GetTranstoseMatrix();
            //var J_b = new List<int>() { 0, 1 };
            //var J_be = new List<int>() { 0, 1 };
            //Console.WriteLine(QuadraticProgramming.Solve(c, D, A, b, x, J_b, J_be));

            //------------------------------------------------------------------------------

            var c1 = new Matrix(new double[,] { { -10, -31, 7, 0, -21, -16, 11, -7 } }).GetTranstoseMatrix();
            var b1 = new Matrix(new double[,] { { 4,5,8 } }).GetTranstoseMatrix();
            var D1 = new Matrix(new double[,]
            {
                {6,11,-1,0,6,-7,-3,-2 },
                {11,41,-1,0,7,-24,0,-3},
                {-1,-1,1,0, -3,-4,2,-1},
                {0,0,0,0,0,0,0,0 },
                {6,7,-3,0,11,6,-7,1 },
                {-7,-24,-4,0,6, 42,-7,10 },
                {-3,0,2,0,-7,-7,5,-1 },
                {-2,-3,-1,0,1,10,-1,3 }
            });
            var A1 = new Matrix(new double[,]
            {
                {1,2,0,1,0,4,-1,-3 },
                {1,3,0,0,1,-1,-1,2},
                {1,4,1,0,0,2,-2,0 }
            });
            var x1 = new Matrix(new double[,] { { 0,0,6,4,5,0,0,0} }).GetTranstoseMatrix();
            var J_b1 = new List<int>() { 2,3,4};
            var J_be1 = new List<int>() { 2,3,4};
            Console.WriteLine("result:");
            var res = QuadraticProgramming.Solve(c1, D1, A1, b1, x1, J_b1, J_be1);
            Console.WriteLine(c1.GetTranstoseMatrix() * res + 0.5 * (res.GetTranstoseMatrix() * D1 * res));

            //var b = new Matrix(new double[,] { { 8, 2, 5 } }).GetTranstoseMatrix();

            //var A = new Matrix(new double[,]
            //{
            //    {11,0,0,1,0,-4,-1,1 },
            //    {1,1,0,0,1,-1,-1,1 },
            //    {1,1,1,0,1,2,-2,1 }
            //});
            //var x = new Matrix(new double[,] { { 0.7273m, 1.2727m, 3, 0, 0, 0, 0, 0 } }).GetTranstoseMatrix();
            //Matrix B = new Matrix(new double[,]
            //{
            //    {1,0,0,3,-1,5,0,1 },
            //    {2,5,0,0,0,4,0,0 },
            //    {-1,9,0,5,2,-1,-1,5 }
            //});
            //Matrix d = new Matrix(new double[,] { { 6, 10, 9 } });
            //Matrix D = B.GetTranstoseMatrix() * B;
            //Matrix c = ((-1 * d) * B).GetTranstoseMatrix();
            //var J_b = new List<int>() { 0, 1, 2 };
            //var J_be = new List<int>() { 0, 1, 2 };
            //Console.WriteLine(QuadraticProgramming.Solve(c, D, A, b, x, J_b, J_be));
            //var res = QuadraticProgramming.Solve(c, D, A, b, x, J_b, J_be);
            //Console.WriteLine(c.GetTranstoseMatrix() * res + 0.5m * (res.GetTranstoseMatrix() * D * res));

            // =========================== OK ==============================================
            //var c = new Matrix(new double[,] { { 1, 3, -1, 3, 5, 2, -2, 0 } }).GetTranstoseMatrix();
            //var b = new Matrix(new double[,] { { 6, 4, 14 } }).GetTranstoseMatrix();
            //var D = new Matrix(new double[,]
            //{
            //    {1,0,0,0,0,0,0,0 },
            //    {0,1,0,0,0,0,0,0 },
            //    {0,0,0,0,0,0,0,0 },
            //    {0,0,0,1,0,0,0,0 },
            //    {0,0,0,0,1,0,0,0 },
            //    {0,0,0,0,0,1,0,0 },
            //    {0,0,0,0,0,0,0,0 },
            //    {0,0,0,0,0,0,0,1 }
            //});
            //var A = new Matrix(new double[,]
            //{
            //    {0,2,1,4,3,0,-5,-10 },
            //    {-1,3,1,0,1,3,-5,-6 },
            //    {1,1,1,0,1,-2,-5,8 }
            //});
            //var x = new Matrix(new double[,] { { 0, 2, 0, 0, 4, 0, 0, 1 } }).GetTranstoseMatrix();
            //var J_b = new List<int>() { 1, 4, 7 };
            //var J_be = new List<int>() { 1, 4, 7 };
            //Console.WriteLine(QuadraticProgramming.Solve(c, D, A, b, x, J_b, J_be));


            //var c = new Matrix(new double[,] { { 1, 3, 4, 3, 5, 6, -2, 0 } }).GetTranstoseMatrix();
            //var b = new Matrix(new double[,] { { 20, 1, 7 } }).GetTranstoseMatrix();
            //var D = new Matrix(new double[,]
            //{
            //    {25,10,0,3,-1,13,0,1 },
            //    {10,45,0,0,0,20,0,0 },
            //    {0,0,20,0,0,0,0,0},
            //    {3,0,0,29,-3,15,0,3},
            //    {-1,0,0,-3,21,-5,0,-1 },
            //    {13,20,0,15,-5,61,0,5 },
            //    {0,0,0,0,0,0,20,0 },
            //    {1,0,0,30,-1,5,0,21 }
            //});
            //var A = new Matrix(new double[,]
            //{
            //    {0,2,1,4,3,0,-5,-10 },
            //    {-1,1,1,0,1,1,-1,-1 },
            //    {1,1,1,0,1,-2,-5,8 }
            //});
            //var x = new Matrix(new double[,] { { 3, 0, 0, 2, 4, 0, 0, 0 } }).GetTranstoseMatrix();
            //var J_b = new List<int>() { 0, 3, 4 };
            //var J_be = new List<int>() { 0, 3, 4 };
            //var res = QuadraticProgramming.Solve(c, D, A, b, x, J_b, J_be);
            //Console.WriteLine(c.GetTranstoseMatrix() * res + 0.5m * (res.GetTranstoseMatrix() * D * res));
        }
        public static string Print(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var item in list)
            {
                sb.Append((item + 1).ToString() + " ");
            }
            return sb.ToString();
        }
        public static string Print(List<double> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item.ToString("0.00") + " ");
            }
            return sb.ToString();
        }
    }
}
