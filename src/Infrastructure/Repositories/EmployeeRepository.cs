using Application.Interfaces;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<Employee> CreateAsync(Employee employee) {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task DeleteAsync(int id) {
        var existing = await _context.Employees.FindAsync(id);
        if (existing is null) return;

        _context.Employees.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Employee>> GetAllAsync() {
        return await _context.Employees
                             .Include(e => e.Department)
                             .ToListAsync();
    }
    public async Task<bool> ExistByDepartmentIdAsync(int departmentId) {
        return await _context.Employees
                             .AnyAsync(e => e.DepartmentId == departmentId);
    }

    public async Task<Employee?> GetByIdAsync(int id) {
        return await _context.Employees.FindAsync(id);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(int pageNumber, int pageSize, string? searchTerm, int? departmentId, string? sortBy, bool sortDescending) {
        var query = _context.Employees
                            .Include(e => e.Department)
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm)) {
            query = query.Where(x =>
                            x.FirstName.Contains(searchTerm) ||
                            x.LastName.Contains(searchTerm));
        }

        if (departmentId.HasValue) {
            query = query.Where(x => x.DepartmentId == departmentId);
        }

        if (!string.IsNullOrWhiteSpace(sortBy)) {
            query = sortDescending
                ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                : query.OrderBy(e => EF.Property<object>(e, sortBy));
        }

        query = query.Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task UpdateAsync(Employee employee) {
        var existing = await _context.Employees.FindAsync(employee.Id);
        if (existing is null) return;

        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.HireDate = employee.HireDate;
        existing.Salary = employee.Salary;
        existing.DepartmentId = employee.DepartmentId;

        await _context.SaveChangesAsync();
    }

}
