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
    /// Logique d'interaction pour CreateCompetence.xaml
    /// </summary>
    public partial class CreateCompetence : Page
    {
        private bool _actif;
        public bool Actif
        {
            get { return _actif; }
            set { _actif = value; }
        }
        private bool _actuel;
        public bool Actuel
        {
            get { return _actuel; }
            set
            {
                _actuel = value;
            }
        }
        public CreateCompetence()
        {
            InitializeComponent();
            this.DataContext = this;
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

                var List = BO.Langue.ListDALtoBO(req2.ToList());
                list.ItemsSource = List ;
                list.SelectedValuePath = "Id_Langue"; 

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
            //lier les eux objets
            using (var db = new BBD_projetEntities())
            {
                BO.Competence t = (BO.Competence)list2.SelectedItem;
                DAL.Competence objet = null;
                test2.Content = Actif.ToString();
                var objet2 = new DAL.IntituleCompetence();

                if (list2.SelectedItem != null)
                {
                    objet = new DAL.Competence()
                    {
                        Actuel = this.Actuel,
                        Id_CompetenceActuel = t.Id_CompetenceActuel,
                        Annee = C3.SelectedDate.ToString(),
                        Actif = this.Actif
                    };
                }
                else
                {
                    objet = new DAL.Competence()
                    {
                        Actuel = this.Actuel,
                        Id_CompetenceActuel = null,
                        Annee = C3.SelectedDate.ToString(),
                        Actif = this.Actif

                    };
                }
                if (list.SelectedItem == null)
                {
                    objet2 = new DAL.IntituleCompetence()
                    {
                        intitule = C1.Text,
                        Description = C2.Text,
                        Id_Langue = 3,
                        Competence = objet
                    };
                }
                else
                {
                    objet2 = new DAL.IntituleCompetence()
                    {
                        intitule = C1.Text,
                        Description = C2.Text,
                        Id_Langue = ((int)list.SelectedValue),
                        Competence = objet
                    };
                }
               
                objet.IntituleCompetences.Add(objet2);
                db.Competences.Add(objet);
                db.SaveChanges();
            ListViewCompetence l = new ListViewCompetence();
            this.NavigationService.Navigate(new Uri("ListViewCompetence.xaml", UriKind.Relative));

        }
    }

        private void B_Actif_Checked(object sender, RoutedEventArgs e)
        {
            Actif = true;
            System.Diagnostics.Debug.Write(true);
        }

        private void B_Actif_Unchecked(object sender, RoutedEventArgs e)
        {
            Actif = false;
            System.Diagnostics.Debug.Write(false);
        }
    }
}
