using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	[SerializeField] UIManager m_UIManager;
	[SerializeField] Board m_board;
	[SerializeField] int m_mainPlayerColor = -1;
	[SerializeField] int m_mainPlayer = -1;

	[SerializeField] int m_currPlayer = 0;
	[SerializeField] int m_totalTurns = 0;
	[SerializeField] int m_maxTurns = 10;

	[SerializeField] List<Player> m_playerList;

	[SerializeField] PlayerControllerSO m_humanController;
	[SerializeField] PlayerControllerSO m_MCTSController;
	[SerializeField] PlayerControllerSO m_miniMaxController;
	[SerializeField] PlayerControllerSO m_machineLearningAIController;
	[SerializeField] PlayerControllerSO m_machineLearningAIController2;
	[SerializeField] PlayerControllerSO m_machineLearningAIController3;
	[SerializeField] PlayerControllerSO m_machineLearningAIController4;
	[SerializeField] PlayerControllerSO m_machineLearningAIController5;
	[SerializeField] PlayerControllerSO m_machineLearningAIController6;

	public bool IsHumanTurn
	{
		get { return m_playerList[m_currPlayer].PlayerController is HumanControllerSO; }
	}

	private Action m_endGameCallback = null;

	public int TotalTurns
	{
		get { return m_totalTurns; }
	}

	public int MainPlayerColor
	{
		get { return m_mainPlayerColor; }
	}

	public int CurrPlayer
	{
		get { return m_currPlayer; }
	}

	public int MainPlayer
	{
		get { return m_mainPlayer; }
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

	public void StartGame(Action endGameCallback = null)
	{
		m_playerList[m_currPlayer].BeginTurn();
		m_endGameCallback = endGameCallback;
		m_UIManager.InPlay();
	}

	public void StartGame()
	{
		m_playerList[m_currPlayer].BeginTurn();
		m_endGameCallback = null;
		m_UIManager.InPlay();
	}

	public void ResetGame()
	{
		m_board.Reset();
	}

	public void FindWinner()
	{
		int maxind = 0;

		for (int i = 1; i < m_playerList.Count; ++i)
		{
			int currScore = m_board.GetScore(m_playerList[i].Color);
			int maxScore = m_board.GetScore(m_playerList[maxind].Color);


			if (currScore > maxScore)
			{
				maxind = i;
			}
		}

		DeclareWinner(m_playerList[maxind].Color, m_playerList[maxind].Name);
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

		m_currPlayer = (m_currPlayer + 1) % 6;
		m_totalTurns++;

		if (m_totalTurns >= m_maxTurns)
		{
			m_totalTurns = 0;
			FindWinner();
			return;
		}

		if (m_totalTurns >= m_maxTurns && m_endGameCallback != null)
		{
			m_totalTurns = 0;
			m_endGameCallback();
			ResetGame();
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

	public void Reload()
	{
		SceneManager.LoadSceneAsync(0);
	}

	public void SetAI(int index, PlayerControllerSO controller)
	{
		m_playerList[index].SetAI(controller);
	}

	public void SetPlayerAI(int index, int controller)
	{
		switch (controller)
		{
			case 1:
				SetAI(index, m_MCTSController);
				break;
			case 2:
				SetAI(index, m_miniMaxController);
				break;
			case 3:
				SetAI(index, m_machineLearningAIController);
				break;
			case 4:
				SetAI(index, m_machineLearningAIController2);
				break;
			case 5:
				SetAI(index, m_machineLearningAIController3);
				break;
			case 6:
				SetAI(index, m_machineLearningAIController4);
				break;
			case 7:
				SetAI(index, m_machineLearningAIController5);
				break;
			case 8:
				SetAI(index, m_machineLearningAIController6);
				break;
			default:
				SetAI(index, m_humanController);
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

	public void SetMaxTurns(string numTurns)
	{
		m_maxTurns = Int32.Parse(numTurns);
	}

	// void OnGUI()
	// {
	// 	if (GUI.Button(new Rect(10, 10, 150, 100), "Begin"))
	// 	{
	// 		m_playerList[m_currPlayer].BeginTurn();
	// 	}
	//
	// 	if (GUI.Button(new Rect(160, 10, 150, 100), "Reset"))
	// 	{
	// 		ResetGame();
	// 	}
	// }
}
