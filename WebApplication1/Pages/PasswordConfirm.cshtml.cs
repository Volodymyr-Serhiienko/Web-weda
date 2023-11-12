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
            Message = "Введен неправильный код. Введите его еще раз!";
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
                            // Считывание данных из БД
                            string encryptFirstname = (string)reader.GetValue(2);
                            string encryptLastname = (string)reader.GetValue(3);
                            string encryptBirthday = (string)reader.GetValue(4);
                            string encryptUtc = (string)reader.GetValue(5);

                            // Установка данных на текущую сессию
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
                    HttpContext.Session.SetString("info", "Что-то пошло не так... Попробуйте еще раз! Если не будет получаться - напишите нам об этом по электронной почте (адрес в Контактах сайта.");
                    try
                    {
                        await _logWriter.WriteSqliteLogAsync("PasswordConfirm. Не получилось считать данные пользователя для загрузки сессии", e);
                        await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"PasswordConfirm. Не получилось считать данные пользователя для загрузки сессии. {e.GetType()}: {e.Message}.");
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