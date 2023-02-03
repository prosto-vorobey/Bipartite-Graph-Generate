using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лабораторная_работа_2
{
    public class Graph
    {
        int FNodeCount;
        int[,] Matrix;

        public Graph (int n, int[,] M)
        {
            FNodeCount = n;
            Matrix = new int[n, n];
            Matrix = M;
        }

        public int NodeCount
        { get { return FNodeCount; } }

        public List<int> GetNeighbors(int NodeNumber)
        {
            List<int> Neighbors = new List<int>();
            for (int i=0; i< FNodeCount; i++)
            {
                if (Matrix[NodeNumber, i] == 1)
                    Neighbors.Add(i);
            }
            return Neighbors;
        }

        public void RemoveNode(int NodeNumber)
        {
            for (int i=0; i < FNodeCount; i++)
            {
                Matrix[i, NodeNumber] = -1;
                Matrix[NodeNumber, i] = -1;
            }
        }

        public void RemoveEdge(int NodeNumber1, int NodeNumber2)
        {
            if (IsNode(NodeNumber1) & (IsNode(NodeNumber2)))
            {
                Matrix[NodeNumber1, NodeNumber2] = 0;
                Matrix[NodeNumber2, NodeNumber1] = 0;
            }
        }

        public void AddNode()
        {
            int[,] saveMatrix = Matrix;
            FNodeCount++;
            Matrix = new int[FNodeCount, FNodeCount];
            for (int i = 0; i < FNodeCount - 1; i++)
                for (int j = 0; j < FNodeCount - 1; j++)
                    Matrix[i, j] = saveMatrix[i, j];
            for (int i = 0; i < FNodeCount; i++)
            {
                Matrix[i, FNodeCount - 1] = 0;
                Matrix[FNodeCount - 1, i] = 0;
            }

        }

        public void AddEdge(int NodeNumber1, int NodeNumber2)
        {
            if (IsNode(NodeNumber1) & (IsNode(NodeNumber2)))
            {
                Matrix[NodeNumber1, NodeNumber2] = 1;
                Matrix[NodeNumber2, NodeNumber1] = 1;
            }

        }

        public bool IsEdge(int Node1, int Node2)
        {
            if ((Matrix[Node1, Node2] == 1) || (Matrix[Node2, Node1] == 1))
                return true;
            else return false;
        }

        public bool IsNode(int Node)
        {
            if (Matrix[Node, Node] != -1)
                return true;
            else return false;
        }
    }
}
