namespace WhoIsThat.Handlers.Utils
{
    public class CustomWebClientFactory : ICustomWebClientFactory
    {
        public ICustomWebClient Create()
        {
            return new CustomWebClient();
        }
    }
}