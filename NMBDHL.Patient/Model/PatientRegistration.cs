
using System;
using System.ComponentModel.DataAnnotations;

namespace NMBDHL.Patient.Model
{
    public class PatientRegistration
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(100)]
        public string Surname { get; set; } = string.Empty;

        [StringLength(20)]
        public string? IDNumber { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Enter a valid contact number.")]
        [StringLength(30)]
        public string? Cellphone { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Street address is required.")]
        [StringLength(200)]
        public string StreetAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Suburb is required.")]
        [StringLength(100)]
        public string Suburb { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Province is required.")]
        [StringLength(100)]
        public string Province { get; set; } = string.Empty;

        [StringLength(10)]
        public string? PostalCode { get; set; }
    }
}
