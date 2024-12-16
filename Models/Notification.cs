using System.ComponentModel.DataAnnotations;

namespace VisualStudio.Models
{
    public class Notification
    {
        public string message { get; set; }
        public string Nid { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
