using UnityEngine;

namespace YleSearch
{
	//This class structure partially reflects the JSON structure retrieved from Yle

	[System.Serializable]
	public class ProgramList
	{
		public Program[] data;
		public Meta meta;
	}

	[System.Serializable]
	public class Program
	{
		public string id;
		public string type;
		public Title title;
		public Description description;
		public Image image;
		public PublicationEvent[] publicationEvent;
	}

	[System.Serializable]
	public class Meta
	{
		public int count;
		public string q;
	}

	[System.Serializable]
	public class Title
	{
		public string fi;
		public string en;
		public string sv;
	}	

	[System.Serializable]
	public class Description
	{
		public string fi;
		public string en;
		public string sv;
	}

	[System.Serializable]
	public class Image
	{
		public string id;
		public bool available;
	}

	[System.Serializable]
	public class PublicationEvent
	{
		public string type; //ondemand or broadcast
		public string startTime;
		public string duration;
		public string region;
		public Media media;
	}

	[System.Serializable]
	public class Media
	{
		public string id;
		public string duration;
		public bool downloadable;
		public bool available;
	}
}
