﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thread = System.Threading;
using System.Diagnostics;

namespace GraineDeChourbe
{
    class Pigeon
    {
        public bool alive = false;
        public int xpos;
        public int ypos;
        private int speed;
        private (int, int) moveDirection;
        private string state;
        private int index;
        private List<Graine> belief;
        private (int, int) desire;
        public Image img;
        public (int, int) windowSize;


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
            windowSize = (646, 354);
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

        public (int, int) get_desire()
        {
            return desire;
        }

        public void set_desire((int, int) new_desire)
        {
            desire = new_desire;
        }

        public List<Graine> get_belief()
        {
            return belief;
        }

        public void set_belief(List<Graine> new_belief)
        {
            belief = new_belief;
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

        // Limits the movement of the pigeon so that it does not leave the window
        public bool window_limit()
        {
            bool is_at_the_limit = false;
            (int, int) xlimit = (45, windowSize.Item1 - 45);
            (int, int) ylimit = (35, windowSize.Item2 - 35);

            if(xlimit.Item1 > get_xpos() || get_xpos() > xlimit.Item2)
            {
                is_at_the_limit = true;
            }
            if(ylimit.Item1 > get_ypos() || get_ypos() > ylimit.Item2)
            {
                is_at_the_limit = true;
            }

            return is_at_the_limit;
        }

        public void sleep()
        {
            set_speed(0);
        }

        public void find_neerest_seed()
        {
            List<Graine> seeds = get_belief();
            double shortest_distance = distance_seed_calculation(seeds[0]);
            (int, int) neerest_seed = seeds[0].get_pos();
            //foreach(Graine seed in seeds)
            for(int i=0; i<seeds.Count ; i++ )
            {
                double seed_distance = distance_seed_calculation(seeds[i]);
                if(seed_distance < shortest_distance)
                {
                    shortest_distance = seed_distance;
                    neerest_seed = seeds[i].get_pos();
                }
            }
            set_desire(neerest_seed);
        }

        // Allows the pigeon's desire to be fixed on the nearest seed
        public void find_freshest_seed()
        {
            List<Graine> seeds = get_belief();
            for (int i = 0; i < seeds.Count; i++)
            {
                if (seeds[i].get_status() == false)
                {
                    set_desire(seeds[i].get_pos());
                }
                else if(seeds.Count == 1 || seeds[i].get_rotten_img() == true)
                {
                    set_desire(get_pos());
                }
            }
        }

        // Changes the direction of the pigeon by directing it towards the food
        public void move_to_food()
        {
            Random random_speed = new Random();

            (int, int) food_direction = (get_desire().Item1 - get_pos().Item1,  (get_desire().Item2 - get_pos().Item2));
            set_direction(food_direction);
        }

        public void move_random()
        {
            Random random_speed = new Random();
            Random random_first_direction = new Random();
            Random random_second_direction = new Random();
            (int, int) random_direction = (random_first_direction.Next(-100, 100), random_second_direction.Next(-100, 100));
            set_speed(random_speed.Next(7, 10));
            set_direction(random_direction);
        }

        // Returns the distance between the pigeon and the seed
        public double distance_seed_calculation(Graine seed)
        {
            double distance = Math.Sqrt(Math.Pow(seed.get_xpos() - get_xpos(), 2) + Math.Pow(seed.get_ypos() - get_ypos(), 2));
            return distance;
        }

        // Calculates the distance the pigeon has to travel to reach its target
        private double distance_target_calculation()
        {
            double direction_pow = Math.Pow(get_desire().Item1 - get_pos().Item1, 2) + Math.Pow(get_desire().Item2 - get_pos().Item2, 2);
            double distance = (Math.Sqrt(direction_pow));
            return distance;
        }

        public (int, int) next_position(int delta_time, String new_state)
        {
            // Distance between the position of the pigeon and the position of its target
            double distance_target = distance_target_calculation();
            // Number of pixels covered during the time period
            double pixel_delta_time = get_speed() * delta_time;

            // Calculation of the ratio between the distance to the target and the distance the pigeon can travel during delta_time
            double distance_ratio = distance_target / pixel_delta_time;

            if(distance_ratio < 1)
            {
                distance_ratio = 1;
            }

            // If there are no seeds on the window, the pigeon will not move
            if ((get_belief().Count < 1) && (new_state == "food"))
            {
                return get_pos();
            }

            // Corresponds to the direction vector of the pigeon
            (double, double) travel_vector = ((get_direction().Item1 / distance_ratio),
               get_direction().Item2 / distance_ratio);

            if(distance_target == 0)
            {
                (int, int) new_position = get_pos();
                return new_position;
            }
            else
            {
                (int, int) new_position = (get_xpos() + Convert.ToInt32(travel_vector.Item1), (get_ypos() + Convert.ToInt32(travel_vector.Item2)));
                return new_position;
            }

        }

        // Returns the position of the seed eaten by the pigeon
        public (int, int) eat()
        {
            (int, int) pos_seed_eat = (-1, -1);
            if(get_pos() == get_desire())
            {
                pos_seed_eat = get_desire();
            }
            return pos_seed_eat;
        }

        public (int, int) run(string new_state, int delta_time)
        {
            (int, int) eat_seed_position = (-1, -1);

            if (new_state == "sleep")
            {
                sleep();
                set_position(next_position(delta_time, new_state));
            }

            else if(new_state == "food")
            {
                find_freshest_seed();
                move_to_food();
                set_position(next_position(delta_time,new_state));
                eat_seed_position = eat();
            }

            else if(new_state == "random")
            {
                move_random();
                if (window_limit() == false)
                {
                    set_position(next_position(delta_time, new_state));
                }
            }

            else
            {
                throw new ArgumentException("Error in the state name");
            }
            return eat_seed_position;
        }
    }
}
