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
        public List<BO.Competence> listCompetences;
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
                listCompetences = new List<BO.Competence>();
                foreach (Competence c in req.ToList())
                {
                    listCompetences.Add
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
                list.ItemsSource = listCompetences;
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
            if(list.SelectedItem is BO.Competence)
           NavigationService.Navigate((new ModificationCompetence(((BO.Competence)list.SelectedItem))));
            //NavigationService nav = ((Frame)this.Parent).NavigationService;
            //nav.Navigate(new ModificationCompetence(((BO.Competence)list.SelectedItem)));
        }

        private void Creation_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateCompetence());
        }

        private void Suppression_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {

                var ObjSupp5 = (from c in db.Competences
                                where c.Id_Competence == ((BO.Competence)list.SelectedItem).Id_Competence
                                select c).ToList();

                if (ObjSupp5.Count != 0)
                {
                    var ObjSupp = (from ic in db.IntituleCompetences
                                   where ic.Id_Competence == ((BO.Competence)list.SelectedItem).Id_Competence
                                   select ic).ToList();
                    foreach (var x in ObjSupp)
                    {
                        db.IntituleCompetences.Remove(x);
                    }
                    var ObjSupp2 = (from lc in db.LiaisonCompetences
                                    where lc.Id_Competence == ((BO.Competence)list.SelectedItem).Id_Competence
                                    select lc).ToList();
                    foreach (var x in ObjSupp2)
                    {
                        db.LiaisonCompetences.Remove(x);
                    }
                    var ObjSupp3 = (from d in db.Demandes
                                    where d.Id_Competence == ((BO.Competence)list.SelectedItem).Id_Competence
                                    select d).ToList();
                    foreach (var z in ObjSupp3)
                    {
                        db.Demandes.Remove(z);
                    }
                    var ObjSupp4 = (from n in db.Notes
                                    where n.Id_Competence == ((BO.Competence)list.SelectedItem).Id_Competence
                                    select n).ToList();
                    foreach (var m in ObjSupp4)
                    {
                        db.Notes.Remove(m);
                    }
                    db.Competences.Remove(ObjSupp5.First());
                    db.SaveChanges();
                    listCompetences.Remove(((BO.Competence)list.SelectedItem));
                    list.Items.Refresh();
                }
            }
        }


        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }
    }
}
