using System;

namespace AlgoriaCore.Application.Configuration
{
    public class EmailOptions
    {
        public EmailSendMethod SendMethod { get; set; }
        public EmailGrpcOptions Grpc { get; set; }
    }

    public enum EmailSendMethod: byte
    {
        Normal = 0,
        Grpc = 1
    }

    public class EmailGrpcOptions
    {
        public string Url { get; set; }
        public bool SendConfiguration { get; set; }
        public EmailHttpVersionOptions HttpVersion { get; set; }
    }

    public class EmailHttpVersionOptions
    {
        public int Major { get; set; }
        public int Minor { get; set; }
    }
}
