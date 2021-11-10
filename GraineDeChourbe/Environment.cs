using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Diagnostics;
using thread = System.Threading;
using System.Threading.Tasks;

namespace GraineDeChourbe
{
    class Environment
    {
        public EventHandler udpateSeeds;

        public List<Pigeon> pigeons = new List<Pigeon>();
        public List<Graine> graines = new List<Graine>();

        thread.Thread threadPigeon;

        int widthEnv = 10;
        int heightEnv = 10;

        // Créer les pigeons
        public void initialise()
        {
            Debug.WriteLine("INITIALISATION ! ");
            Graine seed1 = new Graine(100, 300, false);
            Graine seed2 = new Graine(400, 300, false);
            graines.Add(seed1);
            graines.Add(seed2);

            for (int i=1; i<6 ; i++)
            {
                addPigeon(i * 100, 70, i, GraineDeChourbe.Properties.Resources.pigeon_1);
                pigeons[i - 1].set_belief(graines);
            }

            // Lancement d'un thread pour un pigeon
            threadPigeon = new thread.Thread(new thread.ThreadStart(run));
            threadPigeon.Start();
        }

        // Dessiner l'environnement initiale

        // Ajout de graines dans l'environnement
        public void addSeed(int newX, int newY)
        {
            Graine newSeed = new Graine(newX, newY, false);
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
        public void run()
        {

            foreach (var pigeon in pigeons)
            {
                pigeon.run("food", 3);
            }
        }

        public void stopPigeon()
        {
            //pigeon_is_here = false;
        }

        // On supprime la graine qui a été mangée
        public void deleteSeed(int seedX, int seedY)
        {

            if (udpateSeeds != null)
                udpateSeeds(this, null);
            //for(int i=0; i<graines.Count; i++)
            //{
            //    if((graines[i].get_xpos() == seedX) && (graines[i].get_ypos() == seedY))
            //    {
            //        graines.RemoveAt(i);
            //        if (udpateSeeds != null)
            //            udpateSeeds(this, null);
            //    }
            //}
        }


    }
}
