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
using ApplicationMarcassin;
using ApplicationMarcassin.DAL;

namespace ApplicationMarcassin
{
    /// <summary>
    /// Logique d'interaction pour ListViewCompetence.xaml
    /// </summary>
    public partial class ListViewCompetence : Page
    {
        public ListViewCompetence()
        {
            InitializeComponent();
            using (BBD_projetEntities db = new BBD_projetEntities())
            {
                var l = db.LangueParDefaut();
                var req = from c in db.Competences
                          join b in db.IntituleCompetences
                            on c.Id_Competence equals b.Id_Competence
                          where b.Id_Langue==l
                          select c;
                var ListComp = new List<BO.Competence>();
                foreach (Competence c in req.ToList())
                {
                    ListComp.Add
                        (
                        new BO.Competence()
                        {
                            Actuel = c.Actuel,
                            Actif = c.Actif,
                            Annee = c.Annee,
                            Id_Competence = c.Id_Competence,
                            Id_CompetenceActuel = c.Id_CompetenceActuel,
                            IntituleCompetence = c.IntituleCompetences.FirstOrDefault().intitule.ToString()
                        }
                        );
                }
                list.ItemsSource = ListComp;
                list.SelectionChanged += (sender, e) =>
                {
                    System.Diagnostics.Debug.WriteLine(list.SelectedItem);
                };
            }
        }

        private void list_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           NavigationService.Navigate((new ModificationCompetence(((BO.Competence)list.SelectedItem))));
            //NavigationService nav = ((Frame)this.Parent).NavigationService;
            //nav.Navigate(new ModificationCompetence(((BO.Competence)list.SelectedItem)));
        }
    }
}
