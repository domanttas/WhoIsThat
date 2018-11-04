namespace WhoIsThat.Exceptions
{
    public class ManagerException
    {
        public string ErrorCode;

        public ManagerException() : base()
        {
        }

        public ManagerException(string code) : base()
        {
            ErrorCode = code;
        }
    }
}