using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIT.Models
{
    public class Suspect
    {
        public string Id { get; set; }
         public string Name { get; set; }
         public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Relation { get; set; }
        public string Date { get; set; }
        public string Notice { get; set; }
        public string CaseId { get; set; }

    }
}