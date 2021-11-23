using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using thread = System.Threading;
using Mutex = System.Threading.Mutex;
using System.Threading.Tasks;

namespace GraineDeChourbe
{
    class Environment
    {
        // Création du Mutex
        private static Mutex mutex = new Mutex();

        // Feared pigeon when there is no seed on map
        public bool fearedPigeon = false;

        // Handler pour avertir la class form d'un changement dans la class environment et ainsi mettre
        // à jour le visuel
        public EventHandler udpateSeeds;

        public delegate void UpdateDelegate(object sender, UpdateEventArgs args);
        public event UpdateDelegate UpdateEventHandler;
        public event UpdateDelegate ReplaceSeedEvent;


        public List<Pigeon> pigeons = new List<Pigeon>();
        public List<Graine> graines = new List<Graine>();


        // Index de la dernière graine ajoutée
        public int lastSeedIndex = 0;
        public int deletedSeedIndex = 0;


        public bool pigeon_alive = false;
        public Random random_number = new Random();

        public thread.Thread threadPigeon1;
        public thread.Thread threadPigeon2;
        public thread.Thread threadPigeon3;

        // Créer les pigeons et initialisation des threads relatifs
        public void initialise()
        {
            for (int i = 1; i < 4; i++)
            {
                addPigeon(i * 150, 70, i, GraineDeChourbe.Properties.Resources.pigeon_1);
                pigeons[i - 1].set_belief(graines);
            }

            threadPigeon1 = new thread.Thread(() =>
            {
                run(pigeons[0]);
            });
            threadPigeon1.Name = "pigeon 1";

            threadPigeon2 = new thread.Thread(() =>
            {
                run(pigeons[1]);
            });

            threadPigeon2.Name = "pigeon 2";

            threadPigeon3 = new thread.Thread(() =>
            {
                run(pigeons[2]);
            });

            threadPigeon3.Name = "pigeon 3";
        }

        // Ajout de graines dans l'environnement
        public void addSeed(int newX, int newY)
        {
            lastSeedIndex = +1;

            Graine newSeed = new Graine(newX, newY,lastSeedIndex);
            graines.Add(newSeed);
        }

        // Ajout de pigeons dans l'environnement
        public void addPigeon(int newX, int newY, int newIndex, Bitmap newImg)
        {
            newImg.MakeTransparent(newImg.GetPixel(0, 0));
            Pigeon newPigeon = new Pigeon(newX, newY, newIndex, newImg);
            pigeons.Add(newPigeon);
        }

        public void run(Pigeon pigeon)
        {
            while (pigeon_alive)
            {
                for (int i = 0; i < graines.Count-1; i++)
                {
                    if (graines[i].get_status() == false)
                    { 
                        graines[i].update_seed_expiration(); 
                    }
                    if (graines[i].get_status() == true && graines[i].get_rotten_img() == false)
                    {
                        graines[i].update_seed_expiration();
                        UpdateEventArgs args = new UpdateEventArgs(graines[i].get_xpos(), graines[i].get_ypos());
                        ReplaceSeedEvent.Invoke(this, args);
                        graines[i].set_rotten_img(true);
                    }
                }

                thread.Thread.Sleep(50);

                pigeon.set_belief(graines);
                // Applies the status of the pigeon
                string pigeon_status = "sleep";

                if (fearedPigeon)
                {
                    pigeon_status = "random";
                }
                else if (graines.Count > 0)
                {
                    pigeon_status = "food";
                }

                pigeon.set_speed(random_number.Next(1, 10));
                (int, int) pos_seed_eat = pigeon.run(pigeon_status, 3);

                // Should send coordinate
                if (pos_seed_eat == (-1, -1))
                {
                    continue;
                }
                else
                {
                    //Implémentation du thread
                    CriticalZone(pos_seed_eat.Item1, pos_seed_eat.Item2);
                }
            }
        }

        public void stopPigeon()
        {
            //pigeon_is_here = false;
        }

        public String randomPigeonMove()
        {
            Random random = new Random();
            int num = random.Next(100);

            String move = "sleep";
            if (num > 80)
            {
                move = "random";
            }
            return move;
        }

        // On supprime la graine qui a été mangée
        public void deleteSeed(int seedX, int seedY)
        {
            for (int i = 0; i < graines.Count; i++)
            {
                if ((graines[i].get_xpos() == seedX) && (graines[i].get_ypos() == seedY))
                {
                    deletedSeedIndex = graines[i].get_index();
                    graines.RemoveAt(i);
                    if (udpateSeeds != null)
                    {
                        udpateSeeds(this, null);
                    }

                    // handler avec arguments
                    UpdateEventArgs args = new UpdateEventArgs(seedX, seedY);
                    UpdateEventHandler?.Invoke(this, args);
                }
            }
        }

        void CriticalZone(int seedX, int seedY)
        {
            try
            {
                // On attend avant d'accéder à la liste de graines que celle-ci soit libre
                mutex.WaitOne();
                deleteSeed(seedX, seedY);
            }
            finally
            {
                // Libère la liste de graines de tel sorte à ce que les pigeons puissent y accéder
                mutex.ReleaseMutex();
            }

        }

        // Classe permettant de passer des données lorsque l'on notifie la class form d'un changement dans 
        // la classe environment
        public class UpdateEventArgs : EventArgs
        {
            public int x;
            public int y;
            public UpdateEventArgs(int new_x, int new_y)
            {
                x = new_x;
                y = new_y;
            }
        }
    }
}
