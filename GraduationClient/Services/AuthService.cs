namespace GraduationClient.Services;

public class AuthService
{
    private string _token = "";
    private int _maSV;
    private string _hoTen = "";
    private string _email = "";
    private string _quyen = "";

    public event Action? OnChange;

    public bool IsLoggedIn =>
        !string.IsNullOrWhiteSpace(_token);

    public string Token => _token;

    public int MaSV => _maSV;

    public string HoTen => _hoTen;

    public string Email => _email;

    public string Quyen => _quyen;


    public void Login(
        string token,
        int maSV,
        string hoTen,
        string email,
        string quyen)
    {
        _token = token;
        _maSV = maSV;
        _hoTen = hoTen;
        _email = email;
        _quyen = quyen;

        NotifyStateChanged();
    }


    public void Logout()
    {
        _token = "";
        _maSV = 0;
        _hoTen = "";
        _email = "";
        _quyen = "";

        NotifyStateChanged();
    }


    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}