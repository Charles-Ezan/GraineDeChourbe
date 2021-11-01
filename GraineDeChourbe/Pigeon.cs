using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraineDeChourbe
{
    class Pigeon
    {
        private int xpos;
        private int ypos;
        private int speed;
        private (int, int) moveDirection;
        private string state;
        private int index;
        public Image img;

        public Pigeon(Image new_img)
        {
            xpos = 0;
            ypos = 0;
            img = new_img;
            // Pixels/sec
            speed = 0;
            moveDirection = (0, 0);
            state = "sleep";
        }

        public Pigeon(int new_x, int new_y, int new_index, Image new_img)
        {
            xpos = new_x;
            ypos = new_y;
            index = new_index;
            img = new_img;
            // Pixels/sec
            //speed = 0;
            //moveDirection = (0, 0);
            //state = "sleep";
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
            return (ypos, xpos);
        }

        public float get_speed()
        {
            return speed;
        }

        public void set_speed(int new_speed)
        {
            speed = new_speed;
        }

        public (int, int) get_direction()
        {
            return moveDirection;
        }

        public void set_direction((int, int) new_direction)
        {
            moveDirection = new_direction;
        }

        public string get_state()
        {
            return state;
        }


        public void set_state(string new_state)
        {
            if(new_state == "sleep" || new_state == "food" || new_state == "random")
            {
                state = new_state;
            }
            else
            {
                throw new ArgumentException("This pigeon state doesn't exist");
            }
        }

        public int get_index()
        {
            return index;
        }

        public void set_index(int new_index)
        {
            index = new_index;
        }

        public void sleep()
        {
            set_speed(0);
        }

        public void move_to_food()
        {
            Random random_speed = new Random();
            set_speed(random_speed.Next(5, 10));
            // Function to get the nearest seed
            // Return coordinates (x_seed, y_seed)
            int x_seed = 10;
            int y_seed = 5;

            (int, int) food_direction = (get_pos().Item1 - x_seed, get_pos().Item2 - y_seed);
            set_direction(food_direction);
        }

        public void move_random()
        {
            Random random_speed = new Random();
            (int, int) random_direction = (new Random().Next(0, 10), new Random().Next(0, 10));
            set_speed(random_speed.Next(15, 20));
            set_direction(random_direction);
        }

        // Calculates the distance traveled by the pigeon in a certain period of time 
        private int distance_calculation(int delta_time)
        {
            double direction_pow = Math.Pow(get_direction().Item1, 2) + Math.Pow(get_direction().Item2, 2);
            double distance = (Math.Sqrt(direction_pow) * get_speed())/(1/delta_time);
            return Convert.ToInt32(distance);
        }

        public (int, int) next_position(int delta_time)
        {
            int distance = distance_calculation(delta_time);



            (int, int) new_position = (1, 2);
            return new_position;
        }
    }
}
