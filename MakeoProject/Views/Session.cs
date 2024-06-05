using MakeoProject.DbLib.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeoProject.Views
{
    public static class Session
    {
        public static bool IsLoggedIn { get; set; }
        public static Freelance CurrentUser { get; set; }
    }
}
