using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using System.Text;
using System;

public class Echo : MonoBehaviour
{
    Socket socket;
    public InputField inputField;
    public Text text;
    byte[] readBuff = new byte[1024];
    string recvStr = "";

    public void Connection()//连接函数
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.BeginConnect("127.0.0.1", 8888, ConnectCallBack, socket);//连接服务器（连接方法）
    }
    public void ConnectCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            print("ok");
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack, socket);
        }
        catch(SocketException ex)
        {
            print("fail" + ex.ToString());
        }
    }
    public void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count= socket.EndReceive(ar);
            recvStr = Encoding.Default.GetString(readBuff, 0, count);
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack, socket);
        }
        catch(SocketException ex)
        {
            print("Receive fail" + ex.ToString());
        }
    }
    public void Send()//发送方法
    {
        string sendStr = inputField.text;
        byte[] sendBytes = Encoding.Default.GetBytes(sendStr);
        socket.BeginSend(sendBytes,0,sendBytes.Length,0,SendCallBack,socket);
        //////////////////////////////////////////////////////////////
        ///下面是接受方法
        //int count = socket.Receive(readBuff);
        //string recvStr = Encoding.Default.GetString(readBuff, 0, count);
        //text.text = recvStr;
        ////socket.Close();
    }
    public void SendCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndSend(ar);
            print("send succ");
        }
        catch(SocketException ex)
        {
            print("send fail");
        }
    }
    public void Update()
    {
        text.text = recvStr;
    }
}
