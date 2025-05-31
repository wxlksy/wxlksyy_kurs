using System;
using System.Data.SqlClient;
using wxlksyy_kurs.Data;

namespace wxlksyy_kurs.Services
{
    /// <summary>
    /// Сервис аутентификации пользователей
    /// </summary>
    public class AuthService
    {
        private readonly DatabaseHelper _databaseHelper;

        public AuthService()
        {
            _databaseHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Проверяет учетные данные пользователя
        /// </summary>
        public bool Authenticate(string username, string password)
        {
            try
            {
                var query = "SELECT 1 FROM Users WHERE username = @username AND password = @password";
                var parameters = new[]
                {
                    new SqlParameter("@username", username),
                    new SqlParameter("@password", password)
                };

                var result = _databaseHelper.ExecuteScalar(query, parameters);
                return result != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка аутентификации", ex);
            }
        }
    }
}