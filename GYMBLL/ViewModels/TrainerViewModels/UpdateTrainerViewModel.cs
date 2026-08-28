using GYMDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMBLL.ViewModels.TrainerViewModels
{
    public class UpdateTrainerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$", ErrorMessage = "Invalid Phone Number")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building Number is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number must be a positive integer.")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street is Required")]
        [RegularExpression(@"^[0-9A-Za-z\s.,#\-\/]{5,100}$", ErrorMessage = "Please enter a valid street address (letters, numbers, and common symbols only).")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 100 characters.")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "City must be between 2 and 50 characters.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Specialization Record is Required")]
        public Specialities Specialization { get; set; }
    }
}
