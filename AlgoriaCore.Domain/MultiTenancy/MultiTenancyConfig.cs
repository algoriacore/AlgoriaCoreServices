using AlgoriaCore.Domain.Interfaces.MultiTenancy;

namespace AlgoriaCore.Domain.MultiTenancy
{
    public class MultiTenancyConfig : IMultiTenancyConfig
    {
        private bool Enabled { get; set; }
        private string TenancyNameDefault { get; set; }

        public MultiTenancyConfig() {
            Enabled = true;
            TenancyNameDefault = "Default";
        }

        public bool IsEnabled() {
            return Enabled;
        }

        public string GetTenancyNameDefault()
        {
            return TenancyNameDefault;
        }
    }
}
