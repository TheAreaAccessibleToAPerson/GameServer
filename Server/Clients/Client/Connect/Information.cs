using System.Net.Sockets;
using Butterfly;

namespace server.client.connect
{
    public class Data
    {
        public client.Socket Ssl;
        public client.Socket Tcp;

        public enum Verification
        {
            None,
            // Авторизация прошла успешно.
            Success,
        }

        private const string NAME = "Information.Client:";

        private Verification _verificationResult = Verification.None;

        public IInput I_continueProcess;

        public void ReturnVerificationResult(Verification result, int index = -1)
        {
            if (_verificationResult.HasFlag(Verification.None))
            {
                Index = index;

                _verificationResult = result;

                I_continueProcess.To();
            }
            else throw new Exception();
        }

        public bool IsSuccessVerification()
            => _verificationResult.HasFlag(Verification.Success);

        public int Index { set; get; }

        public string Login { private set; get; } = "";
        private readonly int _loginMinLength;
        private readonly int _loginMaxLength;

        private string _password = "";
        private readonly int _passwordMinLength;
        private readonly int _passwordMaxLength;

        public Data(int loginMinLength, int loginMaxLength,
            int passwordMinLength, int passwordMaxLength)
        {
            _loginMinLength = loginMinLength; _loginMaxLength = loginMaxLength;
            _passwordMinLength = passwordMinLength; _passwordMaxLength = passwordMaxLength;
        }

        public bool SetLogin(string login, out string error)
        {
            error = NAME;

            if (Login != "")
                error += $"Вы попытались повторно назначить в поле login значение {login}.";

            if (login.Length < _loginMinLength)
                error += $"Минимально допустимая длина значение поля login {_loginMinLength}, " +
                    $"переданный параметр [{login}] имеет длину {login.Length}";

            if (login.Length > _loginMaxLength)
                error += $"Максимально допустимая длина значение поля login {_loginMaxLength}, " +
                    $"переданный параметр [{login}] имеет длину {login.Length}";

            if (error == NAME)
            {
                Login = login;

                return true;
            }
            else return false;
        }

        public bool SetPassword(string password, out string error)
        {
            error = NAME;

            if (_password != "")
                error += $"Вы попытались повторно назначить в поле password значение {password}.";

            if (password.Length < _loginMinLength)
                error += $"Минимально допустимая длина значение поля password {_passwordMinLength}, " +
                    $"переданный параметр [{password}] имеет длину {password.Length}";

            if (password.Length > _loginMaxLength)
                error += $"Максимально допустимая длина значение поля password {_passwordMaxLength}, " +
                    $"переданный параметр [{password}] имеет длину {password.Length}";

            if (error == NAME)
            {
                _password = password;

                return true;
            }
            else return false;
        }
    }
}