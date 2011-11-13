using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Janken : MonoBehaviour {

	IPEndPoint remoteIP;
	byte[] data;
	Socket s;
	const byte GOO = 100;
	const byte CHOKI = 101;
	const byte PAA = 102;
	static UdpClient usck;
	
	static void recv(IAsyncResult res){
		IPEndPoint remote = new IPEndPoint(IPAddress.Any,0);
		Debug.Log("hogehoge");
        byte[] data = usck.EndReceive(res, ref remote);
		Debug.Log(remote.Address.ToString() + ": " + Encoding.ASCII.GetString(data));
		usck.BeginReceive(recv, null);
	}
	
	void OnGUI(){
		if(GUI.Button (new Rect (10,10,100,100), "goo")){
			data[0] = GOO;
			s.SendTo(data, 0, data.Length, SocketFlags.None, remoteIP);
		}

		if(GUI.Button (new Rect (120,10,100,100), "choki")){
			data[0] = CHOKI;
			s.SendTo(data, 0, data.Length, SocketFlags.None, remoteIP);
		}
		
		if(GUI.Button (new Rect (230,10,100,100), "paa")){
			data[0] = PAA;
			s.SendTo(data, 0, data.Length, SocketFlags.None, remoteIP);
		}

	}
	
	// Use this for initialization
	void Start () {
		usck = new UdpClient(1111);
		usck.BeginReceive(new AsyncCallback(recv), null);
		Debug.Log("start");
		remoteIP = new IPEndPoint(IPAddress.Parse("49.212.3.92"), 1111);
		data = new byte[16];
		s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IpTimeToLive, 255);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
