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
    /// Logique d'interaction pour ListViewCategorie.xaml
    /// </summary>
    public partial class ListViewCategorie : Page
    {
        List<DAL.Categorie> listCategorie = new List<DAL.Categorie>();

        public ListViewCategorie()
        {
            InitializeComponent();
            /// Ici on génère la liste des Catégorie
            /// 
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
                listDal.ItemsSource = listCategorie;
            }
        }

        private void AddCategorie_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateCategorie());
        }

        private void DeleteCategorie_Click(object sender, RoutedEventArgs e)
        {
            if (listDal.SelectedItem != null)
            {
                var Item = (DAL.Categorie)listDal.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Warning : Vous allez supprimer " + Item.Intitule);

                using (var db = new BBD_projetEntities())
                {
                    List<DAL.IntituleCategorie> listIntitule = new List<DAL.IntituleCategorie>();
                    List<DAL.Categorie> listEnfant = new List<DAL.Categorie>();
                    //partie liaison
                    var req = (from c in db.IntituleCategories
                               where c.Id_Categorie == Item.Id_Categorie
                               select new
                               {
                                   IdCategorie = c.Id_Categorie,
                                   intitule = c.intitule,
                                   Id_Langue = c.Id_Langue
                               }).ToList().Select(l => new DAL.IntituleCategorie
                               {
                                   Id_Categorie = l.IdCategorie,
                                   intitule = l.intitule,
                                   Id_Langue = l.Id_Langue
                               });
                    listIntitule = req.ToList();
                    foreach (var itemIntitule in listIntitule)
                    {
                        db.IntituleCategories.Attach(itemIntitule);
                        db.IntituleCategories.Remove(itemIntitule);
                        db.SaveChanges();
                        //MessageBoxResult unAutreTruc = MessageBox.Show(itemIntitule.intitule);
                    }


                    var IdParent = Item.Id_Super_Categorie;

                    var reqEnfant = (from c in db.Categories
                                     where c.Id_Super_Categorie == Item.Id_Categorie
                                     select new
                                     {
                                         IdCategorie = c.Id_Categorie,
                                         Intitule = c.Intitule,
                                         Id_Super_Categorie = c.Id_Super_Categorie
                                     }).ToList().Select(l => new DAL.Categorie
                                     {
                                         Id_Categorie = l.IdCategorie,
                                         Id_Super_Categorie = l.Id_Super_Categorie,
                                         Intitule = l.Intitule
                                     });
                    listEnfant = reqEnfant.ToList();

                    foreach (var itemEnfant in listEnfant)
                    {
                        var ItemEnfant = db.Categories.Find(itemEnfant.Id_Categorie);
                        ItemEnfant.Id_Super_Categorie = IdParent;
                        db.SaveChanges();
                    }

                    db.Categories.Attach(Item);
                    db.Categories.Remove(Item);
                    db.SaveChanges();
                }
                NavigationService.Navigate(new ListViewCategorie());
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Warning : Vous allez n'avez rien selectionné");
            }

        }

        private void listDal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(listDal.SelectedItem is DAL.Categorie)
            NavigationService.Navigate((new ModificationCategorie(((DAL.Categorie)listDal.SelectedItem))));

        }


        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }
    }
}
