using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YleSearch
{
	public class SetPublicationEventData : MonoBehaviour
	{
		[SerializeField]
		private Text type = null;
		[SerializeField]
		private Text date = null;
		[SerializeField]
		private Text duration = null;
		[SerializeField]
		private Text region = null;
		[SerializeField]
		private Text downloadable = null;

		private PublicationEvent publicationEvent;

		public void SetPublicationEvent (PublicationEvent newPublicationEvent)
		{
			publicationEvent = newPublicationEvent;
			if (publicationEvent.type == "OnDemandPublication") {
				type.text = "On Demand";
				date.text = "From: ";
				duration.text = "Duration: " + ProgramParser.GetReadableDuration (publicationEvent.media.duration);
				downloadable.text = "Downloadable";
			} else if (publicationEvent.type == "ScheduledTransmission") {
				type.text = "Live";
				date.text = "On: ";
				duration.text = "Duration: " + ProgramParser.GetReadableDuration (publicationEvent.duration);
				downloadable.text = "";
			}
			region.text = publicationEvent.region;
			date.text += ProgramParser.GetReadableDate (publicationEvent.startTime);
		}

		public PublicationEvent GetPublicationEvent ()
		{
			return publicationEvent;
		}

	}
}