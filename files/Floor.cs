using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {
	
	public Vector3 floorScope = new Vector3(512,1,512);
	public int upFloorNum = 0;
	public int downFloorNum = 0;
	public float baseFloorPositionY = 0;//参考地平线的Y值
	
	public bool floorNumChanged = false;//发出楼层数量改变消息，为了节约时间，暂时这样实现,后期修改
	
	//private ArrayList floorUpHeightList = new ArrayList();
	//private ArrayList floorDownHeightList = new ArrayList();
	public ArrayList floorInstanceList = new ArrayList();
	
	public GameObject prefabFloor;

	private Texture2D mapTextureNormal;//正常地图
	private Texture2D mapTextureBorder;//只留边框地图
	
	
	public MyDrawMap myDrawMap;
	
	public class FloorInstance{
		public GameObject instanceTop;
		public GameObject instancebottom;
		public float floorHeight = 3;//表示该楼层高度，如果为负数，则表示该层为地下楼层
		public int floorNum = 0;//表示该楼层为第几层楼
		public GameObject prefabFloor;
		public Vector3 position;
		public Quaternion rotation;
		public int showState = 0;
		
		private float ColorA = 0;
		private Texture2D floorTextureShow;//正常地图
		private Texture2D floorTextureHide;//只留边框地图
		
		private float backUpColorA;
		private Texture backUpTexture;
		
		public FloorInstance(GameObject prefabFloor, Vector3 position, Quaternion rotation, int num, float height,Texture2D textureShow, Texture2D textureHide) {
//			instanceTop = (GameObject)Instantiate(prefabFloor, position, rotation);
//			rotation.x = 180;
//			instancebottom = (GameObject)Instantiate(prefabFloor, position, rotation);
//			instancebottom.renderer.material.mainTextureScale = new Vector2(1,-1);
			this.prefabFloor = prefabFloor;
			this.position = position;
			this.rotation = rotation;
			
			ColorA = prefabFloor.renderer.material.color.a;
			floorHeight = height;
			floorNum = num;
			floorTextureShow = textureShow;
			floorTextureHide = textureHide;
		}
		
		public void ShowNormal() {
			backUpColorA = instanceTop.renderer.material.color.a;
			backUpTexture = instanceTop.renderer.material.mainTexture;
			
			Color color = instanceTop.renderer.material.color;
			color.a = ColorA;
			instanceTop.renderer.material.color = color;
			
			color = instancebottom.renderer.material.color;
			color.a = ColorA;
			instancebottom.renderer.material.color = color;
			
			instanceTop.renderer.material.mainTexture = floorTextureShow;
			instancebottom.renderer.material.mainTexture = floorTextureShow;
		}
		
		public void ShowStress() {
			backUpColorA = instanceTop.renderer.material.color.a;
			backUpTexture = instanceTop.renderer.material.mainTexture;
			
			Color color = instanceTop.renderer.material.color;
			color.a = 1;
			instanceTop.renderer.material.color = color;
			
			color = instancebottom.renderer.material.color;
			color.a = 1;
			instancebottom.renderer.material.color = color;
			
			instanceTop.renderer.material.mainTexture = floorTextureShow;
			instancebottom.renderer.material.mainTexture = floorTextureShow;
		}
		
		public void Hide() {
			backUpColorA = instanceTop.renderer.material.color.a;
			backUpTexture = instanceTop.renderer.material.mainTexture;
			
			Color color = instanceTop.renderer.material.color;
			color.a = ColorA;
			instanceTop.renderer.material.color = color;
			
			color = instancebottom.renderer.material.color;
			color.a = ColorA;
			instancebottom.renderer.material.color = color;
			
			instanceTop.renderer.material.mainTexture = floorTextureHide;
			instancebottom.renderer.material.mainTexture = floorTextureHide;
		}
		
		public void HideStress() {
			backUpColorA = instanceTop.renderer.material.color.a;
			backUpTexture = instanceTop.renderer.material.mainTexture;
			
			Color color = instanceTop.renderer.material.color;
			color.a = 0;
			instanceTop.renderer.material.color = color;
			
			color = instancebottom.renderer.material.color;
			color.a = 0;
			instancebottom.renderer.material.color = color;
			
			instanceTop.renderer.material.mainTexture = floorTextureHide;
			instancebottom.renderer.material.mainTexture = floorTextureHide;
		}
		
		public void ShowRestore() {
			Color color = instanceTop.renderer.material.color;
			color.a = backUpColorA;
			instanceTop.renderer.material.color = color;
			
			color = instancebottom.renderer.material.color;
			color.a = backUpColorA;
			instancebottom.renderer.material.color = color;
			
			instanceTop.renderer.material.mainTexture = backUpTexture;
			instancebottom.renderer.material.mainTexture = backUpTexture;
		}
		
		public void Draw() {
			Quaternion r = rotation;
			Destroy();
			instanceTop = (GameObject)Instantiate(prefabFloor, position, r);
			r.x = 180;
			instancebottom = (GameObject)Instantiate(prefabFloor, position, r);
			instancebottom.renderer.material.mainTextureScale = new Vector2(1,-1);
			//这里可能是由于旋转轴选取的问题，导致旋转后的图形与旋转前的图形在Y轴上出现了错位，这里通过实验发现，该值为2.5，所以这里要把Y轴加上2.5
			instancebottom.transform.position = new Vector3 (position.x, position.y+2.5f, position.z);
		}
		
		public void Destroy() {
			GameObject.Destroy(instanceTop);
			GameObject.Destroy(instancebottom);
//			Destroy(instanceTop);
//			Destroy(instancebottom);
		}
	}

	
	// Use this for initialization
	void Start () {
		
		prefabFloor.renderer.material = (Material)Resources.Load("floor");
		mapTextureNormal = (Texture2D)Resources.Load("mapTexture1");
		mapTextureBorder = (Texture2D)Resources.Load("mapTexture2");
		prefabFloor.renderer.material.mainTexture = mapTextureNormal;
		prefabFloor.transform.localScale = new Vector3 (floorScope.x/10,floorScope.y,floorScope.z/10);//必须这样赋值，如果
		//像下面这样赋值，则会报错
		//prefabFloor.transform.localScale.z = floorScopeZ/10;
	
		myDrawMap.MyDrawMapInit(mapTextureNormal, mapTextureBorder);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public FloorInstance FindFloorFloorInstance(int floorNum) {
		for(int i=0; i<floorInstanceList.Count; i++) {
			if(((FloorInstance)floorInstanceList[i]).floorNum == floorNum) {
				return ((FloorInstance)floorInstanceList[i]);
			}
		}
		return null;
	}
	
	public FloorInstance FindFloorFloorInstance(int floorNum, int sn) {
		for(int i=0; i<floorInstanceList.Count; i++) {
			if(((FloorInstance)floorInstanceList[i]).floorNum == floorNum) {
				sn = i;
				return ((FloorInstance)floorInstanceList[i]);
			}
		}
		return null;
	}
	
	private void UpdataFloorPosition() {
		FloorInstance fi;// = FindFloorFloorInstance(floorNum);
		float height = baseFloorPositionY;
		for(int i=1; i<=upFloorNum; i++) {
//			FindFloorFloorInstance(i).position.y;
			fi = FindFloorFloorInstance(i);
			fi.position.y = height;
			height += fi.floorHeight;
		}
		
		height = baseFloorPositionY;
		for(int i=1; i<=downFloorNum; i++) {
			fi = FindFloorFloorInstance(-i);
			height -= fi.floorHeight;
			fi.position.y = height;
		}
	}
	
	public bool AddFloor(float floorHeight, Vector3 position, Quaternion rotation, int thisFloorNum) {
		if(0 == thisFloorNum)
			return false;
		floorInstanceList.Add(new FloorInstance(prefabFloor, position, rotation, thisFloorNum, floorHeight,mapTextureNormal, mapTextureBorder));
		if(thisFloorNum > 0)
			upFloorNum ++;
		else if(thisFloorNum < 0)
			downFloorNum ++;
		
		floorNumChanged = true;
		return true;
	}
	
	public bool DrawFloor() {
		UpdataFloorPosition();
		
		for(int i=0; i<floorInstanceList.Count; i++)
			((FloorInstance)floorInstanceList[i]).Draw();
		return true;
	}
	
	public bool DrawFloor(int floorNum) {
		if(0 == floorNum)
			return false;
//		for(int i=0; i<floorInstanceList.Count; i++) {
//			if(((FloorInstance)floorInstanceList[i]).floorNum == floorNum) {
//				((FloorInstance)floorInstanceList[i]).Draw();
//				return true;
//			}
//		}
		FloorInstance fi = FindFloorFloorInstance(floorNum);
		if(null == fi)
			return false;
		fi.Draw();
		return true;
	}
	
	public bool SetFloorHeight(int floorNum, float floorHeight) {
		if(0 == floorNum)
			return false;
//		for(int i=0; i<floorInstanceList.Count; i++) {
//			if(((FloorInstance)floorInstanceList[i]).floorNum == floorNum) {
//				((FloorInstance)floorInstanceList[i]).floorHeight = floorHeight;
//				return true;
//			}
//		}
//		return false;
		FloorInstance fi = FindFloorFloorInstance(floorNum);
		if(null == fi)
			return false;
		fi.floorHeight = floorHeight;
		UpdataFloorPosition();
		return true;
	}
	
	public bool DelFloor(int floorNum) {
		if(0 == floorNum)
			return false;
//		for(int i=0; i<floorInstanceList.Count; i++) {
//			if(((FloorInstance)floorInstanceList[i]).floorNum == floorNum) {
//				((FloorInstance)floorInstanceList[i]).Destroy();
//				floorInstanceList.Remove(i);
//				if(floorNum > 0)
//					upFloorNum --;
//				else if(floorNum < 0)
//					downFloorNum --;
//				return true;
//			}
//		}
//		return false;
		int i = 0;
		FloorInstance fi = FindFloorFloorInstance(floorNum, i);
		if(null == fi)
			return false;
		fi.Destroy();
		floorInstanceList.Remove(i);
		if(floorNum > 0)
			upFloorNum --;
		else if(floorNum < 0)
			downFloorNum --;
		
		floorNumChanged = true;
		return true;
	}
	
	public void Clear() {
		for(int i=0; i<floorInstanceList.Count; i++)
			((FloorInstance)floorInstanceList[i]).Destroy();
		floorInstanceList.Clear();
		upFloorNum = 0;
		downFloorNum = 0;
		
		floorNumChanged = true;
	}
	
//	private void DrawFloor() {
//		int i = 0;
//		float temp = 0;
//		for(i=0;i<floorInstance.Count;i++)
//			Destroy(floorInstance[i]);
//		floorInstance.Clear();
//	
//		for(i=0; i<floorUpHeight.Count; i++) {
//			//floorInstance.Add(CreatFloor(Vector3(0, parseFloat(floorUpHeight[i].ToString())+temp, 0), Quaternion.identity));
//			floorInstance.Add(CreatFloor(Vector3(floorScope.x/2, temp, floorScope.z/2), Quaternion.identity));
//			temp += parseFloat(floorUpHeight[i].ToString());
//		}
//		temp = 0;
//		for(i=0; i<floorDownHeight.Count; i++) {
//			floorInstance.Add(CreatFloor(Vector3(floorScope.x/2, parseFloat(floorDownHeight[i].ToString())+temp, floorScope.z/2), Quaternion.identity));
//			temp += parseFloat(floorDownHeight[i].ToString());
//		}
//		
//		if(floorInstance.Count != 0) {//保存地图原始的A值
//			GameObject instance = floorInstance[0];
//			floorColorA = instance.renderer.material.color.a;
//		}
//	}
//	
//	private void DrawFloor(int floorNum) {
//		int i = 0;
//		float temp = 0;
//		for(i=0;i<floorInstance.Count;i++)
//			Destroy(floorInstance[i]);
//		floorInstance.Clear();
//		
//		if(floorNum > 0) {
//			for(i=0; i<floorUpHeight.Count; i++) {
//				//floorInstance.Add(CreatFloor(Vector3(0, parseFloat(floorUpHeight[i].ToString())+temp, 0), Quaternion.identity));
//				if((i+1) == floorNum) {
//					floorInstance.Add(CreatFloor(Vector3(floorScope.x/2, temp, floorScope.z/2), Quaternion.identity));
//					break;
//				}
//				temp += parseFloat(floorUpHeight[i].ToString());
//			}
//		}
//		else if(floorNum < 0) {
//			for(i=0; i<floorDownHeight.Count; i++) {
//				if((i+1) == Mathf.Abs(floorNum)) {
//					floorInstance.Add(CreatFloor(Vector3(floorScope.x/2, parseFloat(floorDownHeight[i].ToString())+temp, floorScope.z/2), Quaternion.identity));
//					break;
//				}
//				temp += parseFloat(floorDownHeight[i].ToString());
//			}
//		}
//		
//		if(floorInstance.Count != 0) {//保存地图原始的A值
//			GameObject instance = floorInstance[0];
//			floorColorA = instance.renderer.material.color.a;
//		}
//	}
//	
//
//	private bool StressSingleFloor(int floorNum, bool canRestore) {
//		if(floorUpHeight.Count+floorDownHeight.Count != floorInstance.Count)
//			return false;
//		//var instance : GameObject;
//		uint i = 0;
//		if(floorNum > 0) {
//			if(floorUpHeight.Count < floorNum)
//				return false;
//			stressFloorInstance = floorInstance[floorNum-1];
//		}
//		else if(floorNum < 0) {
//			if(floorDownHeight.Count < Mathf.Abs(floorNum))
//				return false;
//			stressFloorInstance = floorInstance[floorUpHeight.Count + Mathf.Abs(floorNum) - 1];
//		}
//		else {
//			return false;
//		}
//		
//		if(canRestore){
//			beforeStressColorA = stressFloorInstance.renderer.material.color.a;
//			beforeStressTexture = stressFloorInstance.renderer.material.mainTexture;
//			}
//		else {
//			beforeStressColorA = 1;
//			beforeStressTexture = mapTexture1;
//			}
//			
//		stressFloorInstance.renderer.material.color.a = 1;
//		stressFloorInstance.renderer.material.mainTexture = mapTexture1;
//		
//		return true;
//	}
//	
//	private bool RestoreLastStressFloor() {
//		if(null == stressFloorInstance)
//			return false;
//		stressFloorInstance.renderer.material.color.a = beforeStressColorA;
//		stressFloorInstance.renderer.material.mainTexture = beforeStressTexture;
//		return true;
//	}
//	
//	private bool ShowSingleFloor(int floorNum) {
//		if(floorUpHeight.Count+floorDownHeight.Count != floorInstance.Count)
//			return false;
//		GameObject instance;
//		uint i = 0;
//		if(floorNum > 0) {
//			if(floorUpHeight.Count < floorNum)
//				return false;
//			instance = floorInstance[floorNum-1];
//		}
//		else if(floorNum < 0) {
//			if(floorDownHeight.Count < Mathf.Abs(floorNum))
//				return false;
//			instance = floorInstance[floorUpHeight.Count + Mathf.Abs(floorNum) - 1];
//		}
//		else {
//			return false;
//		}
//		instance.renderer.material.color.a = floorColorA;
//		return true;
//	}
//	
//	private bool ShowAllFloor(){
//		DrawFloor();
//		return true;
//	}
//	
//	private bool HideSingleFloor(int floorNum) {
//		if(floorUpHeight.Count+floorDownHeight.Count != floorInstance.Count)
//			return false;
//		GameObject instance;
//		uint i = 0;
//		if(floorNum > 0) {
//			if(floorUpHeight.Count < floorNum)
//				return false;
//			instance = floorInstance[floorNum-1];
//		}
//		else if(floorNum < 0) {
//			if(floorDownHeight.Count < Mathf.Abs(floorNum))
//				return false;
//			instance = floorInstance[floorUpHeight.Count + Mathf.Abs(floorNum) - 1];
//		}
//		else {
//			return false;
//		}
//		//instance.renderer.material.color.a = 0;
//		instance.renderer.material.mainTexture = mapTexture2;
//		return true;
//	}
//	
//	private bool HideAllFloor() {
//		GameObject instance;
//		for(var i=0; i<floorInstance.Count; i++) {
//			instance = floorInstance[i];
//			//instance.renderer.material.color.a = 0;
//			instance.renderer.material.mainTexture = mapTexture2;
//		}
//		return true;
//	}
}
