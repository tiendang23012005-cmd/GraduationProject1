using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationService.Data;
using RegistrationService.Models;

namespace RegistrationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DangKyController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DangKyController> _logger;

    public DangKyController(
        AppDbContext db,
        IHttpClientFactory factory,
        IConfiguration configuration,
        ILogger<DangKyController> logger)
    {
        _db = db;
        _factory = factory;
        _configuration = configuration;
        _logger = logger;
    }


    // =========================================
    // GET ALL
    // =========================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var data = await _db.DangKys
                .AsNoTracking()
                .OrderByDescending(x => x.MaDK)
                .ToListAsync();

            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Lỗi khi lấy danh sách đăng ký");

            return StatusCode(500, new
            {
                message = "Lỗi khi lấy danh sách đăng ký",
                error = ex.Message
            });
        }
    }


    // =========================================
    // GET BY ID
    // =========================================

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dk = await _db.DangKys.FindAsync(id);

        if (dk == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đăng ký"
            });
        }

        return Ok(dk);
    }


    // =========================================
    // REGISTER
    // =========================================

    [HttpPost]
    public async Task<IActionResult> Register(DangKy dk)
    {
        try
        {
            // -----------------------------
            // Kiểm tra sinh viên
            // -----------------------------

            var client = _factory.CreateClient();

            var studentUrl =
                $"{_configuration["Services:StudentService"]}" +
                $"/api/SinhVien/{dk.MaSV}";

            var studentResponse =
                await client.GetAsync(studentUrl);

            if (!studentResponse.IsSuccessStatusCode)
            {
                return BadRequest(new
                {
                    message = "Sinh viên không tồn tại"
                });
            }


            // -----------------------------
            // Kiểm tra đề tài
            // -----------------------------

            var thesisUrl =
                $"{_configuration["Services:ThesisService"]}" +
                $"/api/DeTai/{dk.MaDT}";

            var thesisResponse =
                await client.GetAsync(thesisUrl);

            if (!thesisResponse.IsSuccessStatusCode)
            {
                return BadRequest(new
                {
                    message = "Đề tài không tồn tại"
                });
            }


            // -----------------------------
            // Kiểm tra đăng ký trùng
            // -----------------------------

            var exists = await _db.DangKys
                .AnyAsync(x =>
                    x.MaSV == dk.MaSV &&
                    x.MaDT == dk.MaDT);

            if (exists)
            {
                return BadRequest(new
                {
                    message =
                        "Sinh viên đã đăng ký đề tài này"
                });
            }


            // -----------------------------
            // Tạo đăng ký
            // -----------------------------

            dk.NgayDangKy = DateTime.Now;
            dk.Diem = 0;

            _db.DangKys.Add(dk);

            await _db.SaveChangesAsync();


            _logger.LogInformation(
                "Sinh viên {MaSV} đăng ký đề tài {MaDT}",
                dk.MaSV,
                dk.MaDT);


            return Ok(dk);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Lỗi đăng ký đề tài");

            return StatusCode(500, new
            {
                message = "Lỗi đăng ký đề tài",
                error = ex.Message
            });
        }
    }


    // =========================================
    // DELETE
    // =========================================

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dk = await _db.DangKys.FindAsync(id);

        if (dk == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đăng ký"
            });
        }

        _db.DangKys.Remove(dk);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Hủy đăng ký thành công"
        });
    }


    // =========================================
    // UPDATE SCORE
    // =========================================

    [HttpPut("{id}/diem")]
    public async Task<IActionResult> UpdateDiem(
        int id,
        [FromQuery] decimal diem)
    {
        if (diem < 0 || diem > 10)
        {
            return BadRequest(new
            {
                message = "Điểm phải từ 0 đến 10"
            });
        }

        var dk = await _db.DangKys.FindAsync(id);

        if (dk == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đăng ký"
            });
        }

        dk.Diem = diem;

        await _db.SaveChangesAsync();

        return Ok(dk);
    }
}