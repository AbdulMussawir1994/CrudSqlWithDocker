using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudMySql.Models
{
    [Table("Department")] // ✅ Remove 'Schema = "public"' — not supported in MySQL

    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DepName { get; set; } = string.Empty;

        public virtual ICollection<Employee> Employees { get; private set; } = new List<Employee>();
    }
}
