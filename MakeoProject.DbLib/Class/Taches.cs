using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace MakeoProject.DbLib.Class
{
    public partial class Taches
    {
        public int Id { get; set; }
        public ulong ProjectId { get; set; }
        public DateTime Date { get; set; }
        public int Duree { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Project Project { get; set; }
    }
}

