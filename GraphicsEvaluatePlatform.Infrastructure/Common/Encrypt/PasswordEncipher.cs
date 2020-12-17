namespace GraphicsEvaluatePlatform.Infrastructure.Encrypt
{
    public class PasswordEncipher
    {
        IEncryptPasswordStrategy _strategy;
        public PasswordEncipher(EncryptType type = EncryptType.MD5)
        {
            _strategy = EncryptPasswordFactory.GetEncipher(type);
        }
        public  string Calc(string source)
        {
            return _strategy.EncryptFor(source);
        }
    }
}
