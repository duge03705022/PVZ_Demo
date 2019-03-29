using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RFIBricks_Cores_Libs;
public class RFIB_DEMO : MonoBehaviour {	
	RFIBricks_Cores RFIB;
	static float boxSize=2, distance=boxSize+2; 
	int boxNumX=3,boxNumY=3/*, W=1200,H=900, FrameRate=1000*/;		

	static string[] AllowBlockType = {  /* Color,TAG ON BOTTOM */
		"9999", /*Board*/ "9998", /*Virtual Board*/			
		//70-71-72  73-74-75 76-77-78  79-80-81 81-82-83  84-85-86 87-88-89   
		//90-91  92-93 94-95 96-97   98-99
		"9101", /*Black Block With 1 tag*/ //"9102",/*Black Block With 2 tag*/ 
		"9201", /*white*/ "9301", /*Gray*/ "9401", /*Red*/		
		"9501", /*Red*/	
		"9601", //Tangle �����T���� ID: 20-Black,30-white,40-Gray,50-Red
		"9701", //Tangle   ���T���� ID: 20-Black,30-white,40-Gray,50-Red//
		//			"0001", //Knob-Widget
	};
	
	short[] EnableAntenna = {1};
	string ReaderIP = "192.168.1.93";
	double ReaderPower=28, Sensitive=-70;
	bool Flag_AddPreStackOnce    = true;
	//bool Flag_AllowMouseMovement = false;	
	bool Flag_ToConnectTheReade  = true;
	bool Flag_Draw3DSpace=true;
	bool Flag_BoardWithMaterial = true;

	HashSet<int> Cube_topLava = new HashSet<int>(), 
		Cube_Grass = new HashSet<int>(), 
		Cube_cr016 = new HashSet<int>(), 
		Cube_cr010 = new HashSet<int>(), 
		Cube_Street = new HashSet<int>(), 
		Cube_RedBrick = new HashSet<int>(), 
		Cube_Rabbit = new HashSet<int>();

	public Dictionary<int, GameObject> cubes_Blocks = new Dictionary<int, GameObject>();  //for Dynamic create BlockID

	// Use this for initialization
	void Start () {
		
		RFIB = new RFIBricks_Cores(ReaderIP, ReaderPower, Sensitive, EnableAntenna, Flag_ToConnectTheReade);

		//RFIB.setSerialcomPort("COM3");


		RFIB.setShowSysMesg(false);
		RFIB.setShowReceiveTag(false);
		RFIB.setShowDebugMesg(false);
		//RFIB.setSysTagBased("7428 0000");
		RFIB.setSysTagBased("2250 0000");
		RFIB.setAllowBlockType(AllowBlockType);

		//[OPTIMIZATION]
		RFIB.setRefreshTime(300);
		RFIB.setDisappearTime(300);
		RFIB.setDelayForReceivingTime(200);

		RFIB.Num_Allow_LosslyStack = 1;
		RFIB.startReceive();

		if (Flag_AddPreStackOnce) RFIB.setPreStackOrders(PreStackOrders);

		DrawBoard();
		setBoardIDandSpecialMaterialBlcok();
	}


	void Update()
	{
		keyPressed();
		RFIB.statesUpdate();



		if (Time.time > 3 && autoRun)
		{
			print("StartToBuild...");
			RFIB.startToBuild();
			RFIB.printNoiseIDs();
			
			autoRun = false;
		}

		if (Flag_Draw3DSpace)
		{
			DrawBlock();
		}

		// 從 Resources 下載入 Cube 物件
		if (Input.GetKeyDown(KeyCode.A))
		{
			UnityEngine.Object obj = Resources.Load("Cube");
			GameObject gobj = Instantiate(obj) as GameObject;
			gobj.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
		}
	}

	private int getPositionHeight(int X, int Y)
	{
		int MaxHeight = 0;

		foreach (int BlockID_intType in RFIB.StackedOrders3D.Keys)
		{
			if (RFIB.StackedOrders3D[BlockID_intType][0] == X && RFIB.StackedOrders3D[BlockID_intType][1] == Y)
			{
				if (RFIB.StackedOrders3D[BlockID_intType][2]> MaxHeight)
					MaxHeight  = RFIB.StackedOrders3D[BlockID_intType][2];
			}
		}
		return MaxHeight;
	}


	static string[] PreStackOrdersSim = {};
	int PreStackOrdersSimIndex = 0;


	Boolean autoRun = true;
	

	void OnApplicationQuit()
	{
		
		RFIB.StopReader();
	}

