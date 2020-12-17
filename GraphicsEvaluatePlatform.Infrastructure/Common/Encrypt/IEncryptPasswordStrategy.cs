namespace GraphicsEvaluatePlatform.Infrastructure.Encrypt
{
    public interface IEncryptPasswordStrategy
    {
        string EncryptFor(string source);
    }
}
