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
    /// Logique d'interaction pour ListViewLangue.xaml
    /// </summary>
    public partial class ListViewLangue : Page
    {
        List<DAL.Langue> listLangue = new List<DAL.Langue>();
        public ListViewLangue()
        {
            InitializeComponent();

            using (var db = new BBD_projetEntities())
            {
                // partie pour récupérer la liste
                var req = (from l in db.Langues
                           select new
                           {
                               IdLangue = l.Id_Langue,
                               Nom = l.Nom,
                               defaut = l.Default
                           }).ToList().Select(l => new DAL.Langue
                           {
                               Id_Langue = l.IdLangue,
                               Nom = l.Nom,
                               Default = l.defaut
                           });
                listLangue = req.ToList();
                listDal.ItemsSource = listLangue;
            }
        }

        private void AddLangue_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateLangue());
        }

        private void listBo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(listDal.SelectedItem is DAL.Langue)
            NavigationService.Navigate((new ModificationLangue(((DAL.Langue)listDal.SelectedItem))));
        }

        private void DeleteLangue_Click(object sender, RoutedEventArgs e)
        {
            var Item = (DAL.Langue)listDal.SelectedItem;

            MessageBoxResult result = MessageBox.Show("Warning : Vous allez supprimer " + Item.Nom);
            using (var db = new BBD_projetEntities())
            {
                db.Langues.Attach(Item);
                db.Langues.Remove(Item);
                db.SaveChanges();
            }
            NavigationService.Navigate(new ListViewLangue());

        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }
    }
}

