//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class LiaisonCompetence
    {
        public int Id_Employe { get; set; }
        public int Id_Competence { get; set; }
        public bool EstTutorant { get; set; }
    
        public virtual Competence Competence { get; set; }
        public virtual Employe Employe { get; set; }
    }
}
