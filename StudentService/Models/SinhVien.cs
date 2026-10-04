namespace StudentService.Models;

public class SinhVien
{
    public int MaSV { get; set; }
    public string HoTen { get; set; } = "";
    public string Lop { get; set; } = "";
    public string Email { get; set; } = "";
    public string MatKhau { get; set; } = "";
    public string Quyen { get; set; } = "Student";
}