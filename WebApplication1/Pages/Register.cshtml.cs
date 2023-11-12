using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MyDB;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class RegisterModel : PageModel
    {
        public string? Message { get; private set; } = "";
        private readonly ICaptchaValidator _captchaValidator;
        private readonly EmailService _emailService;
        private readonly ILogWriterService _logWriter;

        public void OnGet()
        {
            Message = HttpContext.Session.GetString("info");
		}
        public IActionResult OnGetError()
        {
            Message = "����� E-mail ��� ���� � �������. ������� ������ � ���������� ��� ���!";
            return Page();
        }

        public RegisterModel(ICaptchaValidator captchaValidator, EmailService emailService, ILogWriterService logWriter) 
        { 
            _captchaValidator = captchaValidator;
            _emailService = emailService;
            _logWriter = logWriter;
        }

        public async Task<IActionResult> OnPost(string email, string password, string firstname, string lastname, DateTime datetime, int utc, string token)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(token))
            {
                try
                {
                    await _logWriter.WriteLogAsync("��� ����������� � ������� - �� �������� �������� reCaptcha");
                    await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", "��� ����������� � ������� - �� �������� �������� reCaptcha!");
                    return RedirectToPage("Robot");
                }
                catch
                {
                    return RedirectToPage("Robot");
                }
            }

            // ���������� ������
            string hashedPasword = Encryption.GetHashString(password);
			string encryptEmail = Encryption.Encrypt(email);
			string encryptFirstname = Encryption.Encrypt(firstname);
			string encryptLastname = Encryption.Encrypt(lastname);
            long ticks = datetime.AddHours(-utc).Ticks;
            string encryptBirthday = Encryption.Encrypt(ticks.ToString());
            string encryptUtc = Encryption.Encrypt(utc.ToString());

			// �������� �� ������������ ������
			try
			{
                using (DBconnect db = DBconnect.getDataBase())
                {
				    string sqlExpression = $"SELECT * FROM users WHERE email='{encryptEmail}'";
				    using SqliteCommand command = db.DoSql(sqlExpression);
                    using SqliteDataReader reader = command.ExecuteReader();
					if (reader.HasRows)
					{
                        return RedirectToPage("Register", "Error");
					}
				}
			}
			catch (SqliteException e)
			{
                HttpContext.Session.SetString("info", "���-�� ����� �� ���... ����������������� ��� ���! ���� �� ����� ���������� - �������� ��� �� ���� �� ����������� ����� (����� � ��������� �����.");
                try
                {
                    await _logWriter.WriteSqliteLogAsync("��� �����������, �������� �� �������� ����������� ����� �� ���������", e);
                    await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"��� �����������, �������� �� �������� ����������� ����� �� ���������. {e.GetType()}: {e.Message}.");
                    return RedirectToPage("Register");
                }
                catch
                {
                    return RedirectToPage("Register");
                }
			}

            // ��������� ���� �������������
            int number = new Random(DateTime.Now.Millisecond).Next(1000, 10000);
            HttpContext.Session.SetString("code", $"{number}");
            HttpContext.Session.SetString("hashedPasword", $"{hashedPasword}");
            HttpContext.Session.SetString("encryptEmail", $"{encryptEmail}");
            HttpContext.Session.SetString("encryptFirstname", $"{encryptFirstname}");
            HttpContext.Session.SetString("encryptLastname", $"{encryptLastname}");
            HttpContext.Session.SetString("encryptBirthday", $"{encryptBirthday}");
            HttpContext.Session.SetString("encryptUtc", $"{encryptUtc}");

            // �������� ������ �������������
            try
            {
                await _emailService.SendEmailAsync($"{email}", "Web Weda", $"��� �������������: {number}.");
            }
            catch(Exception e)
            {
                HttpContext.Session.SetString("info", "���-�� ����� �� ���... ����������������� ��� ���! ���� �� ����� ���������� - �������� ��� �� ���� �� ����������� ����� (����� � ��������� �����.");
                try
                {
                    await _logWriter.WriteExeptionLogAsync("Register. �� ����������� ������ ������������� �����������", e);
                    await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"Register. �� ����������� ������ ������������� �����������. {e.GetType()}: {e.Message}.");
                    return RedirectToPage("Register");
                }
                catch
                {
                    return RedirectToPage("Register");
                }
            }
            
            return RedirectToPage("RegisterConfirm");
        }
    }
}