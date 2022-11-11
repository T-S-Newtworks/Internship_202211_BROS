using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Internship_Template.Models.Entity
{
        [MetadataType(typeof(T_USERMetadata))]
        public partial class T_USER
        {

        }

        [MetadataType(typeof(T_LOGINMetadata))]
        public partial class T_LOGIN
        {

        }
}