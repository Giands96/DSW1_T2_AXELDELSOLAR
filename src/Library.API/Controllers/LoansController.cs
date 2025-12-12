using Microsoft.AspNetCore.Mvc;
using Library.Application.DTOs.Loan;
using Library.Application.Interfaces;
using Library.Domain.Exceptions;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        // GET: api/loans/active (Requisito 4.2.5: Listar préstamos activos)
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetActiveLoans()
        {
            var loans = await _loanService.GetActiveLoansAsync();
            return Ok(loans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetById(int id)
        {
            var loan = await _loanService.GetByIdAsync(id);
            if (loan == null)
            {
                return NotFound(new { message = $"Préstamo con ID {id} no encontrado." });
            }
            return Ok(loan);
        }

        // POST: api/loans (Requisito 4.2.6: Registrar nuevo préstamo)
        [HttpPost]
        public async Task<ActionResult<LoanDto>> Create([FromBody] CreateLoanDto dto)
        {
            try
            {
                var loan = await _loanService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // Libro no existe
            }
            catch (BusinessRuleException ex)
            {
                // 400 Bad Request si el Stock es 0
                return BadRequest(new { message = ex.Message }); 
            }
        }

        // PATCH: api/loans/{id}/return (Requisito 4.2.7: Botón para devolver un préstamo)
        [HttpPatch("{id}/return")]
        public async Task<ActionResult<LoanDto>> ReturnLoan(int id)
        {
            try
            {
                var loan = await _loanService.ReturnLoanAsync(id);
                return Ok(loan);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // Préstamo no encontrado o ya devuelto
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
        }
    }
}