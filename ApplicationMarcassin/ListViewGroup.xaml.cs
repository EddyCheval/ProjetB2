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
    /// Logique d'interaction pour ListViewGroup.xaml
    /// </summary>
    public partial class ListViewGroup : Page
    {
        public List<BO.Groupe> listGroupes;
        public ListViewGroup()
        {
            InitializeComponent();
            using (var db = new BBD_projetEntities())
            {
                var req = (from g in db.Groupes
                          select new 
                          {
                              DateReunion = g.DateReunion,
                              Id_Competence = g.Id_Competence,
                              Id_Groupe = g.Id_Groupe,
                              Titre = g.Titre
                          }).ToList().Select(g => new DAL.Groupe
                          {
                              DateReunion = g.DateReunion,
                              Id_Competence = g.Id_Competence,
                              Id_Groupe = g.Id_Groupe,
                              Titre = g.Titre
                          });
                listGroupes = BO.Groupe.ListDalToBO(req.ToList());
                foreach(var x in listGroupes)
                {
                    System.Diagnostics.Debug.WriteLine(x.Id_Groupe);
                }
                List.ItemsSource = listGroupes;
            }
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(((BO.Groupe)List.SelectedItem) != null)
                NavigationService.Navigate((new ModificationGroupe(((BO.Groupe)List.SelectedItem))));
        }

        private void Creation_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Groupe());
        }

        private void Suppression_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                var ObjSupp = (from g in db.Groupes
                               where g.Id_Groupe == ((BO.Groupe)List.SelectedItem).Id_Groupe
                               select g).ToList();
                if(ObjSupp.Count != 0)
                {
                    var ObjSupp2 = (from m in db.Membres
                                    where m.Id_Groupe == ((BO.Groupe)List.SelectedItem).Id_Groupe
                                    select m).ToList();
                    foreach(var x in ObjSupp2)
                    {
                        db.Membres.Remove(x);
                    }
                    var ObjSupp3 = (from x in db.Messageries
                                    where x.Id_Groupe == ((BO.Groupe)List.SelectedItem).Id_Groupe
                                    select x).ToList();
                    foreach(var w in ObjSupp3)
                    {
                        db.Messageries.Remove(w);
                    }
                    db.Groupes.Remove(ObjSupp.First());
                    db.SaveChanges();
                    listGroupes.Remove((BO.Groupe)List.SelectedItem);
                    List.Items.Refresh();
                }
            }
        }
    }
}
