using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMarcassin.BO
{
    //Rôle changer l'affichage du ToString pour pouvori l'afficher dans le listbox
    public class Langue
    {
        public static List<Langue> ListDALtoBO(List<ApplicationMarcassin.DAL.Langue> listDal)
        {
            List<Langue> ListBo = new List<Langue>();
            foreach(var x in listDal)
            {
                ListBo.Add(new Langue(x));
            }
            return ListBo;
        }
        public Langue()
        {

        }
        public Langue(ApplicationMarcassin.DAL.Langue langue)
        {
            this.Nom = langue.Nom;
            this.Id_Langue = langue.Id_Langue;
            this.Default = langue.Default;
        }
        private int _id_Langue;
        public int Id_Langue
        {
            get { return _id_Langue; }
            set { _id_Langue = value; }
        }
        private string _nom;
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }
        private bool _default;
        public bool Default
        {
            get { return _default; }
            set { _default = value; }
        }
        public override string ToString()
        {
            return this.Nom;
        }
        //Ajouter les autre éléments si sa devient utile 
    }
}
