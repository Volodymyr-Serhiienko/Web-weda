using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MyDB;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class PasswordConfirmModel : PageModel
    {
        public string? Message { get; private set; } = "";
        private readonly EmailService _emailService;
        private readonly ILogWriterService _logWriter;

        public PasswordConfirmModel(EmailService emailService, ILogWriterService logWriter)
        {
            _emailService = emailService;
            _logWriter = logWriter;
        }

        public void OnGet()
        {
        }
        public void OnGetError()
        {
            Message = "������ ������������ ���. ������� ��� ��� ���!";
        }

        public async Task<IActionResult> OnPost(string code)
        {
            if(code == HttpContext.Session.GetString("code"))
            {
                try
                {
                    using (DBconnect db = DBconnect.getDataBase())
                    {
                        string sqlExpression = $"SELECT * FROM users WHERE email='{Encryption.Encrypt(HttpContext.Session.GetString("email")!)}'";
                        using SqliteCommand command = db.DoSql(sqlExpression);
                        using SqliteDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            // ���������� ������ �� ��
                            string encryptFirstname = (string)reader.GetValue(2);
                            string encryptLastname = (string)reader.GetValue(3);
                            string encryptBirthday = (string)reader.GetValue(4);
                            string encryptUtc = (string)reader.GetValue(5);

                            // ��������� ������ �� ������� ������
                            HttpContext.Session.SetString("firstname", $"{Encryption.Decrypt(encryptFirstname)}");
                            HttpContext.Session.SetString("lastname", $"{Encryption.Decrypt(encryptLastname)}");
                            HttpContext.Session.SetString("birthday", $"{Encryption.Decrypt(encryptBirthday)}");
                            HttpContext.Session.SetString("utc", $"{Encryption.Decrypt(encryptUtc)}");
                        }
                    }

                    return RedirectToPage("/Sys/Dayinfo");
                }
                catch (SqliteException e)
                {
                    HttpContext.Session.SetString("info", "���-�� ����� �� ���... ���������� ��� ���! ���� �� ����� ���������� - �������� ��� �� ���� �� ����������� ����� (����� � ��������� �����.");
                    try
                    {
                        await _logWriter.WriteSqliteLogAsync("PasswordConfirm. �� ���������� ������� ������ ������������ ��� �������� ������", e);
                        await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"PasswordConfirm. �� ���������� ������� ������ ������������ ��� �������� ������. {e.GetType()}: {e.Message}.");
                        return RedirectToPage("ForgotPassword");
                    }
                    catch
                    {
                        return RedirectToPage("ForgotPassword");
                    }
                }
            }
            else
            {
                return RedirectToPage("PasswordConfirm", "Error");
            }
        }
    }
}