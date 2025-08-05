using Application.Interfaces;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;

    public EmployeeService(IEmployeeRepository repo) {
        _repo = repo;
    }

    public async Task<Employee> CreateAsync(Employee employee) {
        return await _repo.CreateAsync(employee);
    }

    public async Task DeleteAsync(int id) {
        await _repo.DeleteAsync(id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync() {
        return await _repo.GetAllAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id) {
        return await _repo.GetByIdAsync(id);
    }

    public async Task UpdateAsync(Employee employee) {
        await _repo.UpdateAsync(employee);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(int pageNumber, int pageSize, string? searchTerm, int? departmentId, string? sortBy, bool sortDescending) {
        return await _repo.GetEmployeesAsync(pageNumber, pageSize, searchTerm, departmentId, sortBy, sortDescending);
    }
}
