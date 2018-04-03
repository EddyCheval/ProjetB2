using ApplicationMarcassin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationMarcassin.BO
{
    public class Groupe
    {
        public static List<Groupe> ListDalToBO(List<DAL.Groupe> DALlist)
        {
            List<BO.Groupe> BOList = new List<BO.Groupe>();
            foreach (var x in DALlist)
            {
                BOList.Add(new Groupe(x));
            }
            return BOList;
        }
        public Groupe()
        {

        }
        public Groupe(DAL.Groupe groupe)
        {
            using (var db = new BBD_projetEntities()) {
                var reqTuteur = from employe in db.Employes
                          join membre in db.Membres
                          on employe.Id_Employe equals membre.Id_Employe
                          where membre.Id_Groupe == groupe.Id_Groupe && membre.EstTutorant == true
                          select new BO.Employe
                          {
                              Id_Employe = employe.Id_Employe,
                              Nom = employe.Nom,
                              Prenom = employe.Prénom,
                              Service = employe.Service
                          };

                var reqNbParticipant = from employe in db.Employes
                          join membre in db.Membres
                          on employe.Id_Employe equals membre.Id_Employe
                          where membre.Id_Groupe == groupe.Id_Groupe
                          select new BO.Employe
                          {
                              Id_Employe = employe.Id_Employe,
                              Nom = employe.Nom,
                              Prenom = employe.Prénom,
                              Service = employe.Service
                          };

                Titre = groupe.Titre;
                DateReunion = groupe.DateReunion;
                Id_Groupe = groupe.Id_Groupe;
                if(groupe.Id_Competence != null)
                    Id_Competence = groupe.Id_Competence.Value;
                NbParticipant = reqNbParticipant.ToList().Count;
                Participant = reqNbParticipant.ToList();
                Tuteur = reqTuteur.FirstOrDefault();
             }
        }

        private string _titre;
        public string Titre
        {
            get { return _titre; }
            set { _titre = value; }
        }

        private DateTime _dateReunion;
        public DateTime DateReunion
        {
            get { return _dateReunion; }
            set { _dateReunion = value; }
        }
        private int _id_Competence;
        public int Id_Competence
        {
            get { return _id_Competence; }
            set { _id_Competence = value; }
        }
        private Employe _tuteur;
        public Employe Tuteur
        {
            get { return _tuteur; }
            set { _tuteur = value; }
        }
        private int _nbParticipant;
        public int NbParticipant
        {
            get { return _nbParticipant; }
            set { _nbParticipant = value; }
        }
        private int _id_Groupe;
        public int Id_Groupe
        {
            get { return _id_Groupe; }
            set { _id_Groupe = value; }
        }
        private List<BO.Employe> _participant;
        public List<BO.Employe> Participant
        {
            get { return _participant; }
            set { _participant = value; }
        }
    }
}