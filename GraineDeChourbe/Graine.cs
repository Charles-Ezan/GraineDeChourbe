using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace GraineDeChourbe
{
    public class Graine
    {
        private int xpos;
        private int ypos;
        private bool status;
        private int index;
        private int expiration_date;
        private bool rotten_img;
        public Graine(int x, int y, int newIndex)
        {
            xpos = x;
            ypos = y;
            status = false;
            index = newIndex;
            expiration_date = 100;
            rotten_img = false;
        }

        public int get_index() { return index; }

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
            return status;
        }

        public void set_status(bool new_status)
        {
            status = new_status;
        }

        public bool get_rotten_img()
        {
            return rotten_img;
        }

        public void set_rotten_img(bool new_rotten)
        {
            rotten_img = new_rotten;
        }            

        public void update_seed_expiration()
        {
            expiration_date -= 1;
            if(expiration_date < 0)
            {
                set_status(true);
                expiration_date -= 10;
            }
        }

        public int get_expiration()
        {
            return expiration_date;
        }
    }
}
