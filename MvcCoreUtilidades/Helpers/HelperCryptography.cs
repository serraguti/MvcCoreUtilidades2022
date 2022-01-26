using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace MvcCoreUtilidades.Helpers
{
    public static class HelperCryptography
    {
        //VAMOS A REALIZAR UN CIFRADO BASICO
        public static string EncriptarTextoBasico(string contenido)
        {
            //NECESITAMOS TRABAJAR A NIVEL DE BYTE
            //DEBEMOS CONVERTIR EL CONTENIDO A BYTE[]
            byte[] entrada;
            //UNA VEZ QUE APLIQUEMOS EL CIFRADO, NOS DEOLVERA
            //UN BYTE[] DE SALIDA CON LOS ELEMENTOS CIFRADOS
            byte[] salida;
            //NECESITAMOS UN CONVERSOR PARA TRANSFORMAR BYTE[] A STRING
            //Y VICEVERSA
            UnicodeEncoding encoding = new UnicodeEncoding();
            //NECESITAMOS EL OBJETO PARA EL CIFRADO
            SHA1Managed sha = new SHA1Managed();
            //CONVERTIMOS EL CONTENIDO A BYTE[]
            entrada = encoding.GetBytes(contenido);
            //EL OBJETO SHA CONTIENE UN METODO PARA REALIZAR EL CIFRADO
            //Y DEVOLVER EL CONTENIDO A BYTE[]
            salida = sha.ComputeHash(entrada);
            string resultado = encoding.GetString(salida);
            return resultado;
        }

        private static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 1; i <= 50; i++)
            {
                int aleat = random.Next(0, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        public static string Salt { get; set; }

        public static string EncriptarContenido
            (string contenido, bool comparar)
        {
            if (comparar == false)
            {
                Salt = GenerateSalt();
            }
            string contenidosalt = contenido + Salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida;
            UnicodeEncoding encoding = new UnicodeEncoding();
            salida = encoding.GetBytes(contenidosalt);
            //CIFRAMOS N VECES
            for (int i = 1; i <= 55; i++)
            {
                //REALIZAMOS EL CIFRADO SOBRE SALIDA
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            string resultado = encoding.GetString(salida);
            return resultado;
        }
    }
}
