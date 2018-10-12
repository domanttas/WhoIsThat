namespace WhoIsThat.Handlers.Utils
{
    public interface ICustomWebClient
    {
        byte[] DownloadData(string uri);
    }
}