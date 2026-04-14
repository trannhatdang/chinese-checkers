using UnityEngine;
using System.Collections.Generic;

public class Config : MonoBehaviour
{
	[SerializeField] List<Player> m_playerList;
	[SerializeField] GameMode m_currGameMode;
	[SerializeField] ConstantsSO m_constants;
	[SerializeField] int m_maxTotalTurns;

	public GameMode CurrGameMode
	{
		get { return m_currGameMode; }
	}

	public ConstantsSO Constants
	{
		get { return m_constants; }
	}

	public TextAsset GetCurrBoard()
	{
		switch (m_currGameMode)
		{
			case GameMode.FourPlayer:
				return m_constants.FourPlayerBoard;
			case GameMode.SixPlayer:
				return m_constants.SixPlayerBoard;
			default:
				return m_constants.TwoPlayerBoard;
		}
	}

	public List<TextAsset> GetWinBoardList()
	{
		switch (m_currGameMode)
		{
			case GameMode.FourPlayer:
				return m_constants.FourPlayerWinBoardList;
			case GameMode.SixPlayer:
				return m_constants.SixPlayerWinBoardList;
			default:
				return m_constants.TwoPlayerWinBoardList;
		}
	}

	public int NumPlayers
	{
		get
		{
			switch (m_currGameMode)
			{
				case GameMode.FourPlayer:
					return 4;
				case GameMode.SixPlayer:
					return 6;
				default:
					return 2;
			}
		}
	}

	public int MaxTotalTurns
	{
		get { return m_maxTotalTurns; }
	}

	public void SetAI(int index, PlayerControllerSO controller)
	{
		m_playerList[index].SetAI(controller);
	}

	public void SetAI(int index, int controller)
	{
		if (index < 0)
		{
			return;
		}

		if (m_currGameMode == GameMode.TwoPlayer && index > 2
			|| m_currGameMode == GameMode.FourPlayer && index > 4
			|| m_currGameMode == GameMode.SixPlayer && index > 6)
		{
			return;
		}

		switch (controller)
		{
			case 1:
				SetAI(index, m_constants.PlayerControllerList.MCTSController);
				break;
			case 2:
				if (m_currGameMode == GameMode.TwoPlayer)
				{
					SetAI(index, m_constants.PlayerControllerList.MiniMaxController);
				}
				else
				{
					SetAI(index, m_constants.PlayerControllerList.MaxiMaxController);
				}
				break;
			case 3:
				SetAI(index, m_constants.PlayerControllerList.MachineLearningAIControllerList[0]);
				break;
			case 4:
				SetAI(index, m_constants.PlayerControllerList.MachineLearningAIControllerList[1]);
				break;
			case 5:
				SetAI(index, m_constants.PlayerControllerList.MachineLearningAIControllerList[2]);
				break;
			case 6:
				SetAI(index, m_constants.PlayerControllerList.MachineLearningAIControllerList[3]);
				break;
			case 7:
				SetAI(index, m_constants.PlayerControllerList.MachineLearningAIControllerList[4]);
				break;
			case 8:
				SetAI(index, m_constants.PlayerControllerList.MachineLearningAIControllerList[5]);
				break;
			default:
				SetAI(index, m_constants.PlayerControllerList.HumanController);
				break;
		}
	}

	public void SetPlayerOneAI(int controller)
	{
		SetAI(0, controller);
	}

	public void SetPlayerTwoAI(int controller)
	{
		SetAI(1, controller);
	}

	public void SetPlayerThreeAI(int controller)
	{
		SetAI(2, controller);
	}

	public void SetPlayerFourAI(int controller)
	{
		SetAI(3, controller);
	}

	public void SetPlayerFiveAI(int controller)
	{
		SetAI(4, controller);
	}

	public void SetPlayerSixAI(int controller)
	{
		SetAI(5, controller);
	}

	public void Copy(Config conf)
	{
		m_currGameMode = conf.m_currGameMode;
	}
}
