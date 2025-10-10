using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class AESCipher : ICipherProvider
{
    private const string AES_NAME = "aes_key";

    public string Encrypt(string plainText)
    {
        if (plainText == null)
            throw new ArgumentException(nameof(plainText));

        byte[] encrypted;
        byte[] key = GetOrCreateKey();

        using Aes aesAlg = Aes.Create();

        aesAlg.Key = key;
        aesAlg.GenerateIV();

        using (MemoryStream msEncrypt = new MemoryStream())
        {
            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

            using ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);

            }

            encrypted = msEncrypt.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string cipherText)
    {
        if (cipherText == null)
            throw new ArgumentException(nameof(cipherText));

        string plainText = null;

        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] key = GetOrCreateKey();

        using Aes aesAlg = Aes.Create();

        aesAlg.Key = key;

        byte[] iv = new byte[aesAlg.BlockSize / 8];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aesAlg.IV = iv;

        int cipherOffset = iv.Length;
        int cipherLength = fullCipher.Length - cipherOffset;

        ICryptoTransform decryptor = aesAlg.CreateDecryptor();

        using MemoryStream msDecrypt = new MemoryStream(fullCipher, cipherOffset, cipherLength);
        using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new StreamReader(csDecrypt);

        plainText = srDecrypt.ReadToEnd();

        return plainText;
    }

    // In production better use the keystore/keychain/DPAPI
    private byte[] GetOrCreateKey()
    {
        if (PlayerPrefs.HasKey(AES_NAME))
            return Convert.FromBase64String(PlayerPrefs.GetString(AES_NAME));

        byte[] key = new byte[32];
        RandomNumberGenerator.Fill(key);

        string keyStr = Convert.ToBase64String(key);
        PlayerPrefs.SetString(AES_NAME, keyStr);
        PlayerPrefs.Save();

        return key;
    }
}
