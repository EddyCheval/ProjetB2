using ApplicationMarcassin.BO;
using ApplicationMarcassin.DAL;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour ModificationEmploye.xaml
    /// </summary>
    public partial class ModificationEmploye : Page
    {

        public List<BO.Langue> ListLangue { get; private set; }
        public List<BO.Competence> ListCompetence { get; private set; }
        public List<BO.Langue> ListLangueOrigin { get; private set; }
        public List<BO.Competence> ListCompetenceOrigin { get; private set; }
        private BO.Employe Employe = new BO.Employe();

        public ModificationEmploye(BO.Employe employe)
        {
            InitializeComponent();
            Employe = employe; 
            Service.Text = Employe.Service;
            Entreprise.Text = Employe.Entreprise;
            Metier.Text = Employe.Metier;
            Nom.Text = Employe.Nom;
            Prenom.Text = Employe.Prenom;
            DateArrivee.SelectedDate = Employe.DateArrive;
            LinkedIn.Text = Employe.LinkedIn;
            if(Employe.DateDepart != null)
            {
                DateDepart.SelectedDate = Employe.DateDepart;
            }
            else
            {
                DateDepart.SelectedDate = null;
                DateDepart.DisplayDate = DateTime.Now;
            }
            ChefDeService.IsChecked = Employe.EstChefDeService;
            Admin.IsChecked = Employe.EstAdmin;
            Actif.IsChecked = Employe.Actif;
            AdresseMail.Text = Employe.AdresseMail;
            using (var db = new BBD_projetEntities())
            {
                var langue = from l in db.Langues
                             select new BO.Langue
                             {
                                 Id_Langue = l.Id_Langue,
                                 Default = l.Default,
                                 Nom = l.Nom
                             };

                var languePossede = from l in db.Langues
                                  join lp in db.LanguePossedes
                                  on l.Id_Langue equals lp.Id_Langue
                                  where lp.Id_Employe == Employe.Id_Employe
                                  select new BO.Langue
                                  {
                                      Id_Langue = l.Id_Langue,
                                      Default = l.Default,
                                      Nom = l.Nom
                                  };
                var Competence = from c in db.Competences
                                 select new BO.Competence
                                 {
                                     Id_Competence = c.Id_Competence,
                                     Actif = c.Actif,
                                     Annee = c.Annee,
                                     Actuel = c.Actuel,
                                     Id_CompetenceActuel = c.Id_CompetenceActuel
                                 };
                var CompetencePossede = from c in db.Competences
                                        join ic in db.LiaisonCompetences
                                        on c.Id_Competence equals ic.Id_Competence
                                        where ic.Id_Employe == Employe.Id_Employe
                                        select new BO.Competence
                                        {
                                            Id_Competence = c.Id_Competence,
                                            Actif = c.Actif,
                                            Annee = c.Annee,
                                            Actuel = c.Actuel,
                                            Id_CompetenceActuel = c.Id_CompetenceActuel 
                                        };
                ListLangue = languePossede.ToList();
                ListCompetence = CompetencePossede.ToList();
                ListLangueOrigin = languePossede.ToList();
                ListCompetenceOrigin = CompetencePossede.ToList();

                ListViewCompetence.ItemsSource = Competence.ToList();
                ListViewCompetence.MouseDoubleClick += (sender, e) =>
                {
                    var NouvelleCompetence = ((BO.Competence)ListViewCompetence.SelectedItem);
                    var verifdoublons = 0;
                    foreach (var x in ListCompetence)
                    {
                        if (NouvelleCompetence != null)
                        {
                            if (x.Id_Competence == NouvelleCompetence.Id_Competence)
                            {
                                verifdoublons = 1;
                            }
                        }
                    }

                    if (verifdoublons == 0)
                    {
                        ListCompetence.Add(NouvelleCompetence);
                        ListBoxCompetence.Items.Refresh();
                    }
                };
                ListBoxLangue.ItemsSource = langue.ToList();
                ListBoxLangue.MouseDoubleClick += (sender, e) =>
                {

                    var NouvelleLangue = ((BO.Langue)ListBoxLangue.SelectedItem);
                    var verifdoublons = 0;
                    foreach (var x in ListLangue)
                    {
                        if (x.Id_Langue == NouvelleLangue.Id_Langue)
                        {
                            verifdoublons = 1;
                        }
                    }

                    if (verifdoublons == 0)
                    {
                        ListLangue.Add(NouvelleLangue);
                        ListBoxLangueSelection.Items.Refresh();
                    }
                };
                ListBoxCompetence.ItemsSource = ListCompetence;
                ListBoxLangueSelection.ItemsSource = ListLangue;
            }
        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {

            if (DateArrivee.SelectedDate == null)
            {
                MessageBoxResult result = MessageBox.Show("Error : Aucune date sélectionnée");
            }
            else if (StringCheck(Nom.Text) || StringCheck(Prenom.Text))
            {
                MessageBoxResult result = MessageBox.Show("Error : veuillez saisir votre nom complet");
            }
            else if (StringCheck(AdresseMail.Text) || StringCheck(Service.Text) || StringCheck(Metier.Text) || StringCheck(LinkedIn.Text) || StringCheck(Entreprise.Text))
            {
                MessageBoxResult result = MessageBox.Show("Error : Veuillez remplir tous les champs");
            }
            else if (DateDepart.SelectedDate < DateArrivee.SelectedDate)
            {
                MessageBoxResult result = MessageBox.Show("Error : Incohérence dans les dates");
            }
            else
            {
                using (var db = new BBD_projetEntities())
                {
                    foreach (var x in ListLangue)
                    {
                        System.Diagnostics.Debug.WriteLine((ListLangueOrigin.Where(l => l.Id_Langue == x.Id_Langue).Count()));
                        System.Diagnostics.Debug.WriteLine(x.Id_Langue);
                        if ((ListLangueOrigin.Where(l => l.Id_Langue == x.Id_Langue).Count() > 0))
                            System.Diagnostics.Debug.WriteLine((ListLangueOrigin.Where(l => l.Id_Langue == x.Id_Langue).First()).Id_Langue);
                        if (ListLangueOrigin.Where(l => l.Id_Langue == x.Id_Langue).Count() == 0)
                        {
                            DAL.LanguePossede languePossede = new DAL.LanguePossede
                            {
                                Default = false,
                                Id_Employe = Employe.Id_Employe,
                                Id_Langue = x.Id_Langue
                            };
                            db.LanguePossedes.Add(languePossede);
                        }
                    }
                    foreach (var y in ListLangueOrigin)
                    {
                        if (ListLangue.Where(l => l.Id_Langue == y.Id_Langue).Count() == 0)
                        {
                            var languePossede = new DAL.LanguePossede
                            {
                                Default = false,
                                Id_Employe = Employe.Id_Employe,
                                Id_Langue = y.Id_Langue
                            };
                            db.LanguePossedes.Attach(languePossede);
                            db.LanguePossedes.Remove(languePossede);
                        }
                    }
                    foreach (var w in ListCompetence)
                    {
                        if (ListCompetenceOrigin.Where(c => c.Id_Competence == w.Id_Competence).Count() == 0)
                        {
                            var competencePossede = new DAL.LiaisonCompetence
                            {
                                Id_Competence = w.Id_Competence,
                                Id_Employe = Employe.Id_Employe,
                                EstTutorant = false
                            };
                            db.LiaisonCompetences.Add(competencePossede);
                        }
                    }
                    foreach (var z in ListCompetenceOrigin)
                    {
                        if (ListCompetence.Where(c => c.Id_Competence == z.Id_Competence).Count() == 0)
                        {
                            var competencePossede = new DAL.LiaisonCompetence
                            {
                                Id_Competence = z.Id_Competence,
                                Id_Employe = Employe.Id_Employe,
                                EstTutorant = false
                            };
                            db.LiaisonCompetences.Attach(competencePossede);
                            db.LiaisonCompetences.Remove(competencePossede);
                        }
                    }
                    var DALEmploye = db.Employes.Find(Employe.Id_Employe);
                    if (Service.Text != DALEmploye.Service)
                        DALEmploye.Service = Service.Text;
                    if (Entreprise.Text != DALEmploye.Entreprise)
                        DALEmploye.Entreprise = Entreprise.Text;
                    if (Metier.Text != Employe.Metier)
                        DALEmploye.Metier = Metier.Text;
                    if (Nom.Text != DALEmploye.Nom)
                        DALEmploye.Nom = Nom.Text;
                    if (Prenom.Text != DALEmploye.Prénom)
                        DALEmploye.Prénom = Prenom.Text;
                    if (DateArrivee.SelectedDate != DALEmploye.DateArrive)
                        DALEmploye.DateArrive = DateArrivee.SelectedDate.Value;
                    if (Employe.DateDepart != null)
                    {
                        if (DateDepart.SelectedDate != DALEmploye.DateDepart)
                            DALEmploye.DateDepart = DateDepart.SelectedDate.Value;
                    }
                    if (ChefDeService.IsChecked != DALEmploye.EstChefDeService)
                        DALEmploye.EstChefDeService = ChefDeService.IsChecked.Value;
                    if (Admin.IsChecked != DALEmploye.EstAdmin)
                        DALEmploye.EstAdmin = Admin.IsChecked.Value;
                    if (Actif.IsChecked != DALEmploye.Actif)
                        DALEmploye.Actif = Actif.IsChecked.Value;
                    if (Interne.IsChecked != DALEmploye.EstInterne)
                        DALEmploye.EstInterne = Interne.IsChecked.Value;
                    if (AdresseMail.Text != Employe.AdresseMail)
                        DALEmploye.AdresseMail = AdresseMail.Text;
                    if (LinkedIn.Text != Employe.LinkedIn)
                        DALEmploye.LienLinkedin = LinkedIn.Text;
                    db.SaveChanges();
                    this.NavigationService.Navigate(new Uri("ListViewEmploye.xaml", UriKind.Relative));
                }
            }
        }

        private void SuppressionLangue_Click(object sender, RoutedEventArgs e)
        {
            foreach (var y in ListLangue)
            {
                if (y.Id_Langue == ((BO.Langue)ListBoxLangueSelection.SelectedItem).Id_Langue)
                {
                    ListLangue.Remove(y);
                    ListBoxLangueSelection.Items.Refresh();
                    break;
                }
            }
        }

        private void SuppresionCompetence_Click(object sender, RoutedEventArgs e)
        {
            foreach (var y in ListCompetence)
            {
                if (y.Id_Competence == ((BO.Competence)ListBoxCompetence.SelectedItem).Id_Competence)
                {
                    ListCompetence.Remove(y);
                    ListBoxCompetence.Items.Refresh();
                    break;
                }
            }
        }
        private bool StringCheck(string s)
        {
            foreach (var x in s)
            {
                if (x != ' ')
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
