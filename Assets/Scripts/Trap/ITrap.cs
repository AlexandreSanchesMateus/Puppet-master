using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public interface ITrap
	{
		public void Activate ();
		public void Reset ();
	}
}