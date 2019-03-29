using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// For Serial
using System.IO.Ports;
// For compare array
using System.Linq;

public class Arduino : MonoBehaviour { 
	// Serial
	public HashSet<int> touchID = new HashSet<int>();
    public Stack<string> touchShowID = new Stack<string>();
    public string portName;
	public int baudRate;
	public SerialPort arduinoSerial;
	public string[] a;
    public bool isController;
    [Tooltip("0 for normal, 1 for testing")]public int mode;
    public int rC;
    public int jumpType;
    public int clipStart;
    public int clipNum;
    public int sampleTimes;
    public int testSampleTimes;
    public bool isBaseline;

    private int count;

    private bool arduinoReady;

    void Start () {
		// Open Serial port
		arduinoSerial = new SerialPort (portName, baudRate);
		// Set buffersize so read from Serial would be normal
		arduinoSerial.ReadTimeout = 50;
		arduinoSerial.ReadBufferSize = 8192;
		arduinoSerial.WriteBufferSize = 128;
		arduinoSerial.ReadBufferSize = 4096;
		arduinoSerial.Parity = Parity.None;
		arduinoSerial.StopBits = StopBits.One;
		arduinoSerial.DtrEnable = true;
		arduinoSerial.RtsEnable = true;
		arduinoSerial.Open ();
        count = 0;
        arduinoReady = false;
    }

	void Update() {
        if (isController)
        {
            count++;
            if (count == 10)
            {
                arduinoSerial.Write("6");
            }
        }
        else
        {
            if (!arduinoReady)
            {
                string str = rC.ToString() + jumpType.ToString() + clipStart.ToString() + clipNum.ToString() + mode.ToString() + sampleTimes.ToString() + testSampleTimes.ToString();
                if (isBaseline)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
                arduinoSerial.WriteLine(str);
                arduinoReady = true;
            }
            else
            {
                ReadFromArduino();
            }
        }
    }

	public void ReadFromArduino () {
		string str = null;
        
        try
        {
            str = arduinoSerial.ReadLine();
            if (str.Length >= 3)
            {
                touchShowID.Push(str);
            }
        }
        catch (TimeoutException e)
        {
        }
    }
}