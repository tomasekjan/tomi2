using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using System.Collections;
using System.Drawing;

namespace Plugin
{
    public class Permutations<T>
    {
        private T[] sourceArray;
        private Predicate<T[]> functionForPermutation;
        private bool success = false;
        private int count;

        public void Eval(T[] items, Predicate<T[]> SuccessFunc)
        {
            this.sourceArray = items;
            this.functionForPermutation = SuccessFunc;
            this.count = items.Count();
            Recursion(0);
        }

        private void Recursion(int beginIndex)
        {
            T tmp;

            if (beginIndex == count)
            {
                success = functionForPermutation(sourceArray);
            }
            else
            {
                for (int i = beginIndex; i < count; i++)
                {
                    tmp = sourceArray[beginIndex];
                    sourceArray[beginIndex] = sourceArray[i];
                    sourceArray[i] = tmp;

                    Recursion(beginIndex + 1);

                    if (success)
                        break;

                    tmp = sourceArray[beginIndex];
                    sourceArray[beginIndex] = sourceArray[i];
                    sourceArray[i] = tmp;
                }
            }
        }
    }
}
