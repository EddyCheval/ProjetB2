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
    /// Logique d'interaction pour ModificationLangue.xaml
    /// </summary>
    public partial class ModificationLangue : Page
    {
        public DAL.Langue Langue;
        public ModificationLangue(DAL.Langue langue)
        {
            InitializeComponent();
            Langue = langue;

            var id = Langue.Id_Langue;
            var nom = Langue.Nom;
            var defaut = Langue.Default;

            NewLangue.Text = nom;
            NewDefault.IsChecked = defaut;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                var Item = db.Langues.Find(Langue.Id_Langue);
                //MessageBoxResult result = MessageBox.Show(Langue.Nom);
                Item.Nom = NewLangue.Text;
                Item.Default = NewDefault.IsChecked.Value;
                db.SaveChanges();
            }
            NavigationService.Navigate(new ListViewLangue());
        }
    }
}
