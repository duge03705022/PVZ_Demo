using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impinj.OctaneSdk;
using System;

public class RFIBricks_Cores
{
	public static HashSet<string> X16HS_DetectedTags = new HashSet<string>();
	public static HashSet<string> X16HS_DetectedIDs = new HashSet<string>();
	public static HashSet<string> X16DetectedIDs = new HashSet<string>();
	public static HashSet<string> X16HS_DetectedTagsOLD = new HashSet<string>();

	public static HashSet<string> NoiseIDs = new HashSet<string>();

	public static string SystemBaseTag = "7428 0000 ";
	public static string SystemBaseTag2 = "7441 0000 ";
	public static bool Flag_ShowReceivedTag = true;

	public static string[] BoardBlockMappingArray = new string[100];

	public void setBoardBlockMappingArray(string[] posx)
	{
		if (posx.Length > 0)
			BoardBlockMappingArray = posx;
	}

	static string[] AllowBlockType = {  /* Color,TAG ON BOTTOM */
		"9999", /*Board*/ "9998", /*Virtual Board*/			
		//70-71-72  73-74-75 76-77-78  79-80-81 81-82-83 
		//84-85-86 87-88-89   
		//90-91  92-93 94-95 96-97   98-99
		"9101", /*Black Block With 1 tag*/ "9102",/*Black Block With 2 tag*/ 
		"9201", /*white*/ "9301", /*Gray*/ "9401", /*Red*/		
		"9601", //Tangle ?????T???? ID: 20-Black,30-white,40-Gray,50-Red
		"9701", //Tangle   ???T???? ID: 20-Black,30-white,40-Gray,50-Red//
		//			"0001", //Knob-Widget
	};
	string[][] VirtualBlocks = { /*{"7428 0000 9301 7301 0001","7428 0000 9301 7403 0001"},*/ };

	public static Hashtable DetectedTagsInitial = new Hashtable(); //<string,Double>
	public ArrayList StackedOrders = new ArrayList();  //<string>()
	public ArrayList StackedOrdersRecursive = new ArrayList(); //<string>
	private Dictionary<string, long> DispearCount = new Dictionary<string, long>();
	public Dictionary<int, int[]> StackedOrders3D = new Dictionary<int, int[]>();  //X Y Z SurfaceID StackWay BlockIDType BlockID};
	public static long SystemInitTime, LastRestartTime = 0, startTime;
	//int ShiftOffset = 100, MaxTestNumber = 20, ClearDataSecond = 20, ExitTime = 60;
	static float boxSize = 50, distance = boxSize + 2, halfDis; //int boxNum = 6, boxNumX = 6, boxNumY = 6; int tmpcounnnnn = 0;
	//int RefreshRate = 120;
	static int RestartTime = 1000;
	long DelayForReceivingTime = 200;
	//int D3y_offset = 0, D3x_offset = 0; static int offsetYY = -1;
	private Computing3DRotationX C3DR = new Computing3DRotationX();
	public int countNonStackOnTheBoard = 0;

	public Dictionary<string, Dictionary<string, Dictionary<string, HashSet<string>>>> IDSets = new Dictionary<string, Dictionary<string, Dictionary<string, HashSet<string>>>>();
	public Dictionary<string, int> IDSetsNum = new Dictionary<string, int>();

	Hashtable DispearIDs_DispearedTime = new Hashtable();
	public static Dictionary<string, long> IDs_ReceivedTimeStamp = new Dictionary<string, long>();
	public HashSet<string> NewIDs = new HashSet<string>(), NewBlockAndSurfaceIDs = new HashSet<string>(),
	NewBlockIDs = new HashSet<string>(), NewBlockTyping = new HashSet<string>(),
	StackedIDs = new HashSet<string>(), StackedBlockAndSurfaceIDs = new HashSet<string>(),
	StackedBlockIDs = new HashSet<string>(), StackedBlockTyping = new HashSet<string>(),
	NewIDButBlockExisted_IDs = new HashSet<string>(), NewIDButBlockExisted_BlockAndSurfaceIDs = new HashSet<string>(),
	NewIDButBlockExisted_BlockIDs = new HashSet<string>(), NewIDButBlockExisted_BlockTyping = new HashSet<string>(),

	////
	NewIDAndNotBeUsed_IDs = new HashSet<string>(),

	NewIDAndBlockNonRecorded_IDs = new HashSet<string>(), NewIDAndBlockNonRecorded_BlockAndSurfaceIDs = new HashSet<string>(),
	NewIDAndBlockNonRecorded_BlockIDs = new HashSet<string>(), NewIDAndBlockNonRecorded_BlockTyping = new HashSet<string>(),
	DispearIDs = new HashSet<string>(), DispearBlockAndSurfaceIDs = new HashSet<string>(),
	DispearBlockIDs = new HashSet<string>(), DispearBlockTyping = new HashSet<string>();

	public HashSet<string> VirtualIDs = new HashSet<string>();
	public HashSet<string> DetectedIDs = new HashSet<String>(); //Buffer
	public HashSet<string> DetectedTags = new HashSet<string>(); //Buffer
	string[] PreStackOrders = {  /*"7428 0000 9999 0201 0001,7428 0000 9701 4301 0001" */

	};

	public Dictionary<string, long> getIDs_ReceivedTimeStamp() {
		return IDs_ReceivedTimeStamp;
	}

	public int Num_Allow_LosslyStack = 1;

    HashSet<string> TestingHoldingTags = new HashSet<string>(); //{	 /*"7428 0000 9999 0201 0001,7428 0000 9701 4301 0001" */};
    public void _Testing_AddHoldingTag(string ID1, string ID2) { if (Flag_DetectTheNoiseTags) return; TestingHoldingTags.Add(ID1); TestingHoldingTags.Add(ID2); }
    public void _Testing_AddHoldingTag(string ID) { if (Flag_DetectTheNoiseTags) return; TestingHoldingTags.Add(ID); }
    public void _Testing_AddTestingTemporarilyTag(string ID) { DetectedTags.Add(ID); X16HS_DetectedTags.Add(ID); }
    public void _Testing_AddTestingTemporarilyTag(string ID1, string ID2)
    {

        X16HS_DetectedTags.Add(ID1); X16HS_DetectedTags.Add(ID2);
        X16DetectedIDs.Add(ID1); X16DetectedIDs.Add(ID2);

        DetectedTags.Add(ID1); X16HS_DetectedTags.Add(ID1);
        DetectedTags.Add(ID2); X16HS_DetectedTags.Add(ID2);
    }
    public void _Testing_RemoveHoldingTag(string ID1, string ID2) { if (Flag_DetectTheNoiseTags) return; TestingHoldingTags.Remove(ID1); TestingHoldingTags.Remove(ID2); }
    public void _Testing_RemoveHoldingTag(string ID) { if (Flag_DetectTheNoiseTags) return; TestingHoldingTags.Remove(ID); }

    short[] EnableAntenna = { 1, 2, 3, 4 };
	string ReaderIP = "192.168.1.96";
	double ReaderPower = 31;
	double Sensitive = -70;
	//bool Flag_AddPreStackOnce = false;
	bool Flag_ShowDebugInfo = false;
	bool Flag_ShowSysMesg = false;
	//bool Flag_AllowMouseMovement = true;
	bool Flag_ToConnectTheReade = true;
	bool Flag_AllowStackWithoutConnectingBoard = true;
	//bool Flag_Guidence = false;
	public static bool Flag_DetectTheNoiseTags = true;



	public RFIBricks_Cores(string ReaderIP2, double readerPower2, double sensitive2, short[] EnableAntenna2, bool ConnectTheReader2)
	{

		if(true)
		{
			startTime = CurrentTimeMillis();
			this.EnableAntenna = EnableAntenna2;
			this.ReaderIP = ReaderIP2;
			this.ReaderPower = readerPower2;
			this.Sensitive = sensitive2;
			this.Flag_ToConnectTheReade = ConnectTheReader2;
			Debug.Log("Setup.");
		}

		for (int i = 0; i < 100; i++)
		{

			if (i < 99)
				BoardBlockMappingArray[i] = "0" + Mathf.Floor(i / 10);
			if (i > 0 && i % 10 == 0) BoardBlockMappingArray[i] += "1"; else BoardBlockMappingArray[i] += "0";
			BoardBlockMappingArray[i] += (i % 10) + "";

			//Debug.Log(BoardBlockMappingArray[i]);
			//			BoardBlockMappingArray = new string[100];
		}
	}


	public void setBoardBlockMappingArray(int index, string Value)
	{
		BoardBlockMappingArray[index - 1] = Value;
	}

	string BoardType = "9999 ";

	public int DisappearTime_20170604 = RestartTime * 3;//600;
	public int DispearTime = 500;


	public void statesUpdate()
	{
		//if (System.DateTime.Now.Year * 100 + System.DateTime.Now.Month <= 201904)
		if(true)
		{

			try
			{
				if (Flag_DetectTheNoiseTags)
				{

					foreach (string id in X16HS_DetectedTags)
					{
						if (NoiseIDs.Contains(id) == false && id.Contains("7419 0000 9401 ")==false)
						{
							bool NonExist = true;
							foreach (string prestackorders in PreStackOrders)
							{
								if (prestackorders.Contains(id))
									NonExist = false;
							}
							if (NonExist) NoiseIDs.Add(id);
						}
					}
				}
				else
				{
					foreach (string ID in TestingHoldingTags)
					{
						X16HS_DetectedTags.Add(ID);
						X16DetectedIDs.Add(ID);
					}

					DetectedTags = X16HS_DetectedTags;
					DetectedIDs = X16DetectedIDs;


					foreach (string id in X16HS_DetectedTags)
					{
						//if (IDs_ReceivedTimeStamp.ContainsKey(id) == false)
						IDs_ReceivedTimeStamp.Remove(id);
						IDs_ReceivedTimeStamp.Add(id, CurrentTimeMillis());
					}
					//CurrentTimeMillis()
					//foreach (string id in IDs_ReceivedTimeStamp.Keys)
					//{
					//if (X16HS_DetectedTags.Contains(id))
					//IDs_ReceivedTimeStamp.Add(id, CurrentTimeMillis());
					//else
					//	IDs_ReceivedTimeStamp.Add(id, (long)(Time.time * 1000f) - (long)IDs_ReceivedTimeStamp[ID]);
					//}




					if (CurrentTimeMillis() - LastRestartTime > RestartTime)
					{
						X16HS_DetectedTags = new HashSet<String>();
						X16DetectedIDs = new HashSet<String>();
						LastRestartTime = CurrentTimeMillis();
					}
					else if (CurrentTimeMillis() - LastRestartTime > DelayForReceivingTime)
					{
						CleanVarsEachLoop();
						Update_StackedIDs();

						Update_NewIDs_20170605();
						Update_NonUsedIDs_20181115();
						Update_DispearIDs_20170604();



						Update_NewIDButBlockExisted();
						Update_NewIDAndBlockNonRecorded();

						foreach (string tmpStr in StackedOrders)
						{
							string[] xx = tmpStr.Split(',');

							Boolean tmpFlag = false;
							foreach (string xxxxx in xx)
							{
								int BlockID = getBlockIDFromIDByINT(xxxxx);
								if (BlockID < 999000)
								{
									if (StackedOrders3D.ContainsKey(BlockID) == false)
										tmpFlag = true;
								}
							}
							if (tmpFlag)
								StackedOrders.Remove(tmpStr);
						}



						//Debug.Log(NewBlockAndSurfaceIDs.Count);
						//Debug.Log("NewIDButBlockExisted_BlockAndSurfaceIDs:"+ NewIDButBlockExisted_BlockAndSurfaceIDs.Count);
						if (
							NewBlockAndSurfaceIDs.Count == 2
							//(NewIDAndBlockNonRecorded_BlockAndSurfaceIDs.Count+ NewIDButBlockExisted_BlockAndSurfaceIDs.Count) == 2
							//&& (getCountDispearedBigerThanDispearTime()==0  || getCountDispearedBigerThanDispearTime()<=Num_Allow_LosslyStack)
						)
						{
							OnlyTwoSurfaceStackInSameTime_20170603();
						}
						else if (NewBlockAndSurfaceIDs.Count > 2 && getCountDispearedBigerThanDispearTime() == 0)
						{
							return;
							//ManyBlockSurfaceseStackInSameTime();
							//} else if(NewIDs.size()==0 && DispearIDs.size()>0 && DispearIDs.size()%2==0) {
						}
						else if (NewIDs.Count == 0 && getCountDispearedBigerThanDispearTime() > 0 && getCountDispearedBigerThanDispearTime() % 2 == 0)
						{

							DeleteAndPrintStackedOrders();
						}
						else if (NewIDAndNotBeUsed_IDs.Count == 1 && getCountDispearedBigerThanDispearTime() == 1)
						{

							DeleteAndPrintStackedOrders_updatePatchForQuickMove();
						}

						//其中一個消失的時間 超過多久  & 其中一個偵測到的新時間多久 

						//Debug.Log("NewIDAndNotBeUsed_IDs.Count:" + NewIDAndNotBeUsed_IDs.Count+","+ getCountDispearedBigerThanDispearTime());
					}
				}
			}
			catch (Exception) { }
		}
	}

	public void setRefreshTime(int ms)
	{
		if (ms <= DisappearTime_20170604 && ms > 0)
		{
			RestartTime = ms;
			Debug.Log("RefreshTime:" + RestartTime);
		}
	}
	public void setDisappearTime(int ms)
	{
		if (ms >= RestartTime && ms > 0)
		{
			DisappearTime_20170604 = ms;
			Debug.Log("DisappearTime:" + DisappearTime_20170604);
		}
	}

	public void setDelayForReceivingTime(int ms)
	{
		if (ms <= RestartTime && ms > 0)
		{
			DelayForReceivingTime = ms;
			Debug.Log("DelayForReceivingTime:" + DelayForReceivingTime);
		}
	}


