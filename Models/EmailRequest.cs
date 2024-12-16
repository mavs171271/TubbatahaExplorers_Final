using System.ComponentModel.DataAnnotations;

namespace VisualStudio.Models
{
    public class EmailRequest
    {
        public string currentEmail {  get; set; }
        public string newEmail { get; set; }
        public string userId { get; set; }
        [Key]
        public int requestId { get; set; }
    }
}
