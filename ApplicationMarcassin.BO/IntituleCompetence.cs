using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMarcassin.BO
{
    public class IntituleCompetence
    {
        public static List<IntituleCompetence> ListDALtoBO(List<DAL.IntituleCompetence> list)
        {
            List<IntituleCompetence> ListBo = new List<IntituleCompetence>();
            foreach (var x in list)
            {
                ListBo.Add(new IntituleCompetence(x));
            }
            return ListBo;
        }
        public static IntituleCompetence DALtoBO(DAL.IntituleCompetence dal)
        {
            IntituleCompetence Bo = new IntituleCompetence(dal);
            return Bo;
        }
        public IntituleCompetence(DAL.IntituleCompetence ic)
        {
            this.Description = ic.Description;
            this.Id_Competence = ic.Id_Competence;
            this.Id_Langue = ic.Id_Langue;
            this.Intitule = ic.intitule;
        }

        private int _id_Competence;
        public int Id_Competence
        {
            get { return _id_Competence; }
            set { _id_Competence = value; }
        }
        private int _id_Langue;
        public int Id_Langue
        {
            get { return _id_Langue; }
            set { _id_Langue = value; }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _intitule;
        public string Intitule
        {
            get { return _intitule; }
            set { _intitule = value; }
        }
    }
}
