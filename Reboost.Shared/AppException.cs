namespace Reboost.Shared
{
    public class AppException: System.Exception
    {
        public ErrorCode Code;
        
        public AppException(ErrorCode code, string message): base(message) {
            Code = code;
        }
        public AppException(ErrorCode code, string generalMessage, string detailMessage): base(generalMessage)
        {
            Code = code;
            Data.Add("Ex", detailMessage);
        }
    }
}
