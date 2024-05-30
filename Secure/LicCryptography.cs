using System;
using System.Security.Cryptography;
using System.IO;

namespace Secure
{

    class LicCryptography
	{
		#region Encrypt String
		public string Encrypt(string clearText, string Password) 
		{ 
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText); 
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, 
			new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,  0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76}); 
			byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16)); 
			return Convert.ToBase64String(encryptedData); 
		} 
		#endregion Encrypt String

		#region Encrypt Bytes
		public byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV) 
		{ 
			// Create a MemoryStream that is going to accept the encrypted bytes 
			MemoryStream ms = new MemoryStream(); 
			Rijndael alg = Rijndael.Create(); 
			alg.Key = Key; 
			alg.IV = IV; 

			CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write); 
			// Write the data and make it do the encryption 
			cs.Write(clearData, 0, clearData.Length); 
			cs.Close(); 

			byte[] encryptedData = ms.ToArray(); 
			return encryptedData; 
		} 
		#endregion Encrypt Bytes

		#region Decrypt String
		public string Decrypt(string cipherText, string Password) 
		{ 
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,  0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76}); 

			byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(16)); 
			return System.Text.Encoding.Unicode.GetString(decryptedData); 
		} 

		public string Decrypt(string Order) 
		{ 			
			try 
			{
                string Password = "v3l0c1tys2s";
				byte[] cipherBytes = Convert.FromBase64String(Order); 
				PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, 
					new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,  0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76}); 
				byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(16)); 
				return System.Text.Encoding.Unicode.GetString(decryptedData); 

			}
			catch ( Exception e )
			{
				return "Invalid Key";
			}
			return null;
		}
		#endregion Decrypt String

		#region Decrypt Bytes
		public  byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV) 
		{ 
			// Create a MemoryStream that is going to accept the decrypted bytes 
			MemoryStream ms = new MemoryStream(); 
			Rijndael alg = Rijndael.Create(); 
			alg.Key = Key; 
			alg.IV = IV; 

			CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write); 
			// Write the data and make it do the decryption 
			cs.Write(cipherData, 0, cipherData.Length); 
			cs.Close(); 

			byte[] decryptedData = ms.ToArray(); 
			return decryptedData; 
		} 
		#endregion Decrypt Bytes

		#region Encrypt Order
		public string EncryptNewOrder(string NewOrder) 
		{
            string Password = "v3l0c1tys2s";
			// First we need to turn the input string into a byte array. 
			byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(NewOrder);
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,  0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76}); 

			byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16)); 
			return Convert.ToBase64String(encryptedData); 
		} 
		#endregion Encrypt Order

		#region Decrypt Order
		public string [] DecryptOrder(string Order)
		{
			string delimStr = "|";
			char [] delimiter = delimStr.ToCharArray();
			string [] splitted = null;
			string DecryptedOrder = Decrypt(Order);
			splitted = DecryptedOrder.Split(delimiter);
			return splitted;
		}
		#endregion Decrypt Order
	}
}
