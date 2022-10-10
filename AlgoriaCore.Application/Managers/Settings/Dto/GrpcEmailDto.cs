namespace AlgoriaCore.Application.Managers.Settings.Dto
{
    public class GrpcEmailDto
    {
        public bool SendConfiguration { get; set; }
        public string TenancyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
