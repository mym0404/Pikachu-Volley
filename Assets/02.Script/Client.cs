using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;



public class Client {
    public static string GetGameVersion(out bool isServerOn) {
        byte[ ] Buffer = new Byte[5];

        TcpClient client = null;
        client = new TcpClient();
        try {
            client.Connect("172.17.84.217" , 5001);
        } catch (Exception e) {
            Debug.Log(e.Message);
        }

        if (client.Connected == false) {
            isServerOn = false;
            return "ServerOffline";

        }
        NetworkStream stream = client.GetStream();
        stream.Read(Buffer , 0 , Buffer.Length);

        string gameVersion = Encoding.Default.GetString(Buffer);
        client.Close();

        isServerOn = true;
        return gameVersion;
    }
}


    

