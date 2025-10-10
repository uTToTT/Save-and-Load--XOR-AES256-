public interface ICipherProvider 
{
    string Encrypt(string text);
    string Decrypt(string text);
}
