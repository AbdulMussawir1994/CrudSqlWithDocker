using CrudMySql.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//[Table("Employee")] // ✅ Remove 'Schema = "public"' — not supported in MySQL
public class Employee
{
    [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = Guid.NewGuid().ToString(); // ⚠️ Consider using `Guid` directly or switch to `int` if MySQL auto-increment is preferred

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Email { get; set; }

    public decimal? Salary { get; set; }

    public bool isActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int DepartmentId { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public virtual Department IDepartment { get; set; }
}
