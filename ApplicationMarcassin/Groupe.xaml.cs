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
    /// Logique d'interaction pour Groupe.xaml
    /// </summary>
    public partial class Groupe : Page
    {
        public List<BO.Employe> ListParticipant = new List<BO.Employe>();
        public BO.Employe Tuteur = new BO.Employe();
        public BO.Competence Competence;
        public Groupe()
        {

            InitializeComponent();
            Date.BlackoutDates.Add(new CalendarDateRange(new DateTime(1990, 1, 1),
            DateTime.Now.AddDays(-1)));
            using (var db = new BBD_projetEntities())
            {
                var val = db.LangueParDefaut();
                var participant = from employe in db.Employes
                                  select new BO.Employe
                                  {
                                      Nom = employe.Nom,
                                      Prenom = employe.Prenom,
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

                //Particpant
                ListViewParticipant.ItemsSource = participant.ToList();
                ListViewParticipant.MouseDoubleClick += (sender, e) =>
                {
                    var Nouveauparticipant = ((BO.Employe)ListViewParticipant.SelectedItem);
                    var verifdoublons = 0;
                    foreach(var x in ListParticipant)
                    {
                        if(x.Id_Employe == Nouveauparticipant.Id_Employe)
                        {
                            verifdoublons = 1;
                        }
                    }

                    if(verifdoublons == 0)
                    {
                        ListParticipant.Add(Nouveauparticipant);
                        foreach(var x in ListBoxTuteur.ItemsSource)
                        {
                            System.Diagnostics.Debug.WriteLine(((BO.Employe)x).Nom);
                        }
                        ListBoxTuteur.Items.Refresh();
                    }
                };

                //Tuteur
                ListBoxTuteur.ItemsSource = ListParticipant;
                ListBoxTuteur.SelectionChanged += (sender, e) =>
                {
                    Tuteur =((BO.Employe)ListBoxTuteur.SelectedItem);
                };

                //Competence
                ListViewCompetence.ItemsSource = competence.ToList();
                ListViewCompetence.SelectionChanged += (sender, e) =>
                {
                    Competence = ((BO.Competence)ListViewCompetence.SelectedItem);
                };
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           var x = ((Button)sender).Parent;
            System.Diagnostics.Debug.WriteLine(((Grid)x).Children[2]);
            foreach(var y in ListParticipant)
            {
                if (y.Id_Employe.ToString() == ((Label)((Grid)x).Children[2]).Content.ToString())
                {
                    ListParticipant.Remove(y);
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
                                  where //employe.Nom == TextRecherche.Text || employe.Prenom == TextRecherche.Text || employe.Service == TextRecherche.Text
                                   SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", employe.Nom) > 0
                                  || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", employe.Prenom) > 0
                                  || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", employe.Service) > 0
                                  select new BO.Employe
                                  {
                                      Nom = employe.Nom,
                                      Prenom = employe.Prenom,
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

        private void Enregistrement_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                if (Date.SelectedDate == null)
                {
                    MessageBoxResult result = MessageBox.Show("Error : Aucune date sélectionnée");
                }
                else if(Titre.Text.Count()<=0)
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
                    var groupe = new DAL.Groupe
                    {
                        Id_Competence = Competence.Id_Competence,
                        DateReunion = Date.SelectedDate.Value,
                        DateCreation = DateTime.Now,
                        Titre = Titre.Text,
                        EstValider = true

                    };
                    db.Groupes.Add(groupe);

                    System.Diagnostics.Debug.WriteLine(groupe.Id_Competence);
                    System.Diagnostics.Debug.WriteLine(groupe.DateReunion);
                    System.Diagnostics.Debug.WriteLine(groupe.Titre);

                    //Membre et tuteur
                    var tuteur = false;
                    if(ListParticipant.Count <= 0)
                    {
                        MessageBoxResult result = MessageBox.Show("Warning : Vous n'avez pas de participants");
                    }
                    foreach (var x in ListParticipant)
                    {
                        var membre = new DAL.Membre();
                        if (x.Id_Employe == Tuteur.Id_Employe)
                        {
                            tuteur = true;
                            membre = new DAL.Membre
                            {
                                EstTutorant = true,
                                Id_Employe = x.Id_Employe,
                                //Groupe = groupe
                            };
                        }
                        else
                        {
                            membre = new DAL.Membre
                            {
                                EstTutorant = false,
                                Id_Employe = x.Id_Employe,
                                //Groupe = groupe
                            };
                        }
                        System.Diagnostics.Debug.WriteLine(membre.EstTutorant);
                        System.Diagnostics.Debug.WriteLine(membre.Id_Employe);
                        System.Diagnostics.Debug.WriteLine(membre.Groupe);
                        db.Membres.Add(membre);
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
    }
}
