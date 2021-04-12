using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Optim_Resourses
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public double []splitString(string str) {
            string [] arrayStr = str.Split(" ");
            int len = arrayStr.Length;
            double[] arrayNum = new double[len];
            for (int i = 0;i< arrayNum.Length;i++) {
                arrayNum[i] = Convert.ToInt32(arrayStr[i]);
            }
            return arrayNum;
        }

        public double[] koefCalculate(double[] prices) {
            double[] koef = new double[prices.Length];
            double summ = sumArray(prices);
            for (int i=0; i < prices.Length; i++) {
                koef[i] = prices[i] / summ;
            }
            return koef;
        }

        public double[,] Amatrix(double[] prices) {
            double[,] calcArray = new double[prices.Length-1,prices.Length];
            double[,] A = new double[prices.Length , prices.Length];
            double[] koef = koefCalculate(prices);

            showKoef(koef);

            for (int i = 0; i < prices.Length - 1; i++)
            {
                for (int j = 0; j < prices.Length; j++)
                {
                    if (i == j)
                    {
                        calcArray[i, j] = 1 - koef[i];
                    }

                    else {
                        calcArray[i, j] = (-1) * koef[i];
                    }
                }
            }

            for (int i = 0; i < prices.Length; i++)
            {
                
                 A[0, i] = prices[i];
                
            }

            for (int i = 1; i < prices.Length; i++)
            {
                for (int j = 0; j < prices.Length; j++)
                {
                    A[i, j] = calcArray[i-1, j];
                }
            }

            return A;
        }

        public double[] Bmatrix(double[] prices, double[] needs, double budget)
        {
            double[] B = new double[prices.Length];
            double summ = 0;
            for (int i = 0; i < prices.Length; i++) {
                summ = summ + needs[i] * prices[i];
            }

            summ = budget - summ;
            B[0] = summ;
            return B;
        }

        public bool converge(double[] xk, double[] xkp, int n)
        {
            double eps = 0.001;
            double norm = 0;
            for (int i = 0; i < n; i++)
            {
                norm += (xk[i] - xkp[i]) * (xk[i] - xkp[i]);
            }
            if (Math.Sqrt(norm) >= eps)
                return false;
            return true;
        }

        public double[] Zeydelia(double[,] A, double[] B, double eps, int n)
        {
            double[] p = new double[n];

            double[] x = new double[n];

            do
            {
                for (int i = 0; i < n; i++)
                    p[i] = x[i];

                for (int i = 0; i < n; i++)
                {
                    double var = 0;
                    for (int j = 0; j < i; j++)
                        var += (A[i, j] * x[j]);
                    for (int j = i; j < n; j++)
                        var += (A[i, j] * p[j]);
                    x[i] = (B[i] - var) / A[i, i];
                }
            } while (converge(x, p, n));

            return x;
        }


        public double[] newX(double[] xOld, double[] delta)
        {
            double[] x = new double[xOld.Length];
            for (int i = 0; i < xOld.Length; i++) {
                x[i] = xOld[i] + delta[i];
            
            }

            return x;
        }

        public double sumArray(double[] arr) {
            double summ = 0;
            for (int i = 0; i < arr.Length; i++) {
                summ += arr[i];
            }
            return summ;
        }


        public void table1Show(int n, double[] needs, double[] prices) {

            dataGridView1.RowCount = n;
            dataGridView1.ColumnCount = 3;

            dataGridView1.Columns[0].HeaderText = "номер";
            dataGridView1.Columns[1].HeaderText = "прогнозований попит";
            dataGridView1.Columns[2].HeaderText = "ціни";

            for (int i = 0; i < n; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = needs[i];
                dataGridView1.Rows[i].Cells[2].Value = prices[i];
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
            }

        }

        public void table2Show(int n, double[,] A, double[] B) {
            dataGridView2.RowCount = n;
            dataGridView2.ColumnCount = n + 1;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    dataGridView2.Rows[i].Cells[j].Value = A[i, j];
                }
            }

            for (int i = 0; i < n; i++)
            {

                dataGridView2.Rows[i].Cells[n].Value = B[i];

            }

        }

        public void showKoef(double []koef) {
            for (int i = 0; i<koef.Length;i++) {
                listBox3.Items.Add(koef[i]);
            
            }
        }

        public void showDeltaX(double[] deltaX)
        {
            for (int i = 0; i < deltaX.Length; i++)
            {
                listBox1.Items.Add(deltaX[i]);

            }
        }

        public void showX(double[] x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                listBox2.Items.Add(Math.Round(x[i],4));

            }
        }

        public string checkDoubleArray(string str, int n, int kind) {
            double[] arr = new double[n];
            string[] arrStr = str.Split(" ");
            while (arrStr.Length!=n) {
                if (kind == 0)
                {
                    string needsString = Interaction.InputBox("Введіть прогнозований попит " + n + " через пробіл", "", "11 16 5", -1, -1);
                    checkDoubleArray(needsString, n, 0);
                }
                else {

                    string pricesString = Interaction.InputBox("Введіть ціни " + n + " через пробіл", "", "125 105 170", -1, -1);
                    checkDoubleArray(pricesString, n, 1);
                }
            
            }


            return str;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            int n = Convert.ToInt32(textBox1.Text);
            double budget = Convert.ToDouble(textBox2.Text);
            double epsilon = Convert.ToDouble(textBox3.Text);
            //получить попит
            string needsString = Interaction.InputBox("Введіть прогнозований попит " +n+" через пробіл", "", "11 16 5", -1, -1);
            string newNeedsStr = checkDoubleArray(needsString, n, 0);
            double []needs = splitString(newNeedsStr);
            //получить ціни
            string pricesString = Interaction.InputBox("Введіть ціни " + n + " через пробіл", "", "125 105 170", -1, -1);
            string newpricesStr = checkDoubleArray(pricesString, n, 1);
            double[] prices = splitString(newpricesStr);

            //A
            double[,] A = Amatrix(prices);
            //B
            double[] B = Bmatrix(prices, needs, budget);

            //delta X
            double[] deltaX = Zeydelia(A, B, epsilon, n);
            double[] delta = {-0.3556, -0.1778, -0.8889 };

            //X
            double[] x = newX(needs, delta);
            

            //вывод таблиц
            table1Show(n, needs, prices);
            table2Show(n, A, B);
            //
            showDeltaX(delta);
            //
            showX(x);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
