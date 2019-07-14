using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using FileHelpers;

namespace EligibilityProjectPracticeJson

{
    class Program
    {
        static PovertyData[] PovertyRates;

        private static string _DataDirectory = "C:\\Users\\Liz Fust\\source\\repos\\EligibilityProjectPracticeJson";
        private static string _DataFile = "C:\\Users\\Liz Fust\\source\\repos\\EligibilityProjectPracticeJson\\Applicants.json";

       // private static string _ApplicantsPath = "C:\\Users\\Liz Fust\\Source\\Repos\\EligibilityProjectPracticeJson\\Applicants.json";
        static void Main(string[] args)
        {

            PovertyRates = GetPovertyData();

            if (!Directory.Exists(_DataDirectory))
            {
                Directory.CreateDirectory(_DataDirectory);
            }

            if (!File.Exists(_DataFile))
            {

                File.Create(_DataFile);

            }

            Run();

        }

        private static int DisplayMenu()
        {
            Console.Clear();
            Console.Title = "Eligibility for Assistance";
            Console.WriteLine();
            Console.WriteLine("Determine Eligibility for Gathering Strength Foundation Assistance");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine(" 1. Enter 1 below to Determine your eligibility.");
            Console.WriteLine(" 2. Enter 2 below to Retrieve your data.");
            Console.WriteLine(" 3. Exit");
            Console.WriteLine();
            Console.Write("Enter a number to proceed: ");

            //Handle if something other than number entered
            var result = Console.ReadLine();
            var choice = Convert.ToInt32(result);
            return choice;

        }

        public static void Run()
        {
            int userInput = 0;


            do
            {
                //get the selection
                userInput = DisplayMenu();


                switch (userInput)
                {
                    case 1: //for a new Applicant; to get their information
                        
                        AddApplicant();
                        break;
                    case 2: //for when an applicant returns; can look up their info using their ID#
                        RetrieveApplicantInfo();
                        //Remember to provide way for user to exit program again
                        break;
                    case 3:
                        Console.WriteLine("Exiting...");
                        System.Threading.Thread.Sleep(2000);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine(" Error: Invalid Choice");
                        System.Threading.Thread.Sleep(1000);
                        break;
                        //test
                }

            } while (userInput != 3);
        }

        public static void AddApplicant()
        {
            Applicant applicant = new Applicant();
            List <Applicant> applicants = new List <Applicant>();
            applicants = GetApplicantsFromFile();
            Console.Clear();
            Console.Write("Please enter your first name:\t");
            applicant.FirstName = Console.ReadLine();
            Console.Write("\nPlease enter your last name:\t");
            applicant.LastName = Console.ReadLine();
            Console.Write("\nPlease enter your zip code:\t");
            applicant.ZipCode = Console.ReadLine();

            GetPRateFromZip(applicant, applicants);
            
            SaveApplicant(applicant, applicants);
            //applicants = GetApplicantsFromFile();
            applicants.Add(applicant);

            System.Threading.Thread.Sleep(1000);
        }

        public static List<Applicant> GetPRateFromZip(Applicant applicant, List<Applicant> applicants)
            { 
                var foundRecord = GetPovertyRate(applicant.ZipCode);

                if (foundRecord == null)
                {
                    HandleBadZip(applicant.ZipCode);
                }

                else
                {
                    double povertyrate;

                    if (!double.TryParse(foundRecord, out povertyrate))
                    {
                        throw new InvalidDataException("Can't parse poverty rate to double.  Poverty rate provided was " + foundRecord);
                    
                }
                applicant.PovertyRate = povertyrate;
                Console.WriteLine($"The poverty rate in your zip code is: {foundRecord}");
                
                }

            Console.WriteLine("If the poverty rate in your zipcode is greater than 27.7%, you are eligible for assistance");

            return applicants;
            
        }

        private static void HandleBadZip(string zipCode)
        {
            Console.WriteLine("HandleBadZip needs to be implemented");
        }

        private static string GetPovertyRate(string Zip)
        {
            string returnValue;
            returnValue = PovertyRates.FirstOrDefault(x => x.LouMsaZip == Zip)?.PovertyRate; //find the first value or show the default value. ?. is null-conditional--show poverty rate if not null.
            return returnValue;
        }

