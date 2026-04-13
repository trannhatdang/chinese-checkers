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
	[SerializeField] GameMode m_gameMode;
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
			SetPlayerAI(i, 0);
		}

		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update()
	{

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
				m_totalTurns = 0;
				ResetGame();
				m_endGameCallback();
				return;
			}

			DeclareWinner(m_playerList[m_currPlayer].Color, m_playerList[m_currPlayer].Name);
			return;
		}

		m_currPlayer = (m_currPlayer + 1) % m_playerList.Count;
		m_totalTurns++;

		if (m_totalTurns >= 60 && m_endGameCallback != null)
		{
			m_totalTurns = 0;
			ResetGame();
			m_endGameCallback();
			return;
		}

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

	public void SetAI(int index, PlayerControllerSO controller)
	{
		m_playerList[index].SetAI(controller);
	}

	// AI RELATED
	public void SetPlayerAI(int index, int controller)
	{
		if (m_gameMode == GameMode.TwoPlayer)
		{
			SetPlayerAITwoPlayer(index, controller);
		}
		else
		{
			SetPlayerAISixPlayer(index, controller);
		}
	}

	public void SetPlayerAITwoPlayer(int index, int controller)
	{
		switch (controller)
		{
			case 1:
				SetAI(index, m_playerControllerList.MCTSController);
				break;
			case 2:
				SetAI(index, m_playerControllerList.MiniMaxController);
				break;
			case 3:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[0]);
				break;
			case 4:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[1]);
				break;
			case 5:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[2]);
				break;
			case 6:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[3]);
				break;
			case 7:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[4]);
				break;
			case 8:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[5]);
				break;
			default:
				SetAI(index, m_playerControllerList.HumanController);
				break;
		}

	}

	public void SetPlayerAISixPlayer(int index, int controller)
	{
		switch (controller)
		{
			case 1:
				SetAI(index, m_playerControllerList.MCTSController);
				break;
			case 2:
				SetAI(index, m_playerControllerList.MaxiMaxController);
				break;
			case 3:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[0]);
				break;
			case 4:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[1]);
				break;
			case 5:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[2]);
				break;
			case 6:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[3]);
				break;
			case 7:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[4]);
				break;
			case 8:
				SetAI(index, m_playerControllerList.MachineLearningAIControllerList[5]);
				break;
			default:
				SetAI(index, m_playerControllerList.HumanController);
				break;
		}
	}

	public void SetPlayerOneAI(int controller)
	{
		SetPlayerAI(0, controller);
	}

	public void SetPlayerTwoAI(int controller)
	{
		SetPlayerAI(1, controller);
	}

	public void SetPlayerThreeAI(int controller)
	{
		SetPlayerAI(2, controller);
	}

	public void SetPlayerFourAI(int controller)
	{
		SetPlayerAI(3, controller);
	}

	public void SetPlayerFiveAI(int controller)
	{
		SetPlayerAI(4, controller);
	}

	public void SetPlayerSixAI(int controller)
	{
		SetPlayerAI(5, controller);
	}

	// CONFIG RELATED
}
