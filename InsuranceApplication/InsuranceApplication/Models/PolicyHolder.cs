using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class PolicyHolder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        [DisplayName("Policy")]
        public string PolicyCode { get; set; }
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
        [DisplayName("Amount Paid")]
        public double AmountPaid { get; set; }
        [DisplayName("Amount Remaining")]
        public double AmountRemaining { get; set; }
    }
}
