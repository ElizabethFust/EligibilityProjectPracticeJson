using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPRojectPracticeJson
{
    class Applicant
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ApplicantId { get; set; }

        public string ZipCode { get; set; }

        public double PovertyRate { get; set; }
    }
}
