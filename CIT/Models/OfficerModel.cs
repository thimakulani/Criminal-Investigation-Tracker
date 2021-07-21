using Firebase.Firestore;
using Plugin.CloudFirestore.Attributes;

namespace CIT.Models
{
    public class OfficerModel
    {
        [Id]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ImageUrl { get; set; }

    }
}