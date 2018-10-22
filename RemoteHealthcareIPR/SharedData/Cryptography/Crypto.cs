using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Cryptography
{
    class Crypto
    {
        //Key cannot be higher than the largest ASCII value, as this will cause issues with the decrypting (it adds this amount to prevent negative numbers)
        private const int Key = 100;

        private const int LargestAsciiValue = 128;


        public static String Encrypt(String unencryptedString)
        {

            byte[] unencryptedValues = Encoding.ASCII.GetBytes(unencryptedString);

            byte[] encryptedValues = new byte[unencryptedValues.Length];

            for (int i = 0; i < unencryptedValues.Length; i++)
            {
                encryptedValues[i] = (byte)(((int)unencryptedValues[i] + Key) % LargestAsciiValue);
            }


            String encryptedString = System.Text.Encoding.Default.GetString(encryptedValues);

            return encryptedString;
        }

        public static String Decrypt(String encryptedString)
        {

            byte[] encryptedValues = Encoding.ASCII.GetBytes(encryptedString);

            byte[] decryptedValues = new byte[encryptedValues.Length];


            for (int i = 0; i < encryptedValues.Length; i++)
            {
                decryptedValues[i] = (byte)((((int)encryptedValues[i] - Key)
                    + LargestAsciiValue)
                    % LargestAsciiValue);
            }


            String decryptedString = System.Text.Encoding.Default.GetString(decryptedValues);

            return decryptedString;
        }





    }
}
