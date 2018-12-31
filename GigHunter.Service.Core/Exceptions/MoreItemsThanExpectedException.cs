using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigHunter.Service.Core.Exceptions
{
	public class MoreItemsThanExpectedException : Exception
	{
		public MoreItemsThanExpectedException()
		{
		}

		public MoreItemsThanExpectedException(string message) : base(message)
		{
		}

		public MoreItemsThanExpectedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
