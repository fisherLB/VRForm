namespace GraphicsEvaluatePlatform.Infrastructure.Encrypt
{
    public class EncryptPasswordFactory
    {
        public static IEncryptPasswordStrategy GetEncipher(EncryptType type) 
        {
            switch (type)
            {
                case EncryptType.MD5:
                    return new MD5Algorithm();
                case EncryptType.SHA1:
                    return new SHA1Algorithm();
                default:
                    return new PlaintextAlgorithm();
            }
        }
    }
}
