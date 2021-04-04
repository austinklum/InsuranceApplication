using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public int SecQ1Index { get; set; }
        public byte[] SecQ1ResponseHash { get; set; }
        public int SecQ2Index { get; set; }
        public byte[] SecQ2ResponseHash { get; set; }
        public int SecQ3Index { get; set; }
        public byte[] SecQ3ResponseHash { get; set; }
        public int AccountStatus { get; set; }
        public byte[] Salt { get; set; }

        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public String SecQ1Response { get; set; }
        [NotMapped]
        public String SecQ2Response { get; set; }
        [NotMapped]
        public String SecQ3Response { get; set; }
    }
}
