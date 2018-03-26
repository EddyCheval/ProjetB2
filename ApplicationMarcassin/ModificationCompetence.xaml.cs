using ApplicationMarcassin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationMarcassin
{
    /// <summary>
    /// Logique d'interaction pour ModificationCompetence.xaml
    /// </summary>
    public partial class ModificationCompetence : Page
    {
        private bool _actif;
        public bool Actif
        {
            get { return _actif; }
            set { _actif = value; }
        }
        private bool _actuel;
        public bool Actuel
        {
            get { return _actuel; }
            set
            {
                _actuel = value;
            }
        }
        public BO.Competence Competence;
        public ModificationCompetence(BO.Competence competence)
        {
            InitializeComponent();
            Competence = competence;

            using (var db = new BBD_projetEntities())
            {
                var val = db.LangueParDefaut();
                var req = from c in db.OffrantParCompetenceAvecLangueEtIntituleActu(8, 3)
                          select c;
                foreach (var v in req)
                {
                    this.DataContext = v.Description;
                }
                var req2 = from c in db.Langues
                           select c;

                var List = BO.Langue.ListDALtoBO(req2.ToList());
                list.ItemsSource = List;
                list.SelectedValuePath = "Id_Langue";

                var req3 = from c in db.Competences
                           where c.Actuel == true && c.Actif == true
                           select new BO.Competence
                           {
                               Actuel = c.Actuel,
                               IntituleCompetence = c.IntituleCompetences.Where(b => b.Id_Langue == val).Select(b => b.intitule).FirstOrDefault(),
                               Annee = c.Annee,
                               Id_Competence = c.Id_Competence,
                               Id_CompetenceActuel = c.Id_CompetenceActuel
                           };

                list2.ItemsSource = req3.ToList();

                //Actif
                this.Actif = Competence.Actif;
                if(this.Actif == true)
                {
                    B_Actif.IsChecked = true;
                }

                //Actuel
                this.Actuel = Competence.Actuel;
                if (this.Actuel == true)
                {
                    B_Actuel.IsChecked = true;
                }
            }
        }
        private void b_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new BBD_projetEntities())
            {
                BO.Competence t = (BO.Competence)list2.SelectedItem;
                DAL.Competence objet = null;
                var objet2 = new DAL.IntituleCompetence();
                test2.Content = Actif.ToString();

                IEnumerable<IntituleCompetence> req = null;
                if (((int?)list.SelectedValue) != null)
                {
                    req = (from c in db.IntituleCompetences
                               where c.Id_Competence == Competence.Id_Competence && c.Id_Langue == ((int)list.SelectedValue)
                               select new
                               {
                                   Competence = c.Competence,
                                   Id_Langue = c.Id_Langue,
                                   Id_Competence = c.Id_Competence,
                                   Description = c.Description,
                                   intitule = c.intitule,
                                   Langue = c.Langue
                               }).ToList().Select(c => new DAL.IntituleCompetence
                               {
                                   Competence = c.Competence,
                                   Id_Langue = c.Id_Langue,
                                   Id_Competence = c.Id_Competence,
                                   Description = c.Description,
                                   intitule = c.intitule,
                                   Langue = c.Langue
                               });
                }
                if (!(req==null) || (req != null && req.ToList().Count > 0))
                {
                    var o = new BO.IntituleCompetence(req.ToList()[0]);
                    var IcObjet =db.IntituleCompetences.Find(o.Id_Competence,o.Id_Langue);
                    IcObjet.intitule = C1.Text;
                    IcObjet.Description = C2.Text;
                    if (list.SelectedItem == null)
                        IcObjet.Id_Langue = db.LangueParDefaut();
                    else
                        IcObjet.Id_Langue = ((int)list.SelectedValue);
                }
                else
                {
                    if (list.SelectedItem == null)
                    {
                        var LangueParDef = db.LangueParDefaut();
                        if ((from c in db.IntituleCompetences
                             where c.Id_Competence == Competence.Id_Competence && c.Id_Langue == LangueParDef
                             select new { }).Count() > 1)
                        {
                            objet2 = new DAL.IntituleCompetence()
                            {
                                intitule = C1.Text,
                                Description = C2.Text,
                                Id_Langue = db.LangueParDefaut(),
                                Id_Competence = Competence.Id_Competence,
                                Competence = db.Competences.Find(Competence.Id_Competence),
                                Langue = db.Langues.Find(db.LangueParDefaut())
                            };
                            System.Diagnostics.Debug.WriteLine(objet2.intitule);
                            System.Diagnostics.Debug.WriteLine(objet2.Description);
                            System.Diagnostics.Debug.WriteLine(objet2.Id_Langue);
                            System.Diagnostics.Debug.WriteLine(objet2.Id_Competence);

                            db.IntituleCompetences.Add(objet2);
                        }
                        else
                        {
                            //work
                            var IcObjet = db.IntituleCompetences.Find(Competence.Id_Competence, db.LangueParDefaut());
                            IcObjet.intitule = C1.Text;
                            IcObjet.Description = C2.Text;
                        }

                    }
                    else
                    {
                        objet2 = new DAL.IntituleCompetence()
                        {
                            intitule = C1.Text,
                            Description = C2.Text,
                            Id_Langue = ((int)list.SelectedValue),
                            Id_Competence = Competence.Id_Competence
                        };

                        db.IntituleCompetences.Add(objet2);
                    }
                }
                var DALCompetence = db.Competences.Find(Competence.Id_Competence);
               if (list2.SelectedValue != null)
                    DALCompetence.Id_CompetenceActuel = t.Id_Competence;
              /* System.Diagnostics.Debug.WriteLine(C3.SelectedDate == null);
                System.Diagnostics.Debug.WriteLine(C3.SelectedDate != null);
                System.Diagnostics.Debug.WriteLine(!(C3.SelectedDate == null));*/
              if (C3.SelectedDate != null)
                    DALCompetence.Annee = C3.SelectedDate.ToString();
               if(DALCompetence.Actif == this.Actif)
                    DALCompetence.Actif = this.Actif;


                System.Diagnostics.Debug.WriteLine(C3.SelectedDate.ToString());
                System.Diagnostics.Debug.WriteLine(DALCompetence.Annee);
                System.Diagnostics.Debug.WriteLine(!(C3.SelectedDate == null));
                db.SaveChanges();
                ListViewCompetence l = new ListViewCompetence();
                this.NavigationService.Navigate(new Uri("ListViewCompetence.xaml", UriKind.Relative));

            }
        }

        private void B_Actif_Checked(object sender, RoutedEventArgs e)
        {
            Actif = true;
            System.Diagnostics.Debug.Write(true);
        }

        private void B_Actif_Unchecked(object sender, RoutedEventArgs e)
        {
            Actif = false;
            System.Diagnostics.Debug.Write(false);
        }

        private void B_Actuel_Checked(object sender, RoutedEventArgs e)
        {
            Actuel = true;
        }
        private void B_Actuel_Unchecked(object sender, RoutedEventArgs e)
        {
            Actuel = false;
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var value = "Nouvel intitulé";
            var value2 = "Nouvelle Decription";
            if (Competence.IntituleCompetences != null)
            {
                foreach (var x in Competence.IntituleCompetences)
                {
                    System.Diagnostics.Debug.WriteLine((x.Id_Langue));
                    if (x.Id_Langue == (int)list.SelectedValue)
                    {
                        value = x.Intitule;
                        value2 = x.Description;
                        System.Diagnostics.Debug.WriteLine(("lol"));
                    }
                }
            }
            C1.Text = value;
            C2.Text = value2;
            System.Diagnostics.Debug.WriteLine(value);
            System.Diagnostics.Debug.WriteLine(value2);
            System.Diagnostics.Debug.WriteLine((int)list.SelectedValue);
        }
    }
}
