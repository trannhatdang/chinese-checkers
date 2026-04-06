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

	[SerializeField] List<Player> m_playerList;

	private Action m_endGameCallback;

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

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void StartGame(Action endGameCallback = null)
	{
		m_playerList[m_currPlayer].BeginTurn();
		m_endGameCallback = endGameCallback;
	}

	public void ResetGame()
	{
		m_board.Reset();
	}

	public void NextTurn()
	{
		if (m_board.CheckWin(m_playerList[m_currPlayer].Color))
		{
			DeclareWinner(m_playerList[m_currPlayer].Color, m_playerList[m_currPlayer].Name);
			return;
		}

		m_currPlayer = (m_currPlayer + 1) % 6;
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

	public void Reload()
	{
		SceneManager.LoadSceneAsync(0);
	}

	public void SetAI(int index, PlayerControllerSO controller)
	{
		m_playerList[index].SetAI(controller);
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(10, 10, 150, 100), "Begin"))
		{
			m_playerList[m_currPlayer].BeginTurn();
		}

		if (GUI.Button(new Rect(160, 10, 150, 100), "Reset"))
		{
			ResetGame();
		}
	}
}
