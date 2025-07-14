using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerPage : MonoBehaviour
{
	[SerializeField]
	private InputField IP_Input;
	[SerializeField]
	private ServerListItem customServerItem;
	[SerializeField]
	private ServerListItem templateServerItem;

	private ServerList serverList;
	private List<ServerListItem> serverItems = new List<ServerListItem>();
	private int selectedServerIndex = -1;

	void Awake()
	{
		IP_Input.text = PlayerPrefs.GetString("ServerIP", "");
		serverItems.Add(templateServerItem);

		serverList = Resources.Load<ServerList>("ScriptableObjects/ServerList");
		if (serverList != null)
		{
			for (int i = 0; i < serverList.servers.Count; i++)
			{
				if (i > 0)
				{
					var newItem = Instantiate(templateServerItem, templateServerItem.transform.parent);
					serverItems.Add(newItem);
				}

				var item = serverItems[i];
				item.SetServerIndex(i);
				item.SetServerName(serverList.servers[i].name);
				item.gameObject.SetActive(true);
			}

			// �w�]���Ĥ@�Ӧ��A��
			if (serverList.servers.Count > 0)
			{
				selectedServerIndex = 0;
				serverItems[0].Select();
			}
		}
		else
		{
			Debug.LogWarning("ServerList not found!");
		}
	}

	void Update()
	{
		// �S�� OnFocus event �u�n�ۤv����
		if (selectedServerIndex >= 0 && IP_Input.isFocused)
		{
			selectedServerIndex = -1;
			customServerItem.Select();
		}
	}

	public void Show(bool isShow)
	{
		if (gameObject.activeSelf != isShow)
			gameObject.SetActive(isShow);
	}

	public void SelectServer(int index)
	{
		selectedServerIndex = index;
	}

	public void ClickConnect()
	{
		// ��ܦC�������A��
		if (selectedServerIndex >= 0)
		{
			var server = serverList.servers[selectedServerIndex];
			ConnectPage.Instance.ConnectToServer(server.ip, server.port);
			return;
		}

		string[] param = IP_Input.text.Split(':');
		if (param.Length != 2)
		{
			ConnectPage.Instance.SetConnectMessage("�п�J���T�� IP:PORT �榡");
			return;
		}

		try
		{
			string ip = param[0];
			int port = Int32.Parse(param[1]);
			ConnectPage.Instance.ConnectToServer(ip, port);
		}
		catch
		{
			ConnectPage.Instance.SetConnectMessage("�п�J���T�� IP:PORT �榡");
			return;
		}

		PlayerPrefs.SetString("ServerIP", IP_Input.text);
	}
}
