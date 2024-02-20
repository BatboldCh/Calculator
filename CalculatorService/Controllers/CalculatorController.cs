using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CalculatorController : Controller
    {
        private readonly IAuditLogService _auditLogService;

        public CalculatorController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet("add/{num1}/{num2}")]
        public IActionResult Add([Required][Range(double.MinValue, double.MaxValue)] double num1, [Required][Range(double.MinValue, double.MaxValue)] double num2)
        {
            return PerformOperation(num1, num2, (x, y) => x + y).Result;
        }

        [HttpGet("subtract/{num1}/{num2}")]
        public IActionResult Subtract([Required][Range(double.MinValue, double.MaxValue)] double num1, [Required][Range(double.MinValue, double.MaxValue)] double num2)
        {
            return PerformOperation(num1, num2, (x, y) => x - y).Result;
        }

        [HttpGet("multiply/{num1}/{num2}")]
        public IActionResult Multiply([Required][Range(double.MinValue, double.MaxValue)] double num1, [Required][Range(double.MinValue, double.MaxValue)] double num2)
        {
            return PerformOperation(num1, num2, (x, y) => x * y).Result;
        }

        [HttpGet("divide/{num1}/{num2}")]
        public IActionResult Divide([Required][Range(double.MinValue, double.MaxValue)] double num1, [Required][Range(double.MinValue, double.MaxValue)] double num2)
        {
            if (num2 == 0)
            {
                return BadRequest();
            }
            return PerformOperation(num1, num2, (x, y) => x / y).Result;
        }

        private async Task<IActionResult> PerformOperation(double num1, double num2, Func<double, double, object> operation)
        {
            try
            {
                await _auditLogService.LogAuditEventAsync("Performed calculator action.");

                return Ok(operation(num1, num2));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}