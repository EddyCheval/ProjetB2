using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMarcassin.BO
{
    public class Employe
    {
        public static List<Employe> ListDalToBO (List<DAL.Employe> DALlist)
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
            this.Metier = employe.Metier;
            this.LinkedIn = employe.LienLinkedin;
            this.Entreprise = employe.Entreprise;
            this.DateArrive = employe.DateArrive;
            if(employe.DateDepart != null)
            this.DateDepart = employe.DateDepart.Value;
            this.AdresseMail = employe.AdresseMail;
            this.Actif = employe.Actif;
            this.EstAdmin = employe.EstAdmin;
            this.EstChefDeService = employe.EstChefDeService;
            this.EstInterne = employe.EstInterne;
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
        private string _adresseMail;
        public string AdresseMail
        {
            get{ return _adresseMail; }
            set { _adresseMail = value; }
        }
        private string _entreprise;
        public string Entreprise
        {
            get { return _entreprise; }
            set { _entreprise = value; }
        }

        private bool _estInterne;
        public bool EstInterne
        {
            get { return _estInterne; }
            set { _estInterne = value; }
        }
        private bool _estAdmin;
        public bool EstAdmin
        {
            get { return _estAdmin; }
            set { _estAdmin = value; }
        }
        private bool _estChefDeService;
        public bool EstChefDeService
        {
            get { return _estChefDeService; }
            set { _estChefDeService = value; }
        }

        private bool _actif;
        public bool Actif
        {
            get { return _actif; }
            set { _actif = value; }
        }
        private DateTime _dateArrive;
        public DateTime DateArrive
        {
            get { return _dateArrive; }
            set { _dateArrive = value; }
        }
        private DateTime _dateDepart;
        public DateTime DateDepart
        {
            get { return _dateDepart; }
            set { _dateDepart = value; }
        }

        private string _metier;
        public string Metier
        {
            get { return _metier; }
            set { _metier = value; }
        }
        private string _linkedIn;
        public string LinkedIn
        {
            get { return _linkedIn; }
            set { _linkedIn = value; }
        }
    }
}
