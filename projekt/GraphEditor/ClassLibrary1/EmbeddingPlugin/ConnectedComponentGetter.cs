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
    class ConnectedComponentGetter
    {
        private Embedding embedding;
        private Dictionary<int, bool> visited;
        List<int> comp;

        /// <summary>
        /// creates new instance of component getter
        /// </summary>
        /// <param name="embedding">embedding to search components in</param>
        public ConnectedComponentGetter(Embedding embedding)
        {
            this.embedding = embedding;
            visited = new Dictionary<int, bool>();
            foreach (int v in embedding.neighbors.Keys)
            {
                visited.Add(v, false);
            }
        }

        /// <summary>
        /// get connected components of graph described by embedding
        /// </summary>
        /// <returns></returns>
        public List<Embedding> GetComponents()
        {
            List<Embedding> returnList = new List<Embedding>();
            foreach (int v in embedding.neighbors.Keys)
            {
                comp = new List<int>();                
                if (visited[v] == false)
                {
                    comp.Add(v);
                    visit(v);
                }
                if (comp.Count != 0)
                {
                    Embedding component = new Embedding(comp, embedding);
                    returnList.Add(component);
                }
            }
            return returnList;
        }

        /// <summary>
        /// dfs visit function
        /// </summary>
        /// <param name="u"></param>
        private void visit(int u)
        {
            visited[u] = true;
            foreach (int v in embedding.neighbors[u])
            {
                if (visited[v] == false)
                {
                    comp.Add(v);
                    visit(v);
                }
            }
        }
    }
}