using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLan.Model.aplan.Database
{
    public class BaseElement
    {
        /// <value>
        /// Property <c>Id</c> represents an Id used to connect information between source file and created Eulynx Objects.
        /// </value>
        public string SourceFileConnectingID { get; set; } // (Khaled)
    }
}
