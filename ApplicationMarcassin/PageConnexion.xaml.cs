using ApplicationMarcassin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Logique d'interaction pour PageConnexion.xaml
    /// </summary>
    public partial class PageConnexion : Page
    {
        public PageConnexion()
        {
            InitializeComponent();
        }

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                var Mot = ComputeSha256Hash(MDP.Text);
                var id = ComputeSha256Hash(Id.Text);
                var req = (from em in db.Employes
                           where em.Identifiant == id && em.MotDePasse == Mot && (em.EstAdmin==true || em.EstChefDeService==true)
                           select em).ToList();
                if(req.Count == 1)
                {
                    NavigationService.Navigate(new Menu());
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Aucun Correspondance trouvée !");
                }
            }
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
