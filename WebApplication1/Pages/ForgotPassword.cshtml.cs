using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MyDB;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        public string? Message { get; private set; } = "";
        private readonly EmailService _emailService;
        private readonly ILogWriterService _logWriter;

        public ForgotPasswordModel(EmailService emailService, ILogWriterService logWriter)
        {
            _emailService = emailService;
            _logWriter = logWriter;
        }

        public void OnGet()
        {
            Message = HttpContext.Session.GetString("info");
        }
        public void OnGetError()
        {
            Message = "����� ������� ������ �� �������. ������� ������ �-mail, ��� �����������������!";
        }

        public async Task<IActionResult> OnPost(string email)
        {
            string encryptEmail = Encryption.Encrypt(email);

            // �������� �� ������� ������ � ��
            try
            {
                using (DBconnect db = DBconnect.getDataBase())
                {
                    string sqlExpression = $"SELECT * FROM users WHERE email='{encryptEmail}'";
                    using SqliteCommand command = db.DoSql(sqlExpression);
                    using SqliteDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        // ��������� ���� �������������
                        int number = new Random(DateTime.Now.Millisecond).Next(1000, 10000);
                        HttpContext.Session.SetString("code", $"{number}");
                        HttpContext.Session.SetString("email", $"{email}");

                        // �������� ������
                        try
                        {
                            await _emailService.SendEmailAsync($"{email}", "Web Weda", $"��� �������������: {number}.");
                            return RedirectToPage("PasswordConfirm");
                        }
                        catch (Exception e)
                        {
                            HttpContext.Session.SetString("info", "���-�� ����� �� ���... ���������� ��� ���! ���� �� ����� ���������� - �������� ��� �� ���� �� ����������� ����� (����� � ��������� �����.");
                            try
                            {
                                await _logWriter.WriteExeptionLogAsync("ForgotPassword. �� ����������� ������ ����� ������ ������", e);
                                await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"ForgotPassword. �� ����������� ������ ����� ������ ������. {e.GetType()}: {e.Message}.");
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
                        return RedirectToPage("ForgotPassword", "Error");
                    }
                }
            }
            catch (SqliteException e)
            {
                HttpContext.Session.SetString("info", "���-�� ����� �� ���... ���������� ��� ���! ���� �� ����� ���������� - �������� ��� �� ���� �� ����������� ����� (����� � ��������� �����.");
                try
                {
                    await _logWriter.WriteSqliteLogAsync("ForgotPassword. �������� �� ������� ����������� ����� � �� - �� ���������", e);
                    await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"ForgotPassword. �������� �� ������� ����������� ����� � �� - �� ���������. {e.GetType()}: {e.Message}.");
                    return RedirectToPage("ForgotPassword");
                }
                catch
                {
                    return RedirectToPage("ForgotPassword");
                }
            }
        }
    }
}