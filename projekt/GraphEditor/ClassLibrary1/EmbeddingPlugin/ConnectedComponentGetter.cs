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
        private Embedding embeding;
        private Dictionary<int, bool> visited;
        List<int> comp;

        /// <summary>
        /// creates new instance of component getter
        /// </summary>
        /// <param name="embeding">embedding to search components in</param>
        public ConnectedComponentGetter(Embedding embeding)
        {
            this.embeding = embeding;
            visited = new Dictionary<int, bool>();
            foreach (int v in embeding.neighbors.Keys)
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
            foreach (var v in embeding.neighbors.Keys)
            {
                comp = new List<int>();                
                if (visited[v] == false)
                {
                    comp.Add(v);
                    visit(v);
                }
                if (comp.Count != 0)
                {
                    Embedding component = new Embedding(comp, embeding);
                    returnList.Add(component);
                }
            }
            return returnList;
        }

        /// <summary>
        /// dfs visit functino
        /// </summary>
        /// <param name="u"></param>
        private void visit(int u)
        {
            visited[u] = true;
            foreach (int v in embeding.neighbors[u])
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