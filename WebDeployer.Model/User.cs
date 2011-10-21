using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebDeployer.Model
{
    public class User
    {
        [MaxLength(64)] 
        public string Name { get; set; }
    }
}
