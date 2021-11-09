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
        public List<Pigeon> pigeons = new List<Pigeon>();
        public List<Graine> graines = new List<Graine>();

        thread.Thread threadPigeon;

        int widthEnv = 10;
        int heightEnv = 10;

        // Créer les pigeons
        public void initialise()
        {
            Debug.WriteLine("INITIALISATION ! ");
            for(int i=0; i<5 ; i++)
            {
                addPigeon(i * 100, i, i, GraineDeChourbe.Properties.Resources.pigeon_1);
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

        // Mise à jour de l'affichage
        public void refresh()
        {

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
    }
}
