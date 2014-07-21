using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading;

public class Hacks : MonoBehaviour
{
	public float speed = 5f;
	private Vector3 moveDirection = Vector3.zero;
	private bool failure = false;
	private float scrHeight = (float)Screen.height;
	private float scrWidth = (float)Screen.width;
	public Transform target;
	private string guid;
	private GameObject helpLight = new GameObject();
	//private string path = @"c:\Users\Public\Desktop\Methods.txt";
	//StreamWriter sw;
	private MonoBehaviour[] players;
	private MonoBehaviour[] vehicles;
	//Players
	public static string fieldNetworkUserList = System.Text.Encoding.Default.GetString(new byte[] { 0xE0, 0xA1, 0xB2 });
	public static string fieldNetworkPlayerFromNetworkUser = System.Text.Encoding.Default.GetString(new byte[] { 0xE0, 0xA1, 0x83 });
	public static string fieldSpawnPlayerInstance = System.Text.Encoding.Default.GetString(new byte[] {
		0xE0,
		0xA3,
		0x8C
	});
	public static string fieldNetworkUserGameObject = System.Text.Encoding.Default.GetString(new byte[] {
		0xE0,
		0xA1,
		0x82
	});
	public static string fieldEquipmentFromPlayer = System.Text.Encoding.Default.GetString(new byte[] {0xE0, 0xA3, 0x83});
	public static string fieldIDFromNetworkUser = System.Text.Encoding.Default.GetString(new byte[] {0xE0, 0xA2, 0xBD});
	public static string fieldNetworkUserFromPlayer = System.Text.Encoding.Default.GetString(new byte[] {0xE0, 0xA2, 0x86});

	public static string fieldInventoryPlayer = System.Text.Encoding.Default.GetString(new byte[] { 0xE0, 0xA1, 0xAE });
//Chat
	public static string fieldOneNetworkChat = System.Text.Encoding.Default.GetString(new byte[] { 0xC2, 0x94 });
//Vehicles
	public static string fieldVehicleList = System.Text.Encoding.Default.GetString(new byte[] { 0xE0, 0xA2, 0x96 });
	public static string fieldVehiclePassengers = System.Text.Encoding.Default.GetString(new byte[] { 0xE0, 0xA3, 0x9F });
	public static string fieldPlayerVehicle = System.Text.Encoding.Default.GetString(new byte[] {0x10});
	public static string fieldPlayerLife = System.Text.Encoding.Default.GetString(new byte[] {0xE0, 0xA2, 0x91});
	public static string fieldVehicleHealth = System.Text.Encoding.Default.GetString(new byte[] {0xE0, 0xA2, 0xBC});
	public static string methodSetWrecked = System.Text.Encoding.Default.GetString(new byte[] {0xE0, 0xA0, 0x89});
//Barricades
	public static string methodSpawnBarricade = System.Text.Encoding.Default.GetString(new byte[] {0x18});
	public static string fieldObjectStructure = System.Text.Encoding.Default.GetString(new byte[] {0xE0, 0xA2, 0xB2});

	private int playerSelected = 0;
	private bool hitinsert = false;
	string itemID = "Identifier";
	private string s;
	private string id;
	Stopwatch stopwatch = new Stopwatch();

	private void DrawLabel(Vector3 point, string label, float d2)
	{
		Vector2 vector = Camera.main.WorldToScreenPoint(point);
		float num = this.distanceCalc(point);
		if (d2 > num)
		{
			GUI.Label(new Rect(vector.x - 50f, this.scrHeight - vector.y, 100f, 70f), label);
		}
	}

	private float distanceCalc(Vector3 point)
	{
		return Vector3.Distance(Camera.main.transform.position, point);
	}

	static MethodBase sendChat = typeof(NetworkChat).GetMethod("sendChat");

	public static void sendChathook(string message)
	{
		//sendChat.Invoke(null, new object[] {message + " nigger"});
	}

