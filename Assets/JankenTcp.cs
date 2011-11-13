using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



public class JankenTcp : MonoBehaviour {
	
	IPEndPoint remoteIP;
	byte[] data;
	Socket s;
	
	const byte GOO = 100;
	const byte CHOKI = 101;
	const byte PAA = 102;
	static UdpClient usck;
	int i = 0;
	Thread trd;
	
	int svhand = -1;
	byte judge = 255;
	
	private void recvTask(){
		byte[] recvbuf = new byte[512];
		Debug.Log("recv task");
		while(true){
			i++;
			if(s != null){
				JankenTcp.Receive(s, recvbuf, 0, recvbuf.Length, 10000);
				svhand = recvbuf[1];
				judge = recvbuf[0];
				Debug.Log("recv"+recvbuf[0]+", "+recvbuf[1]);
			}
			Debug.Log("hugehuge"+i);
			Thread.Sleep(1000);
		}
	}
	
	public static void Receive(Socket socket, byte[] buffer, int offset, int size, int timeout)
	{
	  int startTickCount = Environment.TickCount;
	  int received = 0;  // how many bytes is already received
	  do {
	    if (Environment.TickCount > startTickCount + timeout)
	      throw new Exception("Timeout.");
	    try {
	      received += socket.Receive(buffer, offset + received, size - received, SocketFlags.None);
		  Debug.Log("recved.");
	    }
	    catch (SocketException ex)
	    {
	      if (ex.SocketErrorCode == SocketError.WouldBlock ||
	          ex.SocketErrorCode == SocketError.IOPending ||
	          ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
	      {
	        // socket buffer is probably empty, wait and try again
	        Thread.Sleep(30);
	      }
	      else
	        throw ex;  // any serious error occurr
	    }
	  } while (received < size);
	}

	
	public static void Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
	{
	  int startTickCount = Environment.TickCount;
	  int sent = 0;  // how many bytes is already sent
		Debug.Log("send:"+startTickCount);
	  do {
	    if (Environment.TickCount > startTickCount + timeout){
			Debug.Log("time out");
	      	throw new Exception("Timeout.");
		}
	    try {
			Debug.Log("send sock");
	      sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
	    }
	    catch (SocketException ex)
	    {

	      if (ex.SocketErrorCode == SocketError.WouldBlock ||
	          ex.SocketErrorCode == SocketError.IOPending ||
	          ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
	      {
	        // socket buffer is probably full, wait and try again
					Debug.Log("sleep");
	        Thread.Sleep(30);
	      }
	      else
	        throw ex;  // any serious error occurr
	    }
	  } while (sent < size);
	}

	
	void OnGUI(){
		if(GUI.Button (new Rect (10,10,100,100), "goo")){
			data[0] = GOO;
			Debug.Log("write:"+data.GetLength(0));
			JankenTcp.Send(s, data, 0, data.Length, 10000);
		}

		if(GUI.Button (new Rect (120,10,100,100), "choki")){
			data[0] = CHOKI;
			JankenTcp.Send(s, data, 0, data.Length, 10000);

		}
		
		if(GUI.Button (new Rect (230,10,100,100), "paa")){
			data[0] = PAA;
			JankenTcp.Send(s, data, 0, data.Length, 10000);

		}
		
		if(judge != 255){
			if(judge == 0){
				if(GUI.Button (new Rect (230,200,100,100), "you win")){
					s.Close();
					Application.LoadLevel("janken-win-scene");
				}
	
			}
			else if(judge == 1){
				if(GUI.Button (new Rect (230,200,100,100), "you lose")){
				}
			}
			else if(judge == 2){
				if(GUI.Button (new Rect (230,200,100,100), "aiko")){
				}
			}
		}

	}
	
	// Use this for initialization
	void Start () {
		data = new byte[16];

		if(s==null){
			TcpClient tcpClient = new TcpClient();
			tcpClient.Connect("49.212.3.92", 8080);
			Debug.Log("init client");
			s = tcpClient.Client;
		}
		trd = new Thread(new ThreadStart(recvTask));
		trd.IsBackground = true;
		trd.Start();
		Debug.Log("start");

	}
	
	// Update is called once per frame
	void Update () {

		
	}
}
