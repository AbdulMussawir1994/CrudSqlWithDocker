using CrudMySql.DTOs;
using CrudMySql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CrudMySql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeLayer _employeeLayer;

        public EmployeesController(IEmployeeLayer employeeLayer)
        {
            _employeeLayer = employeeLayer;
        }

        [HttpGet]
        [Route("EmployeesList")]
        public async Task<ActionResult> GetEmployeesList(CancellationToken ctx)
        {
            var response = await _employeeLayer.GetEmployeesListAsync(ctx);

            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<ActionResult> CreateEmployee([FromBody] CreateEmployeeDto model, CancellationToken ctx)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            var response = await _employeeLayer.CreateEmployeeAsync(model, ctx);

            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Route("UpdateEmployee/{Id}")]
        public async Task<ActionResult> UpdateEmployee(string Id, [FromBody] CreateEmployeeDto model, CancellationToken ctx)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            var response = await _employeeLayer.UpdateEmployeeAsync(Id, model, ctx);

            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        [Route("GetEmployeeById/{Id}")]
        public async Task<ActionResult> GetEmployeeById(string Id, CancellationToken ctx)
        {
            if (Id is null)
            {
                BadRequest($"{nameof(Id)} is null");
            }

            var response = await _employeeLayer.GetEmployeeByIdAsync(Id, ctx);

            return response.Status ? Ok(response) : NotFound(response);
        }

        [HttpDelete]
        [Route("DeleteEmployee/{Id}")]
        public async Task<ActionResult> DeleteEmployee(string Id, CancellationToken ctx)
        {
            if (Id is null)
            {
                BadRequest($"{nameof(Id)} is null");
            }

            var response = await _employeeLayer.DeleteEmployeeAsync(Id, ctx);

            return response.Status ? Ok(response) : NotFound(response);
        }
    }
}
