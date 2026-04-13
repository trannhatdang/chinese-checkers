using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/ConstantsSO")]
public class ConstantsSO : ScriptableObject
{
	[Header("TwoPlayer")]
	[SerializeField] TextAsset m_twoPlayerBoard;
	[SerializeField] List<TextAsset> m_twoPlayerWinBoardList;

	[Header("FourPlayer")]
	[SerializeField] TextAsset m_fourPlayerBoard;
	[SerializeField] List<TextAsset> m_fourPlayerWinBoardList;

	[Header("SixPlayer")]
	[SerializeField] TextAsset m_sixPlayerBoard;
	[SerializeField] List<TextAsset> m_sixPlayerWinBoardList;

	[Header("AI")]
	[SerializeField] PlayerControllerListSO m_playerControllerList;

	public TextAsset TwoPlayerBoard
	{
		get { return m_sixPlayerBoard; }
	}

	public List<TextAsset> TwoPlayerWinBoardList
	{
		get { return m_sixPlayerWinBoardList; }
	}

	public TextAsset FourPlayerBoard
	{
		get { return m_sixPlayerBoard; }
	}

	public List<TextAsset> FourPlayerWinBoardList
	{
		get { return m_sixPlayerWinBoardList; }
	}

	public TextAsset SixPlayerBoard
	{
		get { return m_sixPlayerBoard; }
	}

	public List<TextAsset> SixPlayerWinBoardList
	{
		get { return m_sixPlayerWinBoardList; }
	}

	public PlayerControllerListSO PlayerControllerList
	{
		get { return m_playerControllerList; }
	}
}
