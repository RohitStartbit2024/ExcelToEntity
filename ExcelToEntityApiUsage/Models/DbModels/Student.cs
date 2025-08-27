using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelToEntityApiUsage.Models.DbModels
{
    public class Student
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [MaxLength(100)]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(100)]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(200)]
            public string Email { get; set; }

            [Required]
            [Phone]
            [MaxLength(15)]
            public string Number { get; set; }

            [MaxLength(10)]
            public string BloodGroup { get; set; }

            [Required]
            public int Age { get; set; }

    }
}
