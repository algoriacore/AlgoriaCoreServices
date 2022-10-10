using System.Collections.Generic;

namespace AlgoriaCore.Application.BaseClases.Dto
{
    public class PagedResultDto<T>
        where T : class
    {
        private readonly int _totalCount = 0;
        private readonly List<T> _items;

        public PagedResultDto(int totalCount, List<T> items)
        {
            _totalCount = totalCount;
            _items = items;
        }

        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
        }

        public List<T> Items
        {
            get
            {
                return _items;
            }
        }
    }
}
