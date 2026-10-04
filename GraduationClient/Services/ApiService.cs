using System.Net.Http.Json;
using GraduationClient.Models;

namespace GraduationClient.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly AuthService _auth;

    public ApiService(
        HttpClient http,
        AuthService auth)
    {
        _http = http;
        _auth = auth;
    }

    private void AddToken()
    {
        _http.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(_auth.Token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    _auth.Token);
        }
    }

    // =====================================================
    // LOGIN
    // =====================================================

    public async Task<LoginResponse?> Login(
        string email,
        string matKhau)
    {
        var login = new
        {
            email = email,
            matKhau = matKhau
        };

        var response = await _http.PostAsJsonAsync(
            "https://localhost:7240/api/SinhVien/login",
            login);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content
            .ReadFromJsonAsync<LoginResponse>();
    }


    // =====================================================
    // SINH VIEN
    // =====================================================

    public async Task<List<SinhVien>> GetSinhViens()
    {
        AddToken();

        return await _http.GetFromJsonAsync<List<SinhVien>>(
            "https://localhost:7240/api/SinhVien")
            ?? new List<SinhVien>();
    }

    public async Task<SinhVien?> GetSinhVien(int id)
    {
        AddToken();

        return await _http.GetFromJsonAsync<SinhVien>(
            $"https://localhost:7240/api/SinhVien/{id}");
    }

    public async Task<bool> AddSinhVien(SinhVien sv)
    {
        AddToken();

        var response = await _http.PostAsJsonAsync(
            "https://localhost:7240/api/SinhVien",
            sv);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateSinhVien(SinhVien sv)
    {
        AddToken();

        var response = await _http.PutAsJsonAsync(
            $"https://localhost:7240/api/SinhVien/{sv.MaSV}",
            sv);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteSinhVien(int id)
    {
        AddToken();

        var response = await _http.DeleteAsync(
            $"https://localhost:7240/api/SinhVien/{id}");

        return response.IsSuccessStatusCode;
    }


    // =====================================================
    // DE TAI
    // =====================================================

    public async Task<List<DeTai>> GetDeTais()
    {
        AddToken();

        return await _http.GetFromJsonAsync<List<DeTai>>(
            "https://localhost:7155/api/DeTai")
            ?? new List<DeTai>();
    }

    public async Task<DeTai?> GetDeTai(int id)
    {
        AddToken();

        return await _http.GetFromJsonAsync<DeTai>(
            $"https://localhost:7155/api/DeTai/{id}");
    }

    public async Task<bool> AddDeTai(DeTai dt)
    {
        AddToken();

        var response = await _http.PostAsJsonAsync(
            "https://localhost:7155/api/DeTai",
            dt);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDeTai(DeTai dt)
    {
        AddToken();

        var response = await _http.PutAsJsonAsync(
            $"https://localhost:7155/api/DeTai/{dt.MaDT}",
            dt);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDeTai(int id)
    {
        AddToken();

        var response = await _http.DeleteAsync(
            $"https://localhost:7155/api/DeTai/{id}");

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDiemDeTai(
        int id,
        decimal diem)
    {
        AddToken();

        var response = await _http.PutAsync(
            $"https://localhost:7155/api/DeTai/{id}/diem?diem={diem}",
            null);

        return response.IsSuccessStatusCode;
    }


    // =====================================================
    // DANG KY
    // =====================================================

    public async Task<List<DangKy>> GetDangKys()
    {
        AddToken();

        return await _http.GetFromJsonAsync<List<DangKy>>(
            "https://localhost:7294/api/DangKy")
            ?? new List<DangKy>();
    }

    public async Task<bool> AddDangKy(DangKy dk)
    {
        AddToken();

        var response = await _http.PostAsJsonAsync(
            "https://localhost:7294/api/DangKy",
            dk);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDangKy(int id)
    {
        AddToken();

        var response = await _http.DeleteAsync(
            $"https://localhost:7294/api/DangKy/{id}");

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDiemDangKy(
        int id,
        decimal diem)
    {
        AddToken();

        var response = await _http.PutAsync(
            $"https://localhost:7294/api/DangKy/{id}/diem?diem={diem}",
            null);

        return response.IsSuccessStatusCode;
    }
}