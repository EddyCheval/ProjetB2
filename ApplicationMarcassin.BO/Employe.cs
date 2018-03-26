using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMarcassin.BO
{
    public class Employe
    {
        static List<Employe> ListDalToBO (List<DAL.Employe> DALlist)
        {
            List<BO.Employe> BOList = new List<BO.Employe>();
            foreach (var x in DALlist)
            {
                BOList.Add(new Employe(x));
            }
            return BOList;
        }
        public Employe() 
        {

        }
        public Employe(DAL.Employe employe)
        {
            this.Nom = employe.Nom;
            this.Prenom = employe.Prénom;
            this.Service = employe.Service;
            this.Id_Employe = employe.Id_Employe;
        }
        private int _id_Employe;
        public int Id_Employe
        {
            get { return _id_Employe; }
            set { _id_Employe = value; }
        }
        private string _nom;
        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }
        private string _prenom;
        public string Prenom
        {
            get { return _prenom; }
            set { _prenom = value; }
        }
        private string _service;
        public string Service
        {
            get { return _service; }
            set { _service = value; }
        }
    }
}
