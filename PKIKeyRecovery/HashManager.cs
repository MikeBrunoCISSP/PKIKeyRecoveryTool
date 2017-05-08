using System;
using System.Text;
using System.Security.Cryptography;

class HashManager
{
    HashAlgorithm hash;
    int hashSize;

    public HashManager(string hashAlgorithm)
    {
        switch (hashAlgorithm.ToUpper())
        {
            case "SHA1":
                hash = new SHA1Managed();
                hashSize = 20;
                break;

            case "SHA256":
                hash = new SHA256Managed();
                hashSize = 32;
                break;

            case "SHA384":
                hash = new SHA384Managed();
                hashSize = 48;
                break;

            case "SHA512":
                hash = new SHA512Managed();
                hashSize = 64;
                break;

            case "MD5":
                hash = new MD5CryptoServiceProvider();
                hashSize = 16;
                break;

            default:
                hash = new SHA256Managed();
                hashSize = 32;
                break;
        }
    }

    public string compute(string plaintext)
    {
        byte[] salt = getBytes(crypto.GeneratePassword(4, 8, true));
        byte[] message = concatenate(getBytes(plaintext), salt);
        byte[] hashBytes = hash.ComputeHash(message);
        byte[] hashWithSaltBytes = concatenate(message, salt);

        return Convert.ToBase64String(hashWithSaltBytes);
    }

    private string compute(string plaintext, byte[] salt)
    {
        byte[] message = concatenate(getBytes(plaintext), salt);
        byte[] hashBytes = hash.ComputeHash(message);
        byte[] hashWithSaltBytes = concatenate(message, salt);

        return Convert.ToBase64String(hashWithSaltBytes);
    }

    public bool verify(string plaintext, string digest)
    {

        byte[] saltBytes = getSalt(digest);
        string expectedHash = compute(plaintext, saltBytes);
        return (expectedHash == plaintext);

    }

    private byte[] getBytes(string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }

    private byte[] getSalt(string digest)
    {
        byte[] hashWithSaltBytes = Convert.FromBase64String(digest);
        byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSize];

        for (int index = 0; index < saltBytes.Length; index++)
            saltBytes[index] = hashWithSaltBytes[hashSize + index];

        return saltBytes;
    }

    private byte[] concatenate(byte[] messageBytes, byte[] saltBytes)
    {
        byte[] combinedMessage = new byte[messageBytes.Length + saltBytes.Length];

        for (int index = 0; index < messageBytes.Length; index++)
            combinedMessage[index] = messageBytes[index];
        for (int index = 0; index < saltBytes.Length; index++)
            combinedMessage[messageBytes.Length + index] = saltBytes[index];

        return combinedMessage;
    }
}
