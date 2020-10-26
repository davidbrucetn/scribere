using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Scribere.Models
{
    public class UserData
    {
        public int Id { get; set; }

        [StringLength(28, MinimumLength = 28)]
        public string FirebaseUserId { get; set; }

        [Required]
        [MaxLength(40)]
        public string NameFirst { get; set; }

        [Required]
        [MaxLength(40)]
        public string NameLast { get; set; }

        [MaxLength(100)]
        public string Pseudonym { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(350)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string State { get; set; }

        public DateTime CreateDate { get; set; }

        public UserImage UserImage { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        [Required]
        public int UserLevelId { get; set; }
        public UserLevel UserLevel { get; set; }
        public int IsActive { get; set; }

        public int AllowMessaging { get; set; }

        public string FullName
        {
            get
            {
                return $"{NameFirst} {NameLast}";
            }
        }
        public bool Error { get; set; }

    }
}
