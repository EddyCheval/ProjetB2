using ApplicationMarcassin.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationMarcassin
{
    /// <summary>
    /// Logique d'interaction pour ModificationGroupe.xaml
    /// </summary>
    public partial class ModificationGroupe : Page
    {

        public List<BO.Employe> ListParticipant = new List<BO.Employe>();
        public BO.Employe Tuteur = new BO.Employe();
        public BO.Competence Competence;
        public BO.Groupe Groupe;
        public ModificationGroupe(BO.Groupe groupe)
        {
            InitializeComponent();
            Date.BlackoutDates.Add(new CalendarDateRange(new DateTime(1990, 1, 1),
            DateTime.Now.AddDays(-1)));
            System.Diagnostics.Debug.WriteLine(groupe.Id_Groupe);
            using (var db = new BBD_projetEntities())
            {
                Groupe = groupe;
                Titre.Text = groupe.Titre;
                ListBoxTuteur.ItemsSource = groupe.Participant;
                Tuteur = groupe.Tuteur;

                var val = db.LangueParDefaut();
                var participant = from employe in db.Employes
                                  select new BO.Employe
                                  {
                                      Nom = employe.Nom,
                                      Prenom = employe.Prénom,
                                      Service = employe.Service,
                                      Id_Employe = employe.Id_Employe
                                  };

                var competence = from c in db.Competences
                                 where c.Actif == true && c.Actuel == true
                                 select new BO.Competence
                                 {
                                     Actuel = c.Actuel,
                                     IntituleCompetence = c.IntituleCompetences.Where(b => b.Id_Langue == val).Select(b => b.intitule).FirstOrDefault(),
                                     Annee = c.Annee,
                                     Id_Competence = c.Id_Competence,
                                     Id_CompetenceActuel = c.Id_CompetenceActuel
                                 };
                var wow = competence.ToList();
                var ListDeParticipant = participant.ToList();
                System.Diagnostics.Debug.WriteLine(groupe.DateReunion);

                if ( DateTime.Now < groupe.DateReunion)
                    Date.SelectedDate = groupe.DateReunion;

                //Competence
                ListViewCompetence.ItemsSource = competence.ToList();
                ListViewCompetence.SelectedIndex = wow.IndexOf(wow.Where(c => c.Id_Competence == groupe.Id_Competence).Select(c => c).FirstOrDefault());
                Competence = wow.Where(c => c.Id_Competence == groupe.Id_Competence).Select(c => c).FirstOrDefault();
                ListViewCompetence.SelectionChanged += (sender, e) =>
                {
                    Competence = ((BO.Competence)ListViewCompetence.SelectedItem);
                };
                
                ///Comment afficher correctement ce qui est dans al base de données 


                //Particpant
                ListViewParticipant.ItemsSource = participant.ToList();
                ListViewParticipant.MouseDoubleClick += (sender, e) =>
                {
                    var Nouveauparticipant = ((BO.Employe)ListViewParticipant.SelectedItem);
                    var verifdoublons = 0;
                    foreach (var x in groupe.Participant)
                    {
                        if (x.Id_Employe == Nouveauparticipant.Id_Employe)
                        {
                            verifdoublons = 1;
                        }
                    }

                    if (verifdoublons == 0)
                    {
                        groupe.Participant.Add(Nouveauparticipant);
                        foreach (var x in ListBoxTuteur.ItemsSource)
                        {
                            System.Diagnostics.Debug.WriteLine(((BO.Employe)x).Nom);
                        }
                        ListBoxTuteur.Items.Refresh();
                    }
                };
                //Tuteur
                System.Diagnostics.Debug.WriteLine(Groupe.Participant.IndexOf(Groupe.Participant.Where(c => c.Id_Employe == Tuteur.Id_Employe).FirstOrDefault()));
                if(Tuteur != null)
                    ListBoxTuteur.SelectedIndex = Groupe.Participant.IndexOf(Groupe.Participant.Where(c => c.Id_Employe == Tuteur.Id_Employe).FirstOrDefault());

                ListBoxTuteur.SelectionChanged += (sender, e) =>
                {
                    Tuteur = ((BO.Employe)ListBoxTuteur.SelectedItem);
                };
            }
        }
        private void Enregistrement_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                if (Date.SelectedDate == null && Groupe.DateReunion == null)
                {
                    MessageBoxResult result = MessageBox.Show("Error : Aucune date sélectionnée");
                }
                else if (Titre.Text.Count() <= 0)
                {
                    MessageBoxResult result = MessageBox.Show("Error : Aucun Titre sélectionné");
                }
                else if (Competence == null)
                {
                    MessageBoxResult result = MessageBox.Show("Error : Veuillez indiqué la compétence enseigné");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(Competence.Id_Competence);
                    //Groupe
                    var DALGroupe = db.Groupes.Find(Groupe.Id_Groupe);
                    System.Diagnostics.Debug.WriteLine(Groupe.Id_Groupe);
                    System.Diagnostics.Debug.WriteLine(DALGroupe);
                    DALGroupe.Id_Competence = Competence.Id_Competence;
                    if(Date.SelectedDate != null && Date.SelectedDate != Groupe.DateReunion)
                        DALGroupe.DateReunion = Date.SelectedDate.Value;
                    DALGroupe.Titre = Titre.Text;
           

                    System.Diagnostics.Debug.WriteLine(DALGroupe.Id_Competence);
                    System.Diagnostics.Debug.WriteLine(DALGroupe.DateReunion);
                    System.Diagnostics.Debug.WriteLine(DALGroupe.Titre);

                    //Membre et tuteur
                    var tuteur = false;
                    if (Groupe.Participant.Count <= 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Warning : Vous n'avez pas de participants");
                    }
                    var req = (from m in db.Membres
                              where m.Id_Groupe == DALGroupe.Id_Groupe
                              select new 
                              {
                                    Id_Groupe = m.Id_Groupe,
                                    Id_Employe = m.Id_Employe,
                                    EstTutorant = m.EstTutorant
                              }).ToList().Select(m => new DAL.Membre
                              {
                                  Id_Groupe = m.Id_Groupe,
                                  Id_Employe = m.Id_Employe,
                                  EstTutorant = m.EstTutorant
                              });
                    foreach (var x in Groupe.Participant)
                    {
                        if (req.Where(c => c.Id_Employe == x.Id_Employe).Count() == 0)
                        {
                            var membre = new DAL.Membre();
                            if (x.Id_Employe == Tuteur.Id_Employe)
                            {
                                tuteur = true;
                                membre = new DAL.Membre
                                {
                                    EstTutorant = true,
                                    Id_Employe = x.Id_Employe,
                                    Groupe = DALGroupe
                                };
                            }
                            else
                            {
                                membre = new DAL.Membre
                                {
                                    EstTutorant = false,
                                    Id_Employe = x.Id_Employe,
                                    Groupe = DALGroupe
                                };
                            }
                            System.Diagnostics.Debug.WriteLine(membre.EstTutorant);
                            System.Diagnostics.Debug.WriteLine(membre.Id_Employe);
                            System.Diagnostics.Debug.WriteLine(membre.Groupe);
                            db.Membres.Add(membre);
                        }
                    }
                    foreach(var x  in req.ToList())
                    {
                        if(Groupe.Participant.Where(c => c.Id_Employe == x.Id_Employe).Count()==0)
                        {
                            db.Membres.Attach(x);
                            db.Membres.Remove(x);
                        }
                    }
                    if (tuteur == false)
                    {
                        MessageBoxResult result = MessageBox.Show("Warning : Vous n'avez pas selectionné de tuteur");
                    }
                    db.SaveChanges();
                    ListViewCompetence l = new ListViewCompetence();
                    this.NavigationService.Navigate(new Uri("ListViewGroup.xaml", UriKind.Relative));
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var x = ((Button)sender).Parent;
            System.Diagnostics.Debug.WriteLine(((Grid)x).Children[2]);
            foreach (var y in Groupe.Participant)
            {
                if (y.Id_Employe.ToString() == ((Label)((Grid)x).Children[2]).Content.ToString())
                {
                    Groupe.Participant.Remove(y);
                    ListBoxTuteur.Items.Refresh();
                    break;
                }
            }
        }

        private void ButtonRecherche_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                var participant = from employe in db.Employes
                                  where //employe.Nom == TextRecherche.Text || employe.Prénom == TextRecherche.Text || employe.Service == TextRecherche.Text
                                   SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", employe.Nom) > 0
                                  || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", employe.Prénom) > 0
                                  || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", employe.Service) > 0
                                  select new BO.Employe
                                  {
                                      Nom = employe.Nom,
                                      Prenom = employe.Prénom,
                                      Service = employe.Service,
                                      Id_Employe = employe.Id_Employe
                                  };

                foreach (var x in participant.ToList())
                {
                    System.Diagnostics.Debug.WriteLine(x.Id_Employe);
                }
                ListViewParticipant.ItemsSource = participant.ToList();
                ListViewParticipant.Items.Refresh();
            }
        }
    }
}
