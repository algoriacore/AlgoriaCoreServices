using AlgoriaCore.Domain.Interfaces.Entity;

namespace AlgoriaCore.Application.BaseClases.Dto
{
    public class PageListByDto : IPagedResult, ISortingResult
    {
        private int pageNumber;
        private int pageSize;

        public PageListByDto()
        {
            pageSize = 10;
            pageNumber = 1;
			IsPaged = true;
        }

        public string Sorting { get; set; }
        public string Filter { get; set; }


        public int? PageSize
        {
            get
            {
                return (int?)pageSize;
            }
            set
            {
                if (value == null || value == 0)
                {
                    pageSize = 10;
                }
                else
                {
                    pageSize = value.Value;
                }
            }
        }

        public int? PageNumber
        {
            get
            {
                return (int?)pageNumber;
            }
            set
            {
                if (value == null || value == 0)
                {
                    pageNumber = 1;
                } else 
                {
                    pageNumber = value.Value;
                }
            }
        }

        public bool IsPaged { get; set; }
    }
}
