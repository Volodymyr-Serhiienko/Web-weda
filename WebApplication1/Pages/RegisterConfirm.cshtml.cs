using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MyDB;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class RegisterConfirmModel : PageModel
    {
        public string? Message { get; private set; } = "";
        private readonly EmailService _emailService;
        private readonly ILogWriterService _logWriter;

        public RegisterConfirmModel(EmailService emailService, ILogWriterService logWriter)
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
            if (code == HttpContext.Session.GetString("code"))
            {
                string encryptEmail = HttpContext.Session.GetString("encryptEmail")!;
                string hashedPasword = HttpContext.Session.GetString("hashedPasword")!;
                string encryptFirstname = HttpContext.Session.GetString("encryptFirstname")!;
                string encryptLastname = HttpContext.Session.GetString("encryptLastname")!;
                string encryptBirthday = HttpContext.Session.GetString("encryptBirthday")!;
                string encryptUtc = HttpContext.Session.GetString("encryptUtc")!;

                // Запись в БД
                try
                {
                    using (DBconnect db = DBconnect.getDataBase())
                    {
                        string sqlExpression = $"INSERT INTO Users (email, password, firstname, lastname, birthday, utc) VALUES ('{encryptEmail}', '{hashedPasword}', '{encryptFirstname}', '{encryptLastname}', '{encryptBirthday}', '{encryptUtc}')";
                        using SqliteCommand command = db.DoSql(sqlExpression);
                        command.ExecuteNonQuery();
                    }
                }
                catch(SqliteException e)
                {
                    HttpContext.Session.SetString("info", "Что-то пошло не так... Зарегистрируйтесь еще раз! Если не будет получаться - напишите нам об этом по электронной почте (адрес в Контактах сайта.");
                    try
                    {
                        await _logWriter.WriteSqliteLogAsync("При регистрации - даные пользователя не внесены в БД", e);
                        await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"При регистрации - даные пользователя не внесены в БД. {e.GetType()}: {e.Message}.");
                        return RedirectToPage("Register");
                    }
                    catch
                    {
                        return RedirectToPage("Register");
                    }
                }

                // Установка данных на текущую сессию
                HttpContext.Session.SetString("email", $"{Encryption.Decrypt(encryptEmail)}");
                HttpContext.Session.SetString("firstname", $"{Encryption.Decrypt(encryptFirstname)}");
                HttpContext.Session.SetString("lastname", $"{Encryption.Decrypt(encryptLastname)}");
                HttpContext.Session.SetString("birthday", $"{Encryption.Decrypt(encryptBirthday)}");
                HttpContext.Session.SetString("utc", $"{Encryption.Decrypt(encryptUtc)}");

                return RedirectToPage("/Sys/Dayinfo");
            }
            else
            {
                return RedirectToPage("RegisterConfirm", "Error");
            }
        }
    }
}