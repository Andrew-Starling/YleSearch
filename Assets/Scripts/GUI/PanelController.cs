using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YleSearch
{
	[RequireComponent (typeof(AudioSource))]
	public class PanelController : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] panels = null;

		private int currentPanelIndex;
		private GameObject currentPanel;
		private Animator animator;
		private AudioSource audioSource;

		private void Start ()
		{
			currentPanelIndex = 0;
			currentPanel = panels [currentPanelIndex];
			animator = currentPanel.GetComponent<Animator> ();
			audioSource = GetComponent<AudioSource> ();
		}

		private void Update ()
		{
				//android back button & desktop 'esc' trigger panel closing
				if (Input.GetKeyUp (KeyCode.Escape)) {
					Close ();
				}
		}

		public void Open ()
		{
			if (currentPanelIndex < panels.Length - 1) {
				currentPanelIndex++;
				IteratePanel ();
			}
			if (animator != null) {
				audioSource.pitch = 1.1f;
				audioSource.PlayDelayed(0.1f);
				animator.SetBool ("isOpen", true);
			}
		}

		public void Close ()
		{
			if (currentPanelIndex > 0) {
				if (animator != null) {
					audioSource.pitch = 1;
					audioSource.Play ();
					animator.SetBool ("isOpen", false);
				}
				currentPanelIndex--;
				IteratePanel ();
			}
		}

		private void IteratePanel ()
		{
			currentPanel = panels [currentPanelIndex];
			animator = currentPanel.GetComponent<Animator> ();
		}

		public GameObject GetCurrentPanel(){
			return currentPanel;
		}
	}
}