	private void Start()
	{
		//sw = File.CreateText(path);
		HostData[] array = MasterServer.PollHostList();
		HostData[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			HostData hostData = array2[i];
			hostData.playerLimit = 30;
		}
		Light light = this.helpLight.AddComponent<Light>();
		light.type = (LightType)2;
		light.range = 10000f;
		light.intensity = 1f;
		light.color = Color.white;
		this.helpLight.light.enabled = false;
		UnityEngine.Object.DontDestroyOnLoad(this.helpLight);
	}

	private void Update()
	{
		Application.targetFrameRate = 20;
		if (Input.GetKeyDown("9") || Input.GetKeyDown("insert"))
		{
			if(!hitinsert){hitinsert = true;}
			else{hitinsert=false;}
			FieldInfo[] fi = typeof(NetworkUser).GetFields();
			string path = @"C:\Users\Public\Desktop\Fields.txt";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			TextWriter tw = new StreamWriter(path, true);
			for (int x = 0; x < fi.Length; x++)
			{
				string name = fi[x].Name.ToString();
				byte[] nameBytes = System.Text.Encoding.Default.GetBytes(name);
				tw.Write(name + ": ");
				for (int i = 0; i < nameBytes.Length; i++)
				{
					tw.Write(String.Format("{0:X} ", nameBytes[i]));
				}
				tw.WriteLine();
			}
			tw.Close();
		}
		if (s != (string)typeof(NetworkChat).GetField(fieldOneNetworkChat).GetValue(null).ToString())
		{
			s = (string)typeof(NetworkChat).GetField(fieldOneNetworkChat).GetValue(null).ToString();
			if (s.Contains("nig"))
			{
			
			}
		}
		//BreakPoint New Reflection Test

	}

