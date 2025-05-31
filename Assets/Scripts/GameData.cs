using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

public class UserData
{
	public ushort UID { get; set; }
	public string Name { get; set; }
}

public class PlayerData
{
	public ushort UID { get; set; }
	public string Question { get; set; }
	public int SuccessRound { get; set; }
	public List<Tuple<string, byte>> GuessHistory { get; set; } = new List<Tuple<string, byte>>();

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

	public Dictionary<ushort, UserData> UserDatas { get; set; } = new Dictionary<ushort, UserData>();
	public Dictionary<ushort, PlayerData> PlayerDatas { get; set; } = new Dictionary<ushort, PlayerData>();
	public GameState CurrentState { get; set; } = GameState.WAITING;
	public List<ushort> PlayerOrder { get; set; } = new List<ushort>();
	public int GuessingPlayerIndex { get; set; } = 0;
	public string VotingGuess { get; set; } = "";
	public Dictionary<ushort, byte> Votes { get; set; } = new Dictionary<ushort, byte>();

	public void ResetGame()
	{
		CurrentState = GameState.WAITING;
		PlayerOrder.Clear();
		GuessingPlayerIndex = 0;
		VotingGuess = "";
		Votes.Clear();
		foreach (var player in PlayerDatas.Values)
			player.Reset();
	}
}
