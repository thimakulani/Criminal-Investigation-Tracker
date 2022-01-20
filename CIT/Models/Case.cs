using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CIT.Models
{
    public class Case
    {
        [Id]
        public string Id { get; set; }
        public string CaseName { get; set; }
        public string OfficerId { get; set; }
        public string OfficerName { get; set; }
        public string Note { get; set; } 
        public string Evidance { get; set; }
        public string Status { get; set; }
        [ServerTimestamp]
        public Timestamp DateCreated { get; set; }
        [ServerTimestamp]
        public Timestamp LastUpdate { get; set; }
        public string CaseNo { get; set; }
        public string VictimNames { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}