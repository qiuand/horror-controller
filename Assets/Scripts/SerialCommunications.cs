using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class SerialCommunications : MonoBehaviour
{

    public static bool communicationReadyFlag=true;

    Thread IOThread = new Thread(DataThread);

    private static SerialPort sp = new SerialPort("COM7", 9600);

    int variable;

    private static byte[] incoming = new byte[8];
    public static byte[] outgoing = new byte[7];

    public static byte[] validatedIncoming = new byte[8];


    float lightTimer;
    float lightTimerOriginal = .5f;
    bool isLightOn = false;

    public static void DataThread()
    {
       
        sp = new SerialPort("COM7", 9600);
        sp.Open();

        while (true)
        {
            sp.Read(incoming, 0, 8);

            if (incoming[0] == 'P' && incoming[1] == 'C')
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
                sp.Write(outgoing, 0, 7);
            }
            /*            Debug.Log("Serial Script: " + validatedIncoming[0] + " + " + validatedIncoming[1] + " + " + validatedIncoming[2] + " + " + validatedIncoming[3] + " + " + validatedIncoming[4] + " + " + validatedIncoming[5] + " + " + validatedIncoming[6]);
            */


            Thread.Sleep(10);
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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
