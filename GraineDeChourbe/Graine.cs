using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraineDeChourbe
{
    public class Graine
    {
        private int xpos;
        private int ypos;
        private bool rotten;

        public Graine(int x, int y, bool isRotten)
        {
            xpos = x;
            ypos = y;
            rotten = isRotten;
        }

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
            return (xpos, ypos);
        }

        public bool get_status()
        {
            return rotten;
        }
    }
}
