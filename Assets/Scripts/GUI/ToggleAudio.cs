using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YleSearch
{
	[RequireComponent (typeof(AudioSource))]
	public class ToggleAudio : MonoBehaviour
	{

		[SerializeField]
		private float alternatePitch = 0.7f;

		private AudioSource audioSource;

		private void Start ()
		{
			audioSource = GetComponent<AudioSource> ();
		}

		public void PlayEffectAlternatingPitch (bool isTrue)
		{
			if (isTrue) {
				audioSource.pitch = 1;
			} else {
				audioSource.pitch = alternatePitch;
			}
			audioSource.Play ();
		}

		public void PlayEffect ()
		{
			audioSource.Play ();
		}

	}
}
