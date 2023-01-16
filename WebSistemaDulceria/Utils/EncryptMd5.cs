using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Utils
{
    public class EncryptMd5
    {
        public string Encrypt (string cadena)
        {
            string hash = "hash010101!*+";
            byte[] data = UTF8Encoding.UTF8.GetBytes(cadena);

            MD5 md5 = MD5.Create();

            TripleDES tripleDES = TripleDES.Create();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));

            tripleDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripleDES.CreateEncryptor();

            byte[] result = transform.TransformFinalBlock(data, 0 , data.Length);

            return Convert.ToBase64String(result);
        }

        public string Descrypt (string cadenaEn)
        {
            string hash = "hash010101!*+";
            byte[] data = Convert.FromBase64String(cadenaEn);

            MD5 md5 = MD5.Create();
            TripleDES tripleDES = TripleDES.Create();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));

            tripleDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripleDES.CreateDecryptor();

            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);

        }
    }
}
