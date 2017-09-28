using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;

namespace YleSearch
{
	public static class ProgramParser
	{
		
		public static ProgramList ProgramListFromJSON (string rawJson)
		{
			return JsonUtility.FromJson<ProgramList> (rawJson);
		}

		//Preference for Finnish, then English, then Swedish
		public static string GetTitle(Program program){
			if (program.title.fi == null) {
				if (program.title.en == null) {
					if (program.title.sv == null) {
						return "Title Unknown";
					} else {
						return program.title.sv;
					}
				} else {
					return program.title.en;
				}
			} else {
				return program.title.fi;
			}
		}

		public static string GetDescription (Program program)
		{
			if (program.description.fi == null) {
				if (program.description.en == null) {
					if (program.description.sv == null) {
						return "No description available.";
					} else {
						return program.description.sv;
					}
				} else {
					return program.description.en;
				}
			} else {
				return program.description.fi;
			}
		}

		public static string GetReadableDate (string rawDate)
		{
			DateTime result = new DateTime ();
			try {
				DateTime.TryParse (rawDate, out result);
			} catch (Exception e) {
				Debug.Log ("Date Parsing Failed :" + e.Message);
				return "Unknown";
			}
			return result.ToShortDateString ();
		}

		public static string GetReadableDuration (string rawDuration)
		{
			TimeSpan timeSpan = new TimeSpan ();
			try {
				timeSpan = System.Xml.XmlConvert.ToTimeSpan (rawDuration);
			} catch (Exception e) {
				Debug.Log ("Duration Parsing Failed :" + e.Message);
				return "Unknown";
			}
			return timeSpan.Hours + "h " + timeSpan.Minutes  + "m " + timeSpan.Seconds + "s";
		}

	}
}
