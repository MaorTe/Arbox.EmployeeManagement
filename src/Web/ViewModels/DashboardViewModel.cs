using Core.Entities;

namespace Web.ViewModels;

public class DashboardViewModel
{
    public string? SearchTerm { get; set; }
    public int? DepartmentId { get; set; }
    public int TotalEmployees { get; set; }

    // maps departmentId → employee count
    public Dictionary<int, int> EmployeeCountByDepartment { get; set; } = new();

    // list of employees hired in the last 30 days
    public IEnumerable<Employee> RecentHires { get; set; } = new List<Employee>();

    // Searchbox filter
    public IEnumerable<Department> Departments { get; set; } = new List<Department>();
    public IEnumerable<Employee> FilteredEmployees { get; set; } = new List<Employee>();
}
