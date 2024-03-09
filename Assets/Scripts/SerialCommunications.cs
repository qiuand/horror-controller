using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialCommunications : MonoBehaviour
{

    public static bool communicationReadyFlag=true;

    Thread IOThread = new Thread(DataThread);

    private static SerialPort sp = new SerialPort("COM6", 9600);

    int variable;

    private static int index;

    private static byte[] incoming = new byte[9];
    public static byte[] outgoing = new byte[8];

    public static byte[] rawData = new byte[100];

    public static byte[] validatedIncoming = new byte[9];

    public static float threadTimer = 0f;
    public static float threadDelay = 0.020f;

    float lightTimer;
    float lightTimerOriginal = .5f;
    bool isLightOn = false;

    public static void DataThread()
    {
/*        while(threadTimer<= threadDelay)
        {
            threadTimer += Time.deltaTime;
        }
        threadTimer = 0f;*/

        sp = new SerialPort("COM6", 9600);
        sp.Open();
        sp.ReadTimeout=20;
        while (true)
        {
            for(int k=0; k<rawData.Length; k++)
            {
                rawData[k] = 0;
            }
            int bytesAvailable = sp.BytesToRead;

            sp.Read(rawData, 0, bytesAvailable);
            sp.Write(outgoing, 0, 8);

            print("bytes: " + bytesAvailable);

            for (int i=0; i<bytesAvailable; i++)
            {
                if((rawData[i]=='P') && (rawData[i+1]=='C') && (rawData[i + 2] == '4'))
                {
                    index = i;
                    break;
                }
            }
            for(int j=0; j<9; j++)
            {
                incoming[j] = rawData[index+j];
            }

            if (/*(incoming[0] == 'P') && (incoming[1] == 'C') && (incoming[2] =='4') && */(incoming[8] == 'Z'))
            {
                if (!communicationReadyFlag)
                { 
                    for(int i = 0; i < 8; i++)
                    {
                        validatedIncoming[i] = incoming[i];
                    }
                    communicationReadyFlag = true;
                }
            }
            else
            {

            }
            /*            Debug.Log("Serial Script: " + validatedIncoming[0] + " + " + validatedIncoming[1] + " + " + validatedIncoming[2] + " + " + validatedIncoming[3] + " + " + validatedIncoming[4] + " + " + validatedIncoming[5] + " + " + validatedIncoming[6]);
            */


            Thread.Sleep(30);
        }
    }

    private void OnDestroy()
    {
        {
            IOThread.Abort();
            sp.Close();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        IOThread.Start();
        IOThread.Priority = System.Threading.ThreadPriority.Highest;

        /*        lightTimer = lightTimerOriginal;

                outgoingMsgChar[0] = (byte)'P';
                outgoingMsgChar[1] = (byte)'C';
                outgoingMsgChar[2] = 2;
                outgoingMsgChar[3] = 0;

                byte LED_B1 = 1;
                byte LED_R1 = 1;

                outgoingMsgChar[4] = (byte)(LED_B1 | (LED_R1<<1));*/

        outgoing[0] = (byte)'L';
        outgoing[1] = (byte)'C';
        outgoing[2] = (byte)'4';
        outgoing[3] = 0b11111111;
        outgoing[4] = 0b11111111;
        outgoing[5] = 0b11111111;
        outgoing[6] = 0b11111111;
        outgoing[7] = (byte)'Z';

    }

    // Update is called once per frame
    void Update()
    {

    }
}
