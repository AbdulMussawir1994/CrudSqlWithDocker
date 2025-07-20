using System.ComponentModel.DataAnnotations;

namespace CrudMySql.DTOs
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Employee Name is Required.")]
        [Display(Name = "EmployeeName")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "EmailAddress")]
        public string Email { get; set; } = string.Empty;
        public decimal Salary { get; set; } = 0;
        public int DepartmentId { get; set; }
    }
}
