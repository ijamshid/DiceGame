using Org.BouncyCastle.Crypto.Digests;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace DiceGameApplication.Classes;

public static class HmacGenerator
{
    public static (string hmac, string key, int value) GenerateHmac(int range)
    {
        byte[] keyBytes = GenerateKey();
        int value = GenerateRandomValue(range);

        HMac hmac = new(new Sha3Digest(256));
        hmac.Init(new KeyParameter(keyBytes));
        byte[] valueBytes = BitConverter.GetBytes(value);
        hmac.BlockUpdate(valueBytes, 0, valueBytes.Length);
        byte[] result = new byte[hmac.GetMacSize()];
        hmac.DoFinal(result, 0);
        return (BitConverter.ToString(result).Replace("-", ""), BitConverter.ToString(keyBytes).Replace("-", ""), value);
    }

    private static byte[] GenerateKey()
    {
        byte[] keyBytes = new byte[32];
        RandomNumberGenerator.Fill(keyBytes);
        return keyBytes;
    }

    private static int GenerateRandomValue(int range)
    {
        return RandomNumberGenerator.GetInt32(range);
    }
}
