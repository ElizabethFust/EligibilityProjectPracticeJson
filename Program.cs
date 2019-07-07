using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace EligibilityPRojectPracticeJson

{
    class Program
    {
        static void Main(string[] args)
        {
            var applicants = new List<Applicant>();
            
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "applicants.json");
            applicantList = GetApplicantInfo();
            SerializeApplicantsToFile(applicants, fileName);

        }

        //gather input from user; turn input into a List; serialize List to Json file to store
        public static List<Applicant> GetApplicantInfo()
        {
            Applicant applicant = new Applicant();
            var applicants = new List<Applicant>();
            Console.Clear();
            Console.Write("Please enter your first name:\t");
            applicant.FirstName = Console.ReadLine();
            Console.Write("Please enter your last name:\t");
            applicant.LastName = Console.ReadLine();
            Console.Write("Please enter your zip code:\t");
            applicant.ZipCode = Console.ReadLine();
            
            applicants.Add(applicant.FirstName);
            applicants.Add(applicant.LastName);
            applicants.Add(applicant.ZipCode);
            return applicants;

        }

            public static void SerializeApplicantsToFile(List<Applicant> applicants, string fileName)
        {
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(fileName))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, applicants);
            }
        }
    }
}
