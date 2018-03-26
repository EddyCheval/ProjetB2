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
                var list = BO.Groupe.ListDalToBO(req.ToList());
                List.ItemsSource = list;
            }
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            NavigationService.Navigate((new ModificationGroupe(((BO.Groupe)List.SelectedItem))));
        }
    }
}
