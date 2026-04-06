using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] PlayerControllerSO m_playerControllerSO;
	[SerializeField] GameManager m_gameManager;
	[SerializeField] Board m_board;
	[SerializeField] int m_color;

	public int Color
	{
		get { return m_color; }
	}

	public string Name
	{
		get { return m_playerControllerSO.Name; }
	}

	public async void BeginTurn()
	{
		m_playerControllerSO.BeginTurn(m_board, m_gameManager, m_color);
	}

	public void SetAI(PlayerControllerSO controller)
	{
		m_playerControllerSO = controller;
	}
}
