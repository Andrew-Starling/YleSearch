using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YleSearch
{
	public class ResetScrollView : MonoBehaviour
	{
		[SerializeField]
		private ScrollRect scroll = null;

		public void ResetToTop(){
			scroll.normalizedPosition = Vector2.one;
		}
	}
}
