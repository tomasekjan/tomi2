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
        /// <summary>
        /// get edge which is before vale edge
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// gets or sets edge on index
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>edge on given index</returns>
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


        /// <summary>
        /// insert item on given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, EdgeMultiGraph item)
        {
            base.Insert(index, item);            
        }

        /// <summary>
        /// get edge which is stored after value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// inserts new value betwen before an after or fais if it is inposible
        /// </summary>
        /// <param name="value"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>        
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

        /// <summary>
        /// debuging to string
        /// </summary>
        /// <returns></returns>
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
