﻿using AlgoriaCore.Application.Managers.Templates.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberForListResponse
    {
        public long Id { get; set; }
        public long Template { get; set; }
        public SecurityMemberType Type { get; set; }
        public string TypeDesc { get; set; }
        public long Member { get; set; }
        public string MemberDesc { get; set; }
        public SecurityMemberLevel Level { get; set; }
        public string LevelDesc { get; set; }
        public bool IsExecutor { get; set; }
        public string IsExecutorDesc { get; set; }
    }
}