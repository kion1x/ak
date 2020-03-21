using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Lab1
{
    public partial class Form1 : Form
    {
        private static int n = 8, N = 1024, Wn = 1500;
        private static double[] X = new double[N];
        private static double Mx, Dx;
        private static Stopwatch stopwatch = new Stopwatch();
        public Form1()
        {
            InitializeComponent();
            Text = "Lab 1";
            Graph();
            stopwatch.Start();
            X = Math_f();
            Mx = Mat_W();
            Dx = Desp();
            stopwatch.Stop();
            
        }

        public static string getSt()
        {
            TimeSpan ts = stopwatch.Elapsed;
            return String.Format("{0:0}:{1:00}", ts.Seconds,ts.Milliseconds);
        }
        public static double[] Math_f()
        {
            Random rand = new Random();
            double Ap, Fp, W = 0;
            
            
            double[] X = new double[N];
        
            for (int i = 0; i < n; i++)
            {
                Ap = rand.NextDouble();
                Fp = rand.NextDouble();
                
                W += Wn / n;    
                
                for (int t = 0; t < N; t++)
                {
                    
                    X[t] += Ap * Math.Sin( W * (t + 1) + Fp );
                }
            }
            
            return X;
        }
        
        public static double Mat_W()
        {
            foreach (double x in X)
            {
                Mx += x;
            }
            Mx /= 1024;

            return Mx;
        }
        
        public static double Desp()
        {
            double[] Xt = Math_f();
            double Mx = Mat_W();
            foreach (double d in Xt)
            {
                Dx += Math.Pow(d - Mx, 2);
            }
            Dx /= 1024 - 1;
            
            return Dx;
        }
        
        public static double[] Math_Rxx()
        {
            double[] R = new double[N/2];
            for (int T = 0; T < N/2; T++)
            {
                for (int t = 0; t < N/2; t++)
                {
                    R[T] += (X[t] - Mx) * (X[T + t]- Mx);
                }

                R[T] /= N - 1;
            }

            return R;
        }
        
        public static double[] Math_Rxy()
        {
            double[] Y = new double[N];
            Array.Copy(X, Y, N);
            double[] R = new double[N/2];
            for (int T = 0; T < N/2; T++)
            {
                for (int t = 0; t < N/2; t++)
                {
                    R[T] += (X[t] - Mx) * (Y[T + t]- Mx);
                }

                R[T] /= N - 1;
            }

            return R;
        }
        
        public void Graph()
        {
            zedGraph.GraphPane.Title.Text = "Lab 1";
            zedGraph.GraphPane.XAxis.Title.Text = "t";
            zedGraph.GraphPane.YAxis.Title.Text = "X(t)";
            
            
            
            zedGraph.GraphPane.CurveList.Clear();
            
            PointPairList list = new PointPairList();
            double[] f = Math_f();

            for (int x = 0; x < 1000; x++)
            {
                list.Add(x, f[x]);
            }

            zedGraph.GraphPane.AddCurve("", list, Color.Chartreuse, SymbolType.None);
            
            zedGraph.AxisChange();
        }
        
    }
}