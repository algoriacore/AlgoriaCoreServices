namespace AlgoriaCore.Domain.Exceptions
{
    public static class AlgoriaCoreExceptionErrorCodes
    {
        public const string GeneralError = "-1001";
        public const string UserNoValid = "-1002";
        public const string UserLocked = "-1003";
        public const string UserMustChangePassword = "-1004";
        public const string UserUnauthorized = "-1005";

        public const string HostMode = "-1500";

        // Excepciones relacionadas a los Entities
        public const string EntityNotFound = "-2001";
        public const string EntityDuplicated = "-2002";

        // Excepciones relacionadas al registro de nuevos tenants
        public const string TenantDuplicatedTenancyName = "-3001";

        public const string RecordHasRelationships = "-4001";

        //Excepciones por validación
        public const string GeneralValidationException = "-5001";
    }
}
