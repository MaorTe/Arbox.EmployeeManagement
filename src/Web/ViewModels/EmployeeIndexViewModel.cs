namespace Web.ViewModels;

public class EmployeeIndexViewModel
{
    public IEnumerable<Core.Entities.Employee> Employees { get; set; } = new List<Core.Entities.Employee>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SearchTerm { get; set; }
    public int? DepartmentId { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
}

