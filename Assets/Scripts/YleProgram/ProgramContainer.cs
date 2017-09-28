using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YleSearch
{
	public class ProgramContainer : MonoBehaviour
	{
		private Program program;

		public void SetProgram(Program newProgram)
		{
			program = newProgram;
		}

		public Program GetProgram()
		{
			return program;
		}
	}
}
