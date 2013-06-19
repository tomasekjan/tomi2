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
        private Embeding embeding;
        private Dictionary<int, bool> visited;
        List<int> comp;
        public ConnectedComponentGetter(Embeding embeding)
        {
            this.embeding = embeding;
            visited = new Dictionary<int, bool>();
            foreach (int v in embeding.neighbors.Keys)
            {
                visited.Add(v, false);
            }
        }
        public List<Embeding> GetComponents()
        {
            List<Embeding> returnList = new List<Embeding>();
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
                    Embeding component = new Embeding(comp, embeding);
                    returnList.Add(component);
                }
            }
            return returnList;
        }

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