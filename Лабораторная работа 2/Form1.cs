using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Лабораторная_работа_2
{
    public partial class Form1 : Form
    {
        List<Point> LP = new List<Point>();
        Graphics gr;
        Graph g;
        public Form1()
        {
            InitializeComponent();
        }

        public Graph Initialize(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string curStr = reader.ReadLine();
            string[] split = curStr.Split(' ');
            int N = Int32.Parse(split[0]);
            int NZ = Int32.Parse(split[1]);
            int[,] M = new int[N, N];
            for (int i = 0; i < NZ; i++)
            {
                curStr = reader.ReadLine();
                split = curStr.Split(' ');
                int _x = Int32.Parse(split[0]) - 1;
                int _y = Int32.Parse(split[1]) - 1;
                if (_x != _y)
                {
                    M[_y, _x] = 1;
                    M[_x, _y] = 1;
                }
            }

            Graph G = new Graph(N, M);

            return G;
        }

        private void DrawGraph(Graph g)
        {
            pictureBox1.Refresh();
            LP.Clear();
            Point centre = new Point(pictureBox1.Size.Width / 2, pictureBox1.Size.Height / 2);
            double alpha = 2 * Math.PI / g.NodeCount;
            int radius = (int)(Math.Min(pictureBox1.Size.Width, pictureBox1.Size.Height) * 0.75) / 2;
            for (int i=0;i<g.NodeCount; i++)
            {
                LP.Add(new Point(centre.X + (int)(Math.Cos(alpha * i) * radius), centre.Y - (int)(Math.Sin(alpha * i) * radius)));
            }

            gr = pictureBox1.CreateGraphics();

            for (int i = 0; i < g.NodeCount; i++)
                for (int j = 0; j < g.NodeCount; j++)
                {
                    if (g.IsEdge(i, j))
                        gr.DrawLine(new Pen(Color.Green), LP[i], LP[j]);
                }

            for (int i = 0; i < g.NodeCount; i++)
            {
                if (g.IsNode(i))
                {
                    gr.DrawEllipse(new Pen(Color.Red), LP[i].X - 2, LP[i].Y - 2, 4, 4);
                    gr.FillEllipse(new SolidBrush(Color.Red), LP[i].X - 2, LP[i].Y - 2, 4, 4);
                }
            }
        }

        private void открытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                g = Initialize(openFileDialog1.FileName);
                DrawGraph(g);
            }
        }

        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                int n = 0;
                for (int i = 0; i < g.NodeCount; i++)
                {
                    n += (g.GetNeighbors(i)).Count;
                }
                n /= 2;
                sw.WriteLine(g.NodeCount.ToString() + ' ' + n.ToString());
                for (int i = 0; i < g.NodeCount; i++)
                    for (int j = 0; j < g.NodeCount; j++)
                    {
                        if ((i < j) && (g.IsEdge(i, j) == true))
                            sw.WriteLine((i + 1).ToString() + ' '.ToString() + (j + 1).ToString());
                    }

                sw.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            g.AddNode();
            DrawGraph(g);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "0") & (g.IsNode(Int32.Parse(textBox1.Text) - 1)))
            {
                g.RemoveNode(Int32.Parse(textBox1.Text) - 1);
                DrawGraph(g);
            }
            else MessageBox.Show("Данная вершина была удалена. Введите другую!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((textBox2.Text != "0") & (textBox3.Text != "0") & (g.IsNode(Int32.Parse(textBox2.Text) - 1)) & (g.IsNode(Int32.Parse(textBox3.Text) - 1)))
            {
                g.AddEdge(Int32.Parse(textBox2.Text) - 1, Int32.Parse(textBox3.Text) - 1);
                DrawGraph(g);
            }
            else MessageBox.Show("Одна из вершин была удалена. Введите другую!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ((textBox2.Text != "0") & (textBox3.Text != "0") & (g.IsNode(Int32.Parse(textBox2.Text) - 1)) & (g.IsNode(Int32.Parse(textBox3.Text) - 1)))
            {
                g.RemoveEdge(Int32.Parse(textBox2.Text) - 1, Int32.Parse(textBox3.Text) - 1);
                DrawGraph(g);
            }
            else MessageBox.Show("Одна из вершин была удалена. Введите другую!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int n, k = 0;
            int[] l1 = new int[g.NodeCount];
            int[] l2 = new int[g.NodeCount];
            for (int j = 0; j < g.NodeCount; j++)
                l1[j] = -1;
            int flag = 0;
            l1[0] = 0;
            l2[0] = 0;
            k = 1;
            for (n = 0; n < g.NodeCount; n++) 
            {
                if (flag == 0)
                {
                    for (int j = 0; j < g.NodeCount; j++)
                        if ((g.GetNeighbors(l2[n]).Contains(j)) & (l1[j] == -1))
                        {
                            l1[j] = 1;
                            l2[k] = j;
                            k++;
                            flag = 1;
                        }
                }
                else
                {
                    for (int j = 0; j < g.NodeCount; j++)
                        if ((g.GetNeighbors(l2[n]).Contains(j)) & (l1[j] == -1))
                        {
                            l1[j] = 0;
                            l2[k] = j;
                            k++;
                            flag = 0;
                        }
                }
                if (k == g.NodeCount) break;
            }
            flag = 1;
            for (int i = 0; i < g.NodeCount; i++) 
            {
                for (int j = i + 1; j < g.NodeCount; j++)
                    if ((l1[i] == l1[j]) && (g.IsEdge(i, j))) flag = 0;
            }

            if (flag == 0) MessageBox.Show("Граф не двудольный");
            else MessageBox.Show("Граф двудольный");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int n = 1;
            for (int i = 0; i < g.NodeCount; i++)
            {
                if ((g.GetNeighbors(i).Count) % 2 == 1) n = 0;
            }
            if (n == 0) MessageBox.Show("Граф не Эйлеров");
            else MessageBox.Show("Граф Эйлеров");
        }
    }
}
