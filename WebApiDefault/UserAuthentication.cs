using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebApiDefault.Models;

namespace WebApiDefault
{
    public class UserAuthentication
    {
        public static bool Login(string UserName, string Password)
        {
            using (DBConnectionNeo2 db = new DBConnectionNeo2())
            {
                string chave = "7f9facc418f74439c5e9709832;0ab8a5:OCOdN5Wl,q8SLIQz8i|8agmu¬s13Q7ZXyno/";
                SHA512 sha512 = new SHA512CryptoServiceProvider();
                byte[] inputBytes = (new UnicodeEncoding()).GetBytes(string.Concat(UserName, Password) + chave);
                byte[] hash = sha512.ComputeHash(inputBytes);
                string Senha = Convert.ToBase64String(hash);

                return db.UsuariosNeo2.Any(user => user.Login.Equals(UserName, StringComparison.OrdinalIgnoreCase) && user.Senha == Senha);
            }
        }
    }
}