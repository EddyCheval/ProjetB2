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
using ApplicationMarcassin.BO;
using ApplicationMarcassin.DAL;

namespace ApplicationMarcassin
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class ListViewCompetence
    {
        //creation d'un tiem acceuillant all
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*this.DataContext = this;
            using (var db = new BBD_projetEntities())
            {
                var val = db.LangueParDefaut();
                var req = from c in db.OffrantParCompetenceAvecLangueEtIntituleActu(8,3)
                          select c;
                foreach (var v in req)
                {
                    this.DataContext = v.Description;
                }
                var req2 = from c in db.Langues
                           select c;
                
                for (int i = 0; i < req2.ToList().Count; i++)
                {
                    list.Items.Add(req2.ToList().ElementAt(i).Nom);
                }
                var req3 = from c in db.Competences
                           where c.Actuel == true && c.Actif == true
                           select new BO.Competence { Actuel = c.Actuel,
                               IntituleCompetence = c.IntituleCompetences.Where(b => b.Id_Langue == val).Select(b => b.intitule).FirstOrDefault(),
                               Annee = c.Annee,
                               Id_Competence = c.Id_Competence,
                               Id_CompetenceActuel = c.Id_CompetenceActuel};

                list2.ItemsSource = req3.ToList();
                
            }
        }
        private void b_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                BO.Competence t = (BO.Competence)list2.SelectedItem;
                Competence objet = null;
                test2.Content = Actif.ToString();
                var objet2 = new IntituleCompetence()
                {
                    intitule = C1.Text,
                    Description = C2.Text,
                    Id_Langue = 3
                };
                if (list2.SelectedItem != null)
                {
                    objet = new Competence()
                    {
                        Actuel = this.Actuel,
                        Id_CompetenceActuel = t.Id_CompetenceActuel,
                        Annee = C3.SelectedDate.ToString(),
                        Actif = this.Actif

                    };
                }
                else
                {
                    objet = new Competence()
                    {
                        Actuel = this.Actuel,
                        Id_CompetenceActuel =null,
                        Annee = C3.SelectedDate.ToString(),
                        Actif = this.Actif

                    };
                }
                objet.IntituleCompetences.Add(objet2);
                //db.Competences.Add(objet);
                //db.SaveChanges();*/
            using (var db = new BBD_projetEntities())
            {
               
                var req3 = from c in db.Competences
                           where c.Actuel == true && c.Actif == true
                           select new BO.Competence
                           {
                               Actuel = c.Actuel,
                               IntituleCompetence = c.IntituleCompetences.Where(b => b.Id_Langue == 3 && b.Id_Competence==c.Id_Competence).Select(b => b.intitule).FirstOrDefault(),
                               Annee = c.Annee,
                               Id_Competence = c.Id_Competence,
                               Id_CompetenceActuel = c.Id_CompetenceActuel
                           };
                var list = req3.ToList();
                foreach (var x in list)
                {
                    var req = db.IntituleCompetences.Where(b =>b.Id_Competence == x.Id_Competence);
                    x.IntituleCompetences =  BO.IntituleCompetence.ListDALtoBO(req.ToList());
                    foreach(var y in x.IntituleCompetences)
                    {
                        System.Diagnostics.Debug.WriteLine("C: " + y.Id_Competence + "  L: " + y.Id_Langue);
                       System.Diagnostics.Debug.WriteLine("I: "+y.Intitule + " D: " + y.Description);
                    }
                }
                System.Diagnostics.Debug.WriteLine(list[3].IntituleCompetences[0].Intitule);
                NavigationService nav = Frame.NavigationService;
                ListViewCompetence l = new ListViewCompetence();
                //nav.Navigate(new Uri("CreateCompetence.xaml",UriKind.Relative));
                //nav.Navigate(new Uri("Groupe.xaml", UriKind.Relative));
                // nav.Navigate(new ModificationCompetence(list[3]));
                //nav.Navigate(new ListViewGroup());
                //nav.Navigate(new ListViewCompetence());
                // nav.Navigate(new ListViewEmploye());
                //nav.Navigate(new CreationEmploye());
                nav.Navigate(new Menu());
            }
        }
        }
    /*
        
    }*/
}
