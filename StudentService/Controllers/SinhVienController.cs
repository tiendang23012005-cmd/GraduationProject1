using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using StudentService.Data;
using StudentService.Models;

namespace StudentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SinhVienController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<SinhVienController> _logger;
    private readonly IConfiguration _configuration;

    public SinhVienController(
        AppDbContext db,
        ILogger<SinhVienController> logger,
        IConfiguration configuration)
    {
        _db = db;
        _logger = logger;
        _configuration = configuration;
    }

    // =========================
    // GET ALL
    // =========================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Lấy danh sách sinh viên");

        var data = await _db.SinhViens.ToListAsync();

        return Ok(data);
    }


    // =========================
    // GET BY ID
    // =========================

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sv = await _db.SinhViens.FindAsync(id);

        if (sv == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sinh viên"
            });
        }

        return Ok(sv);
    }


    // =========================
    // CREATE
    // =========================

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(SinhVien sv)
    {
        var exists = await _db.SinhViens
            .AnyAsync(x => x.MaSV == sv.MaSV);

        if (exists)
        {
            return BadRequest(new
            {
                message = "Mã sinh viên đã tồn tại"
            });
        }

        _db.SinhViens.Add(sv);

        await _db.SaveChangesAsync();

        _logger.LogInformation(
            "Đã thêm sinh viên {MaSV}",
            sv.MaSV);

        return Ok(sv);
    }


    // =========================
    // UPDATE
    // =========================

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        SinhVien sv)
    {
        var old = await _db.SinhViens.FindAsync(id);

        if (old == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sinh viên"
            });
        }

        old.HoTen = sv.HoTen;
        old.Lop = sv.Lop;
        old.Email = sv.Email;
        old.MatKhau = sv.MatKhau;
        old.Quyen = sv.Quyen;

        await _db.SaveChangesAsync();

        return Ok(old);
    }


    // =========================
    // DELETE
    // =========================

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var sv = await _db.SinhViens.FindAsync(id);

        if (sv == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sinh viên"
            });
        }

        _db.SinhViens.Remove(sv);

        await _db.SaveChangesAsync();

        _logger.LogInformation(
            "Đã xóa sinh viên {MaSV}",
            id);

        return Ok(new
        {
            message = "Xóa thành công"
        });
    }


    // =========================
    // LOGIN
    // =========================

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(SinhVien login)
    {
        var email = login.Email?.Trim();
        var matKhau = login.MatKhau?.Trim();

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(matKhau))
        {
            return BadRequest(new
            {
                message = "Email và mật khẩu không được để trống"
            });
        }

        var sv = await _db.SinhViens
            .FirstOrDefaultAsync(x =>
                x.Email == email &&
                x.MatKhau == matKhau);

        if (sv == null)
        {
            return Unauthorized(new
            {
                message = "Sai email hoặc mật khẩu"
            });
        }

        var claims = new[]
        {
        new Claim(
            ClaimTypes.NameIdentifier,
            sv.MaSV.ToString()),

        new Claim(
            ClaimTypes.Name,
            sv.HoTen),

        new Claim(
            ClaimTypes.Email,
            sv.Email),

        new Claim(
            ClaimTypes.Role,
            sv.Quyen)
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                "GraduationProject1_SuperSecret_Key_2026_123456789"));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "GraduationProject1",
            audience: "GraduationClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Ok(new
        {
            token = jwt,
            maSV = sv.MaSV,
            hoTen = sv.HoTen,
            email = sv.Email,
            quyen = sv.Quyen
        });
    }
}