using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraineDeChourbe
{
    class Graine
    {
        private int xpos;
        private int ypos;

        public int get_xpos()
        {
            return xpos;
        }

        public int get_ypos()
        {
            return ypos;
        }

        public (int, int) get_pos()
        {
            return (ypos, xpos);
        }
    }
}
