using System.ComponentModel.DataAnnotations;

namespace MashZavod.Models
{
    public class AccountLoginModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}