using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.ViewModels;

namespace Web.Controllers;

    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public DashboardController(
            IDashboardService dashboardService,
            IEmployeeService employeeService,
            IDepartmentService departmentService) {
            _dashboardService = dashboardService;
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, int? departmentId) {
            // 1) Build the VM with the three core stats
            var vm = new DashboardViewModel {
                TotalEmployees = await _dashboardService.GetTotalEmployeeCountAsync(),
                EmployeeCountByDepartment = await _dashboardService.GetEmployeeCountByDepartmentAsync(),
                RecentHires = await _dashboardService.GetRecentHiresAsync(),

                // 2) Pass along filter inputs
                SearchTerm = searchTerm,
                DepartmentId = departmentId,

                // 3) Dropdown needs all departments
                Departments = await _departmentService.GetAllAsync(),
            };

            // 4) Fetch a filtered list of employees for the dashboard
            vm.FilteredEmployees = await _employeeService.GetEmployeesAsync(
                pageNumber: 1,
                pageSize: int.MaxValue,   // Or some reasonable cap like 100
                searchTerm: searchTerm,
                departmentId: departmentId,
                sortBy: null,
                sortDescending: false
            );

            return View(vm);
        }
}
