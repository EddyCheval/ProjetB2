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
    /// Logique d'interaction pour ListViewEmploye.xaml
    /// </summary>
    public partial class ListViewEmploye : Page
    {
        private List<BO.Employe> listemploye;

        public ListViewEmploye()
        {
            InitializeComponent();
            using (var db = new BBD_projetEntities())
            {
                var req = from e in db.Employes
                          select e;
                listemploye = BO.Employe.ListDalToBO(req.ToList());
                System.Diagnostics.Debug.WriteLine(listemploye[0].Metier);
                list.ItemsSource = listemploye;
            }
        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(((BO.Employe)list.SelectedItem) != null)
                NavigationService.Navigate((new ModificationEmploye(((BO.Employe)list.SelectedItem))));
        }

        private void Suppression_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                if (list.SelectedItem != null)
                {
                    var DALObject4 = (from m in db.Membres
                                      where m.Id_Employe == ((BO.Employe)list.SelectedItem).Id_Employe
                                      select m).ToList();
                    foreach(var w in DALObject4)
                    {
                        db.Membres.Remove(w);
                    }
                    var DALObject3 = (from lc in db.LiaisonCompetences
                                      where lc.Id_Employe == ((BO.Employe)list.SelectedItem).Id_Employe
                                      select lc).ToList();
                    foreach(var y in DALObject3)
                    {
                        db.LiaisonCompetences.Remove(y);
                    }
                    var DALObject2 = (from lp in db.LanguePossedes
                                      where lp.Id_Employe == ((BO.Employe)list.SelectedItem).Id_Employe
                                      select lp).ToList();
                    foreach(var x in DALObject2)
                    {
                        db.LanguePossedes.Remove(x);
                    }
                    var DALObject = (from e2 in db.Employes
                                    where e2.Id_Employe == ((BO.Employe)list.SelectedItem).Id_Employe
                                    select e2).First();
                    db.Employes.Remove(DALObject);
                    listemploye.Remove((BO.Employe)list.SelectedItem);
                    list.Items.Refresh();
                }
                db.SaveChanges();

            }
        }

        private void Creation_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreationEmploye());
        }
    }
}
