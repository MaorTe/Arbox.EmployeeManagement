using System.ComponentModel.DataAnnotations;

namespace Core.Entities;
public class Employee
{
    public int Id { get; set; } // PK

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Date)]
    public DateTime HireDate { get; set; }

    [Required, Range(0.01, double.MaxValue, ErrorMessage = "Salary must be > 0")]
    public decimal Salary { get; set; }

    // Foreign Key to Department  
    [Required]
    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;
}