using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApplication.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string PhysicianName { get; set; }
        public string PhysicianLicenseNumber { get; set; }
        public string PatientName { get; set; }
        public DateTime PatientDOB { get; set; }
        public string PatientAddress { get; set; }
        public DateTime IssuedDate { get; set; }
        public bool? PhysicianVerified { get; set; }
        public bool? PatientVerified { get; set; }
        public bool? BillCreated { get; set; }
    }
}
