using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModels;

namespace Web.Controllers;
public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService) {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, int? departmentId = null, string? sortBy = null, bool sortDescending = false) {
        var employeesList = await _employeeService.GetEmployeesAsync(pageNumber, pageSize, searchTerm, departmentId, sortBy, sortDescending);
        
        var vm = new EmployeeIndexViewModel {
            Employees = employeesList,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            DepartmentId = departmentId,
            SortBy = sortBy,
            SortDescending = sortDescending
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Create() {
        ViewData["Departments"] = new SelectList(
            await _departmentService.GetAllAsync(), "Id", "Name");
        var vm = new EmployeeCreateViewModel {
            HireDate = DateTime.Today
        };

        return View(vm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeCreateViewModel vm) {
        if (!ModelState.IsValid) {
            ViewData["Departments"] = new SelectList(
                await _departmentService.GetAllAsync(), "Id", "Name");
            return View(vm);
        }

        var emp = new Employee {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Email = vm.Email,
            HireDate = vm.HireDate,
            Salary = vm.Salary,
            DepartmentId = vm.DepartmentId
        };

        await _employeeService.CreateAsync(emp);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        var e = await _employeeService.GetByIdAsync(id);
        if (e == null) return NotFound();

        ViewData["Departments"] = new SelectList(
            await _departmentService.GetAllAsync(), "Id", "Name", e.DepartmentId);

        var vm = new EmployeeEditViewModel {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            HireDate = e.HireDate,
            Salary = e.Salary,
            DepartmentId = e.DepartmentId
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeEditViewModel vm) {
        if (!ModelState.IsValid) {
            ViewData["Departments"] = new SelectList(
                await _departmentService.GetAllAsync(), "Id", "Name", vm.DepartmentId);
            return View(vm);
        }

        var emp = await _employeeService.GetByIdAsync(vm.Id);
        if (emp == null) {
            return NotFound();
        }

        emp.FirstName = vm.FirstName;
        emp.LastName = vm.LastName;
        emp.Email = vm.Email;
        emp.HireDate = vm.HireDate;
        emp.Salary = vm.Salary;
        emp.DepartmentId = vm.DepartmentId;

        await _employeeService.UpdateAsync(emp);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id) {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null)
            return NotFound();

        await _employeeService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
