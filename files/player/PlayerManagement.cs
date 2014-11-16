using UnityEngine;
using System.Collections;

public class PlayerManagement : Management {
	
	public GameObject listPrefab;//提供下拉菜单实例化	
	private DropDownList list;
	private GUIStyle listStyle = new GUIStyle();
	
	public GameObject prefabPlayer;
	private ArrayList player = new ArrayList();
	Player testp;
	
	
	// Use this for initialization
	void Start () {
		this.areaLabel = "人员信息";
		this.areaMinSize = new Vector2(300,25);
		list = (DropDownList)((GameObject)(Instantiate(listPrefab, Vector3.zero, Quaternion.identity))).GetComponent("DropDownList");
		if(null == list)
			print ("error in SystemSet.cs null == list");
		list.elementHeight = 100;
		listStyle.alignment = TextAnchor.MiddleLeft;
		listStyle.normal.textColor = Color.white;
		
		GameObject instance = (GameObject)Instantiate(prefabPlayer, new Vector3(15f, 0.5f, 15f), Quaternion.identity);
		testp = (Player)instance.GetComponent("Player");
		if(null == testp)
	  		print("null == testp");
		//testp.InitPlayerInfo(new Rect(30, 40, 300, 100));
//		playerInfoTabJS = gameObject.GetComponent("PlayerInfoTab");
//	  	if(null == playerInfoTabJS)
//	  		print("null == playerInfoTabJS");
	}
	
	// Update is called once per frame
	void Update () {
//		testp.PlayerPosition += new Vector3(1,1,1);
//		print (testp.PlayerPosition);
	}
	
	void OnGUI () {
		//testp.OnCreatGUI(new Rect(30, 40, 300, 100), null);
//		GUI.Box(new Rect (72,100,30,30), playerState_Static);
	}
	
	public void Show(int buttonSN = 0) {
		if(null == area)
			return ;
		list.AddDropDownList(areaLabel,area,listStyle);
		list.AddElement(null, null, null, null,testp.OnCreatGUI);
		list.AddElement(null, null, null, null,testp.OnCreatGUI);
		list.AddElement(null, null, null, null,testp.OnCreatGUI);
		list.AddElement(null, null, null, null,testp.OnCreatGUI);
		list.AddElement(null, null, null, null,testp.OnCreatGUI);
		list.AddElement(null, null, null, null,testp.OnCreatGUI);
	}
	
//	private GameObject CreatPlayer(Vector3 position, Quaternion rotation)
//	{
//		GameObject instance = (GameObject)Instantiate(prefabPlayer, position, rotation);
//		instance.AddComponent<CharacterController>();
//		instance.AddComponent("PlayerCtrl");
//		playerCtrlJS = instance.GetComponent("PlayerCtrl");
//	  	if(null == playerCtrlJS)
//	  		print("null == playerCtrlJS");
//		return instance;
//	}
}
