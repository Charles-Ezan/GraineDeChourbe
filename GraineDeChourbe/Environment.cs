using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
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

        // Liste de thread

        // Handler
        public EventHandler udpateSeeds;

        public delegate void UpdateDelegate(object sender, UpdateEventArgs args);
        public event UpdateDelegate UpdateEventHandler;
        public event UpdateDelegate ReplaceSeedEvent;

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


        //public delegate void SettingsSavedEventHandler(object sender, SettingsSavedEventArgs e);
        //public event SettingsSavedEventHandler SettingsSaved;

        public List<Pigeon> pigeons = new List<Pigeon>();
        public List<Graine> graines = new List<Graine>();


        // Index de la dernière graine ajoutée
        public int lastSeedIndex = 0;
        public int deletedSeedIndex = 0;


        public bool pigeon_alive = false;
        //public thread.Thread threadPigeon;
        public thread.Thread threadPigeon1;
        public thread.Thread threadPigeon2;

        int widthEnv = 10;
        int heightEnv = 10;

        // Créer les pigeons
        public void initialise()
        {

            //// Test pour 1 pigeon
            //addPigeon(100, 70, 0, GraineDeChourbe.Properties.Resources.pigeon_1);
            //addPigeon(200, 100, 0, GraineDeChourbe.Properties.Resources.pigeon_2);
            //pigeons[0].set_belief(graines);

            for (int i = 1; i < 3; i++)
            {
                addPigeon(i * 100, 70, i, GraineDeChourbe.Properties.Resources.pigeon_1);
                pigeons[i - 1].set_belief(graines);
            }

            // Lancement d'un thread pour un pigeon
            // foreach (var pigeon in pigeons)
            // Initialisation des pigeons
            threadPigeon1 = new thread.Thread(() => {
                run(pigeons[0]);
            });
            threadPigeon1.Name = "pigeon 1";

            threadPigeon2 = new thread.Thread(() =>
            {
                run(pigeons[1]);
            });

            threadPigeon2.Name = "pigeon 2";

            //threadPigeon.Start();


            // Lancement d'un thread pour un pigeon
            //foreach(var pigeon in pigeons)
            //{
            //    thread.Thread threadPigeon1 = new thread.Thread(()=> {
            //        run(pigeon);
            //    });
            //    threadPigeon.Start();
            //}
            //threadPigeon = new thread.Thread(new thread.ThreadStart(run));
            //threadPigeon.Start();
        }

        // Dessiner l'environnement initiale

        // Ajout de graines dans l'environnement
        public void addSeed(int newX, int newY)
        {
            lastSeedIndex = +1;

            Graine newSeed = new Graine(newX, newY,lastSeedIndex);
            graines.Add(newSeed);
        }

        // Ajout de graines dans l'environnement
        public void addPigeon(int newX, int newY, int newIndex, Image newImg)
        {

            Pigeon newPigeon = new Pigeon(newX, newY, newIndex, newImg);
            pigeons.Add(newPigeon);
        }

        // Run environment
        // fais vivre les pigeons
        // Fais apparaitre des graines
        //public void run(Pigeon pigeon)
        //{
        //    while (pigeon_alive) {

        //        thread.Thread.Sleep(50);
        //        for (int i = 0; i < 5; i++)
        //        {
        //            pigeons[i].set_belief(graines);
        //            string pigeon_status = "sleep";
        //            if (graines.Count > 0)
        //            {
        //                pigeon_status = "food";
        //            }

        //            (int, int) pos_seed_eat = pigeons[i].run(pigeon_status, 3);

        //            // Should send coordinate
        //            if (pos_seed_eat == (-1, -1))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                //Console.WriteLine("pos_seed_eat : " + pos_seed_eat.Item1 + " " + pos_seed_eat.Item2);
        //                CriticalZone(pos_seed_eat.Item1, pos_seed_eat.Item2);
        //            }
        //        }
        //    }
        //}

        public void run(Pigeon pigeon)
        {
            while (pigeon_alive)
            {
                for (int i = 0; i < graines.Count; i++)
                {
                    if (graines[i].get_status() == false)
                    { 
                        graines[i].update_seed_expiration(); 
                    }
                    else if (graines[i].get_status() == true)
                    {
                        UpdateEventArgs args = new UpdateEventArgs(graines[i].get_xpos(), graines[i].get_ypos());
                        ReplaceSeedEvent.Invoke(this, args);
                    }
                }
                thread.Thread.Sleep(50);
                pigeon.set_belief(graines);
                string pigeon_status = "sleep";
                if (graines.Count > 0)
                {
                    pigeon_status = "food";
                }
             
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

        // On supprime la graine qui a été mangée
        public void deleteSeed(int seedX, int seedY)
        {

            //if (udpateSeeds != null)
            //    udpateSeeds(this, null); 
            for (int i = 0; i < graines.Count; i++)
            {
                if ((graines[i].get_xpos() == seedX) && (graines[i].get_ypos() == seedY))
                {
                    Console.WriteLine("GRAINE SUPPRIME");
                    Console.WriteLine("graine list count before : " + graines.Count);
                    deletedSeedIndex = graines[i].get_index();
                    graines.RemoveAt(i);
                    Console.WriteLine("graine list count After : " + graines.Count);
                    if (udpateSeeds != null)
                    {
                        udpateSeeds(this, null);
                    }

                    // 2ème version handler avec arguments
                    UpdateEventArgs args = new UpdateEventArgs(seedX,seedY);
                    UpdateEventHandler?.Invoke(this, args);

                    //if (SettingsSaved != null)
                    //{
                    //    SettingsSavedEventArgs ss = new SettingsSavedEventArgs() { DeviceIndex = Tuple.Create(seedX,seedY) };
                    //    SettingsSaved(this, ss);
                    //}
                }
            }
        }

        void CriticalZone(int seedX, int seedY)
        {
            Console.WriteLine(thread.Thread.CurrentThread.Name + "Pigeon want to eat");
            Console.WriteLine("Pigeon want to eat");
            try
            {
                // On attend avant d'accéder à la liste de graines que celle-ci soit libre
                mutex.WaitOne();
                deleteSeed(seedX, seedY);
                Console.WriteLine(thread.Thread.CurrentThread.Name + "as completed is task");
            }
            finally
            {
                Console.WriteLine(thread.Thread.CurrentThread.Name + "free");
                // Libère la liste de graines de tel sorte à ce que les pigeons puissent y accéder
                mutex.ReleaseMutex();
            }

        }
    }

    // TEST
    //public class SettingsSavedEventArgs : EventArgs
    //{
    //    public Tuple<int,int> DeviceIndex { get; set; }
    //    // Other settings could be added here
    //}
}
