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
    /// Logique d'interaction pour CreateCategorie.xaml
    /// </summary>
    public partial class CreateCategorie : Page
    {
        List<DAL.Categorie> listCategorie = new List<DAL.Categorie>();
        public CreateCategorie()
        {
            InitializeComponent();
            List<DAL.Langue> listLangue = new List<DAL.Langue>();
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


                var req2 = (from l in db.Langues
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
                listLangue = req2.ToList();
                listLangueDal.ItemsSource = listLangue;
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var checkError = 0;
                foreach (var item in listCategorie)
                {
                    if (NewCategorie.Text == item.Intitule)
                    {
                        checkError = 1;
                    }
                    if (NewCategorie.Text == "")
                    {
                        checkError = 2;
                    }
                }

                if (checkError == 0)
                {
                    using (var db = new BBD_projetEntities())
                    {
                        var Categorie = NewCategorie.Text;

                        var Langue = ((DAL.Langue)listLangueDal.SelectedItem);
                        var SuperCategorie = ((DAL.Categorie)listCategorieDal.SelectedItem);


                        var idLangue = Langue.Id_Langue;
                        var idSuperCategorie = SuperCategorie.Id_Categorie;

                        var dalCategorie = new DAL.Categorie();
                        dalCategorie.Id_Super_Categorie = idSuperCategorie;
                        dalCategorie.Intitule = Categorie;
                        db.Categories.Add(dalCategorie);
                        db.SaveChanges();

                        var req = from c in db.Categories
                                  where c.Intitule == Categorie
                                  select c;

                        var dalIntituleCategorie = new DAL.IntituleCategorie();
                        dalIntituleCategorie.Id_Categorie = req.FirstOrDefault().Id_Categorie;
                        dalIntituleCategorie.Id_Langue = idLangue;
                        dalIntituleCategorie.intitule = Categorie;
                        db.IntituleCategories.Add(dalIntituleCategorie);
                        db.SaveChanges();
                        NavigationService.Navigate(new ListViewCategorie());

                    }
                }
                else if (checkError == 1)
                {
                    MessageBoxResult resultBeta = MessageBox.Show("Warning : Cette categorie existe deja");
                }
                else
                {
                    MessageBoxResult resultDelta = MessageBox.Show("Warning : Vous n'avez pas mis de nom");
                }
            }
            catch (Exception)
            {

                MessageBoxResult resultAlpha = MessageBox.Show("Warning : Veuillez saisir des categories ET langues");
            }
        }
    }
}
