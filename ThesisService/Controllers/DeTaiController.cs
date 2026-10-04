using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ThesisService.Data;
using ThesisService.Models;

namespace ThesisService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeTaiController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<DeTaiController> _logger;

    public DeTaiController(
        AppDbContext db,
        ILogger<DeTaiController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(
            await _db.DeTais.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dt = await _db.DeTais.FindAsync(id);

        if (dt == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đề tài"
            });
        }

        return Ok(dt);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(DeTai dt)
    {
        var exists = await _db.DeTais
            .AnyAsync(x => x.MaDT == dt.MaDT);

        if (exists)
        {
            return BadRequest(new
            {
                message = "Mã đề tài đã tồn tại"
            });
        }

        _db.DeTais.Add(dt);

        await _db.SaveChangesAsync();

        _logger.LogInformation(
            "Đã thêm đề tài {MaDT}",
            dt.MaDT);

        return Ok(dt);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        DeTai dt)
    {
        var old = await _db.DeTais.FindAsync(id);

        if (old == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đề tài"
            });
        }

        old.TenDT = dt.TenDT;
        old.MoTa = dt.MoTa;
        old.Diem = dt.Diem;

        await _db.SaveChangesAsync();

        return Ok(old);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dt = await _db.DeTais.FindAsync(id);

        if (dt == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đề tài"
            });
        }

        _db.DeTais.Remove(dt);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Xóa đề tài thành công"
        });
    }

    [Authorize(Roles = "Admin")]
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

        var dt = await _db.DeTais.FindAsync(id);

        if (dt == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đề tài"
            });
        }

        dt.Diem = diem;

        await _db.SaveChangesAsync();

        return Ok(new
        {
            maDT = dt.MaDT,
            tenDT = dt.TenDT,
            diem = dt.Diem
        });
    }
}