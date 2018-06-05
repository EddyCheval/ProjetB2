using ApplicationMarcassin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Logique d'interaction pour CreationEmploye.xaml
    /// </summary>
    public partial class CreationEmploye : Page
    {
        private List<BO.Competence> ListBoC = new List<BO.Competence>();
        private List<BO.Langue> ListBoL = new List<BO.Langue>();
        public CreationEmploye()
        {
            InitializeComponent();
            using (var db = new BBD_projetEntities())
            {

                var val = db.LangueParDefaut();
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
                var ListDesCompetences = competence.ToList();
                var langue = from l in db.Langues
                             select l;
                var ListDeLangue = BO.Langue.ListDALtoBO(langue.ToList()); ;

                ListViewCompetence.ItemsSource = ListDesCompetences;
                ListViewCompetence.MouseDoubleClick += (sender, e) =>
                {
                    var NouvelleCompetence = ((BO.Competence)ListViewCompetence.SelectedItem);
                    var verifdoublons = 0;
                    foreach (var x in ListBoC)
                    {
                        if (x.Id_Competence == NouvelleCompetence.Id_Competence)
                        {
                            verifdoublons = 1;
                        }
                    }

                    if (verifdoublons == 0)
                    {
                        ListBoC.Add(NouvelleCompetence);
                        ListBoxCompetence.Items.Refresh();
                    }
                };
                ListBoxCompetence.ItemsSource = ListBoC;

                ListBoxLangue.ItemsSource = ListDeLangue;
                ListBoxLangue.MouseDoubleClick += (sender, e) =>
                {

                    var NouvelleLangue = ((BO.Langue)ListBoxLangue.SelectedItem);
                    var verifdoublons = 0;
                    foreach (var x in ListBoL)
                    {
                        if (x.Id_Langue == NouvelleLangue.Id_Langue)
                        {
                            verifdoublons = 1;
                        }
                    }

                    if (verifdoublons == 0)
                    {
                        ListBoL.Add(NouvelleLangue);
                        ListBoxLangueSelection.Items.Refresh();
                    }
                };
                ListBoxLangueSelection.ItemsSource = ListBoL; 
            }
        }

        private void Confirmer_Click(object sender, RoutedEventArgs e)
        {
            if (DateArrivee.SelectedDate == null)
            {
                MessageBoxResult result = MessageBox.Show("Error : Aucune date sélectionnée");
            }
            else if(StringCheck(Nom.Text) || StringCheck(Prenom.Text))
            {
                MessageBoxResult result = MessageBox.Show("Error : veuillez saisir votre nom complet");
            }
            else if(StringCheck(AdresseMail.Text)|| StringCheck(Service.Text) || StringCheck(Metier.Text) || StringCheck(LinkedIn.Text) || StringCheck(Entreprise.Text))
            {
                MessageBoxResult result = MessageBox.Show("Error : Veuillez remplir tous les champs");
            }
            else if (DateDepart.SelectedDate < DateArrivee.SelectedDate)
            {
                MessageBoxResult result = MessageBox.Show("Error : Incohérence dans les dates");
            }
            else {
                using (var db = new BBD_projetEntities())
                {
                    DAL.Employe employe = new DAL.Employe
                    {
                        Actif = Actif.IsChecked.Value,
                        EstAdmin = Admin.IsChecked.Value,
                        EstChefDeService = ChefDeService.IsChecked.Value,
                        EstInterne = Interne.IsChecked.Value,
                        AdresseMail = AdresseMail.Text,
                        Metier = Metier.Text,
                        Service = Service.Text,
                        Nom = Nom.Text,
                        Prenom = Prenom.Text,
                        DateArrive = DateArrivee.SelectedDate.Value,
                        Entreprise = Entreprise.Text,
                        LienLinkedin = LinkedIn.Text,
                        Identifiant = ComputeSha256Hash(Nom + "." + Prenom),
                        MotDePasse = ComputeSha256Hash("Epsi2018!")
                    };
                    if (DateDepart.SelectedDate != null)
                    {
                        employe.DateDepart = DateDepart.SelectedDate.Value;
                    }
                    System.Diagnostics.Debug.Write(employe.DateDepart);
                    db.Employes.Add(employe);
                    foreach (var x in ListBoC)
                    {
                        DAL.LiaisonCompetence competence = new DAL.LiaisonCompetence
                        {
                            EstTutorant = false,
                            Id_Competence = x.Id_Competence,
                            Employe = employe
                        };
                        db.LiaisonCompetences.Add(competence);
                    }
                    foreach (var y in ListBoL)
                    {
                        DAL.LanguePossede langue = new DAL.LanguePossede
                        {
                            Employe = employe,
                            Id_Langue = y.Id_Langue,
                            Default = false
                        };
                        db.LanguePossedes.Add(langue);
                    }
                    db.SaveChanges();
                    this.NavigationService.Navigate(new Uri("ListViewEmploye.xaml", UriKind.Relative));
                }   
            }
        }

        private void SuppressionLangue_Click(object sender, RoutedEventArgs e)
        {
            foreach (var y in ListBoL)
            {
                if (y.Id_Langue == ((BO.Langue)ListBoxLangueSelection.SelectedItem).Id_Langue)
                {
                    ListBoL.Remove(y);
                    ListBoxLangueSelection.Items.Refresh();
                    break;
                }
            }
        }

        private void SuppresionCompetence_Click(object sender, RoutedEventArgs e)
        {
            foreach (var y in ListBoC)
            {
                if (y.Id_Competence == ((BO.Competence)ListBoxCompetence.SelectedItem).Id_Competence)
                {
                    ListBoC.Remove(y);
                    ListBoxCompetence.Items.Refresh();
                    break;
                }
            }
        }
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private bool StringCheck(string s)
        {
            foreach(var x in s)
            {
                if(x != ' ')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
