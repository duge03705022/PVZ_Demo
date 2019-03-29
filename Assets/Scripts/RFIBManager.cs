using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

using RFIBricks_Cores_Libs;

public class RFIBManager : MonoBehaviour
{
    int[,] plantTable;

    #region Block & Touch Setting
    // 允許甚麼編號被接受
    static string[] AllowBlockType = {
        "9999", // 99 floor
        "7101", // 71 shooter
        "7601", // 76 propertyBlock
        "8101", // 81 sunFlower
        "8601", // 86 wall
        "9101", // 91 cherry
        "9601"  // 96 jalapeno
	};

    // 地板編號 0-44 (5*9 左到右 上到下)
    //int[] touchnum = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //0~10 5 7  {3,12,21,30,39,2,11,20,29,38} {5,14,23,32,41,7,16,25,34,43}
    //int[] touchnum2 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //0~10 4 6

    #endregion

    #region RFID parameter
    RFIBricks_Cores RFIB;
    short[] EnableAntenna = { 1, 2, 3, 4 }; //reader port
    string ReaderIP = "192.168.1.96"; //到時再說
    double ReaderPower = 32, Sensitive = -70;  //功率, 敏感度
    bool Flag_ToConnectTheReade = true;    //false就不會連reader
    //bool Flag_AllowStackWithoutConnectingBoard = true;
    public Dictionary<int, GameObject> cubes_Blocks = new Dictionary<int, GameObject>();
    public HandlerForPlant plantHandler;
    public HandlerForTouch touchHandler;
    public BlockController blockController;

    #endregion

    #region Game parameter
    GameModel model;
    private bool[,] updateFlag;
    Dictionary<string, int> plantDic = new Dictionary<string, int>();
    private string[,,] lastIdStack;
    private string[,,] lastDirection;
    private int[,,] lastPlant;
    private bool[,] errorMap;

    #endregion

    #region KeyPress parameter
    private bool[] flag1 = { false, false, false, false, false, false, false, false };
    private bool[] flag2 = { false, false, false, false };
    private bool[] flag3 = { false, false, false };
    private bool[] flag4 = { false, false, false, false, false, false, false, false, false, false };
    private bool[] flag5 = { false, false, false, false };
    private bool[] flag6 = { false, false, false, false };
    private bool[] flag7 = { false, false, false, false };
    private bool[] flag8 = { false, false };


    private bool[] flagV1 = { false, false, false, false, false, false };
    private bool[] flagV2 = { false, false, false };
    private bool[] flagV3 = { false, false, false, false, false, false, false, false };
    private bool[] flagV4 = { false, false, false, false, false, false, false, false, false, false, false, false };
    private bool[] flagV5 = { false, false, false, false, false };
    private bool[] flagV6 = { false, false, false };
    private bool[] flagV7 = { false, false, false };
    private bool[] flagV8 = { false, false };
    private bool[] flagV9 = { false, false, false, false };
    private bool[] flagV10 = { false, false, false, false };

    #endregion