	private void DrawBlock()
	{
		//if(false)
		//foreach (int blockID in cubes_Blocks.Keys)
		//	if(blockID%100<90)
		//{
		//	try {
		//		//Debug.Log(blockID + " : " + cubes_Blocks[blockID].transform.position.x + " ====  " + RFIB.StackedOrders3D[blockID][0]);
		//		if(false)
		//		if (cubes_Blocks[blockID].transform.position.x > 0 && RFIB.StackedOrders3D[blockID][0] >= 0)
		//		{
		//			Debug.Log(blockID + " : " + cubes_Blocks[blockID].transform.position.x);
		//			Destroy(cubes_Blocks[blockID]);
		//			cubes_Blocks.Remove(blockID);
		//		}

		//		if (cubes_Blocks[blockID].transform.position.x <= 0 && RFIB.StackedOrders3D[blockID][0] < 0)
		//		{
		//			Debug.Log(blockID + " : " + cubes_Blocks[blockID].transform.position.x);
		//			Destroy(cubes_Blocks[blockID]);
		//			cubes_Blocks.Remove(blockID);
		//		}

		//		if (cubes_Blocks[blockID].transform.position.x != -RFIB.StackedOrders3D[blockID][0]
		//				|| cubes_Blocks[blockID].transform.position.y != RFIB.StackedOrders3D[blockID][2]
		//				|| cubes_Blocks[blockID].transform.position.z != RFIB.StackedOrders3D[blockID][1]
		//				)
		//		{
		//			Debug.Log(blockID + " : " + cubes_Blocks[blockID].transform.position.x);
		//			Debug.Log(blockID + " : " + cubes_Blocks[blockID].transform.position.y);
		//			Debug.Log(blockID + " : " + cubes_Blocks[blockID].transform.position.z);
		//				Destroy(cubes_Blocks[blockID]);
		//			cubes_Blocks.Remove(blockID);
		//		}
		//		}
		//	catch (Exception e)
		//	{
		//	}
		//}
		try
		{
			foreach (int tmpID in RFIB.StackedOrders3D.Keys)
			{
				DrawTheObjectXYZ(tmpID, RFIB.StackedOrders3D[tmpID][5], RFIB.StackedOrders3D[tmpID][0], RFIB.StackedOrders3D[tmpID][1], RFIB.StackedOrders3D[tmpID][2]);
			}

			if (RFIB.StackedOrders3D.Count == 0 && cubes_Blocks.Count > 0)
			{
				foreach (int BlockID in cubes_Blocks.Keys)
				{
					Destroy(cubes_Blocks[BlockID]);
					cubes_Blocks.Remove(BlockID);
					if (cubes_Blocks.Count == 0) break;
				}
			}

			if (cubes_Blocks.Count > 0)
				foreach (int BlockID in cubes_Blocks.Keys)
				{
					if (RFIB.StackedOrders3D.ContainsKey(BlockID) == false)
					{
						Destroy(cubes_Blocks[BlockID]);
						cubes_Blocks.Remove(BlockID);
					}
					if (cubes_Blocks.Count == 0) break;
				}
		}
		catch (Exception)
		{
		}

		foreach (int BID in RFIB.StackedOrders3D.Keys)
		{
			//Debug.Log("BID::" + BID);
			if (cubes_Blocks.ContainsKey(BID) == false)
			{
				//DrawTheObjectXYZ(tmpID, RFIB.StackedOrders3D[tmpID][5], RFIB.StackedOrders3D[tmpID][0], RFIB.StackedOrders3D[tmpID][1], RFIB.StackedOrders3D[tmpID][2]);
				Debug.Log("BID in :cubes_Blocks  not found:" + BID);
			}
		}

	}

	static string[] PreStackOrders = {
		//"7428 0000 9999 0302 0003,7428 0000 9101 1101 0001",
		//"7428 0000 9999 0102 0004,7428 0000 9301 0701 0001",

		//"7428 0000 9201 0901 0001,7428 0000 9101 1103 0004",
		//"7428 0000 9401 0501 0001,7428 0000 9301 0703 0001",
	};


