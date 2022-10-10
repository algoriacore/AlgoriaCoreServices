namespace AlgoriaCore.WebUI.Middleware
{
    public class CustomExceptionDto
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
		public string Title { get; set; }
		public int StatusCode { get; set; }
	}
}
