using System.Text;

public class XORCipher : ICipherProvider
{
    private string _key;

    public XORCipher(string key) => _key = key;

    public string Encrypt(string text) => Cipher(text, _key);
    public string Decrypt(string text) => Cipher(text, _key);

    private string GetRepeatKey(string s, int n)
    {
        var r = s;
        while (r.Length < n)
        {
            r += r; // ref to stringbuilder
        }

        return r.Substring(0, n);
    }

    private string Cipher(string text, string key)
    {
        var currKey = GetRepeatKey(key, text.Length);
        var sb = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            sb.Append((char)(text[i] ^ currKey[i]));
        }

        return sb.ToString();
    }
}
