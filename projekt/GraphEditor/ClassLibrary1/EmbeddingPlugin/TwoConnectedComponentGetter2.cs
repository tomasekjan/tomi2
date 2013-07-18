using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddInView;
using GraphEditor.GraphDeclaration;
using System.Diagnostics;
using System.Drawing;
using System.Collections;

namespace Plugin
{
    class TwoConnectedComponentGetter
    {
        //http://www.cs.umd.edu/class/fall2005/cmsc451/biconcomps.pdf
        Dictionary<int, VertexProperities> vertexs;
        int count = 0;
        Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
        Embedding embeding;
        List<Embedding> returnList = new List<Embedding>();

        public TwoConnectedComponentGetter(Embedding embeding)
        {
            this.embeding = embeding;
            vertexs = new Dictionary<int, VertexProperities>();
            foreach (int i in embeding.neighbors.Keys)
            {
                vertexs.Add(i, new VertexProperities());
                vertexs[i].visited = false;
                vertexs[i].parrent = -1;
                vertexs[i].d = -1;
                vertexs[i].low = -1;
            }
        }

        public List<Embedding> GetComponents()
        {

            int i = 0;
            foreach (int v in vertexs.Keys)
            {
                if (!vertexs[v].visited)
                {
                    visit(i);
                }
                i = i + 1;
            }

            return returnList;
        }

        private void visit(int u)
        {

            vertexs[u].visited = true;
            count++;
            vertexs[u].d = count;
            vertexs[u].low = vertexs[u].d;
            foreach (int v in embeding.neighbors[u])
            {
                if (!vertexs[v].visited)
                {
                    Push(u, v);
                    vertexs[v].parrent = u;
                    visit(v);
                    if (vertexs[v].low >= vertexs[u].d)
                    {
                        returnList.Add(GetComponent(u, v));
                    }
                    vertexs[u].low = vertexs[u].low < vertexs[v].low ? vertexs[u].low : vertexs[v].low;
                }
                else
                {
                    if (!(vertexs[u].parrent == v) && (vertexs[v].d < vertexs[u].d))
                    {
                        Push(u, v);
                        vertexs[u].low = vertexs[u].low < vertexs[v].d ? vertexs[u].low : vertexs[v].d;
                    }
                }
            }
        }

        private Embedding GetComponent(int u, int v)
        {
            return new Embedding(stack, u, v);
        }

        private void Push(int u, int v)
        {
            stack.Push(new Tuple<int, int>(u, v));
        }

        class VertexProperities
        {
            public bool visited;
            public int parrent;
            public int d;
            public int low;
        }
    }
}
