using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.BaseClases.Dto
{
	internal class StatusQueryDto<T>
	{
		public T Id { get; set; }
		public string Description { get; set; }
	}

	internal class StatusQueryDto : StatusQueryDto<string>
	{

	}
}
