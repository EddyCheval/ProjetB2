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
    /// Logique d'interaction pour ModificationCategorie.xaml
    /// </summary>
    public partial class ModificationCategorie : Page
    {
        public DAL.Categorie Categorie;
        public ModificationCategorie(DAL.Categorie categorie)
        {
            InitializeComponent();
            Categorie = categorie;

            var id = Categorie.Id_Categorie;
            var nom = Categorie.Intitule;
            var lastSuperId = Categorie.Id_Super_Categorie;

            CategorieName.Text = nom;


            List<DAL.Langue> listLangue = new List<DAL.Langue>();
            List<DAL.Categorie> listCategorie = new List<DAL.Categorie>();
            using (var db = new BBD_projetEntities())
            {
                // partie pour récupérer la liste
                var req = (from c in db.Categories
                           select new
                           {
                               IdCategorie = c.Id_Categorie,
                               intitule = c.Intitule,
                               superID = c.Id_Super_Categorie
                           }).ToList().Select(l => new DAL.Categorie
                           {
                               Id_Categorie = l.IdCategorie,
                               Intitule = l.intitule,
                               Id_Super_Categorie = l.superID
                           });

                listCategorie = req.ToList();
                listCategorieDal.ItemsSource = listCategorie;
                listCategorieDal.SelectedIndex = listCategorie.IndexOf(listCategorie.Where(c => c.Id_Categorie == categorie.Id_Super_Categorie).Select(c => c).FirstOrDefault());

                var req2 = from c in db.Langues
                           join b in db.IntituleCategories
                           on c.Id_Langue equals b.Id_Langue
                           where b.Id_Categorie == categorie.Id_Categorie & b.intitule == categorie.Intitule
                           select new BO.Langue
                           {
                               Id_Langue = c.Id_Langue,
                               Nom = c.Nom

                           };

                var langue = req2.ToList();

                try
                {
                    Langue.Text = langue.FirstOrDefault().Nom;
                }
                catch (Exception)
                {
                    Langue.Text = "Warning : Pas de langue attribué";
                }
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var newCategorie = (DAL.Categorie)listCategorieDal.SelectedItem;
            try
            {
                using (var db = new BBD_projetEntities())
                {
                    var Item = db.Categories.Find(Categorie.Id_Categorie);
                    Item.Intitule = CategorieName.Text;
                    Item.Id_Super_Categorie = newCategorie.Id_Categorie;
                    db.SaveChanges();
                }
                NavigationService.Navigate(new ListViewCategorie());
            }
            catch (Exception)
            {

                MessageBoxResult result = MessageBox.Show("Warning : vous n'avez pas saisie d'id_super_categorie");
            }

        }
    }
}
