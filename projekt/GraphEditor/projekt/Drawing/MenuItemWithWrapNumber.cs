using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphEditor.GraphDeclaration;
using GraphEditor.Primitives3D;
using xna = Microsoft.Xna.Framework;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor.Drawing
{
    public class MenuItemWithWrapNumber : MenuItem
    {
        int wrapNumber;
        String joinName;
        public int WrapNumber
        {
            get
            {
                return wrapNumber;
            }
            set
            {
                wrapNumber = value;
                this.Header = String.Format("{0} - wrap number = {1}", JoinName, wrapNumber);
            }
        }
        public String JoinName
        {
            get
            {
                return joinName;
            }
            set
            {
                joinName = value;
            }
        }

        internal void SwitchWrapnumber()
        {
            WrapNumber = ((wrapNumber + 2) % 3) - 1;
        }
    }
}
