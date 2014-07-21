using System;
using UnityEngine;

namespace RedmanInject
{
	public class Loader
	{
		public static GameObject load_object;
		public static void Load()
		{
			Loader.load_object = new GameObject();
			Loader.load_object.AddComponent<Hacks>();
			UnityEngine.Object.DontDestroyOnLoad(Loader.load_object);	
		}
	}
}

