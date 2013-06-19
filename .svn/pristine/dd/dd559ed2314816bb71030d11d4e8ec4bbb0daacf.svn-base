using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using System.Collections;
using System.Drawing;

namespace Plugin
{
    public class CircularListInt : List<int>
    {
        public CircularListInt(IEnumerable<int> circularList) :base(circularList)
        {
            
        }

        public CircularListInt():base()
        {

        }
        public int GetBeforeValue(int value)
        {
            int before = -1;
            for (int i = 0; i < this.Count; i++)
            {
                int bef = this[i % this.Count];
                int selected = this[(i + 1) % this.Count];
                if (value == selected)
                {
                    before = bef;
                }
            }
            return before;
        }

        public int GetBeforeSecondValue(int value)
        {
            int before = -1;
            bool first = false;
            for (int i = 0; i < this.Count; i++)
            {
                int bef = this[i % this.Count];
                int selected = this[(i + 1) % this.Count];
                if (value == selected)
                {
                    if (first)
                    {
                        before = bef;                        
                    }
                    else
                    {
                        first = true;
                    }
                }
            }

            return before;
        }

        public int GetAfterValue(int value)
        {
            int after = -1;
            for (int i = 0; i < this.Count; i++)
            {
                int af = this[(i + 1) % this.Count];
                int selected = this[i % this.Count];
                if (value == selected)
                {
                    after = af;
                }
            }
            return after;
        }

        public int GetAfterSecondValue(int value)
        {
            int after = -1;
            bool first = true;
            for (int i = 0; i < this.Count; i++)
            {
                int af = this[(i + 1) % this.Count];
                int selected = this[i % this.Count];
                if (value == selected)
                {
                    if (first)
                    {
                        after = af;
                    }
                    else
                    {
                        first = true;
                    }
                }
            }
            return after;
        }

        public void InserBeintwen(int value, int before, int after)
        {
            for (int i = 0; i < this.Count; i++)
            {
                int bef = this[i % this.Count];
                int afterIndex = (i + 1) % this.Count;
                int af = this[afterIndex];
                if (bef == before && af == after)
                {
                    this.Insert(afterIndex, value);
                    return;
                }
            }
            throw new ArgumentException();
        }
    }
}
