using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YleSearch
{
	public class EnableDisableGameObject : MonoBehaviour
	{
		[SerializeField]
		private GameObject target = null;

		public void ToggleEnabled ()
		{
			if (target.activeInHierarchy) {
				target.SetActive (false);
			} else {
				target.SetActive (true);
			}
		}

	}
}
