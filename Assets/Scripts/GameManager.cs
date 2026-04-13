using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	[SerializeField] List<Player> m_playerList;
	[SerializeField] UIManager m_UIManager;
	[SerializeField] Board m_board;
	[SerializeField] TurnManager m_turnManager;
	[SerializeField] PlayerControllerListSO m_playerControllerList;
	[SerializeField] Config m_config;
	[SerializeField] int m_currPlayer = 0;
	[SerializeField] int m_totalTurns = 0;

	private Action m_endGameCallback = null;

	public bool IsHumanTurn
	{
		get { return m_playerList[m_currPlayer].PlayerController is HumanControllerSO; }
	}

	public int TotalTurns
	{
		get { return m_totalTurns; }
	}

	public int CurrPlayer
	{
		get { return m_currPlayer; }
	}

	void Start()
	{
		for (int i = 0; i < 6; ++i)
		{
			m_config.SetPlayerAI(i, 0);
		}

		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void endGameWithCallback()
	{
		m_totalTurns = 0;
		ResetGame();
		m_endGameCallback();
	}

	// GAME/TURN RELATED
	public void StartGame(Action endGameCallback = null)
	{
		m_playerList[m_currPlayer].BeginTurn();
		m_endGameCallback = endGameCallback;
		m_UIManager.InPlay();
	}

	public void ResetGame()
	{
		m_board.Reset();
	}

	public void NextTurn()
	{
		if (m_board.CheckWin(m_playerList[m_currPlayer].Color))
		{
			if (m_endGameCallback != null)
			{
				endGameWithCallback();
				return;
			}

			DeclareWinner(m_playerList[m_currPlayer].Color, m_playerList[m_currPlayer].Name);
			return;
		}

		if (m_totalTurns >= m_config.MaxTotalTurns && m_endGameCallback != null)
		{
			endGameWithCallback();
			return;
		}

		m_currPlayer = (m_currPlayer + 1) % m_config.NumPlayers;
		m_totalTurns++;

		m_playerList[m_currPlayer].BeginTurn();
	}

	void DeclareWinner(int color, string name)
	{
		m_UIManager.Winner(color, name);
	}

	public void Pause()
	{
		Time.timeScale = 0.0f;

		m_UIManager.Pause();
	}

	public void UnPause()
	{
		Time.timeScale = 1.0f;

		m_UIManager.UnPause();
	}

	// CONFIG RELATED

	public Config GetConfig()
	{
		return m_config;
	}

	public TextAsset GetCurrBoard()
	{
		return m_config.GetCurrBoard();
	}

	public List<TextAsset> GetWinBoardList()
	{
		return m_config.GetWinBoardList();
	}
}
