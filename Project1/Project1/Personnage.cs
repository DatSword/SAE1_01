using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE101
{
    public class Personnage
    {
        private string nom;
        private int vieBase;
        private int attBase;
        private int defBase;
        private int speBase;
        private string animPath;
        private string special;
        private string[] lesAttaques;
        private string[] lesDescriptions;

        public Personnage(string nom, int vieBase, int attBase, int defBase, int speBase,string animPath,string special, string[] lesAttaques, string[] lesDescriptions)
        {
            this.Nom = nom;
            this.VieBase = vieBase;
            this.AttBase = attBase;
            this.DefBase = defBase;
            this.SpeBase = speBase;
            this.AnimPath = animPath;
            this.Special = special;
            this.LesAttaques = lesAttaques;
            this.LesDescriptions = lesDescriptions;
        }

        public string Nom
        {
            get
            {
                return nom;
            }

            set
            {
                nom = value;
            }
        }

        public int VieBase
        {
            get
            {
                return vieBase;
            }

            set
            {
                vieBase = value;
            }
        }

        public int AttBase
        {
            get
            {
                return attBase;
            }

            set
            {
                attBase = value;
            }
        }

        public int DefBase
        {
            get
            {
                return defBase;
            }

            set
            {
                defBase = value;
            }
        }

        public int SpeBase
        {
            get
            {
                return speBase;
            }

            set
            {
                speBase = value;
            }
        }
        public string AnimPath
        {
            get
            {
                return animPath;
            }

            set
            {
                animPath = value;
            }
        }

        public string Special
        {
            get
            {
                return special;
            }

            set
            {
                special = value;
            }
        }
        public string[] LesAttaques
        {
            get
            {
                return lesAttaques;
            }

            set
            {
                lesAttaques = value;
            }
        }

        public string[] LesDescriptions
        {
            get
            {
                return lesDescriptions;
            }

            set
            {
                lesDescriptions = value;
            }
        }

       
    }
}
