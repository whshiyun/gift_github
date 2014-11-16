using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private ArrayList stateTextureList = new ArrayList();
	private Texture2D playerState;
	
	public GameObject prefabPlayer;
	public Texture2D headPortrait;
	
	private GameObject playerInstance;
	private Quaternion playerRotation = Quaternion.identity;
	private Vector3 playerPosition = new Vector3(0,0,0);
	
	private Rect infoRect = new Rect(0,0,0,0);
	private Rect buttonRect = new Rect(0,0,0,0);
	private Rect dataRect = new Rect(0,0,0,0);
	private Rect stateRect = new Rect(0,0,0,0);
	private const float interval = 3;
	private string dataStr = "";

	private State state = State.Static;
	public enum State{
		Static,
		Walk,
		Run,
		Collapse,
	};
	
	public Vector3 PlayerPosition {
		get {
			return playerPosition;
		}
		set {
			playerPosition = value;
			MoveTo(playerPosition);
		}
	}

	public State PlayerState {
		get {
			return state;
		}
		set {
			state = value;
			playerState = (Texture2D)stateTextureList[(int)state];
		}
	}
	
	// Use this for initialization
	void Start () {
		stateTextureList.Add (Resources.Load("playerStateImage/zhanli"));
		stateTextureList.Add (Resources.Load("playerStateImage/xingzou"));
		stateTextureList.Add (Resources.Load("playerStateImage/paobu"));
		stateTextureList.Add (Resources.Load("playerStateImage/wodao"));
		
		playerState = (Texture2D)stateTextureList[(int)state];
		
		playerPosition = gameObject.transform.position;
		playerRotation = gameObject.transform.rotation;
		
		GameObject playerInstance = (GameObject)Instantiate(prefabPlayer, playerPosition, playerRotation);
		playerInstance.AddComponent<CharacterController>();
		playerInstance.AddComponent("PlayerCtrl");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI () {
	}
	
	public bool OnCreatGUI(Rect rt, string str) {
		if((rt.width <= 3*interval) || (rt.height <= 2*interval) || (rt.width <= rt.height))
			return false;
		infoRect = rt;
		
		buttonRect.height = infoRect.height - 2*interval;
		buttonRect.width = buttonRect.height;
		buttonRect.x = infoRect.x + interval;
		buttonRect.y = infoRect.y + interval;
		
		dataRect.height = buttonRect.height;
		dataRect.width = infoRect.width - 3*interval - buttonRect.width;
		dataRect.x = buttonRect.x + buttonRect.width + interval;
		dataRect.y = buttonRect.y;
		
		stateRect.height = buttonRect.height/3f;
		stateRect.width = stateRect.height;
		stateRect.x = buttonRect.x + (buttonRect.height*2f)/3f ;
		stateRect.y = buttonRect.y;
		
		GUI.Button(buttonRect, headPortrait);
		dataStr = GUI.TextArea(dataRect, dataStr);
		GUI.Box(stateRect, playerState);
		return false;
	}
	
	private void MoveTo(Vector3 position) {
	}
	
	public void SetPosition () {
	}

}
