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
    /// Logique d'interaction pour CreateLangue.xaml
    /// </summary>
    public partial class CreateLangue : Page
    {
        List<DAL.Langue> langue = new List<DAL.Langue>();
        public CreateLangue()
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
                langue = req.ToList();
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var checkError = 0;
            foreach (var item in langue)
            {
                if (NewLangue.Text == item.Nom)
                {

                    checkError = 1;
                }
            }
            if (NewLangue.Text == "")
            {
                checkError = 2;
            }

            if (checkError == 0)
            {
                using (var db = new BBD_projetEntities())
                {
                    var newLangue = NewLangue.Text;

                    var dalLangue = new DAL.Langue();
                    dalLangue.Nom = newLangue;
                    dalLangue.Default = defaultLangue.IsChecked.Value;
                    db.Langues.Add(dalLangue);
                    db.SaveChanges();
                    NavigationService.Navigate(new ListViewLangue());
                }
            }
            else if (checkError == 1)
            {
                MessageBoxResult resultDelta = MessageBox.Show("Warning : Nom de langue deja créer");
            }
            else
            {
                MessageBoxResult resultDelta = MessageBox.Show("Warning : Vous n'avez pas mis de nom");
            }
        }
    }
}
