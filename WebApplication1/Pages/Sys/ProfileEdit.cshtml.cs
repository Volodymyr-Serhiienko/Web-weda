using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MyDB;
using WebApplication1.Services;

namespace WebApplication1.Pages.Sys
{
    public class ProfileEditModel : PageModel
    {
        public string Message { get; private set; } = "";
        private readonly EmailService _emailService;
        private readonly ILogWriterService _logWriter;

        public void OnGet()
        {
        }
        public IActionResult OnGetError()
        {
            Message = "Что-то пошло не так... Попробуйте еще раз!";
            return Page();
        }

        public ProfileEditModel(EmailService emailService, ILogWriterService logWriter)
        {
            _emailService = emailService;
            _logWriter = logWriter;
        }

        public async Task<IActionResult> OnPost(string password, string firstname, string lastname, DateTime datetime, int utc)
        {
            string email = HttpContext.Session.GetString("email")!;

            // Шифрование данных
            string hashedPasword = Encryption.GetHashString(password);
            string encryptEmail = Encryption.Encrypt(email);
            string encryptFirstname = Encryption.Encrypt(firstname);
            string encryptLastname = Encryption.Encrypt(lastname);
            long ticks = datetime.AddHours(-utc).Ticks;
            string encryptBirthday = Encryption.Encrypt(ticks.ToString());
            string encryptUtc = Encryption.Encrypt(utc.ToString());

            // Изменение данных в БД
            try
            {
                using (DBconnect db = DBconnect.getDataBase())
                {
                    string sqlExpression = $"UPDATE users SET password='{hashedPasword}', firstname='{encryptFirstname}', lastname='{encryptLastname}', birthday='{encryptBirthday}', utc='{encryptUtc}' WHERE email='{encryptEmail}' ";
                    using SqliteCommand command = db.DoSql(sqlExpression);
                    command.ExecuteNonQuery();

                    // Установка данных на текущую сессию
                    HttpContext.Session.SetString("firstname", $"{Encryption.Decrypt(encryptFirstname)}");
                    HttpContext.Session.SetString("lastname", $"{Encryption.Decrypt(encryptLastname)}");
                    HttpContext.Session.SetString("birthday", $"{Encryption.Decrypt(encryptBirthday)}");
                    HttpContext.Session.SetString("utc", $"{Encryption.Decrypt(encryptUtc)}");

                    return RedirectToPage("/Sys/ProfileEditSuccess");
                }
            }
            catch (SqliteException e)
            {
                try
                {
                    await _logWriter.WriteSqliteLogAsync("При редактировании профиля - даные пользователя не внесены в БД", e);
                    await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"При редактировании профиля - даные пользователя не внесены в БД. {e.GetType()}: {e.Message}.");
                    return RedirectToPage("/Sys/ProfileEdit", "Error"); ;
                }
                catch
                {
                    return RedirectToPage("/Sys/ProfileEdit", "Error");
                }
            }
        }
    }
}