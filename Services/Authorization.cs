using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services
{
    public static class Authorization
    {
        public static string userLogin;

        public static bool CheckEnter(string login, IEnumerable<string> logins, string password, IEnumerable<string> passwords)
        {
            bool result = false;
            bool resultLogin = false;
            StringBuilder errors = new StringBuilder();
            
            if (login.Length == 0)
            {
                errors.AppendLine("Необходимо заполнить поле логина");
            }
            else
            {
                foreach (string correctLogin in logins)
                {
                    if (login == correctLogin)
                    {
                        resultLogin = true;
                        if (password.Length == 0)
                        {
                            errors.AppendLine("Необходимо заполнить поле пароля");
                        }
                        else
                        {
                            foreach (string correctPassword in passwords)
                            {
                                if (password == correctPassword)
                                {
                                    result = true;
                                    MessageBox.Show($"Добро пожаловать в систему, {login}!", "Авторизация прошла успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                                    break;
                                }
                            }
                            if (result == false)
                            {
                                errors.AppendLine("Введен неправильный пароль");
                                break;
                            }
                        }
                    }
                }
                if(resultLogin==false)
                {
                    errors.AppendLine("Введен неправильный логин");
                }
            }
            if (result == false)
            {
                MessageBox.Show(errors.ToString(), "Ошибка входа!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return result;
        }
    }
}