using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bookworms_Online.Services
{
	public class EncryptionService
	{
		private static readonly string key = "<YOUR SECRET KEY>";
		public static string EncryptString(string plainText)
		{
			byte[] iv = new byte[16];
			byte[] array;

			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(AdjustKeyLength(key, 16));
				aes.IV = iv;

				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(plainText);
						}

						array = memoryStream.ToArray();
					}
				}
			}

			return Convert.ToBase64String(array);
		}

		public static string DecryptString(string cipherText)
		{
			byte[] iv = new byte[16];
			byte[] buffer = Convert.FromBase64String(cipherText);

			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(AdjustKeyLength(key, 16));
				aes.IV = iv;
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader(cryptoStream))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
		}



		private static string AdjustKeyLength(string key, int length)
		{
			if (key.Length > length)
			{
				return key.Substring(0, length);
			}
			else
			{
				return key.PadRight(length, ' '); // Padding with spaces
			}
		}

	}
}
