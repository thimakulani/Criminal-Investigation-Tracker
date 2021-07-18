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
    public class Case
    {
        public string Id { get; set; }
        public string CaseName { get; set; }
        public string OfficerId { get; set; }
        public string OfficerName { get; set; }
        public string Note { get; set; } 
        public string Evidance { get; set; }
        public string Status { get; set; }
    }
}