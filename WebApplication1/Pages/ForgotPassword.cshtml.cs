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
            Message = "Такой учетной записи не найдено. Введите другой Е-mail, или зарегистрируйтесь!";
        }

        public async Task<IActionResult> OnPost(string email)
        {
            string encryptEmail = Encryption.Encrypt(email);

            // Проверка на наличие записи в БД
            try
            {
                using (DBconnect db = DBconnect.getDataBase())
                {
                    string sqlExpression = $"SELECT * FROM users WHERE email='{encryptEmail}'";
                    using SqliteCommand command = db.DoSql(sqlExpression);
                    using SqliteDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        // Генерация кода подтверждения
                        int number = new Random(DateTime.Now.Millisecond).Next(1000, 10000);
                        HttpContext.Session.SetString("code", $"{number}");
                        HttpContext.Session.SetString("email", $"{email}");

                        // Отправка письма
                        try
                        {
                            await _emailService.SendEmailAsync($"{email}", "Web Weda", $"Код подтверждения: {number}.");
                            return RedirectToPage("PasswordConfirm");
                        }
                        catch (Exception e)
                        {
                            HttpContext.Session.SetString("info", "Что-то пошло не так... Попробуйте еще раз! Если не будет получаться - напишите нам об этом по электронной почте (адрес в Контактах сайта.");
                            try
                            {
                                await _logWriter.WriteExeptionLogAsync("ForgotPassword. Не отправилось письмо когда забыли пароль", e);
                                await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"ForgotPassword. Не отправилось письмо когда забыли пароль. {e.GetType()}: {e.Message}.");
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
                HttpContext.Session.SetString("info", "Что-то пошло не так... Попробуйте еще раз! Если не будет получаться - напишите нам об этом по электронной почте (адрес в Контактах сайта.");
                try
                {
                    await _logWriter.WriteSqliteLogAsync("ForgotPassword. Проверка на наличие электронной почты в БД - не выполнена", e);
                    await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"ForgotPassword. Проверка на наличие электронной почты в БД - не выполнена. {e.GetType()}: {e.Message}.");
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