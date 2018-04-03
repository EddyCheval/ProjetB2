﻿using ApplicationMarcassin.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
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
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {

        private List<BO.Employe> ListEmploye;
        private List<BO.Competence> ListCompetence;
        private List<BO.Groupe> ListGroupe;
        private readonly int languedef;

        public Menu()
        {
            InitializeComponent();
            using (var db = new BBD_projetEntities())
            {
                languedef = db.LangueParDefaut();
                var Competence = from c in db.Competences
                                 select new BO.Competence
                                 {
                                     Actuel = c.Actuel,
                                     IntituleCompetence = c.IntituleCompetences.Where(b => b.Id_Langue == languedef).Select(b => b.intitule).FirstOrDefault(),
                                     Annee = c.Annee,
                                     Id_Competence = c.Id_Competence,
                                     Id_CompetenceActuel = c.Id_CompetenceActuel
                                 }; 

                var Groupe = from g in db.Groupes
                             select g;
                var Employe = from e in db.Employes
                              select e;
                ListEmploye = BO.Employe.ListDalToBO(Employe.ToList());
                ListCompetence = Competence.ToList();
                ListGroupe = BO.Groupe.ListDalToBO(Groupe.ToList());

                listViewCompetence.ItemsSource = ListCompetence;
                listViewEmploye.ItemsSource = ListEmploye;
                listViewGroupe.ItemsSource = ListGroupe;
                ComboPicker.SelectedIndex = 4;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBoxItem)ComboPicker.SelectedItem).Content)
            {
                case "Employe":
                    listViewEmploye.Visibility = System.Windows.Visibility.Visible;
                    listViewCompetence.Visibility = System.Windows.Visibility.Collapsed;
                    listViewGroupe.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case "Groupe":
                    listViewEmploye.Visibility = System.Windows.Visibility.Collapsed;
                    listViewCompetence.Visibility = System.Windows.Visibility.Collapsed;
                    listViewGroupe.Visibility = System.Windows.Visibility.Visible;
                    break;
                case "Compétence":
                    listViewEmploye.Visibility = System.Windows.Visibility.Collapsed;
                    listViewCompetence.Visibility = System.Windows.Visibility.Visible;
                    listViewGroupe.Visibility = System.Windows.Visibility.Collapsed; 
                    break;
                default:
                    break;
            }
        }

        private void BtnRecherche_Click(object sender, RoutedEventArgs e2)
        {

            using (var db = new BBD_projetEntities())
            {
                if (ComboPicker.SelectedIndex == 0)
                {
                    ListEmploye = BO.Employe.ListDalToBO(
                            (from e in db.Employes
                             where SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", e.Nom) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", e.Prénom) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", e.Service) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", e.Metier) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", e.Entreprise) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", e.Id_Employe.ToString()) > 0
                             select e).ToList());
                    listViewEmploye.ItemsSource = ListEmploye;
                    listViewEmploye.Items.Refresh();
                }

                if (ComboPicker.SelectedIndex == 1)
                {
                    ListCompetence =
                            (from c in db.Competences
                             join ic in db.IntituleCompetences
                             on c.Id_Competence equals ic.Id_Competence
                             where SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", c.Annee) > 0
                                    || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", c.Id_Competence.ToString()) > 0
                                    || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", ic.intitule) > 0
                                    || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", ic.Description) > 0
                             select new BO.Competence
                             {
                                 Actuel = c.Actuel,
                                 IntituleCompetence = c.IntituleCompetences.Where(b => b.Id_Langue == languedef).Select(b => b.intitule).FirstOrDefault(),
                                 Annee = c.Annee,
                                 Id_Competence = c.Id_Competence,
                                 Id_CompetenceActuel = c.Id_CompetenceActuel
                             }).ToList();
                    listViewCompetence.ItemsSource = ListCompetence;
                    listViewCompetence.Items.Refresh();
                }
                if (ComboPicker.SelectedIndex == 4)
                {
                    ListGroupe = BO.Groupe.ListDalToBO(
                            (from g in db.Groupes
                             where SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", g.Id_Groupe.ToString()) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", g.Titre) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", g.Id_Competence.ToString()) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", g.DateReunion.ToString()) > 0
                                     || SqlFunctions.PatIndex("%" + TextRecherche.Text + "%", g.DateCreation.ToString()) > 0
                             select g).ToList());
                    listViewGroupe.ItemsSource = ListGroupe;
                    listViewGroupe.Items.Refresh();
                }
            }
        }

        private void Competence_Click(object sender, RoutedEventArgs e)
        { 
            this.NavigationService.Navigate(new Uri("ListViewCompetence.xaml", UriKind.Relative));
        }

        private void Employe_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("ListViewEmploye.xaml", UriKind.Relative));
        }

        private void Groupe_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new Uri("ListViewGroup.xaml", UriKind.Relative));
        }
    }
}