	private void OnGUI()
	{
		if (hitinsert)
		{
			GUI.color = Color.magenta;
			try
			{
				itemID = GUI.TextField(new Rect(0, 0, 100, 20), itemID, 25);
				Array array = getNetworkUserList();
				players = new MonoBehaviour[array.Length];
				for (int x = 0; x < array.Length; x++)
				{
					GameObject obj = (GameObject)typeof(NetworkUser).GetField(fieldNetworkUserGameObject).GetValue(array.GetValue(x));
					MonoBehaviour player = obj.GetComponent<Player>();
					players[x] = player;
					if (GUI.Button(new Rect(100f, 0f + (x * 20), 130f, 20f), players[x].name))
					{
						playerSelected = x;
					}

				}
				s = (string)typeof(NetworkChat).GetField(fieldOneNetworkChat).GetValue(null).ToString();
			} catch (Exception e)
			{
				string path = @"C:\Users\Public\Desktop\Failures.txt";
				TextWriter tw = new StreamWriter(path, true);
				tw.Write(e.StackTrace);
				tw.Close();
			}
			if (GUI.Button(new Rect(400f, 0, 100f, 20f), "Zombie Panic"))
			{
				for (int x = 0; x < 15; x++)
				{
					typeof(SpawnAnimals).GetMethod("spawn").Invoke(null, null);
				}
			}
			if (GUI.Button(new Rect(400f, 20f, 250f, 20f), "Spawn Item For: " + players[playerSelected].name))
			{
				object inv = typeof(Player).GetField(fieldInventoryPlayer).GetValue(players[playerSelected]);
				typeof(SpawnItems).GetMethod("spawnItem").Invoke(null, new object[] {int.Parse(itemID),1,players[playerSelected].transform.position});
			}
			if (GUI.Button(new Rect(400f, 40f, 250f, 20f), "Spawn Vehicle For: " + players[playerSelected].name))
			{
					string name = itemID + UnityEngine.Random.Range(0, 2);
					//SpawnVehicles.create(itemID + UnityEngine.Random.Range(0, 2), 100, 100, players[playerSelected].transform.position + Vector3.back, base.transform.rotation * Quaternion.Euler(-90f, 0f, 0f), new Color(1f, 1f, 1f));
					GameObject gameObject = (GameObject)Network.Instantiate(Resources.Load("Prefabs/Vehicles/" + name),players[playerSelected].transform.position, base.transform.rotation * Quaternion.Euler(-90f, 0f, 0f), 0);
					Vehicle vehicle = gameObject.GetComponent<Vehicle>();
					gameObject.name = name;
					getVehicleList().Add(vehicle);
					typeof(Vehicle).GetMethod(methodSetWrecked).Invoke(vehicle, new object[]{false});
			}
			if (GUI.Button(new Rect(400f, 60f, 250f, 20f), "Fill Vehicle For: " + players[playerSelected].name))
			{
				Vehicle vehicle = (Vehicle)typeof(Player).GetField(fieldPlayerVehicle).GetValue(players[playerSelected]);
				vehicle.fill(100);
			}
			if (GUI.Button(new Rect(400f, 80f, 250f, 20f), "Barricade"))
			{
				ExplosionTool.explode(players[playerSelected].transform.position, 20f, -20);
				GameObject soj = (GameObject)typeof(SpawnStructure).GetField(fieldObjectStructure).GetValue(null);

				SpawnStructures.placeStructure(100, );
			}
			if(id!=null)
			{
				GUI.Label(new Rect(400f, 100f, 250f, 20f), id);

			}
			GUI.Label(new Rect(200f, 80f, 200f, 40f), players[playerSelected].transform.position.ToString());
		}
		if (failure)
		{
			GUI.Label(new Rect(200f, 40f, 200f, 40f), "Fail");
		}

		//GUI.Label(new Rect(200f, 40f, 200f, 40f), "Start");
		//if(this.haschatted)
		{	
			//this.haschatted=false;

		}

		// get all public static methods of MyClass type
		/*				try
		{
			MethodBase m  = typeof(NetworkChat).GetMethod(s);
			ParameterInfo[] parameterInfos = m.GetParameters();
			GUI.Label(new Rect(200f, 40f, 200f, 40f), parameterInfos.Length.ToString());
		}
		catch(Exception e)
		{
			GUI.Label(new Rect(200f, 40f, 200f, 40f), e.GetType().FullName);
		}*/
		/*Array.Sort(methodInfos,
		delegate(MethodInfo methodInfo1, MethodInfo methodInfo2)
		{
			return methodInfo1.Name.CompareTo(methodInfo2.Name); 
		});
// write method names	
		int x = 0;
		foreach (MethodInfo methodInfo in methodInfos)
		{
			//sw.WriteLine(methodInfo.Name);
			GUI.Label(new Rect(200f, x+200, 200f, 40f), methodInfo.Name);
			ParameterInfo[] parameterInfos = methodInfo.GetParameters();
			/*						foreach(ParameterInfo parameterInfo in parameterInfos)
			{
				GUI.Label(new Rect(400f, x+200, 200f, 60f), methodInfo.GetParameters());
			}
			sw.Close();*/
			/*x+=15;
		}*/
	}

	public Array getNetworkUserList()
	{
		FieldInfo fieldnu = typeof(NetworkUserList).GetField(fieldNetworkUserList);
		object gl = fieldnu.GetValue(null);
		Type listType = gl.GetType();
		return (Array)listType.GetMethod("ToArray").Invoke(gl, null);
	}
	public List<Vehicle> getVehicleList()
	{
		return (List<Vehicle>) typeof(SpawnVehicles).GetField(fieldVehicleList).GetValue(null);
	}
	public MethodBase UKNetworkChatMethod()
	{
		Byte[] b = new byte[] { 0xE0, 0xA2, 0x8D };
		String s = System.Text.Encoding.Default.GetString(b);
		return(typeof(NetworkChat).GetMethod(s));
	}

	public NetworkPlayer getNetworkPlayer(object NetworkUser)
	{
		return (NetworkPlayer)typeof(NetworkUser).GetField(fieldNetworkPlayerFromNetworkUser).GetValue(NetworkUser);
	}

	public void sendMessage(string message)
	{
		object[] o = new object[]{ message };
		typeof(NetworkChat).GetMethod("sendChat").Invoke(null, o);
	}
}