        //query PovertyRateByZip csv file using FileHelpers nuget package. Shortens code needed to read the file.  Don't need foreach statement to iterate through file;
        private static PovertyData[] GetPovertyData()
        {
            //PovertyData[] results = new PovertyData[];
            //try
            //{

                var engine = new FileHelperEngine<PovertyData>();
                var results = engine.ReadFile("PovertyRateByZip.csv");

                return results;

            //}
            //catch (Exception error)
            //    {
            //        Console.WriteLine(error.Message);
            //        Console.ReadKey();
            //    }

            
        }
        public static void RetrieveApplicantInfo() 
        {
            Applicant applicant = new Applicant();
            List<Applicant> returnValue = new List<Applicant>();
            List<Applicant> _applicantList;
            Console.Clear();
            Console.Write("Please enter your ID number:\t");         
            var result = Console.ReadLine();
            applicant.ApplicantId = Convert.ToInt32(result);
            _applicantList = GetApplicantsFromFile();

            
            var appToShow = _applicantList.SingleOrDefault(a => a.ApplicantId == applicant.ApplicantId);
            if (appToShow != null)
            {
                
                Console.Write($"\nApplicant ID: {appToShow.ApplicantId} \nApplicant's first name: {appToShow.FirstName} \nApplicant's last name: {appToShow.LastName} \nApplicant's zip code:  {appToShow.ZipCode} \nPoverty rate of applicant's zip code: {appToShow.PovertyRate} ");
                Console.WriteLine();
                Console.WriteLine("\nHit enter to return to Main Menu");
                Console.ReadKey();
            }
            else
            {

                Console.Write($"ERROR: Could not find player with ID: {applicant.ApplicantId}.");
                System.Threading.Thread.Sleep(1000);
            }

        }

    private static List<Applicant> GetApplicantsFromFile()
        {
            List<Applicant> returnValue = new List<Applicant>();

            try
            {

                if (File.Exists(_DataFile))
                {
                    string jsonData = File.ReadAllText(_DataFile);

                    if (!String.IsNullOrEmpty(jsonData))
                    {
                        //deserialize the file back into a list
                        returnValue = JsonConvert.DeserializeObject<List<Applicant>>(jsonData);
                    }
                }
                else
                {

                    throw new Exception($"GetAllError: Unable to find file: {_DataFile}");
                }

            }
            catch (Exception ex)
            {
                ex.Data.Add("GetAllError",
                    $"An error occurred while trying to get players.");
                throw;
            }

            return returnValue;
        }
    

        public static void SaveApplicant(Applicant applicant, List<Applicant> applicants)
        {
            Console.Write("\nDo you want to save your information, Yes or No?  ");
            var choice = Console.ReadLine();

            if (choice.ToLower() == "yes")
            {
                int newId = GetId(applicants);
                applicant.ApplicantId = newId;
                applicants.Add(applicant);
                Console.WriteLine($"Your Id number is:  {newId}.  Save this number to retrieve your information.");
                System.Threading.Thread.Sleep(4000);
                try
                {

                    string jsonData = JsonConvert.SerializeObject(applicants);


                    if (!string.IsNullOrEmpty(jsonData))
                    {

                        File.WriteAllText(_DataFile, jsonData);
     
                        Console.WriteLine("Your information has been saved");

                    }
                }
                catch (Exception ex)
                {
                    ex.Data.Add("SaveError", $"An error occurred while trying to save list.");
                    throw;
                }

            }
            else if (choice.ToLower() == "no")
            {
                Console.WriteLine("Goode bye.");
                System.Threading.Thread.Sleep(1000);
            }
        }

        public static int GetId(List<Applicant> applicants)
        {
            int returnValue = 1;

            try
            {
                if (applicants.Any())
                {
                    var applicant = applicants.OrderByDescending(a => a.ApplicantId).FirstOrDefault();
                    int id = applicant.ApplicantId;
                    id++;
                    returnValue = id;
                }

            }

            catch (Exception ex)
            {
                ex.Data.Add("GetId Error", "An error occurred while trying to get your Id number.");
                throw;
            }

            return returnValue;
        }

    }
}
