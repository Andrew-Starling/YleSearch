using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace YleSearch
{
	public class SetItemViewContents : MonoBehaviour
	{
		[SerializeField]
		private Text titleText = null;
		[SerializeField]
		private Text descriptionText = null;
		[SerializeField]
		private GameObject thumbnail = null;
		[SerializeField]
		private GameObject thumbnailLoading = null;
		[SerializeField]
		private GameObject onDemand = null;
		[SerializeField]
		private Text category = null;
		[SerializeField]
		private GameObject publicationEventPrefab = null;
		[SerializeField]
		private GameObject itemViewContent = null;

		private Program program;
		private UnityEngine.UI.Image thumbnailImage;
		private List<GameObject> publicationEvents;

		private void Start ()
		{
			thumbnailImage = thumbnail.GetComponent<UnityEngine.UI.Image> ();
			publicationEvents = new List<GameObject> ();
		}

		public void SetProgram (GameObject listItem)
		{
			program = listItem.GetComponent<ProgramContainer> ().GetProgram ();
			titleText.text = ProgramParser.GetTitle (program);
			SetThumbnail ();
			descriptionText.text = ProgramParser.GetDescription (program);
			SetOnDemand ();
			SetCategory ();
			ClearPublicationEvents ();
			SetPublicationEvents ();
		}

		private void ClearPublicationEvents ()
		{
			foreach (GameObject loopObject in publicationEvents) {
				Destroy (loopObject);
			}
			publicationEvents = new List<GameObject> ();
		}

		private void SetPublicationEvents ()
		{
			for (int loop = 0; loop < program.publicationEvent.Length; loop++) {
				GameObject newPublicationEvent = Instantiate (publicationEventPrefab, itemViewContent.transform);
				newPublicationEvent.GetComponent<SetPublicationEventData> ().SetPublicationEvent (program.publicationEvent [loop]);
				publicationEvents.Add (newPublicationEvent);
			}
		}

		private void SetOnDemand ()
		{
			bool isOnDemand = false;
			for (int loop = 0; loop < program.publicationEvent.Length; loop++) {
				if (program.publicationEvent [loop].type == "OnDemandPublication") {
					isOnDemand = true;
				}
			}
			if (isOnDemand) {
				onDemand.SetActive (true);
			} else {
				onDemand.SetActive (false);
			}
		}

		private void SetCategory ()
		{
			if (program.type == "TVProgram") {
				category.text = "TV";
			} else if (program.type == "RadioProgram") {
				category.text = "Radio";
			}
		}

		private void SetThumbnail ()
		{
			//stop any images already loading
			StopAllCoroutines ();
			thumbnail.SetActive (false);
			thumbnailImage.sprite = null;
			if (program.image.available) {
				thumbnailLoading.SetActive (true);
				Debug.Log ("Trying to load image");
				string url = ConstructURL ();
				StartCoroutine (GetImage (url));
			} else {
				thumbnailLoading.SetActive (false);
				Debug.Log ("No image available");
			}
		}

		private string ConstructURL ()
		{
			//Requires no authentication
			string url = "http://images.cdn.yle.fi/image/upload/";
			url += "w_" + Screen.width + ",c_fit";
			url += "/" + program.image.id + ".png";
			Debug.Log (url);
			return url;
		}

		private IEnumerator GetImage (string url)
		{
			UnityWebRequest request = UnityWebRequest.GetTexture (url);

			yield return request.Send ();

			if (request.isError) {
				Debug.Log ("Error downloading image");
				Debug.Log (request.error);
			} else {
				Debug.Log ("Loaded image");
				Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
				if (texture == null) {
					Debug.Log ("Image is null");
				} else {
					thumbnailLoading.SetActive (false);
					thumbnail.SetActive (true);
					Rect rect = new Rect (0, 0, texture.width, texture.height);
					Sprite newSprite = Sprite.Create (texture, rect, Vector2.zero);
					thumbnailImage.sprite = newSprite;
					thumbnailImage.preserveAspect = true;
				}
			}
		}

	}
}
