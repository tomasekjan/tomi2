using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using System.Collections;
using System.Drawing;

namespace Plugin
{
    class Permutations
    {
        private int elementLevel = -1;
        private int numberOfElements;
        private int[] permutationValue;

        private int[] inputSet;

        public Permutations(int[] inputSet)
        {
            this.inputSet = inputSet;
            permutationValue = new int[inputSet.Length];
            numberOfElements = inputSet.Length;
        }
        private int permutationCount = 0;
        public int PermutationCount
        {
            get { return permutationCount; }
        }

        public void CalcPermutation(int k)
        {
            elementLevel++;
            permutationValue.SetValue(elementLevel, k);

            if (elementLevel == numberOfElements)
            {
                int[] tmp = new int[permutationValue.Length];
                Array.Copy(permutationValue, tmp, permutationValue.Length);
                list.Add(tmp);
            }
            else
            {
                for (int i = 0; i < numberOfElements; i++)
                {
                    if (permutationValue[i] == 0)
                    {
                        CalcPermutation(i);
                    }
                }
            }
            elementLevel--;
            permutationValue.SetValue(0, k);

        }

        List<int[]> list;
        internal List<int[]> CalcPermutation()
        {
            list = new List<int[]>();
            CalcPermutation(0);
            return list;
        }
    }
}
