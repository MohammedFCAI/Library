using LibrarySystem.Data;
using LibrarySystem.Dtos;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BorrowRequestsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: api/borrowrequests
        [HttpPost]
        public async Task<ActionResult<BorrowRequest>> CreateBorrowRequest(UserBorrowRequestDto userBorrowRequestDto)
        {
            var request = new BorrowRequest
            {
                BookId = userBorrowRequestDto.BookId,
                UserId = userBorrowRequestDto.UserId,
                BorrowDate = userBorrowRequestDto.BorrowDate,
                ReturnDate = userBorrowRequestDto.ReturnDate,
                Status = "Pending"
            };
            _dbContext.BorrowRequests.Add(request);
            await _dbContext.SaveChangesAsync();
            return Ok(request);
        }

        // GET: api/borrowrequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowRequest>>> GetBorrowRequests()
        {
            var requests = await _dbContext.BorrowRequests.ToListAsync();
            return Ok(requests);
        }

        // PUT: api/borrowrequests/{requestId}
        [HttpPut("{requestId}")]
        public async Task<IActionResult> UpdateBorrowRequestStatus(int requestId, string status)
        {
            var request = await _dbContext.BorrowRequests.FindAsync(requestId);
            if (request == null)
                return NotFound();

            request.Status = status;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
