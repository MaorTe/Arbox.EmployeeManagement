using Core.Entities;

namespace Application.Interfaces;
public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
    Task<bool> ExistByDepartmentIdAsync(int departmentId);
    Task<IEnumerable<Employee>> GetEmployeesAsync(
    int pageNumber,
    int pageSize,
    string? searchTerm,
    int? departmentId,
    string? sortBy,
    bool sortDescending);
}
