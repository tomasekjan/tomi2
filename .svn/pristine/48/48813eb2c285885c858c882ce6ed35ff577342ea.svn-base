using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using System.Collections;
using System.Drawing;

namespace Plugin
{
    class CircularListEdge : List<EdgeMultiGraph>
    {
        internal EdgeMultiGraph GetBeforeValue(EdgeMultiGraph value)
        {
            EdgeMultiGraph before = null;
            for (int i = 0; i < this.Count; i++)
            {
                EdgeMultiGraph bef = this[i % this.Count];
                EdgeMultiGraph selected = this[(i + 1) % this.Count];
                if (selected == value)
                {
                    before = bef;
                }
            }
            return before;
        }

        public EdgeMultiGraph this[int i]
        {            
            get
            {
                int index = i % Count;
                return base[index];
            }
            set
            {
                int index = i % Count;
                base[index] = value;
            }
        }

        public void Insert(int i, EdgeMultiGraph item)
        {
            base.Insert(i, item);
        }

        internal EdgeMultiGraph GetAfterValue(EdgeMultiGraph value)
        {
            EdgeMultiGraph after = null;
            for (int i = 0; i < this.Count; i++)
            {
                EdgeMultiGraph af = this[(i + 1) % this.Count];
                EdgeMultiGraph selected = this[i % this.Count];
                if (value == selected)
                {
                    after = af;
                }
            }
            return after;
        }
        internal void InserBeintwen(EdgeMultiGraph value, EdgeMultiGraph before, EdgeMultiGraph after)
        {
            for (int i = 0; i < this.Count; i++)
            {
                EdgeMultiGraph bef = this[i % this.Count];
                int afterIndex = (i + 1) % this.Count;
                EdgeMultiGraph af = this[afterIndex];
                if (bef == before && af == after)
                {
                    this.Insert(afterIndex, value);
                    return;
                }
            }
            throw new ArgumentException();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (EdgeMultiGraph edge in this)
            {
                sb.Append(edge.V);
                sb.Append(",");
            }
            return sb.ToString();
        }
    }
}
