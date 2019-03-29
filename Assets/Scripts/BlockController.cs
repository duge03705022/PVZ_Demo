using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class BlockController : MonoBehaviour
{
    [Header("Show Param")]
    public GameObject parentCanvas;
    public GameObject blockPrefeb;
    public bool showOnBoard;
    public float boardRotate;
    public bool boardMirror;
    public float chartBarSize;
    public float blockSize;

    [Header("Changeable Param")]
    public float threshold;
    public int sampleTimes;
    public bool isBaseline;
    public int blockSeriesRow;
    public int blockSeriesCol;

    [Header("Unchangeable Param")]
    public int touchedColBlock;
    public int touchedRowBlock;
    public string[] arduinoData;
    //public int baseDataStart;
    //public int baseDataTimes;
    //public int dataTimes;

    //Test Version public param
    [Header("Test Param")]
    public bool testVersion;
    public int testSampleTimes;
    public int testingRow;
    public int testingCol;
    [Range(0, 100)] public int testTimes;
    public int testMaxTimes;
    public bool touchXMode;
    public bool touchXComplementMode;
    public bool Layer1ComplementMode;
    public bool Layer0ComplementMode;
    public int greenDelayTimes;
    public int yellowDelayTimes;
    public AudioClip[] audios;

    [Header("Test Information")]
    public int[] testCorrectTimes;
    public int[] touchedTestRowBlock;
    public int[] touchedTestColBlock;
    public bool[] isTestCorrect;


    private string[] lastArduinoData;
    private float[] rowData;
    private float[] colData;
    private GameObject[,] blockSeries;
    private int touchedRow, touchedCol;
    private float touchedRowData, touchedColData;
    private Vector2 tmpRowPos;
    private Vector2 tmpColPos;
    private float[] baselineR;
    private float[] baselineC;
    private float[] thresholdR;
    private float[] thresholdC;
    private float[] rowSumData;
    private float[] colSumData;
    private int[] ardBaseCount;
    private int totalBaseCount;
    private bool calBaseFinish;
    private bool baseLineVersion;

    //Test Version private & hidden param
    [HideInInspector] public float testThresholdBase;
    private float[] testThreshold;
    private int delayCount;
    private int testState;
    private float[,] rowTestData;
    private float[,] colTestData;
    private float[,] rowTimestamp;
    private float[,] colTimestamp;
    private bool allTestDataFetch;
    private bool[,,] isTestDataFetch;
    private int[] touchedTestRow;
    private float[] touchedTestRowData;
    private int[] touchedTestCol;
    private float[] touchedTestColData;

    void Start()
    {
        for (int i = 0; i < arduinoData.Length; i++)
        {
            if (testVersion)
            {
                transform.GetChild(0).GetChild(i).GetComponent<Arduino>().mode = 1;
            }
            else
            {
                transform.GetChild(0).GetChild(i).GetComponent<Arduino>().mode = 0;
            }
            transform.GetChild(0).GetChild(i).GetComponent<Arduino>().sampleTimes = this.sampleTimes;
            transform.GetChild(0).GetChild(i).GetComponent<Arduino>().testSampleTimes = this.testSampleTimes;
            transform.GetChild(0).GetChild(i).GetComponent<Arduino>().isBaseline = this.isBaseline;
        }

        baseLineVersion = false;

        //For test version
        rowTestData = new float[8, blockSeriesRow];
        colTestData = new float[8, blockSeriesCol];
        rowTimestamp = new float[8, blockSeriesRow];
        colTimestamp = new float[8, blockSeriesCol];
        testState = 1;
        delayCount = 0;
        isTestDataFetch = new bool[2, 8, blockSeriesRow];
        touchedTestRow = new int[8];
        touchedTestRowData = new float[8];
        touchedTestCol = new int[8];
        touchedTestColData = new float[8];
        touchedTestRowBlock = new int[8];
        touchedTestColBlock = new int[8];
        isTestCorrect = new bool[8];
        testThreshold = new float[8];
        testCorrectTimes = new int[8];
        for (int i = 0; i < 8; i++)
        {
            testThreshold[i] = testThresholdBase * (i + 1);
            testCorrectTimes[i] = 0;
        }
        testTimes = 0;
        if (testVersion)
        {
            showOnBoard = true;
        }


        rowSumData = new float[blockSeriesRow];
        colSumData = new float[blockSeriesCol];

        baselineR = new float[blockSeriesRow];
        baselineC = new float[blockSeriesCol];
        thresholdR = new float[blockSeriesRow];
        thresholdC = new float[blockSeriesCol];
        ardBaseCount = new int[arduinoData.Length];
        totalBaseCount = 0;
        calBaseFinish = false;

        lastArduinoData = new string[arduinoData.Length];

        rowData = new float[blockSeriesRow];
        colData = new float[blockSeriesCol];

        for (int i = 0; i < blockSeriesRow; i++)
        {
            rowData[i] = 0;
            rowSumData[i] = 0;
            baselineR[i] = 0;
            thresholdR[i] = 0;
        }

        for (int i = 0; i < blockSeriesCol; i++)
        {
            colData[i] = 0;
            colSumData[i] = 0;
            baselineC[i] = 0;
            thresholdC[i] = 0;
        }

        if (showOnBoard)
        {
            blockSeries = new GameObject[blockSeriesRow + 2, blockSeriesCol + 2];

            for (int i = 0; i < blockSeriesRow + 2; i++)
            {
                for (int j = 0; j < blockSeriesCol + 2; j++)
                {
                    blockSeries[i, j] = Instantiate(blockPrefeb, parentCanvas.transform);
                    blockSeries[i, j].transform.localPosition = new Vector3((j - (blockSeriesCol - 1) / 2) * (blockSize+5), (i - (blockSeriesRow - 1) / 2) * (blockSize + 5), 0);
                    blockSeries[i, j].transform.Find("Text").GetComponent<Text>().text = "0";
                }
            }

            parentCanvas.transform.Rotate(0, 0, boardRotate, Space.Self);
            
            Vector2 tmpPos;
            for (int i = 0; i < blockSeriesCol; i++)
            {
                updateBlock(blockSeriesRow, i, new Color(0.5f, 1, 0.5f), "0");
                tmpPos = blockSeries[blockSeriesRow, i].GetComponent<RectTransform>().anchoredPosition;
                blockSeries[blockSeriesRow, i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x, tmpPos.y + 20);

                updateBlock(blockSeriesRow + 1, i, new Color(0.5f, 1, 0.5f), "");
                tmpPos = blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().anchoredPosition;
                tmpRowPos = tmpPos;
                tmpRowPos.y += 20;
                blockSeries[blockSeriesRow + 1, i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x, tmpPos.y + 20);
            }

            for (int i = 0; i < blockSeriesRow; i++)
            {
                updateBlock(i, blockSeriesCol, new Color(0.5f, 1, 0.5f), "0");
                tmpPos = blockSeries[i, blockSeriesCol].GetComponent<RectTransform>().anchoredPosition;
                blockSeries[i, blockSeriesCol].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x + 20, tmpPos.y);

                updateBlock(i, blockSeriesCol + 1, new Color(0.5f, 1, 0.5f), "");
                tmpPos = blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().anchoredPosition;
                tmpColPos = tmpPos;
                tmpColPos.x += 20;
                blockSeries[i, blockSeriesCol + 1].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x + 20, tmpPos.y);
            }

            updateBlock(blockSeriesRow, blockSeriesCol, Color.black, "");
            updateBlock(blockSeriesRow+1, blockSeriesCol, Color.black, "");
            updateBlock(blockSeriesRow, blockSeriesCol+1, Color.black, "");
            updateBlock(blockSeriesRow+1, blockSeriesCol+1, Color.black, "");
        }

        //calBaseline();
    }

    private void updateBlock(int row, int col, Color c, string t)
    {
        if (col < blockSeriesCol) col = blockSeriesCol - col - 1;
        blockSeries[row, col].GetComponent<Image>().color = c;
        blockSeries[row, col].transform.Find("Text").GetComponent<Text>().text = t;
    }

    private void fetchArduinoData(int boardNum)
    {
        string[] newData = arduinoData[boardNum].Split(',');
        float[] target;
        if (newData[0] == "J")
        {
            if (newData[2] == "R")
            {
                target = rowData;
            }
            else if (newData[2] == "C")
            {
                target = colData;
            }
            else
            {
                Debug.Log("Invalid Arduino Data in board " + boardNum + ", newData[2]: " + newData[2]);
                return;
            }

            int clipStart, clipNum;
            if (int.Parse(newData[1]) == 0)
            {
                clipStart = int.Parse(newData[3]);
                clipNum = int.Parse(newData[4]);
                for (int i = 0; i < clipNum; i++)
                {
                    target[clipStart + i] = float.Parse(newData[i + 5]);
                }
            }
            else if (int.Parse(newData[1]) == 1)
            {
                clipStart = int.Parse(newData[3]);
                clipNum = int.Parse(newData[4]);
                for (int i = 0; i < clipNum / 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        target[clipStart + i * 6 + j] = float.Parse(newData[i * 3 + j + 5]);
                    }
                }
            }
            else
            {
                Debug.Log("Invalid JUMPTYPE, newData[1]: " + newData[1]);
            }
        }
        else
        {
            Debug.Log("Invalid Arduino Data in board " + boardNum + ", newData[0]: " + newData[0]);
        }
    }

    private void fetchArduinoBaseData(int boardNum)
    {
        string[] newData = arduinoData[boardNum].Split(',');
        float[] target;
        if (newData[0] == "J")
        {
            if (newData[2] == "R")
            {
                target = baselineR;
            }
            else if (newData[2] == "C")
            {
                target = baselineC;
            }
            else
            {
                Debug.Log("Invalid Arduino Data in board " + boardNum + ", newData[2]: " + newData[2]);
                return;
            }

            int clipStart, clipNum;
            if (int.Parse(newData[1]) == 0)
            {
                clipStart = int.Parse(newData[3]);
                clipNum = int.Parse(newData[4]);
                for (int i = 0; i < clipNum; i++)
                {
                    target[clipStart + i] += float.Parse(newData[i + 5]);
                }
            }
            else if (int.Parse(newData[1]) == 1)
            {
                clipStart = int.Parse(newData[3]);
                clipNum = int.Parse(newData[4]);
                for (int i = 0; i < clipNum / 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        target[clipStart + i * 6 + j] += float.Parse(newData[i * 3 + j + 5]);
                    }
                }
            }
            else
            {
                Debug.Log("Invalid JUMPTYPE, newData[1]: " + newData[1]);
            }
        }
        else
        {
            Debug.Log("Invalid Arduino Data in board " + boardNum + ", newData[0]: " + newData[0]);
        }
    }

    private void calData(float[] targetR, float[] targetC, int times)
    {
        float[] tmpR = new float[blockSeriesRow];
        float[] tmpC = new float[blockSeriesCol];

        for (int i = 0; i < blockSeriesRow; i++)
        {
            tmpR[i] = targetR[i];
        }
        for (int i = 0; i < blockSeriesCol; i++)
        {
            tmpC[i] = targetC[i];
        }

        if (times > 1)
        {
            for (int i = 0; i < blockSeriesRow; i++)
            {
                tmpR[i] /= times;
            }
            for (int i = 0; i < blockSeriesCol; i++)
            {
                tmpC[i] /= times;
            }
        }

        for (int i = 0; i < blockSeriesRow; i++)
        {
            tmpR[i] = Math.Max(0, tmpR[i] - baselineR[i]);
        }
        for (int i = 0; i < blockSeriesCol; i++)
        {
            tmpC[i] = Math.Max(0, tmpC[i] - baselineC[i]);
        }

        touchedRow = 0;
        touchedCol = 0;
        touchedRowData = 0;
        touchedColData = 0;

        for (int i = 0; i < blockSeriesRow; i++)
        {
            if (tmpR[i] > touchedRowData)
            {
                touchedRow = i;
                touchedRowData = tmpR[i];
            }
        }
        for (int i = 0; i < blockSeriesCol; i++)
        {
            if (tmpC[i] > touchedColData)
            {
                touchedCol = i;
                touchedColData = tmpC[i];
            }
        }

        if (touchedRowData > threshold && touchedColData > threshold)
        {
            touchedColBlock = touchedCol;
            touchedRowBlock = touchedRow;
        }
        else
        {
            touchedColBlock = -1;
            touchedRowBlock = -1;
        }
    }

    private void updateBoard()
    {
        //Update row & col data
        float tmpR, tmpC;
        for (int i = 0; i < blockSeriesCol; i++)
        {
            tmpC = Math.Max(colData[i] - baselineC[i], 0);
            updateBlock(blockSeriesRow, i, new Color(1 - tmpC / 255, 1, 1 - tmpC / 255), ((int)tmpC).ToString());
            //blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().sizeDelta = new Vector2(blockSize, tmpC / chartBarSize * blockSize);
            //Vector2 tmpPos = blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().anchoredPosition;
            //blockSeries[blockSeriesRow + 1, i].GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x, tmpRowPos.y - (blockSize - (tmpC / chartBarSize * blockSize)) / 2);

            blockSeries[blockSeriesRow + 1, blockSeriesCol - 1 - i].GetComponent<RectTransform>().sizeDelta = new Vector2(blockSize, tmpC / chartBarSize * blockSize);
            Vector2 tmpPos = blockSeries[blockSeriesRow + 1, blockSeriesCol - 1 - i].GetComponent<RectTransform>().anchoredPosition;
            blockSeries[blockSeriesRow + 1, blockSeriesCol - 1 - i].GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpPos.x, tmpRowPos.y - (blockSize - (tmpC / chartBarSize * blockSize)) / 2);
        }
        for (int i = 0; i < blockSeriesRow; i++)
        {
            tmpR = Math.Max(rowData[i] - baselineR[i], 0);
            updateBlock(i, blockSeriesCol, new Color(1 - tmpR / 255, 1, 1 - tmpR / 255), ((int)tmpR).ToString());
            blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().sizeDelta = new Vector2(tmpR / chartBarSize * blockSize, blockSize);
            Vector2 tmpPos = blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().anchoredPosition;
            blockSeries[i, blockSeriesCol + 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(tmpColPos.x - (blockSize - (tmpR / chartBarSize * blockSize)) / 2, tmpPos.y);
        }

        //Update point
        for (int i = 0; i < blockSeriesCol; i++)
        {
            for (int j = 0; j < blockSeriesRow; j++)
            {
                updateBlock(i, j, Color.white, "");
            }
        }
        if (touchedColBlock != -1)
        {
            updateBlock(touchedRowBlock, touchedColBlock, new Color(1, 1 - (touchedRowData + touchedColData) / 2550, 1 - (touchedRowData + touchedColData) / 2550), ((touchedRowData + touchedColData) / 10).ToString());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            nextTestingBlock();
            return;
        }
        else if (Input.GetKeyDown("w"))
        {
            if (showOnBoard) {
                parentCanvas.transform.position = 
                    new Vector3(parentCanvas.transform.position.x, parentCanvas.transform.position.y+10, parentCanvas.transform.position.z);
            } 
            return;
        }
        else if (Input.GetKeyDown("s"))
        {
            if (showOnBoard)
            {
                parentCanvas.transform.position =
                    new Vector3(parentCanvas.transform.position.x, parentCanvas.transform.position.y - 10, parentCanvas.transform.position.z);
            }
        }
        else;

        //Fetch baseline data
            if (baseLineVersion)
        {
            //if (!calBaseFinish)
            //{
            //    if (totalBaseCount < baseDataStart * arduinoData.Length)
            //    {
            //        for (int i = 0; i < arduinoData.Length; i++)
            //        {
            //            if (ardBaseCount[i] < baseDataStart && !string.Equals(arduinoData[i], lastArduinoData[i]))
            //            {
            //                ardBaseCount[i]++;
            //                totalBaseCount++;
            //                updateBlock(0, i, Color.white, ardBaseCount[i].ToString());
            //                lastArduinoData[i] = string.Copy(arduinoData[i]);
            //            }
            //        }
            //    }
            //    else if (totalBaseCount < baseDataTimes * arduinoData.Length)
            //    {
            //        for (int i = 0; i < arduinoData.Length; i++)
            //        {
            //            if (ardBaseCount[i] < baseDataTimes && !string.Equals(arduinoData[i], lastArduinoData[i]))
            //            {
            //                ardBaseCount[i]++;
            //                totalBaseCount++;
            //                updateBlock(0, i, Color.white, ardBaseCount[i].ToString());
            //                lastArduinoData[i] = string.Copy(arduinoData[i]);
            //                fetchArduinoBaseData(i);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < blockSeriesRow; i++)
            //        {
            //            baselineR[i] /= baseDataTimes - baseDataStart;
            //        }
            //        for (int i = 0; i < blockSeriesCol; i++)
            //        {
            //            baselineC[i] /= baseDataTimes - baseDataStart;
            //        }
            //        calBaseFinish = true;
            //        if (testVersion)
            //        {
            //            transform.GetChild(0).gameObject.GetComponent<Arduino>().arduinoSerial.WriteLine("1");
            //            testState = 1;
            //        }
            //    }
            //}
        }

        if (testVersion)
        {
            if (testState == 1)
            {
                if (testTimes >= testMaxTimes)
                {
                    updateBlock(testingRow, testingCol, Color.gray, "");
                    gameObject.GetComponent<AudioSource>().clip = audios[4];
                    gameObject.GetComponent<AudioSource>().Play();
                    return;
                }
                else if (testTimes == 0)
                {
                    for(int i = 0; i < 8; i++)
                    {
                        testCorrectTimes[i] = 0;
                    }
                }
                else;
                delayCount++;
                updateBlock(testingRow, testingCol, Color.green, delayCount.ToString());
                for (int i = 0; i < arduinoData.Length; i++)
                {
                    if (fetchArduinoHighData(i) && delayCount> greenDelayTimes)
                    {
                        transform.GetChild(0).gameObject.GetComponent<Arduino>().arduinoSerial.Write("2");
                        delayCount = 0;
                        testState = 2;
                        break;
                    }
                }
            }
            else if (testState == 2)
            {
                delayCount++;
                updateBlock(testingRow, testingCol, Color.yellow, delayCount.ToString());
                if (delayCount > yellowDelayTimes)
                {
                    gameObject.GetComponent<AudioSource>().clip = audios[0];
                    gameObject.GetComponent<AudioSource>().Play();
                    testState = 3;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            for (int k = 0; k < blockSeriesRow; k++)
                            {
                                isTestDataFetch[i, j, k] = false;
                            }
                        }
                    }
                }
            }
            else if (testState == 3)
            {
                updateBlock(testingRow, testingCol, Color.red, "");

                //Do it in updateArduinoData()
                //for (int i = 0; i < arduinoData.Length; i++)
                //{
                //    fetchTestArduinoData(i);
                //}

                //Check if all data fetched
                allTestDataFetch = true;

                ////Only for 5*4 testing
                //for (int i = 0; i < 8; i++)
                //{
                //    for(int j = 12; j < 15; j++)
                //    {
                //        isTestDataFetch[1, i, j] = true;
                //    }
                //}

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        for (int k = 0; k < blockSeriesRow; k++)
                        {
                            if(isTestDataFetch[i, j, k] == false)
                            {
                                allTestDataFetch = false;
                            }
                        }
                    }
                }

                if (allTestDataFetch)
                {
                    updateBlock(testingRow, testingCol, Color.gray, "");
                    transform.GetChild(0).gameObject.GetComponent<Arduino>().arduinoSerial.Write("1");

                    //Calculate test data
                    for (int i = 0; i < 8; i++)
                    {
                        touchedTestRow[i] = 0;
                        touchedTestRowData[i] = 0;
                        for (int j = 0; j < blockSeriesRow; j++)
                        {
                            if(rowTestData[i, j]> touchedTestRowData[i])
                            {
                                touchedTestRow[i] = j;
                                touchedTestRowData[i] = rowTestData[i, j];
                            }
                        }

                        touchedTestCol[i] = 0;
                        touchedTestColData[i] = 0;
                        for (int j = 0; j < blockSeriesCol; j++)
                        {
                            if (colTestData[i, j] > touchedTestColData[i])
                            {
                                touchedTestCol[i] = j;
                                touchedTestColData[i] = colTestData[i, j];
                            }
                        }

                        //if (touchedTestRowData[i] > testThreshold[i] && touchedTestColData[i] > testThreshold[i])
                        //{
                            touchedTestRowBlock[i] = touchedTestRow[i];
                            touchedTestColBlock[i] = touchedTestCol[i];
                        //}
                        //else
                        //{
                        //    touchedTestRowBlock[i] = -1;
                        //    touchedTestColBlock[i] = -1;
                        //}
                    }

                    //Detect finger mistake
                    if (touchedTestRowBlock[7] != testingRow || touchedTestColBlock[7] != testingCol || touchedTestRowBlock[6] != testingRow || touchedTestColBlock[6] != testingCol)
                    {
                        updateBlock(testingRow, blockSeriesCol, Color.gray, "");
                        delayCount = 0;
                        testState = 4;
                        gameObject.GetComponent<AudioSource>().clip = audios[3];
                        gameObject.GetComponent<AudioSource>().Play();
                        return;
                    }
                    else
                    {
                        updateBlock(testingRow, blockSeriesCol, Color.white, "");
                    }
                    //Test result & record correct times
                    for (int i = 0; i < 8; i++)
                    {
                        if(touchedTestRowBlock[i] == testingRow && touchedTestColBlock[i] == testingCol)
                        {
                            isTestCorrect[i] = true;
                            testCorrectTimes[i]++;
                        }
                        else
                        {
                            isTestCorrect[i] = false;
                        }
                    }

                    //Write into file
                    testTimes++;

                    if (testTimes > testMaxTimes)
                    {
                        delayCount = 0;
                        testState = 1;
                        gameObject.GetComponent<AudioSource>().clip = audios[4];
                        gameObject.GetComponent<AudioSource>().Play();
                        return;
                    }

                    string fileName, str;
                    
                    if (testTimes == 1)
                    {
                        //Create file
                        str = testingRow.ToString() + "," + testingCol.ToString() + "\r\n";
                        fileName = testingRow.ToString() + "_" + testingCol.ToString();
                        File.AppendAllText("TestData/" + fileName + ".csv", str);
                    }
                    else if (testTimes == testMaxTimes)
                    {
                        //Write in summary file
                        str = testingRow.ToString() + "," + testingCol.ToString() + ",T," + testMaxTimes + ",D";
                        for(int i = 0; i < 8; i++)
                        {
                            str = str + "," + testCorrectTimes[i];
                        }
                        str = str + "\r\n";
                        File.AppendAllText("TestData/sum.csv", str);
                    }
                    else;

                    //Write data
                    if(testTimes >= 1 && testTimes <= testMaxTimes)
                    {
                        str = "T," + testTimes.ToString() + ",D";
                        for (int i = 0; i < 8; i++)
                        {
                            str = str + "," + testCorrectTimes[i];
                        }
                        str = str + "\r\n";
                        for (int i = 0; i < 8; i++)
                        {
                            str = str + "S," + (i + 1).ToString() + "," + touchedTestRowBlock[i].ToString() + "," + touchedTestColBlock[i].ToString() + "\r\n";
                            str = str + "R";
                            for (int j = 0; j < blockSeriesRow; j++)
                            {
                                str = str + "," + rowTestData[i, j].ToString();
                            }
                            str = str + "\r\n";
                            str = str + "C";
                            for (int j = 0; j < blockSeriesCol; j++)
                            {
                                str = str + "," + colTestData[i, j].ToString();
                            }
                            str = str + "\r\n";
                            str = str + "TR";
                            for (int j = 0; j < blockSeriesRow; j++)
                            {
                                str = str + "," + rowTimestamp[i, j].ToString();
                            }
                            str = str + "\r\n";
                            str = str + "TC";
                            for (int j = 0; j < blockSeriesCol; j++)
                            {
                                str = str + "," + colTimestamp[i, j].ToString();
                            }
                            str = str + "\r\n";
                        }
                        fileName = testingRow.ToString() + "_" + testingCol.ToString();
                        File.AppendAllText("TestData/" + fileName + ".csv", str);
                    }

                    if (testTimes == testMaxTimes)
                    {
                        gameObject.GetComponent<AudioSource>().clip = audios[2];
                        gameObject.GetComponent<AudioSource>().Play();
                        testTimes = 0;
                        if (touchXMode)
                        {
                            //Auto change block -- X
                            if (testingRow == 6 && testingCol == 8)
                            {
                                testingRow = 8;
                                testingCol = 6;
                            }
                            else if (testingCol == 14 && testingRow == 14)
                            {
                                testingRow = 0;
                            }
                            else if (testingCol < blockSeriesCol - 1 && testingRow == testingCol)
                            {
                                testingRow++;
                                testingCol++;
                            }
                            else if (testingCol > 0)
                            {
                                testingRow++;
                                testingCol--;
                            }
                        }
                        else if (touchXComplementMode)
                        {
                            //Auto change block-- X complement
                            if (testingCol < blockSeriesCol - 1)
                            {
                                testingCol++;
                            }
                            else if (testingRow < blockSeriesRow - 1)
                            {
                                testingCol = 0;
                                testingRow++;
                            }

                            if (testingCol == testingRow || (testingCol + testingRow) == 14)
                            {
                                if (testingCol < blockSeriesCol - 1)
                                {
                                    testingCol++;
                                }
                                else if (testingRow < blockSeriesRow - 1)
                                {
                                    testingCol = 0;
                                    testingRow++;
                                }
                            }
                        }
                        else if (Layer1ComplementMode)
                        {
                            if (testingRow == 0 && testingCol == 3)
                            {
                                testingCol = 7;
                            }
                            else if (testingRow == 0 && testingCol == 7)
                            {
                                testingCol = 9;
                            }
                            else if (testingRow == 0 && testingCol == 9)
                            {
                                testingCol = 12;
                            }
                            else if (testingRow == 0 && testingCol == 12)
                            {
                                testingRow = 1;
                                testingCol = 6;
                            }
                            else if (testingRow == 1 && testingCol == 6)
                            {
                                testingCol = 7;
                            }
                            else if (testingRow == 1 && testingCol == 7)
                            {
                                testingCol = 11;
                            }
                            else if (testingRow == 1 && testingCol == 11)
                            {
                                testingRow = 5;
                                testingCol = 7;
                            }
                            else if (testingRow == 5 && testingCol == 7)
                            {
                                testingRow = 12;
                                testingCol = 6;
                            }
                            else if (testingRow == 12 && testingCol == 6)
                            {
                                testingRow = 12;
                                testingCol = 7;
                            }
                            else if (testingRow == 12 && testingCol == 7)
                            {
                                testingRow = 13;
                            }
                            else if (testingRow == 13 && testingCol == 7)
                            {
                                testingCol = 0;
                                testingRow = 0;
                            }
                        }
                        else if (Layer0ComplementMode)
                        {
                            if (testingRow == 2 && testingCol == 12)
                            {
                                testingRow = 5;
                                testingCol = 3;
                            }
                            else if (testingRow == 5 && testingCol == 3)
                            {
                                testingRow = 10;
                                testingCol = 10;
                            }
                            else if (testingRow == 10 && testingCol == 10)
                            {
                                testingRow = 10;
                                testingCol = 12;
                            }
                            else if (testingRow == 10 && testingCol == 12)
                            {
                                testingRow = 11;
                                testingCol = 14;
                            }
                            else if (testingRow == 11 && testingCol == 14)
                            {
                                testingRow = 12;
                                testingCol = 4;
                            }
                            else if (testingRow == 12 && testingCol == 4)
                            {
                                testingRow = 13;
                            }
                            else if (testingRow == 13 && testingCol == 4)
                            {
                                testingRow = 14;
                                testingCol = 1;
                            }
                            else if (testingRow == 14 && testingCol < 13)
                            {
                                testingCol++;
                            }
                            else
                            {
                                testingCol = 0;
                                testingRow = 0;
                            }
                        }
                        else
                        {
                            //Auto change block-- Regular
                            if (testingCol < blockSeriesCol - 1)
                            {
                                testingCol++;
                            }
                            else if (testingRow < blockSeriesRow - 1)
                            {
                                testingCol = 0;
                                testingRow++;
                            }
                        }
                    }

                    delayCount = 0;
                    testState = 4;
                    if (testTimes != 0)
                    {
                        gameObject.GetComponent<AudioSource>().clip = audios[1];
                        gameObject.GetComponent<AudioSource>().Play();
                    }
                }
            }
            else if (testState == 4)
            {
                updateBlock(testingRow, testingCol, Color.blue, "");
                bool hasHighData = false;
                for (int i = 0; i < arduinoData.Length; i++)
                {
                    string[] newData = arduinoData[i].Split(',');
                    if (newData[0] != "H") return;
                    if (fetchArduinoHighData(i))
                    {
                        hasHighData = true;
                        break;
                    }
                }
                if(hasHighData == false)
                {
                    delayCount = 0;
                    testState = 1;
                }
            }
        }
        //Normal version
        else
        {
            for (int i = 0; i < arduinoData.Length; i++)
            {
                if (!string.Equals(arduinoData[i], lastArduinoData[i]))
                {
                    lastArduinoData[i] = string.Copy(arduinoData[i]);
                    fetchArduinoData(i);
                }
            }
            
            calData(rowData, colData, 1);

            if (showOnBoard)
            {
                updateBoard();
            }
        }
        
    }

    private bool fetchArduinoHighData(int boardNum)
    {
        string[] newData = arduinoData[boardNum].Split(',');
        if (newData[0] != "H") return false;
        for(int i = 1; i < newData.Length; i++)
        {
            if (float.Parse(newData[i])>threshold)
            {
                return true;
            }
        }

        return false;
    }

    private void fetchTestArduinoData(int boardNum)
    {
        string[] newData = arduinoData[boardNum].Split(',');
        float[,] target;
        float[,] targetTimestamp;
        int RC;
        if (newData[0] == "T")
        {
            if (newData[2] == "R")
            {
                target = rowTestData;
                targetTimestamp = rowTimestamp;
                RC = 0;
            }
            else if (newData[2] == "C")
            {
                target = colTestData;
                targetTimestamp = colTimestamp;
                RC = 1;
            }
            else
            {
                Debug.Log("Invalid Arduino Data in board " + boardNum + ", newData[2]: " + newData[2]);
                updateBlock(blockSeriesRow, blockSeriesCol, Color.green, "Wrong");
                return;
            }

            int clipStart, clipNum, sampleTimes;
            clipStart = int.Parse(newData[3]);
            clipNum = int.Parse(newData[4]);
            sampleTimes = int.Parse(newData[5]);
            if (int.Parse(newData[1]) == 0)
            {
                for (int i = 0; i < clipNum; i++)
                {
                    target[sampleTimes, clipStart + i] = float.Parse(newData[i + 6]);
                    targetTimestamp[sampleTimes, clipStart + i] = float.Parse(newData[i + 6 + clipNum]);
                    try
                    {
                        isTestDataFetch[RC, sampleTimes, clipStart + i] = true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log("aaaa: " + sampleTimes.ToString() + "bbbb: " + (clipStart + i).ToString());
                    }
                }
            }
            else if (int.Parse(newData[1]) == 1)
            {
                for (int i = 0; i < clipNum / 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        target[sampleTimes, clipStart + i * 6 + j] = float.Parse(newData[i * 3 + j + 6]);
                        targetTimestamp[sampleTimes, clipStart + i * 6 + j] = float.Parse(newData[i * 3 + j + 6 + clipNum]);
                        try
                        {
                            isTestDataFetch[RC, sampleTimes, clipStart + i * 6 + j] = true;
                        }
                        catch (Exception e)
                        {
                            Debug.Log("cccc: " + sampleTimes.ToString() + "dddd: " + (clipStart + i * 6 + j).ToString());
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Invalid JUMPTYPE, newData[1]: " + newData[1]);
                updateBlock(blockSeriesRow, blockSeriesCol, Color.blue, "Wrong");
            }
        }
        else
        {
            Debug.Log("Invalid Arduino Data in board " + boardNum + ", newData[0]: " + newData[0]);
            //updateBlock(blockSeriesRow, blockSeriesCol, Color.red, "Wrong");
        }
    }

    public void updateArduinoData()
    {
        Stack<string> touchShow;

        for(int i=0; i<arduinoData.Length; i++)
        {
            touchShow = transform.GetChild(0).GetChild(i).gameObject.GetComponent<Arduino>().touchShowID;
            while (touchShow.Count > 0)
            {
                arduinoData[i] = touchShow.Pop();
            }
        }

        if(testVersion && testState == 3)
        {
            for (int i = 0; i < arduinoData.Length; i++)
            {
                fetchTestArduinoData(i);
            }
        }
    }

    public void nextTestingBlock()
    {
        if (touchXMode)
        {
            //Auto change block -- X
            if (testingRow == 6 && testingCol == 8)
            {
                testingRow = 8;
                testingCol = 6;
            }
            else if (testingCol == 14 && testingRow == 14)
            {
                testingRow = 0;
            }
            else if (testingCol < blockSeriesCol - 1 && testingRow == testingCol)
            {
                testingRow++;
                testingCol++;
            }
            else if (testingCol > 0)
            {
                testingRow++;
                testingCol--;
            }
        }
        else if (touchXComplementMode)
        {
            //Auto change block-- X complement
            if (testingCol < blockSeriesCol - 1)
            {
                testingCol++;
            }
            else if (testingRow < blockSeriesRow - 1)
            {
                testingCol = 0;
                testingRow++;
            }

            if (testingCol == testingRow || (testingCol + testingRow) == 14)
            {
                if (testingCol < blockSeriesCol - 1)
                {
                    testingCol++;
                }
                else if (testingRow < blockSeriesRow - 1)
                {
                    testingCol = 0;
                    testingRow++;
                }
            }
        }
        else if (Layer1ComplementMode)
        {
            if (testingRow == 0 && testingCol == 3)
            {
                testingCol = 7;
            }
            else if (testingRow == 0 && testingCol == 7)
            {
                testingCol = 9;
            }
            else if (testingRow == 0 && testingCol == 9)
            {
                testingCol = 12;
            }
            else if (testingRow == 0 && testingCol == 12)
            {
                testingRow = 1;
                testingCol = 6;
            }
            else if (testingRow == 1 && testingCol == 6)
            {
                testingCol = 7;
            }
            else if (testingRow == 1 && testingCol == 7)
            {
                testingCol = 11;
            }
            else if (testingRow == 1 && testingCol == 11)
            {
                testingRow = 5;
                testingCol = 7;
            }
            else if (testingRow == 5 && testingCol == 7)
            {
                testingRow = 12;
                testingCol = 6;
            }
            else if (testingRow == 12 && testingCol == 6)
            {
                testingRow = 12;
                testingCol = 7;
            }
            else if (testingRow == 12 && testingCol == 7)
            {
                testingRow = 13;
            }
            else if (testingRow == 13 && testingCol == 7)
            {
                testingCol = 0;
                testingRow = 0;
            }
        }
        else if (Layer0ComplementMode)
        {
            if (testingRow == 2 && testingCol == 12)
            {
                testingRow = 5;
                testingCol = 3;
            }
            else if (testingRow == 5 && testingCol == 3)
            {
                testingRow = 10;
                testingCol = 10;
            }
            else if (testingRow == 10 && testingCol == 10)
            {
                testingRow = 10;
                testingCol = 12;
            }
            else if (testingRow == 10 && testingCol == 12)
            {
                testingRow = 11;
                testingCol = 14;
            }
            else if (testingRow == 11 && testingCol == 14)
            {
                testingRow = 12;
                testingCol = 4;
            }
            else if (testingRow == 12 && testingCol == 4)
            {
                testingRow = 13;
            }
            else if (testingRow == 13 && testingCol == 4)
            {
                testingRow = 14;
                testingCol = 1;
            }
            else if (testingRow == 14 && testingCol < 13)
            {
                testingCol++;
            }
            else
            {
                testingCol = 0;
                testingRow = 0;
            }
        }
        else
        {
            //Auto change block-- Regular
            if (testingCol < blockSeriesCol - 1)
            {
                testingCol++;
            }
            else if (testingRow < blockSeriesRow - 1)
            {
                testingCol = 0;
                testingRow++;
            }
        }
    }
}

