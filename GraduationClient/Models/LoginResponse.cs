namespace GraduationClient.Models;

public class LoginResponse
{
    public string Token { get; set; } = "";

    public int MaSV { get; set; }

    public string HoTen { get; set; } = "";

    public string Email { get; set; } = "";

    public string Quyen { get; set; } = "";
}