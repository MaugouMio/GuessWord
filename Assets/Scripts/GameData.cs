using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

public class UserData
{
	public ushort UID { get; set; } = 0;
	public string Name { get; set; } = "";
}

public class PlayerData
{
	public ushort UID { get; set; } = 0;
	public string Question { get; set; } = "";
	public ushort SuccessRound { get; set; } = 0;
	public List<string> GuessHistory { get; set; } = new List<string>();

	public void Reset()
	{
		Question = "";
		SuccessRound = 0;
		GuessHistory.Clear();
	}
}

public enum GameState
{
	WAITING,    // �i�H�[�J�C�������q
	PREPARING,  // �C����}�l���X�D���q
	GUESSING,   // �Y�Ӫ��a�q�D��
	VOTING,     // �Y�Ӫ��a�q���@�����O�A���ݨ�L�H�벼�O�_�ŦX
}

public class GameData
{
	public const int MAX_EVENT_RECORD = 30;

	private static GameData instance;
	public static GameData Instance
	{
		get
		{
			if (instance == null)
				instance = new GameData();
			return instance;
		}
	}

	public ushort SelfUID { get; set; } = 0;
	public Dictionary<ushort, UserData> UserDatas { get; set; } = new Dictionary<ushort, UserData>();
	public Dictionary<ushort, PlayerData> PlayerDatas { get; set; } = new Dictionary<ushort, PlayerData>();
	public bool IsCountingDownStart { get; set; } = false;
	public GameState CurrentState { get; set; } = GameState.WAITING;
	public List<ushort> PlayerOrder { get; set; } = new List<ushort>();
	public byte GuessingPlayerIndex { get; set; } = 0;
	public string VotingGuess { get; set; } = "";
	public Dictionary<ushort, byte> Votes { get; set; } = new Dictionary<ushort, byte>();
	public Queue<string> EventRecord { get; set; } = new Queue<string>();

	public void Reset()
	{
		SelfUID = 0;
		UserDatas.Clear();
		PlayerDatas.Clear();
		ResetGame();
	}
	public void ResetGame()
	{
		IsCountingDownStart = false;
		CurrentState = GameState.WAITING;
		PlayerOrder.Clear();
		GuessingPlayerIndex = 0;
		VotingGuess = "";
		Votes.Clear();
		foreach (var player in PlayerDatas.Values)
			player.Reset();
	}

	public void AddEventRecord(string eventText)
	{
		if (EventRecord.Count >= MAX_EVENT_RECORD)
			EventRecord.Dequeue();
		EventRecord.Enqueue(eventText);

		if (GamePage.Instance != null)
			GamePage.Instance.UpdateEventList();
	}
}
