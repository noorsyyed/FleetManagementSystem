using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Models
{
    public class CallObject
    {
        /// <summary>
        /// The type of call whether a data provider or dynamic object
        /// </summary>
        public string CallType { get; set; }
        /// <summary>
        /// The target object to call
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// The method to call
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Paramters to be used during the method call
        /// </summary>
        public string[] Parameters { get; set; }
        /// <summary>
        /// Defines whether the value should be cached or not
        /// </summary>
        public bool IsCached { get; set; }
    }
}
