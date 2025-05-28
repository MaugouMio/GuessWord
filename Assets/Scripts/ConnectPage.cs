using System;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPage : MonoBehaviour
{
	[SerializeField]
	private InputField InputIP;
	[SerializeField]
	private Text ConnectHintText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		NetManager.Instance.OnConnected = OnConnected;
		NetManager.Instance.OnDisconnected = OnDisconnected;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Return))
			ClickConnect();
    }

	void OnDestroy()
	{
		NetManager.Instance.OnConnected = null;
		NetManager.Instance.OnDisconnected = null;
	}

	private void OnConnected()
	{
		ConnectHintText.text = "�s�u���\";
	}

	private void OnDisconnected()
	{
		ConnectHintText.text = "�s�u���_";
	}

	public void ClickConnect()
	{
		string[] param = InputIP.text.Split(':');
		if (param.Length != 2)
		{
			ConnectHintText.text = "�п�J���T�� IP:PORT �榡";
			return;
		}

		try
		{
			string ip = param[0];
			int port = Int32.Parse(param[1]);
			NetManager.Instance.Connect(ip, port);
		}
		catch
		{
			ConnectHintText.text = "�п�J���T�� IP:PORT �榡";
			return;
		}

		ConnectHintText.text = "�s�u��";
	}
}
