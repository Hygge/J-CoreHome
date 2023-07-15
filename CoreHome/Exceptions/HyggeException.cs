namespace CoreHome.Exceptions
{
    public class HyggeException : ApplicationException
    {
        public int code { set; get; }   
        public string message { set; get; }

        public HyggeException(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
        
        public HyggeException(string message)
        {
            this.code = 425;
            this.message = message;
        }
    }
}
