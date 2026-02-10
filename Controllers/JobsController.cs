using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using MyRazorApp.Data;
using MyRazorApp.Models;

namespace MyRazorApp.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public JobsController(AppDbContext db)
        {
            _db = db;
        }

        // ================= GET ALL JOBS =================
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobs()
        {
            var jobs = await _db.Jobs
                .Include(j => j.Author)
                .OrderByDescending(j => j.PubDate)
                .ToListAsync();

            return Ok(jobs);
        }

        // ================= CREATE JOB =================
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] JobDto dto)
        {
            if (dto == null)
                return BadRequest("Job data is null");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("UserId claim missing");

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid user id in token");

            var job = new Job
            {
                JobTitle = dto.JobTitle,
                JobSlug = dto.JobSlug,
                CompanyName = dto.CompanyName,
                CompanyLogo = dto.CompanyLogo,
                JobIndustry = JsonSerializer.Serialize(dto.JobIndustry ?? new string[] { }),
                JobType = JsonSerializer.Serialize(dto.JobType ?? new string[] { }),
                JobGeo = dto.JobGeo,
                JobLevel = dto.JobLevel,
                JobExcerpt = dto.JobExcerpt,
                JobDescription = dto.JobDescription,
                PubDate = dto.PubDate,
                Url = dto.Url,
                AuthorId = userId
            };

            Console.WriteLine("Job Details are:{job}");
            _db.Jobs.Add(job);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // log the exact error for debugging
                return StatusCode(500, $"Server error: {ex.Message}");
            }

            return Ok(job);
        }

        // ================= UPDATE JOB =================
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [FromBody] JobDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var job = await _db.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            if (job.AuthorId != userId)
                return Forbid();

            job.JobTitle = dto.JobTitle;
            job.CompanyName = dto.CompanyName;
            job.CompanyLogo = dto.CompanyLogo;
            job.JobIndustry = JsonSerializer.Serialize(dto.JobIndustry);
            job.JobType = JsonSerializer.Serialize(dto.JobType);
            job.JobGeo = dto.JobGeo;
            job.JobLevel = dto.JobLevel;
            job.JobExcerpt = dto.JobExcerpt;
            job.JobDescription = dto.JobDescription;
            job.PubDate = dto.PubDate;
            job.Url = dto.Url;

            await _db.SaveChangesAsync();

            return Ok(job);
        }

        // ================= DELETE JOB =================
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var job = await _db.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            if (job.AuthorId != userId)
                return Forbid();

            _db.Jobs.Remove(job);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Job deleted" });
        }
    }
}
