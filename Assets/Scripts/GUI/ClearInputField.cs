using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YleSearch
{
	public class ClearInputField : MonoBehaviour
	{

		[SerializeField]
		private InputField inputField = null;

		public void ClearInput ()
		{
			inputField.text = "";
		}
	}
}