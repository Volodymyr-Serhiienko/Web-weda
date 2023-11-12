using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MyDB;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class LoginModel : PageModel
    {
        public string Message { get; private set; } = "";
        private readonly ICaptchaValidator _captchaValidator;
        private readonly EmailService _emailService;
		private readonly ILogWriterService _logWriter;

		public LoginModel(ICaptchaValidator captchaValidator, EmailService emailService, ILogWriterService logWriter)
		{
            _captchaValidator = captchaValidator;
            _emailService = emailService;
            _logWriter = logWriter;
        }

        public void OnGet()
        {
        }
        public void OnGetError()
        {
            Message = "Такой учетной записи не найдено. Введите другой Е-mail, или зарегистрируйтесь!";
        }
        public void OnGetError1()
        {
            Message = "Такая учетная запись есть, но пароль введен неправильно. Введите пароль правильно!";
        }
        public void OnGetError2()
        {
            Message = "Что-то пошло не так... Попробуйте еще раз войти в систему! Если не будет получаться - напишите нам об этом по электронной почте (адрес в Контактах сайта.";
        }

        public async Task<IActionResult> OnPost(string email, string password, string token)
        {
            HttpContext.Session.Clear();

            if (!await _captchaValidator.IsCaptchaPassedAsync(token))
            {
				try
				{
					await _logWriter.WriteLogAsync("При входе в систему - не пройдена проверка reCaptcha");
					await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", "При входе в систему - не пройдена проверка reCaptcha!");
                    return RedirectToPage("Robot");
                }
				catch
				{
					return RedirectToPage("Robot");
				}
            }

            // Кодирование введенных данных
            string hashedPasword = Encryption.GetHashString(password);
            string encryptEmail = Encryption.Encrypt(email);

			// Поиск в базе данных
			try
			{
				using (DBconnect db = DBconnect.getDataBase())
				{
					string sqlExpression = $"SELECT * FROM Users WHERE email='{encryptEmail}'";
					using SqliteCommand command = db.DoSql(sqlExpression);
					using SqliteDataReader reader = command.ExecuteReader();
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							string DBemail = (string)reader.GetValue(0);
							string DBpassword = (string)reader.GetValue(1);
							string firstname = (string)reader.GetValue(2);
							string lastname = (string)reader.GetValue(3);
							string birthday = (string)reader.GetValue(4);
							string utc = (string)reader.GetValue(5);

							// Декодирование данных
							string decryptBirthday = Encryption.Decrypt(birthday);
							string decryptEmail = Encryption.Decrypt(DBemail);
							string decryptFirstname = Encryption.Decrypt(firstname);
							string decryptLastname = Encryption.Decrypt(lastname);
							string decryptUtc = Encryption.Decrypt(utc);

							// Начало сессии
							if (DBpassword == hashedPasword)
							{
								HttpContext.Session.SetString("email", $"{decryptEmail}");
								HttpContext.Session.SetString("firstname", $"{decryptFirstname}");
								HttpContext.Session.SetString("lastname", $"{decryptLastname}");
								HttpContext.Session.SetString("birthday", $"{decryptBirthday}");
								HttpContext.Session.SetString("utc", $"{decryptUtc}");

								return RedirectToPage("/Sys/Dayinfo");
							}
							else
							{
								return RedirectToPage("Login", "Error1");
							}
						}
					}
					else
					{
						return RedirectToPage("Login", "Error");
					}
				}
			}
			catch (SqliteException e)
			{
				try
				{
					await _logWriter.WriteSqliteLogAsync("Вход пользователя в систему не выполнен", e);
					await _emailService.SendEmailAsync("utos.vovik@gmail.com", "Web Weda", $"Вход пользователя в систему не выполнен. {e.GetType()}: {e.Message}.");
					return RedirectToPage("Login", "Error2");
				}
				catch 
				{
					return RedirectToPage("Login", "Error2");
				}
            }

			return RedirectToPage("Login");
		}
    }
}