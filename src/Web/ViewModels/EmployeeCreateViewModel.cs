using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels;
    public class EmployeeCreateViewModel : IValidatableObject
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Hire Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.Today;

    [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than zero")]
        public decimal Salary { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext ctx) {
            if (HireDate.Date > DateTime.UtcNow.Date) {
                yield return new ValidationResult(
                    "Hire date cannot be in the future",
                    new[] { nameof(HireDate) });
            }
        }
    }
