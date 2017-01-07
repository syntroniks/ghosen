using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen
{
	/// <summary>
	/// 11 bits long
	/// </summary>
	public struct ArbitrationId
	{
		public uint Id;

		public ArbitrationId(uint _id) : this()
		{
			this.Id = _id;
		}
	}
}
