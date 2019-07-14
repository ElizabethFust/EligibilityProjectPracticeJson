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
    [DelimitedRecord(",")]
    public class PovertyData
    {
        public string LouMsaZip { get; set; }
        public string PovertyRate { get; set; }
    }
}
