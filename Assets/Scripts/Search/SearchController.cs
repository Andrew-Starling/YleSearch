using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace YleSearch
{
	public class SearchController : MonoBehaviour
	{
		[SerializeField]
		private GameObject mainSearchPanel = null;
		[SerializeField]
		private GameObject loadingWheel = null;
		[SerializeField]
		[Tooltip ("Amount to add to list when scroling down")]
		private int targetResultOffset = 10;
		[SerializeField]
		[Tooltip ("Point on the y axis between 0 (bottom) and 1 (top).")]
		private float listViewUpdatePoint = 0.3f;
		[SerializeField]
		private PanelController panelController = null;
		[SerializeField]
		private ResetScrollView resetScrollView = null;
		[SerializeField]
		[Tooltip ("Prefab List Item UI")]
		private GameObject listItem = null;
		[SerializeField]
		private GameObject listViewContent = null;
		[SerializeField]
		private Text listViewResultCount = null;
		[SerializeField]
		private SetItemViewContents setItemViewContents = null;
		[SerializeField]
		private GameObject errorMessage = null;

		private string currentSearch;
		private bool isOnDemand;
		private string category;

		private ProgramList list;
		private List<GameObject> listItems;
		private bool isLoading;
		private bool isNewSearch;
		private int currentOffset;
		private int surplusOffset;
		private int amountAddedToList;

		private void Start ()
		{
			listItems = new List<GameObject> ();
			isLoading = false;
			loadingWheel.SetActive (false);
			errorMessage.SetActive (false);
			isOnDemand = true;
			category = "program";
		}

		public void StartSearch ()
		{
			if (!isLoading && panelController.GetCurrentPanel() == mainSearchPanel) {
				isNewSearch = true;
				loadingWheel.SetActive (true);
				ClearListView ();
				currentOffset = 0;
				amountAddedToList = 0;
				AddResults ();
			}
		}

		public void OnListViewMoved (Vector2 input)
		{
			if (input.y < listViewUpdatePoint && !isLoading) {
				Debug.Log ("List View Updated with position: " + input);
				amountAddedToList = 0;
				//only update if there are still results to load
				if (currentOffset < list.meta.count) {
					AddResults ();
				}

			}
		}

		private void AddResults ()
		{
			surplusOffset = 0;
			isLoading = true;
			string url = BuildUrl (currentSearch, currentOffset);
			StartCoroutine (GetJSON (url));
		}

		private string BuildUrl (string search, int offset)
		{
			string url = "https://external.api.yle.fi/v1/programs/items.json?";
			url += "app_id=885637d3";
			url += "&app_key=891b2ceeebf6b3b5333ab86d6b0cd5f4";
			url += "&limit=" + targetResultOffset;
			url += "&type=" + category;
			url += "&q=" + search;
			url += "&offset=" + offset;
			if (isOnDemand) {
				url += "&availability=ondemand";
			}
			return url;
		}

		private IEnumerator GetJSON (string url)
		{
			UnityWebRequest request = UnityWebRequest.Get (url);
			Debug.Log (url);

			request.SetRequestHeader ("Content-Type", "application/json");
			yield return request.Send ();


			if (request.isError) {
				Debug.Log ("json request error");
				Debug.Log (request.error);
				loadingWheel.SetActive (false);
				errorMessage.SetActive (true);
			} else {
				Debug.Log (request.downloadHandler.text);
				list = ProgramParser.ProgramListFromJSON (request.downloadHandler.text);
				if (list.data.Length > 0) {
					PopulateListView ();
					if (amountAddedToList == targetResultOffset || list.meta.count < targetResultOffset) {
						Debug.Log ("Total amount added to list view: " + amountAddedToList);
						SetButtonEvents ();
						if (isNewSearch) {
							listViewResultCount.text = list.meta.count + " Results";
							panelController.Open ();
							isNewSearch = false;
							loadingWheel.SetActive (false);
						}
					}
				}
			}
			//Prevent the ListView from updating twice by waiting a frame
			yield return new WaitForEndOfFrame ();
			isLoading = false;
		}

		private void ClearListView ()
		{
			foreach (GameObject loopObject in listItems) {
				Destroy (loopObject);
			}
			listItems = new List<GameObject> ();
		}

		private void PopulateListView ()
		{
			foreach (Program loopProgram in list.data) {
				surplusOffset++;
				if (loopProgram.type.ToLower () == category || category == "program") {
					if (amountAddedToList < targetResultOffset) {
						//add item to scrollView
						GameObject loopListItem = Instantiate (listItem, listViewContent.transform);
						//add data container to item and set data on container
						loopListItem.AddComponent<ProgramContainer> ().SetProgram (loopProgram);
						//set item title
						//Some Finnish titles aren't available: in that case, the english or swedish is used
						loopListItem.transform.GetComponentInChildren<Text> ().text = ProgramParser.GetTitle(loopProgram);

						//store item
						listItems.Add (loopListItem);
						amountAddedToList++;
					} else {
						//once desired amount of results are added, stop this function
						Debug.Log (amountAddedToList + " added to listview");
						currentOffset += surplusOffset;
						return;
					}
				}
			}

			Debug.Log (amountAddedToList + " added to listview");
			currentOffset += surplusOffset;

			//if all results have been added, stop trying to add more
			if (list.meta.count < targetResultOffset) {
				return;
			}

			//add another round of results to reach desired amount
			if (amountAddedToList < targetResultOffset) {
				AddResults ();
			}
		}

		private void SetButtonEvents ()
		{
			foreach (GameObject listItem in listItems) {
				//add listeners to item button for opening ItemView panel and setting panel contents
				listItem.GetComponentInChildren<Button> ().onClick.AddListener (() => setItemViewContents.SetProgram (listItem));
				listItem.GetComponentInChildren<Button> ().onClick.AddListener (() => panelController.Open ());
				listItem.GetComponentInChildren<Button> ().onClick.AddListener (() => resetScrollView.ResetToTop ());

			}
		}

		//These functions are called by UI events:

		public void SetCategory (int index)
		{
			switch (index) {
			case(0):
				category = "program";
				break;
			case(1):
				category = "tvprogram";
				break;
			case(2):
				category = "radioprogram";
				break;
			}
		}

		public void SetCurrentSearch (string newSearch)
		{
			this.currentSearch = newSearch;
		}

		public void SetIsOnDemand (bool newOnDemand)
		{
			this.isOnDemand = newOnDemand;
		}

	}
}