	void Start()
	{
		SystemInitTime = CurrentTimeMillis();
		var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.name = "brown_cube";
		cube.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime, Input.GetAxis("Vertical") * Time.deltaTime, 3);
		//InitialReader ();
	}

	// Update is called once per frame
	void Update()
	{
		//statesUpdate ();
	}

	void OnApplicationQuit()
	{
		Debug.Log("Application ending after " + Time.time + " seconds");
		reader.Stop();
		reader.Disconnect();
	}

	public void StopReader()
	{
		//Debug.Log("Application ending after " + Time.time + " seconds");
		try
		{
			reader.Stop();
			reader.Disconnect();
		}
		catch (Exception)
		{
		}
	}


	void InitialReader()
	{
		try
		{
			//print (1);
			reader.Connect(ReaderIP);
			Settings settings = reader.QueryDefaultSettings();
			//settings.Report.IncludeAntennaPortNumber = true;
			settings.ReaderMode = ReaderMode.AutoSetDenseReader;
			settings.SearchMode = SearchMode.DualTarget;
			settings.Session = 1;
			settings.Antennas.DisableAll();

			foreach (ushort PortID in EnableAntenna)
			{
				settings.Antennas.GetAntenna(PortID).IsEnabled = true;
				settings.Antennas.GetAntenna(PortID).MaxTxPower = true;
				settings.Antennas.GetAntenna(PortID).MaxRxSensitivity = true;
				settings.Antennas.GetAntenna(PortID).TxPowerInDbm = ReaderPower;
				settings.Antennas.GetAntenna(PortID).RxSensitivityInDbm = Sensitive;
			}


			reader.ApplySettings(settings);
			reader.TagsReported += OnTagsReported;
			reader.Start();
			Debug.Log("InitialReader.................");
		}
		catch (Exception e)
		{
			Console.WriteLine("Exception : {0}", e.Message);
		}
	}



	static ImpinjReader reader = new ImpinjReader();
	static void OnTagsReported(ImpinjReader sender, TagReport report)
	{
		//Debug.Log(System.DateTime.UtcNow);
		foreach (Tag tag in report)
		{
			//Debug.Log(System.DateTime.UtcNow);
			//Debug.Log(System.DateTime.UtcNow+ " @ OnTagsReported:" +tag.Epc);
			string ID = tag.Epc + "";
			if(ID.Equals("7427 0000 9201 0301 0001")){
				ID = "7429 0000 9401 0101 0001";
			}

			//if (Flag_ShowReceivedTag)
			//Debug.Log("OnTagsReported():" + ID);

			//if (Flag_DetectTheNoiseTags)
			//	NoiseIDs.Add(ID);

			if (NoiseIDs.Contains(ID) == false)
				for (int i = 0; i < AllowBlockType.Length; i++)
				{
					if (ID.Contains(SystemBaseTag + AllowBlockType[i]) || ID.Contains(SystemBaseTag2 + AllowBlockType[i]))
					{


						//20180102  
						if (true)
						{

							if (ID.Contains(SystemBaseTag))
							{
								Boolean xxxxx = false;
								foreach (string idxxx in X16HS_DetectedTags)
									if (idxxx.Contains(SystemBaseTag2))
										xxxxx = true;
								if (xxxxx) continue;
							}


							if (ID.Contains(SystemBaseTag2))
							{
								Boolean xxxxx = false;
								foreach (string idxxx in X16HS_DetectedTags)
									if (idxxx.Contains(SystemBaseTag))
										xxxxx = true;
								if (xxxxx) continue;
							}

							if (ID.Contains(SystemBaseTag + "9999") || ID.Contains(SystemBaseTag2 + "9999"))
							{

								String ID2 = ID;
								int blockIDDD = int.Parse(ID2.Substring(15, 4));
								String xxx = "";
								if (blockIDDD < 100)
								{
									xxx = ID2.Substring(0, 15) + BoardBlockMappingArray[int.Parse(ID2.Substring(15, 4)) - 1] + ID2.Substring(19, 5);
									//Debug.Log("Mapping: From '" + ID + "' -> '" + xxx+"'");
								} 
								else
									xxx = ID;
								X16HS_DetectedTags.Add(xxx);
								X16DetectedIDs.Add(xxx);





							}
							else
							{
								X16HS_DetectedTags.Add(ID);
								X16DetectedIDs.Add(ID);
							}

							if (Flag_ShowReceivedTag)
								Debug.Log("OnTagsReported() =:" + ID);
							//break;
						}
					}
				}
		}
	}

	///(Java to Unity)=========================================================================== 
	/// print
	/// .Add -> .Add
	/// .Remove -> .Remove
	/// Dictionary<string,long>   -> Hashtable
	/// ((int[]) StackedOrders3D[BlockID1])[0]; 
	public static long CurrentTimeMillis()
	{
		return (long)(Time.time * 1000f);
	}



	//===========*****========================================================
	public void ManyBlockSurfaceseStackInSameTime()
	{
		try
		{
			if (Flag_ShowSysMesg) foreach (string stringX in NewBlockAndSurfaceIDs) Debug.Log(NewBlockAndSurfaceIDs.Count + "] NewBlockAndSurfaceIDs:" + stringX);

			//		//CHECK NEW BlockSirface number not in stackorder3D
			if (Flag_ShowSysMesg) foreach (string tmpID in NewIDButBlockExisted_IDs) if (Flag_ShowDebugInfo)
				Debug.Log("ManyBlockSurfaceseStackInSameTime(): NewIDButBlockHasBeenStacked_IDs: [" + NewIDButBlockExisted_IDs.Count + "] " + tmpID);
			if (Flag_ShowSysMesg) foreach (string tmpID in NewIDAndBlockNonRecorded_IDs) if (Flag_ShowDebugInfo)
				Debug.Log(" ManyBlockSurfaceseStackInSameTime(): NewIDAndBlockNonRecorded_IDs: [" + NewIDAndBlockNonRecorded_IDs.Count + "] " + tmpID);
			if (NewIDButBlockExisted_IDs.Count != NewIDAndBlockNonRecorded_IDs.Count) if (Flag_ShowDebugInfo)
			{

				Debug.Log("[Err Mesg] ManyBlockSurfaceseStackInSameTime(): NewIDButBlockExisted_IDs.Count!=NewIDAndBlockNonRecorded_IDs.Count");
				Debug.Log("_" + NewIDButBlockExisted_IDs + " " + NewIDAndBlockNonRecorded_IDs); return;
			}
		}
		catch (Exception)
		{
		}


		//========Find the Max Distance=======
		//==ToBeAdded check it is 1x3
		double maxDistanceFromNewIDButBlockExisted = getMaxDistanceFromNewIDButBlockExisted();
		double maxDistanceFromNewIDAndBlockNonExisted = getMaxDistanceFromNewIDButBlockExisted();

		//Retry
		if (maxDistanceFromNewIDButBlockExisted != maxDistanceFromNewIDAndBlockNonExisted) { Debug.Log("[Err Mesg] ManyBlockSurfaceseStackInSameTime(): maxDistanceFromNewIDButBlockExisted(" + maxDistanceFromNewIDButBlockExisted + ")!=maxDistanceFromNewIDAndBlockNonExisted(" + maxDistanceFromNewIDAndBlockNonExisted + ")"); return;  /*Application.Quit();*/ }

		HashSet<int> EnableDirectionFromNewIDButBlockExisted = getEnableDirectionFromNewIDButBlockExisted();
		HashSet<int> EnableDirectionFromNewIDAndBlockNonExisted = getEnableDirectionFromNewIDAndBlockNonExisted();
		// Debug.Log(EnableDirectionFromNewIDAndBlockNonRecorded + " " + EnableDirectionFromNewIDButBlockExisted);

		if(Flag_ShowDebugInfo) if (EnableDirectionFromNewIDAndBlockNonExisted.Count != 1 || EnableDirectionFromNewIDButBlockExisted.Count != EnableDirectionFromNewIDAndBlockNonExisted.Count) { Debug.Log("[Err Mesg]: EnableDirectionFromNewIDButBlockExisted.Equals(EnableDirectionFromNewIDAndBlockNonExisted)."); Application.Quit(); }

		//===== get The Direction
		int startIndex = -1;
		foreach (int i in EnableDirectionFromNewIDButBlockExisted) startIndex = i;

		if (Flag_ShowDebugInfo) Debug.Log("startIndex: " + startIndex);
		ResetNewHashSet();

		string XY_FromNewIDButBlockExisted = "";
		string START_FromNewIDAndBlockNonRecorded = "";
		if (startIndex == 1)
		{

			XY_FromNewIDButBlockExisted = getSBIDFromNewIDButBlockExisted("X", "BIGGEST");
			START_FromNewIDAndBlockNonRecorded = getSBIDFromNewIDAndBlockNonRecorded("SMALLEST");
		}
		else if (startIndex == 3)
		{
			XY_FromNewIDButBlockExisted = getSBIDFromNewIDButBlockExisted("X", "SMALLEST");
			START_FromNewIDAndBlockNonRecorded = getSBIDFromNewIDAndBlockNonRecorded("SMALLEST");
		}
		else if (startIndex == 2)
		{

			XY_FromNewIDButBlockExisted = getSBIDFromNewIDButBlockExisted("Y", "SMALLEST");
			START_FromNewIDAndBlockNonRecorded = getSBIDFromNewIDAndBlockNonRecorded("BIGGEST");
		}
		else if (startIndex == 0)
		{

			XY_FromNewIDButBlockExisted = getSBIDFromNewIDButBlockExisted("Y", "SMALLEST");
			START_FromNewIDAndBlockNonRecorded = getSBIDFromNewIDAndBlockNonRecorded("SMALLEST");
		}

		AddIDIntoNewHashSet(XY_FromNewIDButBlockExisted);
		AddIDIntoNewHashSet(START_FromNewIDAndBlockNonRecorded);
		OnlyTwoSurfaceStackInSameTime();
	}

	public void OnlyTwoSurfaceStackInSameTime_20170603()
	{
		if (Flag_ShowDebugInfo)
			Debug.Log("OnlyTwoSurfaceStackInSameTime_20170603()");

		string tmpStackedStr = getStrFromHashSet(NewIDs, ",");
		tmpStackedStr = ReOrder(tmpStackedStr);
		bool Flag_LocalContinue = true;  //for stackWithoutBoard

		//if((isJustExistingOneNewBlockID(tmpStackedStr) && CheckNewCorrectIDNum(false))==false) return;

		int tmpC = 0;
		foreach (string Xid in NewIDs)
			if (Xid.Contains("9999")) tmpC++;
		if (tmpC > 1) return;

		if (Flag_AllowStackWithoutConnectingBoard)
		{
			if (tmpStackedStr.Contains(SystemBaseTag + "9999 ") || tmpStackedStr.Contains(SystemBaseTag2 + "9999 "))
			{
				string[] tmps = tmpStackedStr.Split(',');
				if (Flag_ShowDebugInfo) Debug.Log("[Debug Info] draw(): tmp = " + tmpStackedStr);
				for (int i = 0; i < StackedOrders.Count; i++)
				{
					if ((("" + ("" + StackedOrders[i])).Contains(SystemBaseTag + "9998 ") ||
						("" + ("" + StackedOrders[i])).Contains(SystemBaseTag2 + "9998 "))
						&& (("" + StackedOrders[i]).Contains(tmps[0]) || ("" + StackedOrders[i]).Contains(tmps[1])))
					{
						if (StackedOrders.Contains(tmpStackedStr) == false)
						{
							StackedOrders.Add(tmpStackedStr);
							string[] tmppp = ("" + StackedOrders[i]).Split(',');

							if (Flag_ShowSysMesg) Debug.Log("1111~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
							if (Flag_ShowSysMesg) foreach (string stringX in VirtualIDs) Debug.Log("VirtualIDs : 1," + stringX);

							if (Flag_ShowDebugInfo)
							{
								Debug.Log("[Debug Info] Draw() : VirtualIDs.Remove : " + tmppp[0]);
								Debug.Log("[Debug Info] Draw() : VirtualIDs.Remove : " + tmppp[1]);
								Debug.Log(tmppp[0]);
							}
							VirtualIDs.Remove(tmppp[1]);
							VirtualIDs.Remove(tmppp[0]);
							if (Flag_ShowSysMesg) Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
							if (Flag_ShowSysMesg) foreach (string stringX in VirtualIDs) Debug.Log("2," + stringX);
							if (Flag_ShowSysMesg) Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
							if (Flag_ShowSysMesg) Debug.Log("(" + StackedOrders[i] + ("" + StackedOrders[i]));
							//if(StackedOrders.Contains(tmp)==false)
							StackedOrders[i] = tmpStackedStr;
							StackedOrders.Remove(StackedOrders.Count - 1);
							for (int j = 0; j < StackedOrders.Count; j++)
							{
								//if(StackedOrdersRecursive.Contains(StackedOrders[j])==false) 
								StackedOrdersRecursive.Add(StackedOrders[j]);
							}
							ReConstruction();
							Flag_LocalContinue = false;
						}
					}
				}
			}
		}
		if (Flag_LocalContinue)
		{
			AddStrIntoStackedOrders(tmpStackedStr);

			//ComputingThe3DPositionOfNewBlock();
			ReComputingStackedOrders3DFromStackedOrders_20170603();

		}
	}
	private string getStrFromHashSet(HashSet<string> hashset, string splitSymbol)
	{
		string tmp = "";
		foreach (string id in hashset) tmp += id + splitSymbol;
		return tmp.Replace(",,", "");
	}
	public void OnlyTwoSurfaceStackInSameTime()
	{
		string tmpStackedStr = "";
		foreach (string id in NewIDs) tmpStackedStr += id + ",";
		tmpStackedStr = ReOrder((tmpStackedStr + ",").Replace(",,", ""));
		bool Flag_LocalContinue = true;  //for stackWithoutBoard
		string[] tmps = tmpStackedStr.Split(',');
		if ((isJustExistingOneNewBlockID(tmpStackedStr) && CheckNewCorrectIDNum(false)) == false) return;


		if (Flag_AllowStackWithoutConnectingBoard)
		{
			if (tmpStackedStr.Contains(SystemBaseTag + "9999 ") || tmpStackedStr.Contains(SystemBaseTag2 + "9999 "))
			{
				if (Flag_ShowDebugInfo) Debug.Log("[Debug Info] draw(): tmp = " + tmpStackedStr);
				for (int i = 0; i < StackedOrders.Count; i++)
				{
					if ((("" + StackedOrders[i]).Contains(SystemBaseTag + "9998 ") ||
						("" + StackedOrders[i]).Contains(SystemBaseTag2 + "9998 "))
						&& (("" + StackedOrders[i]).Contains(tmps[0]) || ("" + StackedOrders[i]).Contains(tmps[1])))
					{
						if (StackedOrders.Contains(tmpStackedStr) == false)
						{
							StackedOrders.Add(tmpStackedStr);
							string[] tmppp = ("" + StackedOrders[i]).Split(',');

							if (Flag_ShowSysMesg) Debug.Log("1111~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
							if (Flag_ShowSysMesg) foreach (string stringX in VirtualIDs) Debug.Log("VirtualIDs : 1," + stringX);

							if (Flag_ShowDebugInfo)
							{
								Debug.Log("[Debug Info] Draw() : VirtualIDs.Remove : " + tmppp[0]);
								Debug.Log("[Debug Info] Draw() : VirtualIDs.Remove : " + tmppp[1]);
								Debug.Log(tmppp[0]);
							}
							VirtualIDs.Remove(tmppp[1]);
							VirtualIDs.Remove(tmppp[0]);
							if (Flag_ShowSysMesg) Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
							if (Flag_ShowSysMesg) foreach (string stringX in VirtualIDs) Debug.Log("2," + stringX);
							if (Flag_ShowSysMesg) Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
							if (Flag_ShowSysMesg) Debug.Log("StackedOrders[i]" + StackedOrders[i]);
							//if(StackedOrders.Contains(tmp)==false)
							StackedOrders[i] = tmpStackedStr;
							StackedOrders.Remove(StackedOrders.Count - 1);
							for (int j = 0; j < StackedOrders.Count; j++)
							{
								//if(StackedOrdersRecursive.Contains(StackedOrders[j])==false) 
								StackedOrdersRecursive.Add(StackedOrders[j]);
							}
							ReConstruction();
							Flag_LocalContinue = false;
						}
					}
				}
			}
		}
		if (Flag_LocalContinue)
		{
			AddStrIntoStackedOrders(tmpStackedStr);

			ComputingThe3DPositionOfNewBlock();
			//ReComputingStackedOrders3DFromStackedOrders_20170603();

		}
	}
	public void DeleteAndPrintStackedOrders()
	{
		if(Flag_ShowDebugInfo)
			Debug.Log("[Debug Info] DeleteAndPrintStackedOrders(): tmpStackedStr = " + getStrFromHashSet(DispearIDs, ",") + DispearIDs_DispearedTime.Count);
		String targetDel = "";

		ArrayList StackedOrders_CHECK = new ArrayList();
		StackedOrders_CHECK = (ArrayList)StackedOrders.Clone();
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			//if(StackedOrders.get(i).contains(" 0305 ")) System.out.println(StackedOrders.size()+"["+i+"]"+StackedOrders.get(i));
			String[] ids = ("" + StackedOrders[i]).Split(',');
			bool Flag_AllDisappearedInOneStackedOrder = true;
			for (int j = 0; j < ids.Length; j++)
			{
				if (DispearIDs_DispearedTime.ContainsKey(ids[j]) == false)
				{
					Flag_AllDisappearedInOneStackedOrder = false;
					//if(StackedOrders.get(i).contains(" 0305 ")) System.out.println("1 StackedOrders.get(i):" + StackedOrders.get(i) + " = " +ids[j]);
				}
				//					if(ids[j].contains(" 0305 ")) {
				//						System.out.println( StackedOrders.get(i)+ " ----  (" + ids[j]+")");
				//					}
				if (DispearIDs_DispearedTime.ContainsKey(ids[j]) &&
					((CurrentTimeMillis() - (long)DispearIDs_DispearedTime[ids[j]]) < DisappearTime_20170604))
				{
					Flag_AllDisappearedInOneStackedOrder = false;
					//						if(StackedOrders.get(i).contains(" 0305 ")) {
					//							System.out.println("2 StackedOrders.get(i):" + StackedOrders.get(i) + " = " +ids[j]);
					//							System.out.println((System.currentTimeMillis()-DispearIDs_DispearedTime.get(ids[j])));
					//						}
				}
			}
			//System.out.println("DispearIDs_DispearedTime.get(7428 0000 9999 0305 0001): "+(System.currentTimeMillis()-DispearIDs_DispearedTime.get("7428 0000 9999 0305 0001")));

			//			if(StackedOrders.get(i).contains(" 0305 ")) { 
			//				System.out.println("~~~~~~~~~~~> Flag_AllDisappearedInOneStackedOrder : " + Flag_AllDisappearedInOneStackedOrder );
			//			}

			if (Flag_AllDisappearedInOneStackedOrder)
			{
				//System.out.println("diffSize1 :"+(StackedOrders_CHECK.size()-StackedOrders.size()));
				if(Flag_ShowSysMesg)
					Debug.Log("StackedOrders.remove:" + StackedOrders[i]);
				targetDel = ("" + StackedOrders[i]);
				StackedOrders.Remove(StackedOrders[i]);
				//System.out.println("diffSize2 :"+(StackedOrders_CHECK.size()-StackedOrders.size()));
			}
		}

		if (StackedOrders_CHECK.Count - StackedOrders.Count >= 4)
		{
			StackedOrders = StackedOrders_CHECK;
			Debug.Log("Protected.");
		}

		int diffSize = StackedOrders_CHECK.Count - StackedOrders.Count;
		if (diffSize < 4 && diffSize > 0)
		{

			String[] ids = targetDel.Split(',');
			for (int j = 0; j < ids.Length; j++)
			{
				DispearIDs_DispearedTime.Remove(ids[j]);
				if(Flag_ShowSysMesg)
					Debug.Log("Remove:" + ids[j]);
				int blockID = getBlockIDFromIDByINT(ids[j]);
			}
			ReComputingStackedOrders3DFromStackedOrders_20170603();
		}

		if (Flag_ShowSysMesg) printStackedOrders();
		if (Flag_ShowSysMesg) printStackedOrders3D();
	}
	public void DeleteAndPrintStackedOrders(string tmpStackedStr)
	{
		if (Flag_ShowDebugInfo) Debug.Log("[Debug Info] DeleteAndPrintStackedOrders(): tmpStackedStr = " + tmpStackedStr);
		if (tmpStackedStr.Contains(SystemBaseTag + "9998") || tmpStackedStr.Contains(SystemBaseTag2 + "9998")) return;
		try
		{
			//逐一測試
			if (StackedOrders != null && StackedOrders.Count > 0 && StackedOrders.Count > 0)
			{
				foreach (string StackedOrdersX in StackedOrders)
				{
					//偵測到 消失  surface 上的全部  ID (方向) 都不在了，就砍掉

					string[] StackedOrdersXs = StackedOrdersX.Split(',');
					bool TheseIDsInThisStackedOrderAreNotExist = true;
					if (Flag_ToConnectTheReade)
						for (int i = 0; i < StackedOrdersXs.Length; i++)
							if (DispearIDs.Contains(StackedOrdersXs[i]) == false) { TheseIDsInThisStackedOrderAreNotExist = false; break; }
					// Debug.Log(StackedOrdersX+" " + TheseIDsInThisStackedOrderAreNotExist);
					if (TheseIDsInThisStackedOrderAreNotExist)
					{
						if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] DeleteAndPrintStackedOrders(): StackedOrders Deletes ====>" + StackedOrdersX);
						StackedOrders.Remove(StackedOrdersX);
						for (int i = 0; i < StackedOrdersXs.Length; i++)
						{
							X16HS_DetectedTagsOLD.Remove(StackedOrdersXs[i]);
							//如果 這個  BLOCK ID (面、方向) 都沒有再  STACKORDER 中，就砍掉 StackedOrders3D 的資料 ( 還要排除特殊情況 )
							string BlockID = StackedOrdersXs[i].Substring(0, 17);


							bool TheBlockIDNotAllStackedOrder = true;
							foreach (string StackedOrdersX1 in StackedOrders)
								if (StackedOrdersX1.Contains(BlockID) == true
									|| ((BlockID.Substring(10, 3).Equals("920") && BlockID.Substring(15, 2).Equals("01"))
										&& (
											StackedOrdersX1.Contains(SystemBaseTag + "9201 01")
											|| StackedOrdersX1.Contains(SystemBaseTag + "9202 01")
											|| StackedOrdersX1.Contains(SystemBaseTag + "9203 01")

											|| StackedOrdersX1.Contains(SystemBaseTag2 + "9201 01")
											|| StackedOrdersX1.Contains(SystemBaseTag2 + "9202 01")
											|| StackedOrdersX1.Contains(SystemBaseTag2 + "9203 01")


										)
									)
								) { TheBlockIDNotAllStackedOrder = false; break; }

							if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] DeleteAndPrintStackedOrders(): " + StackedOrdersX + " TheBlockIDNotAllStackedOrder:" + TheBlockIDNotAllStackedOrder);
							if (TheBlockIDNotAllStackedOrder)
							{
								if (Flag_ShowSysMesg) Debug.Log("[Sys Mesgs] DeleteAndPrintStackedOrders(): StackedOrders3D.Remove: " + int.Parse(StackedOrdersXs[i].Substring(0, 18).Replace("7428 0000 ", "").Replace(" ", "")));
								StackedOrders3D.Remove(int.Parse(StackedOrdersXs[i].Substring(0, 17).Replace(SystemBaseTag, "").Replace(" ", "")));
								//Eric??
								StackedOrders3D.Remove(int.Parse(StackedOrdersXs[i].Substring(0, 18).Replace(SystemBaseTag, "").Replace(" ", "")));

								StackedOrders3D.Remove(int.Parse(StackedOrdersXs[i].Substring(0, 17).Replace(SystemBaseTag2, "").Replace(" ", "")));
								//Eric??
								StackedOrders3D.Remove(int.Parse(StackedOrdersXs[i].Substring(0, 18).Replace(SystemBaseTag2, "").Replace(" ", "")));
							}

							//特殊方塊 1x3 
							if (StackedOrdersX.Contains("7428 0000 9201 01") || StackedOrdersX.Contains("7428 0000 9202 01") || StackedOrdersX.Contains("7428 0000 9203 01"))
							{
								bool ThisConnectedBlockIDNotAllStackedOrder = true;
								foreach (string StackedOrdersX1 in StackedOrders)
									if (StackedOrdersX1.Contains("7428 0000 9201 01") || StackedOrdersX1.Contains("7428 0000 9202 01") || StackedOrdersX1.Contains("7428 0000 9203 01")) { ThisConnectedBlockIDNotAllStackedOrder = false; break; }
								if (ThisConnectedBlockIDNotAllStackedOrder)
								{
									StackedOrders3D.Remove(920101); StackedOrders3D.Remove(920201); StackedOrders3D.Remove(920301);
								}
							}
						}
					}
				}

			}


			//		int countSpeicalCase =0;
			//		for (string ID : DispearBlockIDs) { if(ID.Contains("7428 0000 9201 01") || ID.Contains("7428 0000 9202 01") || ID.Contains("7428 0000 9203 01")) countSpeicalCase++; }
			//		for (string Orders : StackedOrders) if(Orders.Contains("7428 0000 9201 01") || Orders.Contains("7428 0000 9202 01") || Orders.Contains("7428 0000 9203 01")) countSpeicalCase--;
			//		
			//		for (string ID : DispearBlockIDs) {			
			//			if(ID.Contains("7428 0000 9201 01")  || ID.Contains("7428 0000 9202 01") || ID.Contains("7428 0000 9203 01")) { if(countSpeicalCase==0) StackedOrders3D.Remove(getBlockIDFromDispearBlockIDs(ID)); }
			//			else StackedOrders3D.Remove(getBlockIDFromDispearBlockIDs(ID));
			//		}
		}
		catch (Exception)
		{
			// TODO: handle exception
		}

		ReComputingStackedOrders3DFromStackedOrders_20170603();

		if (Flag_ShowSysMesg) printStackedOrders();
		if (Flag_ShowSysMesg) printStackedOrders3D();

	}

	public void DeleteAndPrintStackedOrders_updatePatchForQuickMove_v1()
	{
		if (Flag_ShowDebugInfo)
			Debug.Log("[Debug Info] DeleteAndPrintStackedOrders_updatePatchForQuickMove()");
		//String targetDel = "";

		ArrayList StackedOrders_CHECK = new ArrayList();
		ArrayList tmpDispearIDs = new ArrayList();
		ArrayList tmpNewIDs = new ArrayList();
		StackedOrders_CHECK = (ArrayList)StackedOrders.Clone();
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			String[] ids = ("" + StackedOrders[i]).Split(',');
			//bool Flag_AllDisappearedInOneStackedOrder = true;
			for (int j = 0; j < ids.Length; j++)
			{
				if (DispearIDs_DispearedTime.ContainsKey(ids[j]) &&
					((CurrentTimeMillis() - (long)DispearIDs_DispearedTime[ids[j]]) > DisappearTime_20170604)

					&& (long)IDs_ReceivedTimeStamp[getStrFromHashSet(NewIDAndNotBeUsed_IDs, "")] > DisappearTime_20170604

				)
				{

					if (Flag_ShowSysMesg)
						Debug.Log("StackedOrders.update:" + StackedOrders[i]);

					StackedOrders.Remove(StackedOrders[i]);





					ReComputingStackedOrders3DFromStackedOrders_20170603();
					return;
				}
			}
		}				
	}
	public void DeleteAndPrintStackedOrders_updatePatchForQuickMove()
	{
		if (Flag_ShowDebugInfo)
			Debug.Log("[Debug Info] DeleteAndPrintStackedOrders_updatePatchForQuickMove()");
		//String targetDel = "";

		ArrayList StackedOrders_CHECK = new ArrayList();
		ArrayList tmpDispearIDs = new ArrayList();
		ArrayList tmpNewIDs = new ArrayList();
		StackedOrders_CHECK = (ArrayList)StackedOrders.Clone();
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			String[] ids = ("" + StackedOrders[i]).Split(',');
			//bool Flag_AllDisappearedInOneStackedOrder = true;
			for (int j = 0; j < ids.Length; j++)
			{
				if (DispearIDs_DispearedTime.ContainsKey(ids[j]) &&
					((CurrentTimeMillis() - (long)DispearIDs_DispearedTime[ids[j]]) > DisappearTime_20170604)

					&& (long)IDs_ReceivedTimeStamp[getStrFromHashSet(NewIDAndNotBeUsed_IDs, "")] > DisappearTime_20170604

				)
				{


					StackedOrders.Remove(StackedOrders[i]);




					ReComputingStackedOrders3DFromStackedOrders_20170603();
					return;
				}
			}
		}
	}

	public void ComputingTheStackedOrders3D_20170603(string stackedOrder)
	{
		if (stackedOrder == null) { Debug.Log("[Sys Err1] ComputingTheStackedOrders3D_20170603(): " + stackedOrder); Application.Quit(); }
		if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] ComputingTheStackedOrders3D_20170603(): " + stackedOrder);
		//System.out. Debug.Log(stackedOrder);
		string[] tmps = stackedOrder.Split(',');
		string topID = "", bottomID = "";
		int X1 = -1, Y1 = -1, Z1 = -1, X2 = -1, Y2 = -1, Z2 = -1, BlockID1 = 0, BlockID2 = 0, BlockSN2 = 0, BlockSurfaceID1 = 0, BlockSurfaceID2 = 0, StackWay1 = 0, StackWay2 = 0, BlockIDType1 = 0, BlockIDType2 = 0;
        //, BlockDirectionID1 = 0, BlockDirectionID2 = 0;

        if (tmps.Length != 2) { Debug.Log("[Err Mesg] ComputingTheStackedOrders3D_20170603() Errir. = " + stackedOrder); return; }

		//================== 組在地板 ===================================================
		if (stackedOrder.Contains(SystemBaseTag + BoardType) || stackedOrder.Contains(SystemBaseTag2 + BoardType))
		{
			if (tmps[0].Contains(SystemBaseTag + BoardType) || tmps[0].Contains(SystemBaseTag2 + BoardType)) { bottomID = tmps[0]; topID = tmps[1]; }
			else { bottomID = tmps[1]; topID = tmps[0]; }
		}
		//================== 裝在已經存在的上面 ===================================================
		else if (StackedOrders3D.ContainsKey(getBlockIDFromIDByINT(tmps[1])) && getBlockSurfaceSNIntFromID(tmps[1]) == 3)
		{ //找 surface 是 03
			bottomID = tmps[1]; topID = tmps[0];
		}
		else if (StackedOrders3D.ContainsKey(getBlockIDFromIDByINT(tmps[0])) && getBlockSurfaceSNIntFromID(tmps[0]) == 3)
		{ //找 surface 是 03
			bottomID = tmps[0]; topID = tmps[1];
		}
		//================== 預先組裝 ===================================================
		else if (getSurfaceSNFromID(tmps[0]).Equals("03"))
		{
			bottomID = tmps[0]; topID = tmps[1];
			// Debug.Log("0BlockTYPE:"+ getBlockTypeIntFromID(bottomID) + "\nBlockID"+getBlockIDFromIDByINT(bottomID)); Debug.Log();
			StackedOrders3D.Add(getBlockIDFromIDByINT(bottomID), new int[] { -1, 5, 1, 3, 123456, getBlockTypeIntFromID(bottomID), getBlockIDFromIDByINT(bottomID) });
		}
		else if (getSurfaceSNFromID(tmps[1]).Equals("03"))
		{
			bottomID = tmps[1]; topID = tmps[0];
			// Debug.Log("1BlockTYPE:"+ getBlockTypeIntFromID(bottomID) + "\nBlockID"+getBlockIDFromIDByINT(bottomID)); Debug.Log();
			StackedOrders3D.Add(getBlockIDFromIDByINT(bottomID), new int[] { -1, 5, 1, 3, 123456, getBlockTypeIntFromID(bottomID), getBlockIDFromIDByINT(bottomID) });
		}
		else
		{
			if(Flag_ShowDebugInfo)
				Debug.Log("[Err Mesg] ComputingTheStackedOrders3D_20170603() . 2=> " + stackedOrder); return;
			//Application.Quit();
		}


		X1 = getDataStackOrderIn3D_WithBoard_ByID(bottomID, "X");
		Y1 = getDataStackOrderIn3D_WithBoard_ByID(bottomID, "Y");
		Z1 = getDataStackOrderIn3D_WithBoard_ByID(bottomID, "Z");
		BlockID1 = getBlockIDFromIDByINT(bottomID);
		BlockIDType1 = getDataStackOrderIn3D_WithBoard_ByID(bottomID, "BlockIDType");
		BlockSurfaceID1 = getDataStackOrderIn3D_WithBoard_ByID(bottomID, "SurfaceSN");
		//getBlockSurfaceSNIntFromID(bottomID);

		StackWay1 = getDataStackOrderIn3D_WithBoard_ByID(bottomID, "StackWay");
		// Debug.Log(bottomID+"=:~ ("+X1+","+Y1+","+Z1+")==> StackWay1,BlockSurfaceID1,contacedSurfacePosition):("+StackWay1+","+BlockSurfaceID1+",3)");


		//1*N
		if (StackedOrders3D.ContainsKey(getBlockIDFromIDByINT(topID)))
		{
			X2 = getDataStackOrderIn3D_WithBoard_ByID(topID, "X");
			Y2 = getDataStackOrderIn3D_WithBoard_ByID(topID, "Y");
			Z2 = getDataStackOrderIn3D_WithBoard_ByID(topID, "Z");

			if (Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2) + Math.Pow(Z1 - Z2, 2)) > 1)
			{

				Debug.Log("Worng 3D");

				BlockSN2 = getBlockSNFromIDByINT(topID);
				BlockIDType2 = getBlockTypeIntFromID(topID);
				//				 Debug.Log("BlockIDType2:"+BlockIDType2);				
				//				 Debug.Log("Distance:"+Math.sqrt(Math.Pow(X1-X2,2)+Math.Pow(Y1-Y2,2)+Math.Pow(Z1-Z2,2)));
				BlockID2 = getBlockIDFromIDByINT(topID);
				// Debug.Log("BlockSN2:"+BlockSN2);
				//				 Debug.Log("BlockID2:"+BlockID2);
				//				 Debug.Log(SystemBaseTag+BlockIDType2+" "+(70)+"01 000");
				// Debug.Log(Math.floor((BlockSN2-70)/3)+" " + (BlockSN2-70)%3);

				int Ntype = 0;
				if (BlockSN2 >= 70 && BlockSN2 < 90) Ntype = 3;
				else if (BlockSN2 >= 90) Ntype = 2;

				for (int i = 0; i < StackedOrders.Count; i++)
				{
					for (int j = 0; j < Ntype; j++)
					{
						int tmpSN = (int)(70 + Math.Floor(0f + (BlockSN2 - 70) / 3) * 3 + j);
						StackedOrders3D.Remove(BlockIDType2 * 100 + tmpSN);
					}
				}
				foreach (string XXXX in StackedOrders)
				{
					for (int j = 0; j < Ntype; j++)
					{

						int tmpSN = (int)(70 + Math.Floor(0f + (BlockSN2 - 70) / 3) * 3 + j);
						Debug.Log("DEL target:" + tmpSN);
						if (XXXX.Contains(SystemBaseTag + BlockIDType2 + " " + (tmpSN) + "01 000")
							|| XXXX.Contains(SystemBaseTag2 + BlockIDType2 + " " + (tmpSN) + "01 000"))
						{
							Debug.Log("DEL1:" + tmpSN);
							Debug.Log(XXXX);
							StackedOrders.Remove(XXXX);
						}
					}
				}
				//printStackedOrders3D();
				//printStackedOrders();
				return;
				//if (false)
				//	for (int i = 0; i < StackedOrders.Count; i++)
				//	{
				//		for (int j = 0; j < Ntype; j++)
				//		{

				//			//20180107		
				//			try {
				//				//int tmpSN = (int)(70 + Math.Floor(0f + (BlockSN2 - 70) / 3) * 3 + (BlockSN2 - 70) % 3 + j);
				//				int tmpSN = (int)(70 + Math.Floor(0f + (BlockSN2 - 70) / 3) * 3 +  j);
				//				//Debug.Log(tmpSN);
				//				// Debug.Log((70+Math.floor((BlockSN2-70)/3)*3+(BlockSN2-70)%3)+i);
				//				//20180107		
				//				//if (("" + StackedOrders[i]).Contains(SystemBaseTag + BlockIDType2 + " " + (tmpSN) + "01 000")
				//				//|| ("" + StackedOrders[i]).Contains(SystemBaseTag2 + BlockIDType2 + " " + (tmpSN) + "01 000")
				//				if (("" + StackedOrders[i]).Contains(SystemBaseTag + BlockIDType2 + " " + (tmpSN) + "01 000")
				//					|| ("" + StackedOrders[i]).Contains(SystemBaseTag2 + BlockIDType2 + " " + (tmpSN) + "01 000")
				//				) {
				//					Debug.Log("DEL:" + tmpSN);
				//					Debug.Log(("" + StackedOrders[i]));
				//					//StackedOrders.Remove(i);
				//					StackedOrders.Remove(StackedOrders[i]);


				//					printStackedOrders3D();
				//					printStackedOrders();

				//					//									try {
				//					//							for (int jjj = tmpSN;
				//					//						
				//				}
				//			}
				//			catch (Exception e) { }
				//		}
				//	}
			}
			return;
		}


		X2 = X1; Y2 = Y1; Z2 = Z1 + 1; //Z:more Direction in The future
		BlockID2 = getBlockIDFromIDByINT(topID);
		BlockIDType2 = getBlockTypeIntFromID(topID);
		BlockSurfaceID2 = getBlockSurfaceSNIntFromID(topID);

		int contacedSurfacePosition = ("" + StackWay1 + "").IndexOf(BlockSurfaceID1 + "") + 1;  //1=DOWN, 2=FRONT, 3=UP, 4=BACK, 5=RIGHT, 6=LEFT		
		int[] tmpIs = C3DR.Computing3DRotation(StackWay1, BlockSurfaceID2, contacedSurfacePosition, X1, Y1, Z1);
		X2 = tmpIs[0]; Y2 = tmpIs[1]; Z2 = tmpIs[2];    //StackWay2 = tmpIs[3];	
		int ThisBlockDirection = -1;
		if (BlockIDType2 % 100 == 1)
		{
			int EnableBelowID = getEnableDirectionIDFromID(bottomID) - 1; //0:front, 1:left, 2:back, 3:right
			int BelowBlockDirection = -1;
			BelowBlockDirection = getBlockHozRotation(StackWay1);
			ThisBlockDirection = (EnableBelowID + BelowBlockDirection) % 4;
			StackWay2 = getBlockHozRotation(ThisBlockDirection);
		}
		// Debug.Log(".");




		if (StackedOrders3D.ContainsKey(BlockID2) == false) StackedOrders3D.Add(BlockID2, new int[] { X2, Y2, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 });

		//有紀錄
		//沒紀錄:直接新增給一個 -----很大，然後在下次新增的時候 判斷負很大



		//20170531
		MakeUpTheBlock_1xN_20170619(X2, Y2, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2, ThisBlockDirection);
	}
	public void Operating3DPosition(string stringX)
	{
		if (stringX == null) { Debug.Log("[Sys Err1] Operating3DPosition: " + stringX); Application.Quit(); }
		if (stringX.Contains(SystemBaseTag) == false && stringX.Contains(SystemBaseTag2) == false) { Debug.Log("[Sys Err1] Operating3DPosition: " + stringX); Application.Quit(); }
		string str_NewBlockAndSurfaceIDs1 = ReOrder(getStr_NewBlockAndSurfaceIDs());
		if (Flag_ShowDebugInfo) Debug.Log("[Debug Info.] Enter Operating3DPosition:: " + stringX + " <=> " + str_NewBlockAndSurfaceIDs1);

		string[] strss = stringX.Split(',');
		string strssX = "";
		for (int i = 0; i < strss.Length; i++) if (strssX.Contains(getIDLevelStr(strss[i], 1)) == false) strssX += getIDLevelStr(strss[i], 1) + ",";
		strssX = (strssX + ",").Replace(",,", "");
		string str_NewBlockAndSurfaceIDs = strssX;

		if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] Operating3DPosition(): " + str_NewBlockAndSurfaceIDs);


		string[] tmpStrs = str_NewBlockAndSurfaceIDs.Split(',');
		int X1 = -1, Y1 = -1, Z1 = -1, X2 = -1, Y2 = -1, Z2 = -1, BlockID1 = 0, BlockID2 = 0, BlockSurfaceID1 = 0, BlockSurfaceID2 = 0, StackWay1 = 0, StackWay2 = 0, BlockIDType1 = 0, BlockIDType2 = 0;
        //, BlockDirectionID1 = 0, BlockDirectionID2 = 0;



        //處理浮在半空中的，預組裝！
        try
        {

			if (str_NewBlockAndSurfaceIDs1.Contains(SystemBaseTag + "9999 ") == false
				&& str_NewBlockAndSurfaceIDs1.Contains(SystemBaseTag + "9998 ") == false
				&& str_NewBlockAndSurfaceIDs1.Contains(SystemBaseTag2 + "9999 ") == false
				&& str_NewBlockAndSurfaceIDs1.Contains(SystemBaseTag2 + "9998 ") == false
				&& tmpStrs.Length == 2)
			{
				BlockID1 = getBlockID(tmpStrs[0], 0); BlockID2 = getBlockID(tmpStrs[1], 1);//eric
				if (StackedOrders3D.ContainsKey(BlockID1) == false && StackedOrders3D.ContainsKey(BlockID2) == false && (BlockID1 >= 930170 && BlockID1 <= 930193))
				{
					if (tmpStrs[0].Substring(18).Equals("1"))
					{
						string tmpppp = tmpStrs[0]; tmpStrs[0] = tmpStrs[1]; tmpStrs[1] = tmpppp; int tmppI = BlockID1; BlockID1 = BlockID2; BlockID2 = tmppI;
					}

					X1 = -3 - BlockID1 % 10; Y1 = -3; Z1 = 1;
					BlockID1 = 0; BlockSurfaceID1 = 0; StackWay1 = 123456; BlockIDType1 = 9998;
					if (Flag_ShowSysMesg) Debug.Log(tmpStrs[0] + "=: (" + X1 + "," + Y1 + "," + Z1 + ")==> BlockID1,StackWay1,BlockSurfaceID1,BlockIDType1,BlockDirectionID1):(" + BlockID1 + "," + StackWay1 + "," + BlockSurfaceID1 + "," + BlockIDType1 + ")");

					//======================= Block on the Board ========================
					X2 = X1; Y2 = Y1; Z2 = Z1 + 1;
					BlockID2 = getBlockID(tmpStrs[0], 1);
					BlockIDType2 = getBlockTypeFromID(tmpStrs[0], 0);
					BlockSurfaceID2 = getBlockSurfaceID(tmpStrs[0], 1);
					StackWay2 = 123456;

					int[] tmpArray2 = { X2, Y2, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 };
					if (StackWay2 < 0) { if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] Cannot find the StackWay Model."); Application.Quit(); }
					//					if(StackedOrders3D.ContainsKey(BlockID2)==false) StackedOrders3D.Add(BlockID2, tmpArray2);
					if (Flag_ShowSysMesg) Debug.Log(tmpStrs[0] + "=: (" + X2 + "," + Y2 + "," + Z2 + ")==> BlockID2,StackWay2,BlockSurfaceID2,BlockIDType2):(" + BlockID1 + "," + StackWay2 + "," + BlockSurfaceID2 + "," + BlockIDType2 + ")\n");

					countNonStackOnTheBoard++;

					if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] To add virtual board for these blocks stacking out of board, and reconstruct all stackedOrder.");
					if (Flag_ShowDebugInfo) Debug.Log("[Debug Info.] : Operating3DPosition : tmpStrs[0] : " + tmpStrs[0]);


					StackedOrders.Remove(StackedOrders.Count - 1);
					//					NewIDs = new HashSet<string>(); NewBlockAndSurfaceIDs = new HashSet<string>(); NewBlockIDs = new HashSet<string>(); DispearIDs = new HashSet<string>();	 NewBlockTyping = new HashSet<string>();
					//					DispearIDs = new HashSet<string>(); DispearBlockAndSurfaceIDs = new HashSet<string>(); DispearBlockIDs = new HashSet<string>(); DispearBlockTyping = new HashSet<string>();	
					//addoper("7428 0000 9998 0202 0001,7428 0000 9201 5001 0001");
					// Debug.Log(tmpStrs[0].Substring(0,19));
					//=Application.Quit();

					//ToCountNumOf9998
					int countNum9998 = 0;
					for (int i = 0; i < StackedOrders.Count; i++)
						if (("" + StackedOrders[i]).Contains(SystemBaseTag + "9998") || ("" + StackedOrders[i]).Contains(SystemBaseTag2 + "9998")) countNum9998++;
					VirtualIDs.Add("7428 0000 9998 0" + (countNum9998 * 2) + "03 0001");
					VirtualIDs.Add(tmpStrs[0].Substring(0, 18) + "1 0001");
					addoper("7428 0000 9998 0" + (countNum9998 * 2) + "03 0001," + tmpStrs[0].Substring(0, 18) + "1 0001");

					//					NewIDs = new HashSet<string>(); NewBlockAndSurfaceIDs = new HashSet<string>(); NewBlockIDs = new HashSet<string>(); DispearIDs = new HashSet<string>();	 NewBlockTyping = new HashSet<string>();
					//					DispearIDs = new HashSet<string>(); DispearBlockAndSurfaceIDs = new HashSet<string>(); DispearBlockIDs = new HashSet<string>(); DispearBlockTyping = new HashSet<string>();	
					addoper(stringX);
					return;
				}
			}
		}
		catch (Exception) { }

		if ((tmpStrs[0].Contains(SystemBaseTag + "9998 ") || tmpStrs[0].Contains(SystemBaseTag2 + "9998 ")) && IsIDAllowOpweatinginDB(tmpStrs[1]))
		{
			if (Flag_ShowSysMesg) Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~1");
			//======================= Board Type Block ========================
			string tmpStrs0 = tmpStrs[0].Replace(SystemBaseTag + "9998 ", "");
			tmpStrs0.Replace(SystemBaseTag2 + "9998 ", "");
			X1 = int.Parse(tmpStrs0.Substring(0, 2)) - 1; Y1 = int.Parse(tmpStrs0.Substring(2, 2)) - 1; Z1 = 0;
			BlockID1 = 0; BlockSurfaceID1 = 0; StackWay1 = 123456; BlockIDType1 = 9998;
			if (Flag_ShowSysMesg) Debug.Log(tmpStrs[0] + "=: (" + X1 + "," + Y1 + "," + Z1 + ")==> BlockID1,StackWay1,BlockSurfaceID1,BlockIDType1,BlockDirectionID1):(" + BlockID1 + "," + StackWay1 + "," + BlockSurfaceID1 + "," + BlockIDType1 + ")");

			//======================= Block on the Board ========================
			X2 = X1; Y2 = Y1; Z2 = 1;
			BlockID2 = getBlockID(tmpStrs[1], 1);
			BlockIDType2 = getBlockTypeFromID(tmpStrs[1], 0);
			BlockSurfaceID2 = getBlockSurfaceID(tmpStrs[1], 1);

			StackWay2 = 123456;

			int[] tmpArray2 = { -X2, -Y2, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 };
			if (StackWay2 < 0) { if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] Cannot find the StackWay Model."); Application.Quit(); }
			if (StackedOrders3D.ContainsKey(BlockID2) == false) StackedOrders3D.Add(BlockID2, tmpArray2);
			if (Flag_ShowSysMesg) Debug.Log(tmpStrs[1] + "=: (" + X2 + "," + Y2 + "," + Z2 + ")==> BlockID2,StackWay2,BlockSurfaceID2,BlockIDType2):(" + BlockID2 + "," + StackWay2 + "," + BlockSurfaceID2 + "," + BlockIDType2 + ")\n");
		}
		else if ((tmpStrs[0].Contains(SystemBaseTag + "9999 ") || tmpStrs[0].Contains(SystemBaseTag2 + "9999 ")) && IsIDAllowOpweatinginDB(tmpStrs[1]))
		{
			if (Flag_ShowSysMesg) Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~2");
			//======================= Board Type Block ========================
			string tmpStrs0 = tmpStrs[0].Replace(SystemBaseTag + "9999 ", "").Replace(SystemBaseTag2 + "9999 ", "");
			// Debug.Log("tmpStrs[0]:" + tmpStrs[0]);
			// Debug.Log("tmpStrs0:"+ tmpStrs0);
			X1 = int.Parse(tmpStrs0.Substring(0, 2)) - 1;
			Y1 = int.Parse(tmpStrs0.Substring(2, 2)) - 1;
			Z1 = 0;
			BlockID1 = 0; BlockSurfaceID1 = 0; StackWay1 = 123456; BlockIDType1 = 9999;
			// Debug.Log(tmpStrs[0]+"=: ("+X1+","+Y1+","+Z1+")==> BlockID1,StackWay1,BlockSurface1,BlockType1,BlockDirection1):("+BlockID1+","+StackWay1+","+BlockSurfaceID1+","+BlockIDType1+")");	

			//======================= Block on the Board ========================
			X2 = X1; Y2 = Y1; Z2 = 1;
			BlockID2 = getBlockID(tmpStrs[1], 1);
			BlockIDType2 = getBlockTypeFromID(tmpStrs[1], 0);
			BlockSurfaceID2 = getBlockSurfaceID(tmpStrs[1], 1);

			if (BlockIDType2 % 100 == 1)
			{
				if (NewIDs.Contains(tmpStrs[0] + " 0001")) StackWay2 = 123456;
				else if (NewIDs.Contains(tmpStrs[0] + " 0002")) StackWay2 = 153642;
				else if (NewIDs.Contains(tmpStrs[0] + " 0003")) StackWay2 = 143265;
				else if (NewIDs.Contains(tmpStrs[0] + " 0004")) StackWay2 = 163524;
			}
			else if (BlockIDType2 % 100 == 2)
			{
				if (NewIDs.Contains(tmpStrs[0] + " 0001") && NewIDs.Contains(tmpStrs[0] + " 0002")) StackWay2 = 123456;
				else if (NewIDs.Contains(tmpStrs[0] + " 0002") && NewIDs.Contains(tmpStrs[0] + " 0003")) StackWay2 = 153642;
				else if (NewIDs.Contains(tmpStrs[0] + " 0003") && NewIDs.Contains(tmpStrs[0] + " 0004")) StackWay2 = 143265;
				else if (NewIDs.Contains(tmpStrs[0] + " 0004") && NewIDs.Contains(tmpStrs[0] + " 0001")) StackWay2 = 163524;
			}
			else if (BlockIDType2 % 100 == 3)
			{
				if (NewIDs.Contains(tmpStrs[0] + " 0001") && NewIDs.Contains(tmpStrs[0] + " 0002") && NewIDs.Contains(tmpStrs[0] + " 0003")) StackWay2 = 123456;
				else if (NewIDs.Contains(tmpStrs[0] + " 0001") && NewIDs.Contains(tmpStrs[0] + " 0002") && NewIDs.Contains(tmpStrs[0] + " 0004")) StackWay2 = 163524;
				else if (NewIDs.Contains(tmpStrs[0] + " 0001") && NewIDs.Contains(tmpStrs[0] + " 0003") && NewIDs.Contains(tmpStrs[0] + " 0004")) StackWay2 = 143265;
				else if (NewIDs.Contains(tmpStrs[0] + " 0002") && NewIDs.Contains(tmpStrs[0] + " 0003") && NewIDs.Contains(tmpStrs[0] + " 0004")) StackWay2 = 153642;
			}

			int[] tmpArray2 = { X2, Y2, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 };
			if (StackWay2 < 0) { Debug.Log("[Sys Mesg] Cannot find the StackWay Model."); Application.Quit(); }
			if (StackedOrders3D.ContainsKey(BlockID2) == false) StackedOrders3D.Add(BlockID2, tmpArray2);
			if (Flag_ShowSysMesg) Debug.Log(tmpStrs[1] + "=: (" + X2 + "," + Y2 + "," + Z2 + ")==> BlockID2,StackWay2,BlockSurface2,BlockType2):(" + BlockID2 + "," + StackWay2 + "," + BlockSurfaceID2 + "," + BlockIDType2 + ")\n");
		}
		else
		{
			try
			{
				//============== Get Block ID & Order them, [0] is existing in record =================
				BlockID1 = getBlockID(tmpStrs[0], 0);
				BlockID2 = getBlockID(tmpStrs[1], 0);

				if (StackedOrders3D.ContainsKey(BlockID1) == false && StackedOrders3D.ContainsKey(BlockID2) == true)
				{
					string tmpppp = tmpStrs[0]; tmpStrs[0] = tmpStrs[1]; tmpStrs[1] = tmpppp; int tmppI = BlockID1; BlockID1 = BlockID2; BlockID2 = tmppI;
				}
				//============== Get Existing Block's Data ==================
				X1 = ((int[])StackedOrders3D[BlockID1])[0];
				Y1 = ((int[])StackedOrders3D[BlockID1])[1];
				Z1 = ((int[])StackedOrders3D[BlockID1])[2];

				BlockSurfaceID1 = getBlockSurfaceID(tmpStrs[0], 0);
				StackWay1 = ((int[])StackedOrders3D[BlockID1])[4];
				BlockIDType1 = getBlockTypeFromID(tmpStrs[0], 0);
				BlockSurfaceID2 = getBlockSurfaceID(tmpStrs[1], 0);
				int contacedSurfacePosition = ("" + StackWay1 + "").IndexOf(BlockSurfaceID1 + "") + 1;
				//1=DOWN, 2=FRONT, 3=UP, 4=BACK, 5=RIGHT, 6=LEFT

				if (Flag_ShowSysMesg) Debug.Log(tmpStrs[0] + "=:~ (" + X1 + "," + Y1 + "," + Z1 + ")==> StackWay1,BlockSurfaceID1,contacedSurfacePosition):(" + StackWay1 + "," + BlockSurfaceID1 + "," + contacedSurfacePosition + ")");

				BlockIDType2 = getBlockTypeFromID(tmpStrs[1], 0);
				int[] tmpIs = C3DR.Computing3DRotation(StackWay1, BlockSurfaceID2, contacedSurfacePosition, X1, Y1, Z1);
				X2 = tmpIs[0]; Y2 = tmpIs[1]; Z2 = tmpIs[2];    //StackWay2 = tmpIs[3];	

				int ThisBlockDirection = -1;



				if (BlockIDType2 % 100 == 1)
				{
					int EnableBelowID = -1;
					for (int i = 1; i <= 4; i++) if (NewIDs.Contains(tmpStrs[0] + " 000" + i)) { EnableBelowID = i - 1; break; }
					//0:front, 1:left, 2:back, 3:right
					int BelowBlockDirection = -1;
					BelowBlockDirection = getBlockHozRotation(StackWay1);
					ThisBlockDirection = (EnableBelowID + BelowBlockDirection) % 4;
					StackWay2 = getBlockHozRotation(ThisBlockDirection);
				}
				///OLD Compound 
				//				else if(BlockIDType2%100==2) {
				//					if(NewIDs.Contains(tmpStrs[0]+" 0001") && NewIDs.Contains(tmpStrs[0]+" 0002"))ThisBlockDirection = 0;
				//					else if(NewIDs.Contains(tmpStrs[0]+" 0002") && NewIDs.Contains(tmpStrs[0]+" 0003"))ThisBlockDirection = 1;
				//					else if(NewIDs.Contains(tmpStrs[0]+" 0003") && NewIDs.Contains(tmpStrs[0]+" 0004"))ThisBlockDirection = 2;
				//					else if(NewIDs.Contains(tmpStrs[0]+" 0004") && NewIDs.Contains(tmpStrs[0]+" 0001"))ThisBlockDirection = 3;
				//					ThisBlockDirection=(ThisBlockDirection+getBlockHozRotation(StackWay1))%4;
				//					StackWay2=getBlockHozRotation(ThisBlockDirection);
				//				} else if(BlockIDType2%100==3){
				//					if(NewIDs.Contains(tmpStrs[0]+" 0001") && NewIDs.Contains(tmpStrs[0]+" 0002") && NewIDs.Contains(tmpStrs[0]+" 0003"))ThisBlockDirection = 0;
				//					else if(NewIDs.Contains(tmpStrs[0]+" 0001") && NewIDs.Contains(tmpStrs[0]+" 0002") && NewIDs.Contains(tmpStrs[0]+" 0004"))ThisBlockDirection = 1;
				//					else if(NewIDs.Contains(tmpStrs[0]+" 0001") && NewIDs.Contains(tmpStrs[0]+" 0003") && NewIDs.Contains(tmpStrs[0]+" 0004"))ThisBlockDirection = 2;
				//					else if(NewIDs.Contains(tmpStrs[0]+" 0002") && NewIDs.Contains(tmpStrs[0]+" 0003") && NewIDs.Contains(tmpStrs[0]+" 0004"))ThisBlockDirection = 3;
				//					ThisBlockDirection=(ThisBlockDirection+getBlockHozRotation(StackWay1))%4;
				//					StackWay2=getBlockHozRotation(ThisBlockDirection);
				//				}

				if (Flag_ShowSysMesg) Debug.Log(tmpStrs[0] + "=:~ (" + X1 + "," + Y1 + "," + Z1 + ")==> StackWay1,BlockSurfaceID1,contacedSurfacePosition):(" + StackWay1 + "," + BlockSurfaceID1 + "," + contacedSurfacePosition + ")");
				if (Flag_ShowSysMesg) Debug.Log(tmpStrs[1] + "=: (" + X2 + "," + Y2 + "," + Z2 + ")==> StackWay2,BlockSurfaceID2):(" + StackWay2 + "," + BlockSurfaceID2 + ")\n");

				int[] tmpArray2 = { X2, Y2, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 };
				StackedOrders3D.Add(BlockID2, tmpArray2);

				//20170531
				MakeUpTheBlock_1xN_20170619(X2, Y2, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2, ThisBlockDirection);
			}
			catch (Exception) { Debug.Log(stringX); Application.Quit(); }
		}
		return;
	}
	public void addoper(string string1)
	{
		string tmp = ReOrder(string1);
		string[] tmps = tmp.Split(',');
		if (StackedOrders.Contains(tmp) == false) StackedOrders.Add(tmp);
		if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] addoper(): " + string1);
		NewIDs = new HashSet<string>(); NewBlockAndSurfaceIDs = new HashSet<string>(); NewBlockIDs = new HashSet<string>(); DispearIDs = new HashSet<string>(); NewBlockTyping = new HashSet<string>();
		DispearIDs = new HashSet<string>(); DispearBlockAndSurfaceIDs = new HashSet<string>(); DispearBlockIDs = new HashSet<string>(); DispearBlockTyping = new HashSet<string>();
		for (int i = 0; i < tmps.Length; i++)
		{
			AddIDIntoNewHashSet(tmps[i]);
			//			NewIDs.Add(tmps[i]); 
			//			NewBlockTyping.Add(""+getBlockTypeFromID(tmps[i], 0));
			//			NewBlockIDs.Add(tmps[i].Substring(0,17)); 
			//			NewBlockAndSurfaceIDs.Add(tmps[i].Substring(0,19)); 
			if (Flag_ShowDebugInfo) Debug.Log("=====>" + tmps[i].Substring(0, 19));
			X16HS_DetectedTags.Add(tmps[i]);
			X16HS_DetectedTagsOLD.Add(tmps[i]);
		}

		if (Flag_AllowStackWithoutConnectingBoard)
		if (tmp.Contains(SystemBaseTag + "9999 ") || tmp.Contains(SystemBaseTag2 + "9999 "))
		{
			for (int i = 0; i < StackedOrders.Count; i++)
			{
				if ((("" + StackedOrders[i]).Contains(SystemBaseTag + "9998 ") ||
					("" + StackedOrders[i]).Contains(SystemBaseTag2 + "9998 "))
					&& (("" + StackedOrders[i]).Contains(tmps[0]) || ("" + StackedOrders[i]).Contains(tmps[1])))
				{
					if (Flag_ShowSysMesg) Debug.Log("StackedOrders[i]" + StackedOrders[i]);
					if (StackedOrders.Contains(string1) == false)
						StackedOrders[i] = string1;
					StackedOrders.Remove(StackedOrders.Count - 1);

					for (int j = 0; j < StackedOrders.Count; j++)
					{
						if (StackedOrdersRecursive.Contains(StackedOrders[j]) == false)
							StackedOrdersRecursive.Add(StackedOrders[j]);
					}


					//StackedOrdersRecursive.AddAll(StackedOrders);
					//StackedOrdersRecursive = new ArrayList();
					ReConstruction();

					return;
				}
			}
		}
		ComputingThe3DPositionOfNewBlock();
	}
	public void AddStrIntoStackedOrders(string target)
	{
		string tmpS = ReOrder(target);
		if(Flag_ShowSysMesg)
			Debug.Log("AddStrIntoStackedOrders():" + target);
		if (StackedOrders.Contains(tmpS) == false) StackedOrders.Add(tmpS);
		if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] AddandPrintStackedOrders(): StackedOrders Adds ===> " + tmpS);
		string[] tmps = tmpS.Split(',');
		for (int i = 0; i < tmps.Length; i++)
			X16HS_DetectedTagsOLD.Add(tmps[i]);
	}

	public void ReComputingStackedOrders3DFromStackedOrders_20170603()
	{
		//ToBeAdded Check and Fix The StackedOrders 
		update_fix_StackedOrders();
		StackedOrders3D = new Dictionary<int, int[]>();
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			ComputingTheStackedOrders3D_20170603(("" + StackedOrders[i]));
			//Operating3DPosition((""+StackedOrders[i]));
		}
	}
	public void ComputingThe3DPositionOfNewBlock()
	{
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			string[] tmpStrs = ("" + StackedOrders[i]).Split(',');
			for (int j = 0; j < tmpStrs.Length; j++)
			{
				if (((StackedOrders3D.ContainsKey(getBlockID(tmpStrs[j], 0)) == false)
					&& tmpStrs[j].Contains(SystemBaseTag + "9999 ") == false
					&& tmpStrs[j].Contains(SystemBaseTag + "9998 ") == false
					&& tmpStrs[j].Contains(SystemBaseTag2 + "9999 ") == false
					&& tmpStrs[j].Contains(SystemBaseTag2 + "9998 ") == false
				)
				)
				{
					Operating3DPosition(("" + StackedOrders[i]));

					if (Flag_ShowDebugInfo) Debug.Log("[Debug Info] ComputingThe3DPositionOfNewBlock:tmpStrs[" + j + "]:" + tmpStrs[j]);
				}
			}
		}
	}
	public void ForceDeleteBlock(int targetDELBlockID)
	{
		Debug.Log("[Sys Mesg] Force Deleted: " + targetDELBlockID);
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			if (("" + StackedOrders[i]).Contains("7428 6666 0" + targetDELBlockID))
				StackedOrders.Remove(i);
			else if (("" + StackedOrders[i]).Contains("7428 7777 0" + targetDELBlockID))
				StackedOrders.Remove(i);
			else if (("" + StackedOrders[i]).Contains("7428 8888 0" + targetDELBlockID))
				StackedOrders.Remove(i);
		}
		StackedOrders3D.Remove(targetDELBlockID);
	}
	private void MakeUpTheBlock_1xN_20170601(int X2, int Y2, int Z2, int BlockSurfaceID2, int StackWay2, int BlockIDType2, int BlockID2, int ThisBlockDirection)
	{
		//Handle 1xN, N>1 - 20170531
		int pureBlockID = BlockID2 % 100;
		if (BlockIDType2 >= 9100 && BlockIDType2 < 9500 && pureBlockID >= 70 && pureBlockID <= 99)
		{
			// Debug.Log(pureBlockID);
			// Debug.Log("ThisBlockDirection+1:"+ ThisBlockDirection+1);
			int BlockLength = 3, ModeIDBase = 70;
			if (pureBlockID >= 90) { BlockLength = 2; ModeIDBase = 90; }
			int BlockIndex = (pureBlockID - ModeIDBase) % BlockLength;
			//Find The Starting Position
			int LocalX = X2, LocalY = Y2;
			if (ThisBlockDirection + 1 == 1)
			{
				LocalX -= BlockIndex;
				for (int i = 0; i < BlockLength; i++) StackedOrders3D.Add(BlockID2 + i - BlockIndex,
					new int[] { LocalX + i, LocalY, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 + i - BlockIndex });
			}
			if (ThisBlockDirection + 1 == 3)
			{
				LocalX += BlockIndex;
				for (int i = 0; i < BlockLength; i++) StackedOrders3D.Add(BlockID2 + i - BlockIndex,
					new int[] { LocalX - i, LocalY, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 + i - BlockIndex });
			}
			if (ThisBlockDirection + 1 == 2)
			{
				LocalY -= BlockIndex;
				for (int i = 0; i < BlockLength; i++) StackedOrders3D.Add(BlockID2 + i - BlockIndex,
					new int[] { LocalX, LocalY + i, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 + i - BlockIndex });
			}
			if (ThisBlockDirection + 1 == 4)
			{
				LocalY += BlockIndex;
				for (int i = 0; i < BlockLength; i++) StackedOrders3D.Add(BlockID2 + i - BlockIndex,
					new int[] { LocalX, LocalY - i, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 + i - BlockIndex });
			}
		}
	}

	private void MakeUpTheBlock_1xN_20170619(int X2, int Y2, int Z2, int BlockSurfaceID2, int StackWay2, int BlockIDType2, int BlockID2, int ThisBlockDirection)
	{
		//Handle 1xN, N>1 - 20170531
		int pureBlockID = BlockID2 % 100;
		if (BlockIDType2 >= 9100 && BlockIDType2 < 9500 && pureBlockID >= 70 && pureBlockID <= 99)
		{
			// Debug.Log(pureBlockID);
			// Debug.Log("ThisBlockDirection+1:"+ ThisBlockDirection+1);
			int BlockLength = 3, ModeIDBase = 70;
			if (pureBlockID >= 90) { BlockLength = 2; ModeIDBase = 90; }
			int BlockIndex = (pureBlockID - ModeIDBase) % BlockLength;
			//Find The Starting Position

			// Debug.Log(ThisBlockDirection);
			int LocalX = X2, LocalY = Y2;
			if (ThisBlockDirection + 1 == 3)
			{
				//Debug.Log(11);
				LocalX -= BlockIndex;
				for (int i = 0; i < BlockLength; i++)
				{
					try
					{
						StackedOrders3D.Add(BlockID2 + i - BlockIndex,
							new int[] { LocalX + i, LocalY, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 + i - BlockIndex });
					}
					catch (Exception) { }
				}
			}
			if (ThisBlockDirection + 1 == 1)
			{
				//Debug.Log(22);
				LocalX += BlockIndex;
				for (int i = 0; i < BlockLength; i++)
				{
					//Debug.Log("ADD : " + (BlockID2 + i - BlockIndex) + " , " + BlockLength);
					try
					{
						StackedOrders3D.Add(BlockID2 + i - BlockIndex, new int[] { LocalX - i, LocalY, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 + i - BlockIndex });
					} catch (Exception) { }
				}
			}
			if (ThisBlockDirection + 1 == 4)
			{

				LocalY -= BlockIndex;
				for (int i = 0; i < BlockLength; i++) {
					try
					{
						StackedOrders3D.Add(BlockID2 + i - BlockIndex,
							new int[] { LocalX, LocalY + i, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 + i - BlockIndex });
					}
					catch (Exception) { }
				}
			}
			if (ThisBlockDirection + 1 == 2)
			{

				LocalY += BlockIndex;
				for (int i = 0; i < BlockLength; i++) {
					try {
						StackedOrders3D.Add(BlockID2 + i - BlockIndex,
							new int[] { LocalX, LocalY - i, Z2, BlockSurfaceID2, StackWay2, BlockIDType2, BlockID2 + i - BlockIndex });
					}
					catch (Exception) { }
				}
			}
		}
	}

	//===========OnlyTools========================================================
	private void AddIDIntoNewHashSet(string targetID)
	{
		NewIDs.Add(targetID);
		NewBlockTyping.Add(getBlockTypeFromID(targetID));
		NewBlockIDs.Add(getBlockIDFromID(targetID));
		NewBlockAndSurfaceIDs.Add(getBlockAndSurfaceIDFromID(targetID));
	}


	public int getCountDispearedBigerThanDispearTime()
	{
		int tmpi = 0;
		foreach (String id in DispearIDs_DispearedTime.Keys)
			if (CurrentTimeMillis() - (long)DispearIDs_DispearedTime[id] > DisappearTime_20170604)
			{
				tmpi++;
			}
		return tmpi;
	}

	//===========Re========================================================
	public void ReConstruction()
	{
		AllClean();
		StackedOrders = new ArrayList();
		StackedOrders3D = new Dictionary<int, int[]>();
		countNonStackOnTheBoard--;
		if (Flag_ShowSysMesg) Debug.Log("countNonStackOnTheBoard:" + countNonStackOnTheBoard);
		for (int j = 0; j < StackedOrdersRecursive.Count; j++)
			Debug.Log("StackedOrdersRecursive[" + j + "]:" + StackedOrdersRecursive[j]);

		if (StackedOrdersRecursive.Count > StackedOrders.Count)
		{
			for (int j = 0; j < StackedOrdersRecursive.Count; j++)
			{
				if (StackedOrders.Count == (j) && StackedOrders.Contains(StackedOrdersRecursive[j]) == false)
					addoper((string)StackedOrdersRecursive[j]);
			}
			//00:37
			StackedOrdersRecursive = new ArrayList();
		}
	}
	private string ReOrder(string tmpStackedStr)
	{
		if (tmpStackedStr.Substring(tmpStackedStr.Length - 1) == ",")
			tmpStackedStr = tmpStackedStr.Substring(0, tmpStackedStr.Length - 1);
		// Debug.Log("ReOrder:"+tmpStackedStr+"=");
		string[] strs = tmpStackedStr.Split(',');
		// Debug.Log(strs.Length);
		if (strs.Length > 1)
		{
			long[] ints = new long[strs.Length];
			for (int i = 0; i < ints.Length; i++)
			{
				// Debug.Log("ReOrder:+strs["+i+"]" + strs[i]);
				ints[i] = long.Parse(strs[i].Replace(SystemBaseTag, "").Replace(SystemBaseTag2, "").Replace(" ", ""));
			}

			for (int i = 0; i < ints.Length; i++)
			{
				for (int j = i; j < ints.Length; j++)
				{
					if (i != j)
					{
						if (ints[i] < ints[j])
						{
							long tmpI = ints[i]; ints[i] = ints[j]; ints[j] = tmpI;
							string tmpS = strs[i]; strs[i] = strs[j]; strs[j] = tmpS;
						}
					}
				}
			}
			string tmpSS = "";
			for (int i = 0; i < ints.Length - 1; i++)
			{
				tmpSS += strs[i] + ",";
			}
			tmpSS += strs[ints.Length - 1];
			return tmpSS;
		}
		else if (strs.Length == 1)
		{
			return tmpStackedStr;
		}
		else
		{
			// Debug.Log("[Sys Err] ReOrder: "+tmpStackedStr);
			//Application.Quit();
		}
		return "";
	}
	private void ResetNewHashSet()
	{
		NewIDs = new HashSet<string>(); NewBlockAndSurfaceIDs = new HashSet<string>();
		NewBlockIDs = new HashSet<string>(); NewBlockTyping = new HashSet<string>();
	}
	private void ResetDispearHashSet()
	{
		DispearIDs = new HashSet<string>();
		DispearBlockTyping = new HashSet<string>();
		DispearBlockIDs = new HashSet<string>();
		DispearBlockAndSurfaceIDs = new HashSet<string>();
	}

	//===========Clean========================================================
	public void CleanTAGArray()
	{
		try
		{
			X16HS_DetectedTags = new HashSet<string>();
		}
		catch (Exception)
		{
		}
	}
	public void CleanAllBlocks()
	{
		AllClean();
	}
	/*ToBeMakeUp*/
	public void AllClean()
	{
		LastRestartTime = CurrentTimeMillis() + 1000;
		X16HS_DetectedTags = new HashSet<string>();
		X16HS_DetectedTagsOLD = new HashSet<string>();

		NewIDs = new HashSet<string>(); NewBlockAndSurfaceIDs = new HashSet<string>(); NewBlockIDs = new HashSet<string>(); NewBlockTyping = new HashSet<string>(); DispearIDs = new HashSet<string>(); DispearBlockAndSurfaceIDs = new HashSet<string>(); DispearBlockIDs = new HashSet<string>(); DispearBlockTyping = new HashSet<string>();
		IDSets = new Dictionary<string, Dictionary<string, Dictionary<string, HashSet<string>>>>();
		IDSetsNum = new Dictionary<string, int>();
		//VirtualIDs = new HashSet<string>();

		StackedOrders3D = new Dictionary<int, int[]>();
		countNonStackOnTheBoard = 0;
		//StackedOrdersRecursive = new ArrayList();
		StackedOrders = new ArrayList();
		DispearCount = new Dictionary<string, long>();
		//Flag_AddPreStackOnce = false;
		if (Flag_ShowSysMesg) Debug.Log("[Sys Mesg] System Restart Completed.\n\n\n\n\n\n\n\n\n\n\n");
	}
	public void ClearIDReceivingBufferPerTimeSlot()
	{
		if (CurrentTimeMillis() - LastRestartTime > RestartTime) { CleanTAGArray(); LastRestartTime = CurrentTimeMillis(); }
	}

	//===========Get========================================================
	//X,Y,Z,SurfaceID,StackWay,BlockIDType,FacingDirect
	public int getDataStackOrderIn3D(int BlockID, string target)
	{
		if (target.Equals("X")) return ((int[])StackedOrders3D[BlockID])[0];
		if (target.Equals("Y")) return ((int[])StackedOrders3D[BlockID])[1];
		if (target.Equals("Z")) return ((int[])StackedOrders3D[BlockID])[2];
		if (target.Equals("SurfaceID"))
			return ((int[])StackedOrders3D[BlockID])[3];
		if (target.Equals("StackWay")) return ((int[])StackedOrders3D[BlockID])[4];
		if (target.Equals("BlockIDType")) return ((int[])StackedOrders3D[BlockID])[5];
		if (target.Equals("FacingDirect")) return getBlockHozRotation(((int[])StackedOrders3D[BlockID])[4]);
		return -1;
	}
	public int getDataStackOrderIn3D_WithBoard(string id, int BlockID, string target)
	{
		if (id.Contains(SystemBaseTag + BoardType) || id.Contains(SystemBaseTag2 + BoardType))
		{
			string tmp = id.Replace(SystemBaseTag + BoardType, "").Replace(SystemBaseTag2 + BoardType, "");
			//			 Debug.Log(tmp.Substring(0,2));
			int x = int.Parse(tmp.Substring(0, 2));
			int y = int.Parse(tmp.Substring(2, 2));
			//int z = 1;
			if (target.Equals("X")) return x;
			if (target.Equals("Y")) return y;
			if (target.Equals("Z")) return 1;
			if (target.Equals("SurfaceID")) return 3;
			if (target.Equals("StackWay")) return 123456;
			if (target.Equals("BlockIDType")) return 9999;
			if (target.Equals("FacingDirect")) return 0;
		}
		else
		{

			if (target.Equals("X")) return ((int[])StackedOrders3D[BlockID])[0];
			if (target.Equals("Y")) return ((int[])StackedOrders3D[BlockID])[1];
			if (target.Equals("Z")) return ((int[])StackedOrders3D[BlockID])[2];
			if (target.Equals("SurfaceID")) return ((int[])StackedOrders3D[BlockID])[3];
			if (target.Equals("StackWay")) return ((int[])StackedOrders3D[BlockID])[4];
			if (target.Equals("BlockIDType")) return ((int[])StackedOrders3D[BlockID])[5];
			if (target.Equals("FacingDirect")) return getBlockHozRotation(((int[])StackedOrders3D[BlockID])[4]);
		}
		return -1;
	}
	public int getDataStackOrderIn3D_WithBoard_ByID(string id, string target)
	{
		if (id.Contains(SystemBaseTag + BoardType) || id.Contains(SystemBaseTag2 + BoardType))
		{
			string tmp = id.Replace(SystemBaseTag + BoardType, "").Replace(SystemBaseTag2 + BoardType, "");
			//			 Debug.Log(tmp.Substring(0,2));
			int x = int.Parse(tmp.Substring(0, 2));
			int y = int.Parse(tmp.Substring(2, 2));
			//int z = 1;
			if (target.Equals("X")) return x - 1;
			if (target.Equals("Y")) return y - 1;
			if (target.Equals("Z")) return 0;
			if (target.Equals("SurfaceID")) return 3;
			if (target.Equals("SurfaceSN")) return 3;
			if (target.Equals("StackWay")) return 123456;
			if (target.Equals("BlockIDType")) return 9999;
			if (target.Equals("FacingDirect")) return 0;
		}
		else
		{
			int BlockID = getBlockIDFromIDByINT(id);
			if (target.Equals("X")) return ((int[])StackedOrders3D[BlockID])[0];
			if (target.Equals("Y")) return ((int[])StackedOrders3D[BlockID])[1];
			if (target.Equals("Z")) return ((int[])StackedOrders3D[BlockID])[2];
			if (target.Equals("SurfaceID")) return ((int[])StackedOrders3D[BlockID])[3];
			if (target.Equals("SurfaceSN")) return getBlockSurfaceSNIntFromID(id);
			if (target.Equals("StackWay")) return ((int[])StackedOrders3D[BlockID])[4];
			if (target.Equals("BlockIDType")) return ((int[])StackedOrders3D[BlockID])[5];
			if (target.Equals("FacingDirect")) return getBlockHozRotation(((int[])StackedOrders3D[BlockID])[4]);
		}
		return -1;
	}
	public int[] getBlockXYZInStackOrder3D(int BlockID)
	{
		try
		{
			return new int[] { ((int[])StackedOrders3D[BlockID])[0], ((int[])StackedOrders3D[BlockID])[1], ((int[])StackedOrders3D[BlockID])[2] };
		}
		catch (Exception)
		{
			Debug.Log(BlockID);
			Application.Quit();
		}
		return null;
	}
	public int[] getBlockXYZInStackOrder3DFromID_forboard(string id, int BlockID)
	{
		try
		{
			if (id.Contains(SystemBaseTag + BoardType) || id.Contains(SystemBaseTag2 + BoardType))
			{
				string tmp = id.Replace(SystemBaseTag + BoardType, "").Replace(SystemBaseTag2 + BoardType, "");
				//				 Debug.Log(tmp.Substring(0,2));
				int x = int.Parse(tmp.Substring(0, 2));
				int y = int.Parse(tmp.Substring(2, 2));
				int z = 1;
				return new int[] { x, y, z };
			}

			return new int[] { ((int[])StackedOrders3D[BlockID])[0], ((int[])StackedOrders3D[BlockID])[1], ((int[])StackedOrders3D[BlockID])[2] };
		}
		catch (Exception)
		{
			Debug.Log(BlockID);

			Application.Quit();
		}
		return null;
	}
	private int getBlockID(string str, int index)
	{
		try
		{
			str = str.Replace(SystemBaseTag, "").Replace(SystemBaseTag2, "").Replace(" ", "");
			//for (int i = 0; i < RecordedTagID.Length; i++) str = str.Replace(RecordedTagID[i]+" ", "");

			string[] strs = str.Split(',');
			//			 Debug.Log(str);
			//			if(strs.Length==1)
			//			 Debug.Log("getBlockID:'"+strs[0].Substring(0, 6)+"'");
			//			if(strs.Length==2)
			//			 Debug.Log("getBlockID:'"+strs[1].Substring(0, 6)+"'");

			if (strs.Length > 0)
			{
				if (strs.Length == 1 || index == 0)
				{
					return int.Parse(strs[0].Substring(0, 6));
				}
				else if (index == 1)
				{
					return int.Parse(strs[1].Substring(0, 6));
				}
			}
		}
		catch (Exception)
		{
			Debug.Log(str);

			Application.Quit();
		}

		return -1;
	}
	private int getBlockSurfaceID(string strA, int index)
	{
		string str = strA.Replace(SystemBaseTag, "").Replace(SystemBaseTag2, "");
		for (int i = 0; i < AllowBlockType.Length; i++) str = str.Replace(AllowBlockType[i] + " ", "");

		string[] strs = str.Split(',');
		//		 Debug.Log(strA+" - getBlockSurfaceID's str:"+str);
		//		 Debug.Log("'"+strs[0].Substring(2, 4)+"'");
		//		 Debug.Log("'"+strs[1].Substring(2, 4)+"'");

		if (strs.Length > 0)
		{
			if (strs.Length == 1 || index == 0)
			{
				return int.Parse(strs[0].Substring(2, 2));
			}
			else if (index == 1)
			{
				return int.Parse(strs[1].Substring(2, 2));
			}
		}
		return -1;
	}
	private string getBlockTypeFromID(string id)
	{
		return "" + getBlockTypeFromID(id, 0);
	}
	private int getBlockTypeIntFromID(string str)
	{
		return getBlockTypeFromID(str, 0);
	}
	private int getBlockTypeFromID(string str, int index)
	{
		str = str.Replace(SystemBaseTag, "").Replace(SystemBaseTag2, "").Replace(" ", "");
		//for (int i = 0; i < RecordedTagID.Length; i++) str = str.Replace(RecordedTagID[i]+" ", "");

		string[] strs = str.Split(',');
		//		 Debug.Log(str);
		//		if(strs.Length==1)
		//		 Debug.Log("getBlockID:'"+strs[0].Substring(0, 6)+"'");
		//		if(strs.Length==2)
		//		 Debug.Log("getBlockID:'"+strs[1].Substring(0, 6)+"'");

		if (strs.Length > 0)
		{
			if (strs.Length == 1 || index == 0)
			{
				return int.Parse(strs[0].Substring(0, 4));
			}
			else if (index == 1)
			{
				return int.Parse(strs[1].Substring(0, 4));
			}
		}
		return -1;
	}
	private int getBlockHozRotation(int target)
	{
		if (target < 4)
		{
			if (target == 0) return 123456;
			else if (target == 1) return 153642;
			else if (target == 2) return 143265;
			else if (target == 3) return 163524;
		}
		else if (target > 100000)
		{
			if (target == 123456) return 0;
			else if (target == 153642) return 1;
			else if (target == 143265) return 2;
			else if (target == 163524) return 3;
		}
		return -1;
	}
	private string getStr_NewBlockAndSurfaceIDs()
	{
		string tmps = "";
		foreach (string t in NewBlockAndSurfaceIDs)
		{
			tmps += t + ",";
			// Debug.Log(t);
		}
		return tmps.Replace(",,", "");
	}
	private int getBlockIDFromIDByINT(string id)
	{
		if (id.Length < 20)
		{
			Debug.Log("[Err Mesg]: getBlockIDFromID(): id length not enough.");
			Debug.Log("id: " + id);
			Application.Quit();
		}
		return getINTByReplacingSpace(id.Substring(0, 17));
	}
	private int getBlockSNFromIDByINT(string id)
	{
		if (id.Length < 20)
		{
			Debug.Log("[Err Mesg]: getBlockIDFromID(): id length not enough.");
			Debug.Log("id: " + id);
			Application.Quit();
		}
		return getINTByReplacingSpace(id.Substring(15, 2));
	}
	private string getBlockIDFromID(string id)
	{
		if (id.Length < 20)
		{
			Debug.Log("[Err Mesg]: getBlockIDFromID(): id length not enough.");
			Debug.Log("id: " + id);
			Application.Quit();
		}
		return id.Substring(0, 17);
	}
	private string getBlockAndSurfaceIDFromID(string id)
	{
		if (id.Length < 20)
		{
			Debug.Log("[Err Mesg]: getBlockAndSurfaceIDFromID(): id length not enough.");
			Debug.Log("id: " + id);
			Application.Quit();
		}
		return id.Substring(0, 19);
	}
	private string getSurfaceSNFromID(string id)
	{
		if (id.Length < 20)
		{
			Debug.Log("[Err Mesg]: getBlockAndSurfaceIDFromID(): id length not enough.");
			Debug.Log("id: " + id);
			Application.Quit();
		}
		return id.Substring(17, 2);
	}
	private int getBlockSurfaceSNIntFromID(string id)
	{
		if (id.Length < 20)
		{
			Debug.Log("[Err Mesg]: getBlockAndSurfaceIDFromID(): id length not enough.");
			Debug.Log("id: " + id);
			Application.Quit();
		}
		return int.Parse(id.Substring(17, 2));
	}
	private int getEnableDirectionIDFromID(string id)
	{
		if (id.Length < 20)
		{
			Debug.Log("[Err Mesg]: getBlockIDFromID(): id length not enough.");
			Debug.Log("id: " + id);
			Application.Quit();
		}
		// Debug.Log(id.Substring(20));
		return int.Parse(id.Substring(20));
	}
	private int getBlockIDFromDispearBlockIDs(string id)
	{
		return int.Parse(id.Replace(" ", "").Substring(8, 6));   ///(8-14)  Eric20170720
	}
	private string getSBIDFromNewIDButBlockExisted(string twoD, string condition)
	{
		string targetID = "";
		if (twoD.Equals("X"))
		{
			if (condition.Equals("BIGGEST"))
			{
				int tmpBX = -99;
				foreach (string id in NewIDButBlockExisted_IDs)
				{
					int tmpX = getDataStackOrderIn3D_WithBoard(id, getBlockIDFromIDByINT(id), "X");
					if (tmpX > tmpBX) { tmpBX = tmpX; targetID = id; }
				}
			}
			else if (condition.Equals("SMALLEST"))
			{
				int tmpBX = 100;
				foreach (string id in NewIDButBlockExisted_IDs)
				{
					int tmpX = getDataStackOrderIn3D_WithBoard(id, getBlockIDFromIDByINT(id), "X");
					if (tmpX < tmpBX) { tmpBX = tmpX; targetID = id; }
				}
			}
		}
		else if (twoD.Equals("Y"))
		{
			if (condition.Equals("BIGGEST"))
			{
				int tmpBY = -99;
				foreach (string id in NewIDButBlockExisted_IDs)
				{
					int tmpY = getDataStackOrderIn3D_WithBoard(id, getBlockIDFromIDByINT(id), "Y");
					if (tmpY > tmpBY) { tmpBY = tmpY; targetID = id; }
				}
			}
			else if (condition.Equals("SMALLEST"))
			{
				int tmpBY = 100;
				foreach (string id in NewIDButBlockExisted_IDs)
				{
					int tmpY = getDataStackOrderIn3D_WithBoard(id, getBlockIDFromIDByINT(id), "Y");
					if (tmpY < tmpBY) { tmpBY = tmpY; targetID = id; }
				}
			}
		}
		return targetID;
	}
	private HashSet<int> getEnableDirectionFromNewIDAndBlockNonExisted()
	{
		HashSet<int> tmpEnableDirection = new HashSet<int>();
		foreach (string id in NewIDAndBlockNonRecorded_IDs)
		{
			// Debug.Log(id);
			//tmpXYZs.Add(getBlockXYZInStackOrder3D(getINTByReplacingSpace(BlockID)));
			int NewDirectionInExistedBlock = getEnableDirectionIDFromID(id);
			// Debug.Log(ExistedBlockDirection+1);
			// Debug.Log(NewDirectionInExistedBlock);

			int ThisBlockDirection = (NewDirectionInExistedBlock);
			// Debug.Log("ThisBlockDirection:"+ThisBlockDirection);
			tmpEnableDirection.Add(ThisBlockDirection);
		}
		if (tmpEnableDirection == null || tmpEnableDirection.Count == 0)
		{
			Debug.Log("[Err Mesg] getEnableDirectionFromNewIDAndBlockNonRecorded().");
			Application.Quit();
		}
		return tmpEnableDirection;
	}
	private HashSet<int> getEnableDirectionFromNewIDButBlockExisted()
	{
		HashSet<int> tmpEnableDirection = new HashSet<int>();
		foreach (string id in NewIDButBlockExisted_IDs)
		{
			// Debug.Log(id);
			//tmpXYZs.Add(getBlockXYZInStackOrder3D(getINTByReplacingSpace(BlockID)));
			int ExistedBlockDirection = getDataStackOrderIn3D_WithBoard(id, getINTByReplacingSpace(getBlockIDFromID(id)), "FacingDirect");
			int NewDirectionInExistedBlock = getEnableDirectionIDFromID(id);
			// Debug.Log(ExistedBlockDirection+1);
			// Debug.Log(NewDirectionInExistedBlock);

			int ThisBlockDirection = (NewDirectionInExistedBlock + ExistedBlockDirection) % 4;
			// Debug.Log("ThisBlockDirection:"+ThisBlockDirection);
			tmpEnableDirection.Add(ThisBlockDirection);
		}
		if (tmpEnableDirection == null || tmpEnableDirection.Count == 0)
		{
			if(Flag_ShowDebugInfo)
				Debug.Log("[Err Mesg] getEnableDirectionFromNewIDButBlockExisted().");
			Application.Quit();
		}
		return tmpEnableDirection;
	}
	private string getSBIDFromNewIDAndBlockNonRecorded(string condition)
	{
		string targetID = "";
		if (condition.Equals("BIGGEST"))
		{
			int tmpSID = -100;
			foreach (string id in NewIDAndBlockNonRecorded_IDs)
			{
				int BSN = getBlockIDFromIDByINT(id) % 100;
				if (tmpSID < BSN) { tmpSID = BSN; targetID = id; }
			}
		}
		else if (condition.Equals("SMALLEST"))
		{
			int tmpSID = 100;
			foreach (string id in NewIDAndBlockNonRecorded_IDs)
			{
				int BSN = getBlockIDFromIDByINT(id) % 100;
				if (tmpSID > BSN) { tmpSID = BSN; targetID = id; }
			}
		}
		return targetID;
	}
	private double getMaxDistanceFromNewIDAndBlockNonExist()
	{
		HashSet<int> tmpBlockIDs = new HashSet<int>();
		foreach (string BlockID in NewIDAndBlockNonRecorded_BlockIDs)
		{
			tmpBlockIDs.Add(getINTByReplacingSpace(BlockID));
			// Debug.Log(getINTByReplacingSpace(BlockID)%100);
		}

		//== ToBeAdded : Check All Belong a object : ex 1x3, 1x2
		int maxID = -1, minID = 999;
		double maxDistance = -1;
		foreach (int BlockSN in tmpBlockIDs)
		{
			int tmpBlockSN = BlockSN % 100;
			if (tmpBlockSN > maxID) maxID = tmpBlockSN;
			if (tmpBlockSN < minID) minID = tmpBlockSN;
		}

		if (maxID != minID && maxID - minID < 3) maxDistance = maxID - minID;

		return maxDistance;
	}
	private double getMaxDistanceFromNewIDButBlockExisted()
	{
		HashSet<int[]> tmpXYZs = new HashSet<int[]>();
		foreach (string id in NewIDButBlockExisted_IDs)
			tmpXYZs.Add(getBlockXYZInStackOrder3DFromID_forboard(id, getBlockIDFromIDByINT(id)));


		double maxDistance = -1;
		foreach (int[] x in tmpXYZs)
		{
			foreach (int[] y in tmpXYZs)
			{
				if (x.Equals(y) == false)
				{
					//					System.out. Debug.Log("y:"); for (int i : y) System.out. Debug.Log(i+" "); Debug.Log();
					//					System.out. Debug.Log("x:"); for (int i : x) System.out. Debug.Log(i+" "); Debug.Log();
					//					 Debug.Log(Math.sqrt(Math.Pow(x[0]-y[0], 2)+Math.Pow(x[1]-y[1], 2)+Math.Pow(x[2]-y[2], 2)));
					maxDistance = Math.Sqrt(Math.Pow(x[0] - y[0], 2) + Math.Pow(x[1] - y[1], 2) + Math.Pow(x[2] - y[2], 2));
				}
			}
		}
		return maxDistance;
	}
	private int getINTByReplacingSpace(string tmpID)
	{
		try
		{
			// Debug.Log("=>tmpID:"+tmpID);
			return int.Parse(tmpID.Replace(SystemBaseTag, " ").Replace(SystemBaseTag2, " ").Replace(" ", ""));
		}
		catch (Exception)
		{
			Debug.Log("tmpID:" + tmpID);

			Application.Quit();
		}
		return -1;
	}
	private string getIDLevelStr(string KeyName, int ii)
	{

		//7428 0000 9203
		//7428 0000 9203 01
		//7428 0000 9203 0101
		//7428 0000 9203 0101 0003
		//		 Debug.Log(getIDLevelStr(KeyName,3));
		//		 Debug.Log(getIDLevelStr(KeyName,2));
		//		 Debug.Log(getIDLevelStr(KeyName,1));
		//		 Debug.Log(getIDLevelStr(KeyName,0));
		int Length = ii - 1;
		int Offset = 0;
		if (ii == 3) Length = 2;
		if (ii == 2) { Length = 2; Offset = 3; }
		if (ii == 1) Length = 1;
		if (ii == 0) Length = 0;

		return KeyName.Substring(0, KeyName.Length - 4 * Length - Length + Offset);
	}

	//===========Check========================================================
	public bool CheckTags(string tagetID)
	{
		if (DetectedTags.Contains(tagetID)) return true;
		return false;
	}
	private bool checkIDisAllow(string keyName)
	{
		for (int i = 0; i < AllowBlockType.Length; i++)
			if (keyName.Contains(SystemBaseTag + AllowBlockType[i]) || keyName.Contains(SystemBaseTag2 + AllowBlockType[i])) return true;
		return false;
	}
	private bool CheckNewCorrectIDNum(bool printable)
	{
		int tmpCount = 0;
		foreach (string stringX in NewBlockIDs)
		{
			// Debug.Log(string);
			if (NewBlockAndSurfaceIDs.Contains(stringX + "01"))
			{
				// Debug.Log(string+"01");
				// Debug.Log(getBlockSurfaceID(string, 0));
				int tmpBlockType = int.Parse(stringX.Substring(12, 2));
				if (tmpBlockType != 99) tmpCount += tmpBlockType * 2;
			}
		}
		if (tmpCount == NewIDs.Count) { if (printable) Debug.Log(tmpCount + " = " + NewIDs.Count); return true; }
		return false;
	}
	private bool CheckDispearCorrectIDNum(bool printable)
	{
		int tmpCount = 0;
		foreach (string ID in DispearBlockAndSurfaceIDs)
		{
			Debug.Log(ID);
			//			if(DispearBlockAndSurfaceIDs.Contains(string+"01")) {
			//				// Debug.Log(string+"01");
			//				// Debug.Log(getBlockSurfaceID(string, 0));
			//				int tmpBlockType = int.Parse(string.Substring(12,14));
			//				if(tmpBlockType!=99) tmpCount+=tmpBlockType*2;
			//			}
		}
		if (tmpCount == NewIDs.Count) { if (printable) Debug.Log(tmpCount + " = " + NewIDs.Count); return true; }
		return false;
	}
	private bool IsIDAllowOpweatinginDB(string tmpStr)
	{
		for (int i = 0; i < AllowBlockType.Length; i++)
			if (tmpStr.Contains(SystemBaseTag + AllowBlockType[i] + " ") || tmpStr.Contains(SystemBaseTag2 + AllowBlockType[i] + " ")) return true;
		return false;
	}
	private bool isJustExistingOneNewBlockID(string tmpStackedStr)
	{
		if (Flag_ShowDebugInfo) Debug.Log("[Debug Info] isJustExistingOneNewBlockID(): input Str= " + tmpStackedStr);
		if (StackedOrders.Contains(tmpStackedStr) == false)
		{
			int CountInStackedOrders = 0;
			foreach (string ID in NewBlockIDs)
			{
				for (int i = 0; i < StackedOrders.Count; i++)
				{
					if (Flag_ShowDebugInfo) Debug.Log("[Debug Info] isJustExistingOneNewBlockID(): ID = " + ID);
					if (("" + StackedOrders[i]).Contains(ID)
						//&& VirtualIDs.Contains(ID)==false
					) { CountInStackedOrders++; break; }
				}
			}

			if (Flag_ShowDebugInfo) Debug.Log("[Debug Info] isJustExistingOneNewBlockID(): NewBlockIDs.Count!=CountInStackedOrders = " + NewBlockIDs.Count + " " + CountInStackedOrders);
			if (NewBlockIDs.Count != CountInStackedOrders) return true;
			/* for 5x5 board */
			if (NewBlockIDs.Count == CountInStackedOrders
				&& (tmpStackedStr.Contains(SystemBaseTag + "9999 ") || tmpStackedStr.Contains(SystemBaseTag2 + "9999 "))) return true;
		}
		return false;
	}
	private bool notInStackOrder(string KeyName)
	{
		if (VirtualIDs.Contains(KeyName)) return true;
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			if (("" + StackedOrders[i]).Contains(KeyName)) return false;
		}
		return true;
	}

	//===========Update========================================================
	private void Update_StackedIDs()
	{
		foreach (string orders in StackedOrders)
		{
			string[] tmpIDs = orders.Split(',');
			for (int i = 0; i < tmpIDs.Length; i++)
			{
				StackedIDs.Add(tmpIDs[i]);
				StackedBlockTyping.Add(getBlockTypeFromID(tmpIDs[i]));
				StackedBlockIDs.Add(getBlockIDFromID(tmpIDs[i]));
				StackedBlockAndSurfaceIDs.Add(getBlockAndSurfaceIDFromID(tmpIDs[i]));
			}
		}
	}
	private void Update_NewIDButBlockExisted()
	{
		foreach (string id in NewIDs)
		{
			string tmpBlockID = getBlockIDFromID(id);
			if (id.Contains(SystemBaseTag + BoardType) || id.Contains(SystemBaseTag2 + BoardType))
			{
				NewIDButBlockExisted_IDs.Add(id);
				NewIDButBlockExisted_BlockTyping.Add(getBlockTypeFromID(id));
				NewIDButBlockExisted_BlockIDs.Add(getBlockIDFromID(id));
				NewIDButBlockExisted_BlockAndSurfaceIDs.Add(getBlockAndSurfaceIDFromID(id));
			}
			else
				foreach (string order in StackedOrders)
				{
					if (order.Contains(tmpBlockID))
					{
						NewIDButBlockExisted_IDs.Add(id);
						NewIDButBlockExisted_BlockTyping.Add(getBlockTypeFromID(id));
						NewIDButBlockExisted_BlockIDs.Add(getBlockIDFromID(id));
						NewIDButBlockExisted_BlockAndSurfaceIDs.Add(getBlockAndSurfaceIDFromID(id));
					}
				}


		}
	}
	private void Update_NewIDAndBlockNonRecorded()
	{
		foreach (string id in NewIDs)
		{
			if (NewIDButBlockExisted_IDs.Contains(id) == false)
			{
				NewIDAndBlockNonRecorded_IDs.Add(id);
				NewIDAndBlockNonRecorded_BlockTyping.Add(getBlockTypeFromID(id));
				NewIDAndBlockNonRecorded_BlockIDs.Add(getBlockIDFromID(id));

			}

			NewIDAndBlockNonRecorded_BlockAndSurfaceIDs.Add(getBlockAndSurfaceIDFromID(id));
		}
	}
	private void UpdateDetectedTags()
	{
		if (CurrentTimeMillis() - LastRestartTime > RestartTime / 4 * 3)
			DetectedTags = X16HS_DetectedTags;
	}
	private void Update_NewIDs()
	{
		foreach (string id in X16HS_DetectedTags)
		{
			if (X16HS_DetectedTagsOLD.Contains(id) == false || VirtualIDs.Contains(id))
			{
				if (Flag_ShowSysMesg) foreach (string KeyName1 in VirtualIDs) Debug.Log("VirtualIDs:" + KeyName1);
				if (checkIDisAllow(id))
				{
					if (notInStackOrder(id))
					{
						AddIDIntoNewHashSet(id);

						if (IDSets.ContainsKey(getIDLevelStr(id, 3)) == false)
							IDSets.Add(getIDLevelStr(id, 3), new Dictionary<string, Dictionary<string, HashSet<string>>>());
						if (IDSets[getIDLevelStr(id, 3)].ContainsKey(getIDLevelStr(id, 2)) == false)
							IDSets[getIDLevelStr(id, 3)].Add(getIDLevelStr(id, 2), new Dictionary<string, HashSet<string>>());
						if (IDSets[getIDLevelStr(id, 3)][getIDLevelStr(id, 2)].ContainsKey(getIDLevelStr(id, 1)) == false)
							IDSets[getIDLevelStr(id, 3)][getIDLevelStr(id, 2)].Add(getIDLevelStr(id, 1), new HashSet<string>());
						if (IDSets[getIDLevelStr(id, 3)][getIDLevelStr(id, 2)][getIDLevelStr(id, 1)].Contains(getIDLevelStr(id, 0)) == false)
							IDSets[getIDLevelStr(id, 3)][getIDLevelStr(id, 2)][getIDLevelStr(id, 1)].Add(id);
						for (int i = 0; i < 4; i++)
						{
							if (IDSetsNum.ContainsKey(getIDLevelStr(id, i)) == false)
								IDSetsNum.Add(getIDLevelStr(id, i), 1);
							else
								IDSetsNum.Add(getIDLevelStr(id, i), (int)IDSetsNum[getIDLevelStr(id, i)] + 1);
						}
					}
				}
			}
		}
	}
	private void Update_NewIDs_20170605()
	{
		//2018
		string tmp_existed_id = "";
		foreach (string id in DetectedIDs)
		{
			try
			{
				// Debug.Log("Update_NewIDs_20170605():==============================" + id);
				if (NoiseIDs.Contains(id) == false)
				if (StackedIDs.Contains(id) == false)
				{
					//2018
					if (tmp_existed_id.Length < 2)
					{
						if (id.Contains(SystemBaseTag)) tmp_existed_id = SystemBaseTag;
						if (id.Contains(SystemBaseTag2)) tmp_existed_id = SystemBaseTag2;
					}
					//2018
					if (id.Contains(tmp_existed_id))
						AddIDIntoNewHashSet(id);
				}
			}
			catch
			{
			}
		}
	}

	private void Update_NonUsedIDs_20181115() {
		//2018
		NewIDAndNotBeUsed_IDs = new HashSet<string>();
		string tmp_existed_id = "";
		foreach (string id in DetectedIDs)
		{
			try
			{
				// Debug.Log("Update_NewIDs_20170605():==============================" + id);
				if (NoiseIDs.Contains(id) == false)
				if (StackedIDs.Contains(id) == false)
				{

					//


					//2018
					if (tmp_existed_id.Length < 2)
					{
						if (id.Contains(SystemBaseTag)) tmp_existed_id = SystemBaseTag;
						if (id.Contains(SystemBaseTag2)) tmp_existed_id = SystemBaseTag2;
					}
					//2018
					if (id.Contains(tmp_existed_id))
					{
						NewIDAndNotBeUsed_IDs.Add(id);
						//Debug.Log("Update_NonUsedIDs_20181115: id : " + id);
					}
					//AddIDIntoNewHashSet(id);
				}
			}
			catch
			{
			}
		}
	}

	private void CleanVarsEachLoop()
	{
		IDSets = new Dictionary<string, Dictionary<string, Dictionary<string, HashSet<string>>>>();
		IDSetsNum = new Dictionary<string, int>();
		NewIDs = new HashSet<string>(); NewBlockAndSurfaceIDs = new HashSet<string>(); NewBlockIDs = new HashSet<string>(); NewBlockTyping = new HashSet<string>();
		DispearIDs = new HashSet<string>(); DispearBlockAndSurfaceIDs = new HashSet<string>(); DispearBlockIDs = new HashSet<string>(); DispearBlockTyping = new HashSet<string>();

		StackedIDs = new HashSet<string>(); StackedBlockAndSurfaceIDs = new HashSet<string>();
		StackedBlockIDs = new HashSet<string>(); StackedBlockTyping = new HashSet<string>();

		NewIDButBlockExisted_IDs = new HashSet<string>(); NewIDButBlockExisted_BlockAndSurfaceIDs = new HashSet<string>();
		NewIDButBlockExisted_BlockIDs = new HashSet<string>(); NewIDButBlockExisted_BlockTyping = new HashSet<string>();

		NewIDAndBlockNonRecorded_IDs = new HashSet<string>(); NewIDAndBlockNonRecorded_BlockAndSurfaceIDs = new HashSet<string>();
		NewIDAndBlockNonRecorded_BlockIDs = new HashSet<string>(); NewIDAndBlockNonRecorded_BlockTyping = new HashSet<string>();
	}
	private void Update_DispearIDs()
	{
		//string tmpDispearStackedStr="";
		/*CHECK DELETED*/
		//if(tmpStackedStr.Length()==0)
		foreach (string KeyName in X16HS_DetectedTagsOLD)
		{
			if (X16HS_DetectedTags.Contains(KeyName) == false)
			{
				if (CurrentTimeMillis() - DispearCount[KeyName] > DispearTime)
				{
					//tmpDispearStackedStr+=KeyName+",";	
					//tmpStackedStr+=KeyName+",";

					DispearIDs.Add(KeyName);
					DispearBlockTyping.Add(getBlockTypeFromID(KeyName));
					DispearBlockIDs.Add(getBlockIDFromID(KeyName));
					DispearBlockAndSurfaceIDs.Add(getBlockAndSurfaceIDFromID(KeyName));

					// Debug.Log("Detect dispeared '"+KeyName+"'");
				}
			}
		}
		//return ReOrder((tmpDispearStackedStr+",").Replace(",,", ""));
	}
	private void Update_DispearIDs_20170604()
	{
		foreach (String id in DetectedIDs)
		{
			if (DispearIDs_DispearedTime.ContainsKey(id))
			{
				DispearIDs_DispearedTime.Remove(id);
				//System.out.println("DispearIDs.remove(id):"+id);
			}
		}

		//Find Disappeared id
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			String[] tmps = ("" + StackedOrders[i]).Split(',');
			for (int j = 0; j < tmps.Length; j++)
			{
				try
				{
					if (DetectedIDs.Contains(tmps[j]) == false && DispearIDs_DispearedTime.ContainsKey(tmps[j]) == false)
					{
						DispearIDs.Add(tmps[j]);
						DispearBlockTyping.Add(getBlockTypeFromID(tmps[j]));
						DispearBlockIDs.Add(getBlockIDFromID(tmps[j]));
						DispearBlockAndSurfaceIDs.Add(getBlockAndSurfaceIDFromID(tmps[j]));
						DispearIDs_DispearedTime.Add(tmps[j], CurrentTimeMillis());
						//System.out.println("DispearIDs_DispearedTime.put() :" + tmps[j] +","+ System.currentTimeMillis());
					}
				}
				catch (Exception) { }
			}
		}

		foreach (String id in DispearIDs_DispearedTime.Keys)
		{
			if ((CurrentTimeMillis() - (long)DispearIDs_DispearedTime[id]) >= DisappearTime_20170604)
			if (StackedIDs.Contains(id) == true)
			if(Flag_ShowSysMesg)
				Debug.Log(id + " was dispeared : " + (CurrentTimeMillis() - (long)DispearIDs_DispearedTime[id]));
			if ((CurrentTimeMillis() - (long)DispearIDs_DispearedTime[id]) >= DisappearTime_20170604 * 10)
			if (StackedIDs.Contains(id) == false)
			{
				DispearIDs_DispearedTime.Remove(id);
				Debug.Log("[Sys Mesg] Remove The dispearing too long : " + id);
			}
		}
	}

	private void update_fix_StackedOrders()
	{
		//=========== Sort Board ===============================
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			for (int j = i + 1; j < StackedOrders.Count; j++)
			{
				if (i != j)
				{
					if (("" + StackedOrders[i]).Contains(SystemBaseTag + BoardType) == false
						&& ("" + StackedOrders[i]).Contains(SystemBaseTag2 + BoardType) == false
						&& ((string)StackedOrders[j]).Contains(SystemBaseTag + BoardType) == true
						&& ((string)StackedOrders[j]).Contains(SystemBaseTag2 + BoardType) == true
					)
					{
						string tmp = ("" + StackedOrders[i]);
						StackedOrders[i] = StackedOrders[j];
						StackedOrders[j] = tmp;
					}
				}
			}
		}
		//=========== Sort Non Connected to Last ===============================
		for (int ii = 0; ii < StackedOrders.Count * 1; ii++)
			for (int i = 0; i < StackedOrders.Count; i++)
			{
				if (("" + StackedOrders[i]).Contains(SystemBaseTag + BoardType) || ("" + StackedOrders[i]).Contains(SystemBaseTag2 + BoardType)) continue;

				string[] tmps = ("" + StackedOrders[i]).Split(',');
				string targetID = tmps[0];
				if (getSurfaceSNFromID(tmps[1]).Equals("03"))
					targetID = tmps[1];

				string targetBlockID = getBlockIDFromID(targetID);
				// Debug.Log(targetBlockID);
				bool Flag_BlockSurface01_Existed = false;
				for (int j = 0; j < i; j++)
				{
					if (((string)StackedOrders[j]).Contains(targetBlockID + "01"))
					{
						Flag_BlockSurface01_Existed = true;
						break;
					}
				} // Debug.Log(targetID + " = "+Flag_BlockSurface01_Existed);

				if (Flag_BlockSurface01_Existed == false && i < StackedOrders.Count - 1)
				{
					if (("" + StackedOrders[i]).Contains(SystemBaseTag + BoardType) == false
						&& ("" + StackedOrders[i]).Contains(SystemBaseTag2 + BoardType) == false)
					{
						string tmp = ("" + StackedOrders[i]);
						StackedOrders[i] = StackedOrders[i + 1];
						StackedOrders[i + 1] = tmp;
					}
				}
			}
	}

	//===========Setup========================================================
	public void startReceive()
	{
		if (Flag_ToConnectTheReade)
		{
			Debug.Log("InitialReader...");
			InitialReader();
		}
	}
	public void setShowSysMesg(bool b)
	{
		Flag_ShowSysMesg = b;
	}
	public void setShowDebugMesg(bool b)
	{
		Flag_ShowDebugInfo = b;
	}
	public void setAllowBlockType(string[] AllowBlockTypeX)
	{
		AllowBlockType = AllowBlockTypeX;
	}
	public void setSysTagBased(string SystemBaseTagX)
	{
		SystemBaseTag = SystemBaseTagX + " ";
	}
	public void setSysTagBased2(string SystemBaseTagX)
	{
		SystemBaseTag2 = SystemBaseTagX + " ";
	}
	public void setShowReceiveTag(bool Flag_ShowReceivedTagX)
	{
		Flag_ShowReceivedTag = Flag_ShowReceivedTagX;
	}

	public void setPreStackOrders(string[] PreStackOrders)
	{
		this.PreStackOrders = PreStackOrders;
		if (PreStackOrders.Length > StackedOrders.Count)
		{
			if (PreStackOrders.Length > StackedOrders.Count)
			{
				for (int i = 0; i < PreStackOrders.Length; i++)
				{
					if (Flag_ShowDebugInfo) Debug.Log("[Debug Info.]: AddPreOrder : " + PreStackOrders[i] + " - " + i + " - " + countNonStackOnTheBoard + " == " + StackedOrders.Count);
					if (StackedOrders.Count == (i + countNonStackOnTheBoard))
						addoper(PreStackOrders[i]);
				}
			}
		}
	}

	public void EnableFilterNoise()
	{
		Flag_DetectTheNoiseTags = false;
	}

	public void startToBuild()
	{
		Flag_DetectTheNoiseTags = false;
	}
	public bool isStartToBuild()
	{
		return Flag_DetectTheNoiseTags == false;
    }

    public string[] GetTags()
    {
        string[] re = new string[X16HS_DetectedTags.Count];
        int count = 0;
        foreach (var tag in X16HS_DetectedTags)
        {
            re[count] = tag;
            count++;
        }

        return re;
    }

    public bool IfContainTag(string tag)
    {
        if (X16HS_DetectedTags.Contains(tag))
            return true;
        else
            return false;
    }
    //===========Print========================================================
    public void printStackedOrders()
	{
		string xx = "";
		Debug.Log("======= printStackedOrders() ==========================");
		for (int i = 0; i < StackedOrders.Count; i++)
		{
			xx += "\"" + StackedOrders[i] + "\",\n" +
				"";
			Debug.Log("(" + i + ") " + ("" + StackedOrders[i]));
		}
		Debug.Log("=====================================================\n");
		Debug.Log(xx);
	}
	public void printNewIDs()
	{
		Debug.Log("================= NewIDs() ==========================");
		foreach (string id in NewBlockIDs) Debug.Log("[" + NewIDs.Count + "] " + id);
		Debug.Log("=====================================================\n");
	}
	public void printDispearIDs()
	{
		Debug.Log("================= DispearIDs() ==========================");
		foreach (string id in DispearIDs) Debug.Log("[" + DispearIDs.Count + "] " + id + " @ " + (CurrentTimeMillis() - (long)DispearIDs_DispearedTime[id]));
		Debug.Log("=====================================================\n");
	}
	public void printNewIDButBlockExisted_IDs()
	{
		Debug.Log("================= NewIDButBlockExisted_IDs() ==========================");
		foreach (string id in NewIDButBlockExisted_IDs) Debug.Log("[" + NewIDs.Count + "] " + id);
		Debug.Log("=======================================================================\n");
	}
	public void printNewIDAndBlockNonRecorded_IDs()
	{
		Debug.Log("================= NewIDAndBlockNonRecorded_IDs() ==========================");
		foreach (string id in NewIDAndBlockNonRecorded_IDs) Debug.Log("[" + NewIDs.Count + "] " + id);
		Debug.Log("===========================================================================\n");
	}
	public void printStackedOrders3D()
	{
		Debug.Log("======= printStackedOrders3D() ==========================");
		foreach (int tmpID in StackedOrders3D.Keys)
		{
			Debug.Log(tmpID + " ::: " + StackedOrders3D[tmpID][0] + " " + +StackedOrders3D[tmpID][1] + " " + +StackedOrders3D[tmpID][2] + " " + +StackedOrders3D[tmpID][3] + " " + +StackedOrders3D[tmpID][4] + " " + StackedOrders3D[tmpID][5] + " " + StackedOrders3D[tmpID][6]);
		}
		Debug.Log("=====================================================\n");
	}


	public string getBlockInfoXYZ(int X, int Y, int Z, string TARGET) 
	{
		foreach (int tmpID in StackedOrders3D.Keys)
		{
			if (StackedOrders3D[tmpID][0] == X && StackedOrders3D[tmpID][1] == Y && StackedOrders3D[tmpID][2] == Z)
			{
				if (TARGET.Equals("BlcokID")) return tmpID+"";
				if (TARGET.Equals("SurfaceID")) return StackedOrders3D[tmpID][3] + "";
				if (TARGET.Equals("StackWay")) return StackedOrders3D[tmpID][4]+"";
				if (TARGET.Equals("BlockIDType")) return StackedOrders3D[tmpID][5]+"";
				if (TARGET.Equals("FacingDirect")) return getBlockHozRotation(((int[])StackedOrders3D[tmpID])[4])+"";
			}
		}
		return null;
	}


	public void printAllReceivedIDs()
	{
		Debug.Log("================= NewIDButBlockExisted_IDs() ==========================");
		foreach (string id in X16HS_DetectedTags) Debug.Log("[" + X16HS_DetectedTags.Count + "] " + id);
		Debug.Log("=======================================================================\n");
	}
	public void printNoiseIDs()
	{
		Debug.Log("================= NoiseIDs() ==========================");
		foreach (string id in NoiseIDs) Debug.Log("[" + NoiseIDs.Count + "] " + id);
		Debug.Log("=======================================================================\n");
	}
}