using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

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

    private DeobfuscatedMembers deobMems;

	//Players
    public static string fieldNetworkUserList = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa2, 0x8e });
    public static string fieldNetworkPlayerFromNetworkUser = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa2, 0xbF });
    public static string fieldSpawnPlayerInstance = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa3, 0xb8 });
    public static string fieldNetworkUserGameObject = System.Text.Encoding.Default.GetString(new byte[] { 0xc2, 0x83 });
    public static string fieldEquipmentFromPlayer = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa2, 0xb9 });
    public static string fieldIDFromNetworkUser = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa3, 0xb2 });
    public static string fieldNetworkUserFromPlayer = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa0, 0x8b });

    public static string fieldInventoryPlayer = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa0, 0xac });
    //Chat
    public static string fieldNetworkChatLastSaid = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa3, 0x82 });
    //Vehicles
    public static string fieldVehicleList = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa1, 0xb7 });
    public static string fieldVehiclePassengers = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa1, 0xb9 });
    public static string fieldPlayerVehicle = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa1, 0x89 });
    public static string fieldPlayerLife = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa0, 0x9a });
    public static string fieldVehicleHealth = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa2, 0x82 });
    //public static string methodSetWrecked = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa2, 0x9c });
    //Barricades
    //public static string methodSpawnBarricade = "";
    public static string fieldObjectStructure = System.Text.Encoding.Default.GetString(new byte[] { 0xe0, 0xa2, 0xb0 });

	private int playerSelected = 0;
	private bool hitinsert = false;
	string itemID = "Identifier";
	private string s;
	private string id;
	Stopwatch stopwatch = new Stopwatch();

	static MethodBase sendChat = typeof(NetworkChat).GetMethod("sendChat");

	public static void sendChathook(string message)
	{
		//sendChat.Invoke(null, new object[] {message + " nigger"});
	}

	private void Start()
	{
        dumpMethodsAndFields();
        try
        {
            deobMems = Deobfuscator.deobfuscate(Assembly.GetAssembly(typeof(Player)));
        }
        catch (Exception e)
        {
            //printError(e);
        }
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
		Application.targetFrameRate = 120;
		if (Input.GetKeyDown("9") || Input.GetKeyDown("insert"))
		{
			if(!hitinsert){hitinsert = true;}
			else{hitinsert=false;}
		}
		if (s != (string)typeof(NetworkChat).GetField(fieldNetworkChatLastSaid).GetValue(null).ToString())
		{
            s = (string)typeof(NetworkChat).GetField(fieldNetworkChatLastSaid).GetValue(null).ToString();
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
			itemID = GUI.TextField(new Rect(0, 0, 100, 20), itemID, 25);
			Array array = deobMems.getNetworkUserList().ToArray();//getNetworkUserList();
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
            s = (string)typeof(NetworkChat).GetField(fieldNetworkChatLastSaid).GetValue(null).ToString();
			if (GUI.Button(new Rect(400f, 0, 100f, 20f), "Zombie Panic"))
			{
				for (int x = 0; x < 15; x++)
				{
					typeof(SpawnAnimals).GetMethod("spawn").Invoke(null, null);
				}
			}

            bool canIndexPlayer = true;//playerSelected < players.Length && players[playerSelected] != null;

            if (GUI.Button(new Rect(400f, 20f, 250f, 20f), "Spawn Item For Player") && canIndexPlayer)
			{
				object inv = typeof(Player).GetField(fieldInventoryPlayer).GetValue(players[playerSelected]);
				typeof(SpawnItems).GetMethod("spawnItem").Invoke(null, new object[] {int.Parse(itemID),1,players[playerSelected].transform.position});
			}
            if (GUI.Button(new Rect(400f, 40f, 250f, 20f), "Spawn Vehicle For Player") && canIndexPlayer)
			{
					string name = itemID + UnityEngine.Random.Range(0, 2);
					//SpawnVehicles.create(itemID + UnityEngine.Random.Range(0, 2), 100, 100, players[playerSelected].transform.position + Vector3.back, base.transform.rotation * Quaternion.Euler(-90f, 0f, 0f), new Color(1f, 1f, 1f));
					GameObject gameObject = (GameObject)Network.Instantiate(Resources.Load("Prefabs/Vehicles/" + name),players[playerSelected].transform.position, base.transform.rotation * Quaternion.Euler(-90f, 0f, 0f), 0);
					Vehicle vehicle = gameObject.GetComponent<Vehicle>();
					gameObject.name = name;
					getVehicleList().Add(gameObject);
					//typeof(Vehicle).GetMethod(methodSetWrecked).Invoke(vehicle, new object[]{false});
			}
            if (GUI.Button(new Rect(400f, 60f, 250f, 20f), "Fill Vehicle For Player") && canIndexPlayer)
			{
				Vehicle vehicle = (Vehicle)typeof(Player).GetField(fieldPlayerVehicle).GetValue(players[playerSelected]);
				vehicle.fill(100);
			}
			if (GUI .Button(new Rect(400f, 80f, 250f, 20f), "Explode Player"))
			{
				ExplosionTool.explode(players[playerSelected].transform.position, 20f, 100);
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
	}

	public Array getNetworkUserList()
	{
		FieldInfo fieldnu = typeof(NetworkUserList).GetField(fieldNetworkUserList);
		object gl = fieldnu.GetValue(null);
		Type listType = gl.GetType();
        return (Array)listType.GetMethod("ToArray").Invoke(gl, null);
	}
	public List<GameObject> getVehicleList()
	{
		return (List<GameObject>) typeof(SpawnVehicles).GetField(fieldVehicleList).GetValue(null);
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

    // Use this to make life 30% easier.
    private static void dumpMethodsAndFields()
    {
        string path = @"C:\Users\James\data\names.txt";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        TextWriter tw = new StreamWriter(path, true);
        try
        {
            Type[] types = Assembly.GetAssembly(typeof(Player)).GetTypes();
            foreach (Type type in types)
            {
                tw.WriteLine("\nClass " + type.Name + "\n\nFields:");
                FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                foreach (FieldInfo fi in fields)
                {
                    string name = fi.Name;
                    byte[] nameBytes = System.Text.Encoding.Default.GetBytes(name);
                    tw.Write(fi.FieldType + " " + name + ": ");
                    if (nameBytes.Length != 0)
                    {
                        tw.Write(String.Format("{0:x}", nameBytes[0]));
                        for (int i = 1; i < nameBytes.Length; i++)
                        {
                            tw.Write(String.Format(", {0:x}", nameBytes[i]));
                        }
                    }
                    else
                    {
                        tw.Write("LENGTHZERO");
                    }
                    tw.WriteLine();
                }
                tw.WriteLine("\nMethods:");
                MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                foreach (MethodInfo mi in methods)
                {
                    string name = mi.Name;
                    byte[] nameBytes = System.Text.Encoding.Default.GetBytes(name);
                    tw.Write(name + ": " + nameBytes[0]);
                    for (int i = 0; i < name.Length; i++)
                    {
                        tw.Write(String.Format(", {0:x}", nameBytes[i]));
                    }
                    tw.WriteLine();
                }
            }
        }
        catch (Exception e)
        {
            tw.WriteLine(e.GetType());
            tw.WriteLine(e.StackTrace);
        }
        finally
        {
            tw.Close();
        }
    }

    public static void printError(Exception e)
    {
		string path = @"C:\Users\Public\Desktop\Failures.txt";
        //File.Delete(path);
		TextWriter tw = new StreamWriter(path, true);
		tw.WriteLine(e.GetType());
        tw.WriteLine(e.StackTrace);
		tw.Close();
    }

    public static void print(string msg)
    {
        string path = @"C:\Users\Public\Desktop\Failures.txt";
        TextWriter tw = new StreamWriter(path, true);
        tw.WriteLine(msg);
        tw.Close();
    }
}

