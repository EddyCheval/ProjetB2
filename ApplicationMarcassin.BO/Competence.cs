using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationMarcassin;

namespace ApplicationMarcassin.BO
{
    public class Competence
    {
        public Competence()
        {

        }
        public Competence(DAL.Competence c)
        {
            this.Annee = c.Annee;
            this.Id_Competence = c.Id_Competence;
            this.Id_CompetenceActuel = c.Id_CompetenceActuel;
            this.IntituleCompetences = BO.IntituleCompetence.ListDALtoBO(c.IntituleCompetences.ToList());
        }
        private int _id_Competence;
        public int Id_Competence
        {
            get { return _id_Competence; }
            set { _id_Competence = value; }
        }
        private bool _actuel;
        public bool Actuel
        {
            get { return _actuel; }
            set { _actuel = value; }
        }
        private bool _actif;
        public bool Actif
        {
            get { return _actif; }
            set { _actif = value; }
        }
        private string _annee;
        public string Annee
        {
            get { return _annee; }
            set { _annee = value; }
        }

        private Nullable<int> _id_CompetenceActuel;

        public Nullable<int> Id_CompetenceActuel
        {
            get { return _id_CompetenceActuel; }
            set { _id_CompetenceActuel = value; }
        }
        private List<IntituleCompetence> _intituleCompetences;
        public List<IntituleCompetence> IntituleCompetences
        {
            get { return _intituleCompetences; }
            set { _intituleCompetences = value; }
        }
        private string _intituleCompetence;
        public string IntituleCompetence
        {
            get { return _intituleCompetence; }
            set { _intituleCompetence = value; }
        }
    }
}
