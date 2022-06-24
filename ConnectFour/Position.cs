using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ConnectFour
{
    internal class Position
    {
        public int X;
        public int Y;
        private int[] _position;
        public Position(int[] position)
        {
            _position = position;
            X = position[0];
            Y = position[1];

        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