	void setBoardIDandSpecialMaterialBlcok() {


		RFIB.setBoardBlockMappingArray(1, "0101");
		RFIB.setBoardBlockMappingArray(2, "0201");
		RFIB.setBoardBlockMappingArray(3, "0301");

		RFIB.setBoardBlockMappingArray(4, "0102");
		RFIB.setBoardBlockMappingArray(5, "0202");
		RFIB.setBoardBlockMappingArray(6, "0302");

		RFIB.setBoardBlockMappingArray(7, "0103");
		RFIB.setBoardBlockMappingArray(8, "0203");
		RFIB.setBoardBlockMappingArray(9, "0303");
		

		int[] Cube_StreetA = { 940104 };
		int[] Cube_topLavaA = { 920135, 920143, 910112, 910173, 920120, 910128 };
		int[] Cube_RedBrickA = { 910109, 920119, 920134, 920117, 910174, 910175 };
		int[] Cube_cr010A = { 940103 };
		int[] Cube_cr016A = { 940101 };
		int[] Cube_GrassA = { 940102 };


		foreach (int xID in Cube_GrassA)
			Cube_Grass.Add(xID);
		foreach (int xID in Cube_cr016A)
			Cube_cr016.Add(xID);
		foreach (int xID in Cube_StreetA)
			Cube_Street.Add(xID);
		foreach (int xID in Cube_topLavaA)
			Cube_topLava.Add(xID);

		foreach (int xID in Cube_RedBrickA)
			Cube_RedBrick.Add(xID);

		foreach (int xID in Cube_cr010A)
			Cube_cr010.Add(xID);
	}

