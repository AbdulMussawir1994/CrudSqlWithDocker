using CrudMySql.DataContext;
using CrudMySql.DTOs;
using CrudMySql.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CrudMySql.Repositories
{
    public class EmployeeLayer : IEmployeeLayer
    {
        private readonly DataDbContext _db;

        public EmployeeLayer(DataDbContext db)
        {
            _db = db;
        }
        public async Task<MobileResponse<IEnumerable<GetEmployeeDto>>> GetEmployeesListAsync(CancellationToken ctx)
        {
            try
            {
                var employees = await _db.Employees.AsNoTracking().AsSplitQuery().ToListAsync(ctx);

                if (!employees.Any())
                {
                    return MobileResponse<IEnumerable<GetEmployeeDto>>.EmptySuccess(Enumerable.Empty<GetEmployeeDto>(), "No Employees Found.");
                }

                var empList = employees.Adapt<IEnumerable<GetEmployeeDto>>();
                return MobileResponse<IEnumerable<GetEmployeeDto>>.Success(empList, "Employees Fetched Successfully");
            }
            catch (Exception ex)
            {
                return MobileResponse<IEnumerable<GetEmployeeDto>>.Fail($"An error occured: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetEmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, CancellationToken ctx)
        {
            try
            {
                var employee = model.Adapt<Employee>();

                await _db.Employees.AddAsync(employee, ctx);
                int result = await _db.SaveChangesAsync(ctx);

                return result > 0
                    ? MobileResponse<GetEmployeeDto>.Success(employee.Adapt<GetEmployeeDto>(), "Employee Created Successfully")
                    : MobileResponse<GetEmployeeDto>.Fail("Employee Failed to Create");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetEmployeeDto>.Fail($"An Error Occured: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetEmployeeDto>> UpdateEmployeeAsync(string id, CreateEmployeeDto model, CancellationToken ctx)
        {
            try
            {
                // Get the employee entity with tracking
                var employee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id, ctx);
                if (employee is null)
                {
                    return MobileResponse<GetEmployeeDto>.Fail("Employee not found");
                }

                // Update properties
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DepartmentId = model.DepartmentId;

                // Save changes
                _db.Employees.Update(employee);
                var result = await _db.SaveChangesAsync(ctx).ConfigureAwait(false);

                // Return response
                return result > 0
                    ? MobileResponse<GetEmployeeDto>.Success(employee.Adapt<GetEmployeeDto>(), "Employee updated successfully")
                    : MobileResponse<GetEmployeeDto>.Fail("Failed to update employee");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetEmployeeDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        public async Task<MobileResponse<GetEmployeeDto>> GetEmployeeByIdAsync(string id, CancellationToken ctx)
        {
            try
            {
                var employee = await _db.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ctx);

                if (employee is null)
                {
                    return MobileResponse<GetEmployeeDto>.Fail("Employee Not Found");
                }

                return MobileResponse<GetEmployeeDto>.Success(employee.Adapt<GetEmployeeDto>(), "Employee Fetched Successfully");
            }
            catch (Exception ex)
            {
                return MobileResponse<GetEmployeeDto>.Fail($"An error occured: {ex.Message}");
            }
        }

        public async Task<MobileResponse<bool>> DeleteEmployeeAsync(string id, CancellationToken ctx)
        {
            try
            {
                var employee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id, ctx);

                if (employee is null)
                {
                    return MobileResponse<bool>.Fail("Employee Not Found");
                }

                _db.Employees.Remove(employee);
                int result = await _db.SaveChangesAsync(ctx);

                return result > 0
                    ? MobileResponse<bool>.EmptySuccess(true, "Employee Deleted Successfully")
                    : MobileResponse<bool>.Fail("Failed to Delete Employee");


            }
            catch (Exception ex)
            {
                return MobileResponse<bool>.Fail($"An error occurred: {ex.Message}");
            }
        }
    }
}
