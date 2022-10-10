using AlgoriaCommon;
using AlgoriaCore.Domain.Session;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model
{
    public class SessionContext: IAppSession
    {
		private readonly WriteOnce<int?> _wtenantId = new WriteOnce<int?>();
		private readonly WriteOnce<string> _wtenancyName = new WriteOnce<string>();
        private readonly WriteOnce<long?> _wuserId = new WriteOnce<long?>();
        private readonly WriteOnce<string> _wuserName = new WriteOnce<string>();
        private readonly WriteOnce<bool> _wisImpersonalized = new WriteOnce<bool>();
        private readonly WriteOnce<long?> _wimpersonalizerUserId = new WriteOnce<long?>();
        private readonly WriteOnce<string> _wtimeZone = new WriteOnce<string>();

        public int? TenantId
		{
			get { return _wtenantId; }
			set { _wtenantId.Value = value; }
		}

		public string TenancyName
        {
            get { return _wtenancyName; }
            set { _wtenancyName.Value = value; }
        }

        public long? UserId {
            get { return _wuserId; }
            set { _wuserId.Value = value; }
        }

        public string UserName
        {
            get { return _wuserName; }
            set { _wuserName.Value = value; }
        }

        public bool IsImpersonalized
        {
            get { return _wisImpersonalized; }
            set { _wisImpersonalized.Value = value; }
        }

        public long? ImpersonalizerUserId
        {
            get { return _wimpersonalizerUserId; }
            set { _wimpersonalizerUserId.Value = value; }
        }

        public string TimeZone
        {
            get { return _wtimeZone; }
            set { _wtimeZone.Value = value; }
        }
    }
}
