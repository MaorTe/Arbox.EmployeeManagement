using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Department
{
    public int Id { get; set; }  // PK
    public string Name { get; set; } = string.Empty;

    // Navigation property — list of employees in this department
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
