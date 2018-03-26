﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApplicationMarcassin.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BBD_projetEntities : DbContext
    {
        public BBD_projetEntities()
            : base("name=BBD_projetEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Categorie> Categories { get; set; }
        public virtual DbSet<Competence> Competences { get; set; }
        public virtual DbSet<Demande> Demandes { get; set; }
        public virtual DbSet<Employe> Employes { get; set; }
        public virtual DbSet<Groupe> Groupes { get; set; }
        public virtual DbSet<IntituleCategorie> IntituleCategories { get; set; }
        public virtual DbSet<IntituleCompetence> IntituleCompetences { get; set; }
        public virtual DbSet<Langue> Langues { get; set; }
        public virtual DbSet<LanguePossede> LanguePossedes { get; set; }
        public virtual DbSet<LiaisonCompetence> LiaisonCompetences { get; set; }
        public virtual DbSet<Membre> Membres { get; set; }
        public virtual DbSet<Messagerie> Messageries { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    
        [DbFunction("BBD_projetEntities", "CompCategorie")]
        public virtual IQueryable<CompCategorie_Result> CompCategorie(Nullable<int> id, Nullable<int> idlangue)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var idlangueParameter = idlangue.HasValue ?
                new ObjectParameter("Idlangue", idlangue) :
                new ObjectParameter("Idlangue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<CompCategorie_Result>("[BBD_projetEntities].[CompCategorie](@id, @Idlangue)", idParameter, idlangueParameter);
        }
    
        [DbFunction("BBD_projetEntities", "Demandeur")]
        public virtual IQueryable<Demandeur_Result> Demandeur(Nullable<int> idLangue)
        {
            var idLangueParameter = idLangue.HasValue ?
                new ObjectParameter("IdLangue", idLangue) :
                new ObjectParameter("IdLangue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Demandeur_Result>("[BBD_projetEntities].[Demandeur](@IdLangue)", idLangueParameter);
        }
    
        [DbFunction("BBD_projetEntities", "DemandeurParCompetennceAvecLangueEtIntituleActu")]
        public virtual IQueryable<DemandeurParCompetennceAvecLangueEtIntituleActu_Result> DemandeurParCompetennceAvecLangueEtIntituleActu(Nullable<int> idComp, Nullable<int> idLangue)
        {
            var idCompParameter = idComp.HasValue ?
                new ObjectParameter("IdComp", idComp) :
                new ObjectParameter("IdComp", typeof(int));
    
            var idLangueParameter = idLangue.HasValue ?
                new ObjectParameter("IdLangue", idLangue) :
                new ObjectParameter("IdLangue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<DemandeurParCompetennceAvecLangueEtIntituleActu_Result>("[BBD_projetEntities].[DemandeurParCompetennceAvecLangueEtIntituleActu](@IdComp, @IdLangue)", idCompParameter, idLangueParameter);
        }
    
        [DbFunction("BBD_projetEntities", "Offrant")]
        public virtual IQueryable<Offrant_Result> Offrant(Nullable<int> idLangue)
        {
            var idLangueParameter = idLangue.HasValue ?
                new ObjectParameter("IdLangue", idLangue) :
                new ObjectParameter("IdLangue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Offrant_Result>("[BBD_projetEntities].[Offrant](@IdLangue)", idLangueParameter);
        }
    
        [DbFunction("BBD_projetEntities", "OffrantParCompetenceAvecLangueEtIntituleActu")]
        public virtual IQueryable<OffrantParCompetenceAvecLangueEtIntituleActu_Result> OffrantParCompetenceAvecLangueEtIntituleActu(Nullable<int> idComp, Nullable<int> idLangue)
        {
            var idCompParameter = idComp.HasValue ?
                new ObjectParameter("IdComp", idComp) :
                new ObjectParameter("IdComp", typeof(int));
    
            var idLangueParameter = idLangue.HasValue ?
                new ObjectParameter("IdLangue", idLangue) :
                new ObjectParameter("IdLangue", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<OffrantParCompetenceAvecLangueEtIntituleActu_Result>("[BBD_projetEntities].[OffrantParCompetenceAvecLangueEtIntituleActu](@IdComp, @IdLangue)", idCompParameter, idLangueParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        [DbFunction("BBD_projetEntities", "GroupeMessagerie")]
        public virtual IQueryable<GroupeMessagerie_Result> GroupeMessagerie(Nullable<int> id_Groupe)
        {
            var id_GroupeParameter = id_Groupe.HasValue ?
                new ObjectParameter("Id_Groupe", id_Groupe) :
                new ObjectParameter("Id_Groupe", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<GroupeMessagerie_Result>("[BBD_projetEntities].[GroupeMessagerie](@Id_Groupe)", id_GroupeParameter);
        }
    
        [DbFunction("BBD_projetEntities", "MembresGroupe")]
        public virtual IQueryable<MembresGroupe_Result> MembresGroupe(Nullable<int> id_Groupe)
        {
            var id_GroupeParameter = id_Groupe.HasValue ?
                new ObjectParameter("Id_Groupe", id_Groupe) :
                new ObjectParameter("Id_Groupe", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<MembresGroupe_Result>("[BBD_projetEntities].[MembresGroupe](@Id_Groupe)", id_GroupeParameter);
        }
    }
}