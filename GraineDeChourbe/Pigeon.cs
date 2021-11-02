using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GraineDeChourbe
{
    class Pigeon
    {
        public int xpos;
        public int ypos;
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
            speed = 0;
            moveDirection = (0, 0);
            state = "sleep";
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

        public void set_position((int, int) new_pos)
        {
            xpos = new_pos.Item1;
            ypos = new_pos.Item2;
        }

        public int get_speed()
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

        // Changes the direction of the pigeon by directing it towards the food
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
            Random random_first_direction = new Random();
            Random random_second_direction = new Random();
            (int, int) random_direction = (random_first_direction.Next(-10, 10), random_second_direction.Next(-10, 10));
            set_speed(random_speed.Next(3, 5));
            set_direction(random_direction);
        }

        // Calculates the distance the pigeon has to travel to reach its target
        private double distance_target_calculation(int delta_time)
        {
            double direction_pow = Math.Pow(get_direction().Item1, 2) + Math.Pow(get_direction().Item2, 2);
            double distance = (Math.Sqrt(direction_pow));
            return distance;
        }

        public (int, int) next_position(int delta_time)
        {
            // Distance between the position of the pigeon and the position of its target
            double distance_target = distance_target_calculation(delta_time);
            // Number of pixels covered during the time period
            double pixel_delta_time = get_speed() * delta_time;

            // Calculation of the ratio between the distance to the target and the distance the pigeon can travel during delta_time
            double distance_ratio = Math.Ceiling(distance_target / pixel_delta_time);

            // Calculation of the travel vector           
            (double, double) travel_vector = (Math.Ceiling(get_direction().Item1 / distance_ratio),
                Math.Ceiling(get_direction().Item2 / distance_ratio));

            (int, int) new_position = (get_xpos() + (int) Math.Ceiling(travel_vector.Item1), (int) Math.Ceiling(get_ypos() + travel_vector.Item2));

            return new_position;
        }

        public void run(string new_state, int delta_time)
        {
            if(new_state == state && new_state != "food")
            {
                set_position(next_position(delta_time));
            }
            
            else if(new_state == "sleep")
            {
                sleep();
                set_position(next_position(delta_time));
            }

            else if(new_state == "food")
            {
                move_to_food();
                set_position(next_position(delta_time));
            }

            else if(new_state == "random")
            {
                move_random();
                set_position(next_position(delta_time));
            }

            else
            {
                throw new ArgumentException("Error in the state name");
            }
        }
    }
}
