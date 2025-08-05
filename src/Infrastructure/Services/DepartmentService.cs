using Application.Interfaces;
using Core.Entities;

namespace Infrastructure.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repo;
    private readonly IEmployeeRepository _empRepo;

    public DepartmentService(IDepartmentRepository repo, IEmployeeRepository employeeRepository) {
        _repo = repo;
        _empRepo = employeeRepository;
    }

    public async Task<IEnumerable<Department>> GetAllAsync() {
        return await _repo.GetAllAsync();
    }

    public async Task<Department?> GetByIdAsync(int id) {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<Department> CreateAsync(Department department) {
        return await _repo.CreateAsync(department);
    }

    public async Task UpdateAsync(Department department) {
        await _repo.UpdateAsync(department);
    }

    public async Task DeleteAsync(int id) {
        await _repo.DeleteAsync(id);
    }

    public async Task<bool> HasEmployeesAsync(int departmentId) {
        return await _empRepo.ExistByDepartmentIdAsync(departmentId);
    }
}
