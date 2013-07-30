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
        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="circularList">source list</param>
        public CircularListInt(IEnumerable<int> circularList) :base(circularList)
        {
            
        }

        public CircularListInt():base()
        {

        }

        /// <summary>
        /// gets value in circular list which is just before given value
        /// </summary>
        /// <param name="value">value to search for</param>
        /// <returns>value before given value</returns>
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
                    break;
                }
            }
            return before;
        }

        /// <summary>
        /// gets value of circular list which is just before second appearance of given value
        /// </summary>
        /// <param name="value">value to search for</param>
        /// <returns>value before second appearance of given value</returns>
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
                        break;
                    }
                    else
                    {
                        first = true;
                    }
                }
            }

            return before;
        }
        /// <summary>
        /// gets value of circular list which is just after given value
        /// </summary>
        /// <param name="value">value to search for</param>
        /// <returns>value after given value</returns>
        public int GetAfterValue(int value)
        {
            int after = -1;
            for (int i = 1; i <= this.Count; i++)
            {
                int af = this[(i + 1) % this.Count];
                int selected = this[i % this.Count];
                if (value == selected)
                {
                    after = af;
                    break;
                }
            }
            return after;
        }

        /// <summary>
        /// gets value of circular list which is just after second appearance of given value
        /// </summary>
        /// <param name="value">value to search for</param>
        /// <returns>value after second appearance of given value</returns>
        public int GetAfterSecondValue(int value)
        {
            int after = -1;
            bool first = false;
            for (int i = 1; i <= this.Count; i++)
            {
                int af = this[(i + 1) % this.Count];
                int selected = this[i % this.Count];
                if (value == selected)
                {
                    if (first)
                    {
                        after = af;
                        break;
                    }
                    else
                    {
                        first = true;
                    }
                }
            }
            return after;
        }
                
        /// <summary>
        /// insert value in circular list between two given values
        /// </summary>
        /// <param name="value">value to insert</param>
        /// <param name="before">insert after this </param>
        /// <param name="after">insert before this</param>
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
