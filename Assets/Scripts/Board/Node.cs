using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Node : MonoBehaviour
{
	[SerializeField] GameManager m_gameManager;
	[SerializeField] SpriteRenderer m_spr;
	[SerializeField] Board m_gameBoard;
	[SerializeField] int m_isOfPlayer = 0;
	[SerializeField] int m_x = 0;
	[SerializeField] int m_y = 0;

	[SerializeField] InputAction m_mouse;

	public Vector3 Position;

	public GameManager GameMan
	{
		get { return m_gameManager; }
		set { m_gameManager = value; }
	}

	public Board GameBoard
	{
		get { return m_gameBoard; }
		set { m_gameBoard = value; }
	}

	public int IsOfPlayer
	{
		get { return m_isOfPlayer; }
		set
		{
			m_isOfPlayer = value;

			changeColor();
		}
	}

	public int X
	{
		get { return m_x; }
		set { m_x = value; }
	}

	public int Y
	{
		get { return m_y; }
		set { m_y = value; }
	}

	void Awake()
	{
		m_spr = GetComponent<SpriteRenderer>();
		m_mouse = InputSystem.actions.FindAction("Point");
	}

	void Start()
	{

	}

	void changeColor()
	{
		switch (m_isOfPlayer)
		{
			case 1:
				m_spr.color = Color.blue;
				break;
			case 2:
				m_spr.color = Color.red;
				break;
			case 3:
				m_spr.color = Color.green;
				break;
			case 4:
				m_spr.color = Color.yellow;
				break;
			case 5:
				m_spr.color = Color.pink;
				break;
			case 6:
				m_spr.color = Color.purple;
				break;
			default:
				m_spr.color = Color.white;
				break;

		}
	}

	void OnMouseDown()
	{
		if (!m_gameManager.IsHumanTurn)
		{
			return;
		}

		if (m_gameManager.CurrPlayer != m_isOfPlayer - 1)
		{
			return;
		}

		m_gameBoard.HighlightPossibleMove(this);
	}

	void OnMouseDrag()
	{
		if (!m_gameManager.IsHumanTurn)
		{
			return;
		}

		if (m_gameManager.CurrPlayer != m_isOfPlayer - 1)
		{
			return;
		}

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(m_mouse.ReadValue<Vector2>());
		transform.position = mousePos;
	}

	async void OnMouseUp()
	{
		if (!m_gameManager.IsHumanTurn)
		{
			return;
		}

		if (m_gameManager.CurrPlayer != m_isOfPlayer - 1)
		{
			return;
		}

		List<List<Node>> possibleMoves = m_gameBoard.PossibleMoves(this);
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(m_mouse.ReadValue<Vector2>());

		for (int i = 0; i < possibleMoves.Count; ++i)
		{
			List<Node> move = possibleMoves[i];

			if ((mousePos - (Vector2)move[move.Count - 1].transform.position).magnitude < 1)
			{
				await m_gameBoard.ChangePosition(this, move);
				await m_gameBoard.ResetHighlight();
				m_gameManager.NextTurn();
				return;
			}
		}

		transform.position = Position;
		m_gameBoard.ResetHighlight();
	}

	public void Highlight()
	{
		m_spr.color = Color.black;
		m_spr.DOFade(0.8f, 0.0f);
	}

	public void ResetHighlight()
	{
		m_spr.DOFade(1.0f, 0.0f);
		changeColor();
	}
}