    // Use this for initialization
    void Start()
    {
        model = GameModel.GetInstance();
        InitPlantTable();

        RFIB = new RFIBricks_Cores(ReaderIP, ReaderPower, Sensitive, EnableAntenna, Flag_ToConnectTheReade);
        //RFIB.setSerialcomPort("COM3"); //arduino connect
        RFIB.setShowSysMesg(true);
        RFIB.setShowReceiveTag(true);
        RFIB.setShowDebugMesg(true);

        RFIB.setSysTagBased("8940 0000");
        RFIB.setAllowBlockType(AllowBlockType);

        RFIB.setRefreshTime(1000); //clear beffer
        RFIB.setDisappearTime(600); //id 消失多久才會的消失
        RFIB.setDelayForReceivingTime(400); //清空之後停多久才收id

        BoardMapping(); // 開始接收ID前要將地板配對

        RFIB.startReceive();

        RFIB.startToBuild();
        RFIB.printNoiseIDs();

        updateFlag = new bool[5, 9];
        errorMap = new bool[5, 9];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                updateFlag[i, j] = false;
                errorMap[i, j] = false;
            }
        }

        lastIdStack = new string[5, 9, 3];
        lastDirection = new string[5, 9, 3];
        lastPlant = new int[5, 9, 3];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    lastIdStack[i, j, k] = "0000";
                    lastDirection[i, j, k] = "000000";
                    lastPlant[i, j, k] = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RFIB.statesUpdate();
        StackSensing();
        TouchSensing();
        KeyPressed();
        for (int k = 0; k < RFIB.StackedOrders.Count; k++)
        {
            string[] tmp = RFIB.StackedOrders[k].ToString().Split(',');
            //Debug.Log(tmp[0]);
            //Debug.Log(tmp[1]);
            //Debug.Log(RFIB.StackedOrders[k]);
            //Debug.Log(RFIB.StackedOrders[k].GetType());
        }

        //foreach (string str in RFIB.DetectedIDs)
        //{
        //    Debug.Log(str);
        //}
    }

    private void BoardMapping()
    {
        // 在開始接收ID前，這邊要將接收到的地板ID進行配對編號。
        // 使用的function是 setBoardBlockMappingArray(地板編號,"給予的座標x+1 給予的座標y+1");
        #region old code
        //String pos;
        //for (int i = 40; i < 45; i++)
        //{
        //    pos = "0" + (((i % 5) + 1) % 6).ToString() + "09";
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        //for (int i = 35; i < 40; i++)
        //{
        //    pos = "0" + (((i % 5) + 1) % 6).ToString() + "08";
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        //for (int i = 30; i < 35; i++)
        //{
        //    pos = "0" + (((i % 5) + 1) % 6).ToString() + "07";
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        //for (int i = 25; i < 30; i++)
        //{
        //    pos = "0" + (((i % 5) + 1) % 6).ToString() + "06";
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        //for (int i = 0; i < 9; i++)
        //{
        //    pos = "010" + (((i % 9) + 1) % 10).ToString();
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        //for (int i = 9; i < 18; i++)
        //{
        //    pos = "020" + (((i % 9) + 1) % 10).ToString();
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        //for (int i = 18; i < 27; i++)
        //{
        //    pos = "030" + (((i % 9) + 1) % 10).ToString();
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        //for (int i = 27; i < 36; i++)
        //{
        //    pos = "040" + (((i % 9) + 1) % 10).ToString();
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        //for (int i = 36; i < 45; i++)
        //{
        //    pos = "050" + (((i % 9) + 1) % 10).ToString();
        //    RFIB.setBoardBlockMappingArray(i + 1, pos);
        //    Debug.Log((i + 1).ToString() + " " + pos);
        //}
        #endregion

        // ＼ y   |
        // x  ＼  | [01] [02] [03] [04] [05] [06] [07] [08] [09]
        //-------＼-----------------------------------------------
        //   [01] |   1    2    3    4    5    6    7    8    9
        //   [02] |  10   11   12   13   14   15   16   17   18
        //   [03] |  19   20   21   22   23   24   25   26   27
        //   [04] |  28   29   30   31   32   33   34   35   36
        //   [05] |  37   38   39   40   41   42   43   44   45

        //// floor ID "9999 0101" - "9999 0145"
        for (int i = 0; i < 45; i++)
        {
            string pos = "0" + ((i / 9) + 1).ToString() + "0" + (((i % 9) + 1) % 10).ToString();
            RFIB.setBoardBlockMappingArray(i + 1, pos);
            //Debug.Log((i + 1).ToString() + '\t' + pos);
        }

        //這裡的(y, x);
        //RFIB.setBoardBlockMappingArray (18, "0101");
        // 上面這行的意思就是把編號為xxxx 0000 9999 01"18" XXXX的地板塊，配對成座標(x, y) = (0, 0)的意思。

        //RFIB.setBoardBlockMappingArray (45, "0508");
        // 另一個例子：上面這行的意思就是把編號為8930 0000 9999 01"45" XXXX的地板塊，配對成座標(x, y) = (4, 7)的意思。

        // 植物大戰殭屍的地板為3*4，所以這邊要把12個地板方塊都編好

    }

    private void StackSensing()
    {
        // 偵測每格地板上堆疊了幾個方塊，並把數值更新到HandlerForPlant的stackSensing表格
        // 堆疊後的處理，我在HandlerForPlant實作。

        UpdatePlant(lastIdStack, lastDirection);

        for (int i = 0; i < StageMap.MAP_MAX; i++)
        {
            string[] idStack = new string[3] { "0000", "0000", "0000" };
            string[] idDirection = new string[3] { "000000", "000000", "000000" };

            idStack[0] = GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1, "BlockIDType");
            idDirection[0] = GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1, "StackWay");
            if (idStack[0] != "0000")
            {
                idStack[1] = GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2, "BlockIDType");
                idDirection[1] = GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2, "StackWay");
                if (idStack[1] != "0000")
                {
                    idStack[2] = GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 3, "BlockIDType");
                    idDirection[2] = GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 3, "StackWay");
                }
            }
            else
            {
                lastPlant[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0] = 0;
                lastPlant[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1] = 0;
                lastPlant[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2] = 0;
            }

            if (idStack[0] != lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0] ||
                idStack[1] != lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1] ||
                idStack[2] != lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2])
            {
                plantHandler.stackSensing[i] = 0;
                if (errorMap[i / StageMap.COL_MAX, i % StageMap.COL_MAX])
                {
                    touchHandler.ErrorEffectOff(i / StageMap.COL_MAX, i % StageMap.COL_MAX);
                    errorMap[i / StageMap.COL_MAX, i % StageMap.COL_MAX] = false;
                }

                updateFlag[i / StageMap.COL_MAX, i % StageMap.COL_MAX] = true;

                lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0] = idStack[0];
                lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1] = idStack[1];
                lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2] = idStack[2];

                lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0] = idDirection[0];
                lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1] = idDirection[1];
                lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2] = idDirection[2];
            }
        }
    }

    private void TouchSensing()
    {
        //HashSet<int> touch = Arduino.touchID;
        //HashSet<int> touch2 = Arduino2.touchID;
        //foreach (int item in RFIB.getTouchIDs())
        //{
        //    Debug.Log(item);
        //}

        //for (int i = 0; i < 10; i++)
        //{
        //    if (touch.Contains(i))
        //        touchHandler.touchSensing[touchnum[i]] = true;
        //    else
        //        touchHandler.touchSensing[touchnum[i]] = false;
        //}

        //for (int i = 0; i < 10; i++)
        //{
        //    if (touch2.Contains(i))
        //        touchHandler.touchSensing[touchnum2[i]] = true;
        //    else
        //        touchHandler.touchSensing[touchnum2[i]] = false;
        //}

        // 我並不清楚現在這個touch sensing是怎樣實作的。
        // 但基本上就是把偵測到的哪格被碰觸，就更新HandlerForTouch中touchSensing的boolean表格即可。
        // 至於碰觸後的處理，我都在HandlerForTouch實作。
        // 
        // For Example: 
        // if(格子0號被碰觸)
        //		touchHandler.touchSensing [0] = true;
        // else
        // 		touchHandler.toucSensing [0] = false;

        blockController.updateArduinoData();
        if (blockController.touchedColBlock != -1 && blockController.touchedRowBlock != -1)
        {
            touchHandler.touchSensing[(blockController.touchedRowBlock / 3 * (-1) + 4) * 9 + blockController.touchedColBlock / 3] = true;
            Debug.Log((blockController.touchedRowBlock / 3 * (-1) + 4) * 9 + blockController.touchedColBlock / 3);
        }
        else
        {
            for (int i = 0; i < 45; i++)
            {
                touchHandler.touchSensing[i] = false;
            }
        }
    }

    #region tmpNoUse
    // 下面這個function就是來偵測地板上方堆了幾個方塊的。
    //private int GetPositionHeight(int X, int Y)
    //{
    //    int MaxHeight = 0;

    //    foreach (int BlockID_intType in RFIB.StackedOrders3D.Keys)
    //    {
    //        if (RFIB.StackedOrders3D[BlockID_intType][0] == X && RFIB.StackedOrders3D[BlockID_intType][1] == Y)
    //        {
    //            if (RFIB.StackedOrders3D[BlockID_intType][2] > MaxHeight)
    //                MaxHeight = RFIB.StackedOrders3D[BlockID_intType][2];
    //        }
    //    }

    //    if (MaxHeight >= 4)
    //        MaxHeight = 4;
    //    return MaxHeight;
    //} 

    #endregion

    // 系統編號 此欄空白 方塊種類 編號+上下 方向
    // 9999 0101 就是 (0, 0)
    // 9999 0203 就是 (1, 2)
    public void KeyPressed()
    {
        if (Input.GetKeyDown("1"))
            ChangeTags("8940 0000 9999 0302 0001", "8940 0000 7101 4501 0001");
        if (Input.GetKeyDown("2"))
            ChangeTags("8940 0000 7101 4503 0001", "8940 0000 7101 4601 0001");
        if (Input.GetKeyDown("3"))
            ChangeTags("8940 0000 7101 4603 0001", "8940 0000 7101 4701 0001");
        if (Input.GetKeyDown("4"))
            ChangeTags("8940 0000 9999 0501 0001", "8940 0000 8601 5401 0001");
        if (Input.GetKeyDown("5"))
            ChangeTags("8940 0000 8601 5403 0001", "8940 0000 7101 5501 0001");
        if (Input.GetKeyDown("6"))
            ChangeTags("8940 0000 9999 0206 0001", "8940 0000 9101 5801 0001");
        if (Input.GetKeyDown("7"))
            ChangeTags("8940 0000 9101 5803 0001", "8940 0000 9601 5901 0001");
        if (Input.GetKeyDown("8"))
            touchHandler.touchSensing[14] = true;
        if (Input.GetKeyUp("8"))
            touchHandler.touchSensing[14] = false;

        #region Information
        if (Input.GetKeyUp(";"))
        {
            string[] tags = RFIB.GetTags();
            for (int i = 0; i < tags.Length; i++)
                Debug.Log(tags[i]);
        }
        if (Input.GetKeyUp("."))
        {
            RFIB.printAllReceivedIDs();
            RFIB.printNoiseIDs();
        }

        #endregion
    }

    public void ChangeTags(string str1, string str2)
    {
        if (!RFIB.IfContainTag(str1))
            RFIB._Testing_AddHoldingTag(str1, str2);
        else
            RFIB._Testing_RemoveHoldingTag(str1, str2);
    }

    public string GetBlockInfoXYZ(int X, int Y, int Z, string TARGET)
    {
        foreach (int tmpID in RFIB.StackedOrders3D.Keys)
        {
            if (RFIB.StackedOrders3D[tmpID][0] == X && RFIB.StackedOrders3D[tmpID][1] == Y && RFIB.StackedOrders3D[tmpID][2] == Z)
            {
                //Debug.Log(RFIB.DetectedIDs.ToString());
                if (TARGET.Equals("BlockID"))                   // BlockIDType + BlockID,  Ex:910102
                    return tmpID + "";
                if (TARGET.Equals("SurfaceID"))
                    return RFIB.StackedOrders3D[tmpID][3] + "";
                if (TARGET.Equals("BlockIDType"))
                    return RFIB.StackedOrders3D[tmpID][5] + "";
                if (TARGET.Equals("StackWay"))                  // 123456 下前上後右左
                    return RFIB.StackedOrders3D[tmpID][4] + "";
                else
                    return "0000";
            }
        }
        return "0000";
    }

    public int GetDirection(string directionStr)
    {
        if (directionStr.IndexOf("2") == 1)
            return 0;
        else if (directionStr.IndexOf("2") == 5)
            return 1;
        else if (directionStr.IndexOf("2") == 3)
            return 2;
        else if (directionStr.IndexOf("2") == 4)
            return 3;

        return -1;
    }

    public int FindDirection(string BlockID)
    {
        //Debug.Log(BlockID);
        string tmp = BlockID.Substring(0, 4) + " " + BlockID.Substring(4, 2);

        foreach (string str in RFIB.DetectedIDs)
        {
            if (str.Contains(tmp))
            {
                switch (str.Split(' ')[4])
                {
                    case "0001":
                        return 0;

                    case "0002":
                        return 1;

                    case "0003":
                        return 2;

                    case "0004":
                        return 3;

                    default:
                        return -1;
                }
            }
        }

        return -1;

        //switch (layer)
        //{
        //    case 1:
        //        for (int k = 0; k < RFIB.StackedOrders.Count; k++)
        //        {
        //            string[] stackIDs = RFIB.StackedOrders[k].ToString().Split(',');
        //            string[] bottomDetail = stackIDs[0].Split(' ');
        //            string[] topDetail = stackIDs[1].Split(' ');

        //            //if (bottomDetail[2] == "9999" && topDetail[2] == BlockID.Substring())
        //            //{

        //            //}
        //        }
        //        return -1;

        //    case 2:
        //        return -1;

        //    case 3:
        //        return -1;

        //    default:
        //        return -1;
        //}
    }

    public void InitPlantTable()
    {
        plantTable = new int[30, 4]
        {
            {1, 2, 3, 4},           // shooter Lv1              Right(1), Up(2), Left(3), Down(4)
            {5, 5, 5, 5},           // shooter Lv2              (5)
            {6, 6, 6, 6},           // shooter Lv3              (6)
            {7, 7, 7, 7},           // wallShooter Lv2          (7)
            {8, 8, 8, 8},           // shooter fire Lv2         (8)

            {9, 9, 9, 9},           // shooter snow Lv2         (9)
            {10, 10, 10, 10},       // shooter shroom Lv2       (10)
            {11, 11, 11, 11},       // shooter star Lv2         (11)
            {12, 12, 12, 12},       // sunFlower Lv1            (12)
            {13, 13, 13, 13},       // sunFlower Lv2            (13)

            {14, 14, 14, 14},       // wallFlower Lv2           (14)
            {15, 15, 15, 15},       // wall Lv1                 (15)
            {16, 16, 16, 16},       // wall Lv2                 (16)
            {17, 17, 17, 17},       // cherry Lv1               (17)
            {18, 18, 18, 18},       // cherry Lv2               (18)

            {19, 19, 19, 19},       // cherry Lv3               (19)
            {20, 20, 20, 20},       // jalaCherry Lv2           (20)
            {21, 21, 21, 21},       // jalapeno row Lv1         (21)
            {22, 22, 22, 22},       // jalapeno col Lv1         (22)
            {23, 23, 23, 23},       // jalapeno cross Lv2       (23)
            
            {24, 24, 24, 24},       // shooterRight
            {25, 25, 25, 25},       // shooterUp
            {26, 26, 26, 26},       // shooterLeft
            {27, 27, 27, 27},       // shooterDown
            {28, 28, 28, 28},       // propertyRight


            {29, 29, 29, 29 },
            {30, 30, 30, 30 },
            {31, 31, 31, 31 },
            {32, 32, 32, 32 },
            {33, 33, 33, 33 }
        };

        plantDic.Add("7101", 0);            // shooter Lv1
        plantDic.Add("71017101", 1);        // shooter Lv2
        plantDic.Add("710171017101", 2);    // shooter Lv3
        plantDic.Add("86017101", 3);        // wallShooter Lv2
        plantDic.Add("71010", 4);           // shooter fire Lv2

        plantDic.Add("71011", 5);           // shooter snow Lv2
        plantDic.Add("71012", 6);           // shooter shroom Lv2
        plantDic.Add("71013", 7);           // shooter star Lv2
        plantDic.Add("8101", 8);            // sunflower Lv1
        plantDic.Add("81018101", 9);        // sunflower Lv2

        plantDic.Add("86018101", 10);       // wallFlower Lv2
        plantDic.Add("8601", 11);           // wall Lv1
        plantDic.Add("86018601", 12);       // wall Lv2
        plantDic.Add("9101", 13);           // cherry Lv1
        plantDic.Add("91019101", 14);       // cherry Lv2

        plantDic.Add("910191019101", 15);   // cherry Lv3
        plantDic.Add("91019601", 16);       // jalaCherry Lv2
        plantDic.Add("96019101", 16);       // jalaCherry Lv2
        plantDic.Add("96010", 17);          // jalapeno row Lv1
        plantDic.Add("96012", 17);          // jalapeno row Lv1
        plantDic.Add("96011", 18);          // jalapeno col Lv1
        plantDic.Add("96013", 18);          // jalapeno col Lv1
        plantDic.Add("9601196011", 19);     // jalapeno cross Lv2
        plantDic.Add("9601096011", 19);     // jalapeno cross Lv2
        plantDic.Add("9601196013", 19);     // jalapeno cross Lv2
        plantDic.Add("9601296011", 19);     // jalapeno cross Lv2
        plantDic.Add("9601396011", 19);     // jalapeno cross Lv2
        plantDic.Add("9601096013", 19);     // jalapeno cross Lv2
        plantDic.Add("9601396013", 19);     // jalapeno cross Lv2
        plantDic.Add("9601296013", 19);     // jalapeno cross Lv2
    }

    public void UpdatePlant(string[,,] lastIdStack, string[,,] lastDirection)
    {
        for (int i = 0; i < StageMap.MAP_MAX; i++)
        {
            if (updateFlag[i / StageMap.COL_MAX, i % StageMap.COL_MAX])
            {
                if (lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0] != "0000")
                {
                    if (lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1] == "0000")       // 1層
                    {
                        //SetPlantId(
                        //    i,
                        //    lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0],
                        //    GetDirection(lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0])
                        //);
                        //Debug.Log("X " + i / StageMap.COL_MAX);
                        //Debug.Log("Y " + i % StageMap.COL_MAX);
                        //Debug.Log(GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1, "BlockID"));
                        SetPlantId(
                            i,
                            lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0],
                            FindDirection(GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1, "BlockID"))
                        );
                    }
                    else
                    {
                        if (lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2] == "0000")   // 2層
                        {
                            //SetPlantId(
                            //    i,
                            //    lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0],
                            //    lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1],
                            //    GetDirection(lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0]),
                            //    GetDirection(lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1])
                            //);
                            SetPlantId(
                                i,
                                lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0],
                                lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1],
                                FindDirection(GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1, "BlockID")),
                                FindDirection(GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2, "BlockID"))
                            );
                        }
                        else                                                                        // 3層
                        {
                            //SetPlantId(
                            //    i,
                            //    lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0],
                            //    lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1],
                            //    lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2],
                            //    GetDirection(lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0]),
                            //    GetDirection(lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1]),
                            //    GetDirection(lastDirection[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2])
                            //);
                            SetPlantId(
                                i,
                                lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 0],
                                lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1],
                                lastIdStack[i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2],
                                FindDirection(GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 1, "BlockID")),
                                FindDirection(GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 2, "BlockID")),
                                FindDirection(GetBlockInfoXYZ(i / StageMap.COL_MAX, i % StageMap.COL_MAX, 3, "BlockID"))
                            );
                        }
                    }
                }
                updateFlag[i / StageMap.COL_MAX, i % StageMap.COL_MAX] = false;
            }
        }
    }

    public void SetPlantId(int pos, string blockId0, int direction0)
    {
        int tmpPlant = 0;
        if (blockId0 != "9601")
            tmpPlant = plantTable[plantDic[blockId0], direction0];
        else
            tmpPlant = plantTable[plantDic[blockId0 + direction0.ToString()], direction0];

        if (tmpPlant != lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 0])
        {
            lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 0] = tmpPlant;
            plantHandler.stackSensing[pos] = tmpPlant;
        }
        else
        {
            model.sun += 150;
            plantHandler.stackSensing[pos] = lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 0];
        }
    }

    public void SetPlantId(int pos, string blockId0, string blockId1, int direction0, int direction1)
    {
        //Debug.Log(direction1);
        int tmpPlant = 0;
        if (plantDic.ContainsKey(blockId0 + blockId1))                                                                                                       // 一般組合
            tmpPlant = plantTable[plantDic[blockId0 + blockId1], direction1];
        else if (blockId0 == "7101" && blockId1 == "7601")                                                                                                   // 4屬性
            tmpPlant = plantTable[plantDic[blockId0 + direction1.ToString()], direction1];
        else if (blockId0 == "9601" && blockId1 == "9601" && plantDic.ContainsKey(blockId0 + direction0.ToString() + blockId1 + direction1.ToString()))      // 爆炸辣椒
            tmpPlant = plantTable[plantDic[blockId0 + direction0.ToString() + blockId1 + direction1.ToString()], direction0];

        if (tmpPlant == 0)
        {
            model.sun += 150;
            plantHandler.stackSensing[pos] = lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 0];
            touchHandler.ErrorEffectOn(pos / StageMap.COL_MAX, pos % StageMap.COL_MAX);
            errorMap[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX] = true;
        }
        else
        {
            if (tmpPlant != lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 1])
            {
                lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 1] = tmpPlant;
                plantHandler.stackSensing[pos] = tmpPlant;
            }
            else
            {
                model.sun += 150;
                plantHandler.stackSensing[pos] = lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 1];
            }
        }
    }

    public void SetPlantId(int pos, string blockId0, string blockId1, string blockId2, int direction0, int direction1, int direction2)
    {
        //string[] lv3Plants = { "7101", "9101" };        // shooter, cherry

        int tmpPlant = 0;
        if (plantDic.ContainsKey(blockId0 + blockId1 + blockId2))
        {
            tmpPlant = plantTable[plantDic[blockId0] + 2, direction0];
        }

        if (tmpPlant == 0)
        {
            model.sun += 150;
            plantHandler.stackSensing[pos] = lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 1];
            touchHandler.ErrorEffectOn(pos / StageMap.COL_MAX, pos % StageMap.COL_MAX);
            errorMap[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX] = true;
        }
        else
        {
            if (tmpPlant != lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 2])
            {
                lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 2] = tmpPlant;
                plantHandler.stackSensing[pos] = tmpPlant;
            }
            else
            {
                model.sun += 150;
                plantHandler.stackSensing[pos] = lastPlant[pos / StageMap.COL_MAX, pos % StageMap.COL_MAX, 2];
            }
        }
    }
}