	private void DrawTheObjectXYZ(int blockID, int BlockType, int X, int Y, int Z)
	{
		Color tmpColor = new Color(255, 0, 0);
		Color tmpBoardColor = new Color(255, 0, 0);
		int pureBlockSN = blockID % 100;
		for (int z = 0; z < 10; z++)
		{
			for (int y = -boxNumY * 2; y < boxNumY * 2; y++)
			{
				for (int x = -boxNumX * 2; x < boxNumX * 2; x++)
					if (x == X && y == Y && z == Z)
					{
						//========== Set Color ================
						if (BlockType == 8701 || BlockType == 8601 || BlockType == 8602 || BlockType == 8603 || BlockType == 8604 || BlockType == 8501 || BlockType == 801 || BlockType == 8301 || BlockType == 8201 || BlockType == 8101 || BlockType == 8401)
						{ 
						}
						else if ((BlockType >= 9100 && BlockType < 9500) || (BlockType >= 7100 && BlockType <= 7500))
						{
							int blockIDa = BlockType - BlockType % 100 - 9000;
							if (blockIDa == 100) { tmpColor = new Color(0, 0, 0); tmpBoardColor = new Color(0, 0, 0); }  //Black
							else if (blockIDa == 200) { tmpColor = new Color(255, 255, 255); tmpBoardColor = new Color(100, 100, 100); }  //White
							else if (blockIDa == 300) { tmpColor = new Color(100, 100, 100); }  //Gray
							else if (blockIDa == 400) { tmpColor = new Color(255, 0, 0); }  //Red
						}
						else if (BlockType == 9601 || BlockType == 9602 || BlockType == 9603 || BlockType == 9604 || BlockType == 9701 || BlockType == 8602 || BlockType == 8603 || BlockType == 8604)
						{
						}
						else {
						}

						//========== Draw Object ================
						//print("DrawTheObjectXYZ():" + blockID + "-" + X + "-" + Y + "-" + Z);


						
						if (cubes_Blocks.ContainsKey(blockID) == false)
						{
							 if (blockID == 910111)
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("CubeB1")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
                                float scale = 1f;
                                //, offset = 0f;
								cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}
							else if (blockID == 920172)
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("CubeG1")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
                                float scale = 1f;
                                //, offset = 0f;
								cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}
							else if (blockID == 920170)
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("CubeR1")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}
							else if (blockID == 920171)
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("CubeB1")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}
							else if (Cube_cr016.Contains(blockID))
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("Cube_cr016")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3((-X) * scale, Z * scale, (Y) * scale));
								cubes_Blocks[blockID].transform.Rotate(new Vector3(0, (RFIB.getDataStackOrderIn3D(blockID, "FacingDirect")-2) * 90, 0));
								if (blockID == 920136)
									cubes_Blocks[blockID].transform.Rotate(new Vector3(0, -90, 0));
							} else if (Cube_cr010.Contains(blockID))
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("Cube_cr010")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3((-X) * scale, Z * scale, (Y) * scale));
								
								cubes_Blocks[blockID].transform.Rotate(new Vector3(0, (RFIB.getDataStackOrderIn3D(blockID, "FacingDirect") - 2) * 90, 0));
								if(blockID==910105)
									cubes_Blocks[blockID].transform.Rotate(new Vector3(0,  90, 0));
								if (blockID == 920121)
									cubes_Blocks[blockID].transform.Rotate(new Vector3(0, 180, 0));
							}
							else if (Cube_topLava.Contains(blockID))
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("Cube_topLava")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}
							else if (Cube_Grass.Contains(blockID))
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("Cube_Grass")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}
							else if (Cube_Street.Contains(blockID))
							{
								//Debug.Log("Cube_Street");
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("Cube_Street")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}
							else if (Cube_RedBrick.Contains(blockID))
							{
								Debug.Log("Cube_RedBrick");
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("Cube_RedBrick")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}

							else if (Cube_cr010.Contains(blockID))
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("Cube_Rabbit")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(X * scale, Z * scale, Y * scale));
								cubes_Blocks[blockID].transform.Rotate(new Vector3(0, RFIB.getDataStackOrderIn3D(blockID, "FacingDirect")*90, 0));
							}
							//Black
							else if (BlockType == 9101) {
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("CubeB1")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}  
							else if (BlockType == 9201) {
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("CubeW1")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}  //White
							else if (BlockType == 9301)
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("CubeG1")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}  //White
							else if (BlockType == 9401)
							{
								cubes_Blocks.Add(blockID, GameObject.Instantiate(Resources.Load("CubeR1")) as GameObject);
								cubes_Blocks[blockID].name = "Block-" + blockID;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}  //White
							else
							{
								cubes_Blocks.Add(blockID, GameObject.CreatePrimitive(PrimitiveType.Cube));
								cubes_Blocks[blockID].name = "Block-" + blockID;
								cubes_Blocks[blockID].GetComponent<Renderer>().material.color = tmpColor;
								float scale = 1f;
                                //, offset = 0f;
                                cubes_Blocks[blockID].transform.Translate(new Vector3(-X * scale, Z * scale, Y * scale));
							}
						}					
					}
			}
		}
	}
	
	private void DrawBoard()
	{
		GameObject[] cubes_Boards = new GameObject[boxNumX*boxNumY];
		for (int z = 0; z < 1; z++)
		{
			for (int y = 0; y < boxNumY; y++)
			{
				for (int x = 0; x < boxNumX; x++)
				{
					if (Flag_BoardWithMaterial)
					{
						cubes_Boards[x * y + x] = GameObject.Instantiate(Resources.Load("Cube_Grass")) as GameObject;
						cubes_Boards[x * y + x].name = "Board-" + x + "-" + y + "-" + z;
						float scale = 1f;
                        //, offset = 1f;
                        cubes_Boards[x * y + x].transform.Translate(new Vector3((-x) * scale, z * scale, y * scale));
					}
					else
					{
						if ((x + y) % 2 == 1)
						{
							cubes_Boards[x * y + x] = GameObject.Instantiate(Resources.Load("Boards_B")) as GameObject;
							cubes_Boards[x * y + x].name = "Board-" + x + "-" + y + "-" + z;
							float scale = 1f;
                            //, offset = 1f;
                            cubes_Boards[x * y + x].transform.localScale -= new Vector3(0, 0, 0);
							cubes_Boards[x * y + x].transform.Translate(new Vector3(-x * scale, z * scale, y * scale));
						} 
						else
						{
							cubes_Boards[x * y + x] = GameObject.Instantiate(Resources.Load("Boards_W")) as GameObject;
							cubes_Boards[x * y + x].name = "Board-" + x + "-" + y + "-" + z;
							float scale = 1f;
                            //, offset = 1f;
                            cubes_Boards[x * y + x].transform.localScale -= new Vector3(0, 0, 0);
							cubes_Boards[x * y + x].transform.Translate(new Vector3(-x * scale, z*scale, y * scale));
						}
					}
				}
			}
		}
	}


	public void keyPressed()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			print("StartToBuild...");
			RFIB.startToBuild();
			RFIB.printNoiseIDs();
			
			autoRun = false;
			
			foreach (string xx in PreStackOrders)
			{
				string[] xxs = xx.Split(',');
				//RFIB._Testing_AddHoldingTag(xxs[0], xxs[1]);
				RFIB._Testing_AddTestingTemporarilyTag(xxs[0], xxs[1]);
			}
			//RFIB._Testing_AddHoldingTag("7428 0000 9999 0302 0003", "7428 0000 9101 1101 0001");
		}//920136

		if (Input.GetKeyDown("d"))
		{
			PreStackOrdersSimIndex++;
			Debug.Log(PreStackOrdersSimIndex);
		}
		if (Input.GetKey("r"))
		{
			RFIB._Testing_AddHoldingTag("7428 0000 9999 0102 0001", "7428 0000 9201 3601 0001");

		}
		if (Input.GetKey("e"))
		{
			RFIB._Testing_AddTestingTemporarilyTag("7428 0000 9999 0102 0001", "7428 0000 9201 3601 0001");

		}

		if (Input.GetKey("e"))
		{
			RFIB.StopReader();
		}

		if (Input.GetKeyDown("o"))  RFIB.printStackedOrders3D();
		if (Input.GetKeyDown("p"))  RFIB.printStackedOrders();

	}
	public static long CurrentTimeMillis() {
		return (long)(Time.time * 1000f);
	}

}
