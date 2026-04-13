using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Board : MonoBehaviour
{
	[SerializeField] List<List<Node>> m_nodeList;
	[SerializeField] Node m_node;
	[SerializeField] GameManager m_gameManager;

	void Start()
	{
		m_nodeList = new List<List<Node>>();
		createBoard();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void createBoard()
	{
		List<List<int>> data = Utils.ReadCSV(m_gameManager.GetCurrBoard());

		int m = data.Count;
		for (int i = 0; i < m; ++i)
		{
			int n = data[i].Count;
			List<Node> m_nodeRow = new List<Node>();
			for (int j = 0; j < n; ++j)
			{
				int y = m - i;

				if (data[i][j] < 0)
				{
					m_nodeRow.Add(null);
				}
				else
				{
					m_nodeRow.Add(Instantiate(m_node, new Vector3(j - 9, y - 8, 0), Quaternion.identity, transform));

					m_nodeRow[j].IsOfPlayer = data[i][j];
					m_nodeRow[j].X = j;
					m_nodeRow[j].Y = i;
					m_nodeRow[j].Position = new Vector3(j - 9, y - 8, 0);
					m_nodeRow[j].GameBoard = this;
					m_nodeRow[j].GameMan = m_gameManager;
				}
			}

			m_nodeList.Add(m_nodeRow);
		}
	}

	public List<Node> GetNodesOfColor(int color)
	{
		List<Node> ret = new List<Node>();
		for (int i = 0; i < m_nodeList.Count; ++i)
		{
			for (int j = 0; j < m_nodeList[i].Count; ++j)
			{
				if (m_nodeList[i][j] == null) continue;

				if (m_nodeList[i][j].IsOfPlayer == color)
				{
					ret.Add(m_nodeList[i][j]);
				}
			}
		}

		return ret;
	}

	public void HighlightPossibleMove(Node node)
	{
		List<List<Node>> possibleMoves = PossibleMoves(node);
		int m = possibleMoves.Count;

		for (int i = 0; i < m; ++i)
		{
			int n = possibleMoves[i].Count;
			for (int j = 0; j < n; ++j)
			{
				if (possibleMoves[i][j] == node) continue;

				if (j == n - 1)
				{
					possibleMoves[i][j].Highlight();
				}
			}
		}
	}

	public void ResetHighlight()
	{
		int m = m_nodeList.Count;
		for (int i = 0; i < m; ++i)
		{
			int n = m_nodeList[i].Count;
			for (int j = 0; j < n; ++j)
			{
				if (m_nodeList[i][j] == null) continue;

				m_nodeList[i][j].ResetHighlight();
			}
		}
	}

	public async UniTask ChangePosition(Node node, List<Node> move)
	{
		for (int i = 1; i < move.Count; ++i)
		{
			await node.transform.DOMove(move[i].transform.position, 0.5f).AsyncWaitForCompletion();
		}

		node.transform.DOMove(node.Position, 0f);

		move[move.Count - 1].IsOfPlayer = node.IsOfPlayer;
		node.IsOfPlayer = 0;
	}

	public List<List<Node>> PossibleMoves(Node node)
	{
		List<List<Node>> ret = new List<List<Node>>();
		List<Node> initialNode = new();
		initialNode.Add(node);
		ret.Add(initialNode);

		exploreMoves(ret, node, node.IsOfPlayer);

		return ret;
	}

	void exploreMoves(List<List<Node>> allMoves, Node initialNode, int color, int numIter = 0)
	{
		bool existNewNode = false;
		if (numIter > 20) return;

		for (int i = 0; i < allMoves.Count; ++i)
		{
			bool prevPathObsolete = false;

			List<Node> prevPath = allMoves[i];
			List<Node> prevPathLeft = new List<Node>(prevPath);
			List<Node> prevPathRight = new List<Node>(prevPath);
			List<Node> prevPathUpLeft = new List<Node>(prevPath);
			List<Node> prevPathUpRight = new List<Node>(prevPath);
			List<Node> prevPathDownLeft = new List<Node>(prevPath);
			List<Node> prevPathDownRight = new List<Node>(prevPath);

			Node lastNode = prevPath[prevPath.Count - 1];
			int x = lastNode.X;
			int y = lastNode.Y;

			Node left = exploreNode(prevPathLeft, prevPathLeft[prevPathLeft.Count - 1], x - 2, y, color, prevPathLeft.Count == 1);
			if (left != null)
			{
				prevPathObsolete = true;
				existNewNode = true;

				// Debug.Log("left");
				prevPathLeft.Add(left);
				allMoves.Add(prevPathLeft);
			}

			Node right = exploreNode(prevPathRight, prevPathRight[prevPathRight.Count - 1], x + 2, y, color, prevPathRight.Count == 1);
			if (right != null)
			{
				prevPathObsolete = true;
				existNewNode = true;

				// Debug.Log("right");
				prevPathRight.Add(right);
				allMoves.Add(prevPathRight);
			}

			Node upleft = exploreNode(prevPathUpLeft, prevPathUpLeft[prevPathUpLeft.Count - 1], x - 1, y - 1, color, prevPathUpLeft.Count == 1);
			if (upleft != null)
			{
				prevPathObsolete = true;
				existNewNode = true;

				// Debug.Log("upleft");
				prevPathUpLeft.Add(upleft);
				allMoves.Add(prevPathUpLeft);
			}

			Node upright = exploreNode(prevPathUpRight, prevPathUpRight[prevPathUpRight.Count - 1], x + 1, y - 1, color, prevPathUpRight.Count == 1);
			if (upright != null)
			{
				prevPathObsolete = true;
				existNewNode = true;

				// Debug.Log($"prevPathUpRight: {prevPathUpRight.Count}");
				// Debug.Log($"upright: {x + 1} {y - 1}");
				prevPathUpRight.Add(upright);
				allMoves.Add(prevPathUpRight);
			}

			Node downleft = exploreNode(prevPathDownLeft, prevPathDownLeft[prevPathDownLeft.Count - 1], x - 1, y + 1, color, prevPathDownLeft.Count == 1);
			if (downleft != null)
			{
				prevPathObsolete = true;
				existNewNode = true;

				// Debug.Log("downleft");
				prevPathDownLeft.Add(downleft);
				allMoves.Add(prevPathDownLeft);
			}

			Node downright = exploreNode(prevPathDownRight, prevPathDownRight[prevPathDownRight.Count - 1], x + 1, y + 1, color, prevPathDownRight.Count == 1);
			if (downright != null)
			{
				prevPathObsolete = true;
				existNewNode = true;

				// Debug.Log("downright");
				prevPathDownRight.Add(downright);
				allMoves.Add(prevPathDownRight);
			}

			if (prevPathObsolete)
			{
				allMoves.Remove(prevPath);
				--i;
			}
		}

		if (existNewNode)
		{
			exploreMoves(allMoves, initialNode, color, ++numIter);
		}
	}

	Node exploreNode(List<Node> path, Node lastNode, int x, int y, int color, bool isInitial)
	{
		if (x < 0 || y < 0 || y >= m_nodeList.Count || x >= m_nodeList[y].Count)
		{
			return null;
		}

		if (m_nodeList[y][x] == null)
		{
			return null;
		}

		Node nextNode = m_nodeList[y][x];
		if (path.Contains(nextNode))
		{
			return null;
		}

		if (nextNode.IsOfPlayer == 0 && isInitial)
		{
			return nextNode;
		}
		else if (nextNode.IsOfPlayer == color)
		{
			int lastX = lastNode.X;
			int lastY = lastNode.Y;
			int newY = lastY + (2 * (y - lastY));
			int newX = lastX + (2 * (x - lastX));

			if (newX < 0 || newY < 0 || newY >= m_nodeList.Count || newX >= m_nodeList[newY].Count)
			{
				return null;
			}

			if (m_nodeList[newY][newX] == null)
			{
				return null;
			}

			if (m_nodeList[newY][newX].IsOfPlayer != 0)
			{
				return null;
			}

			if (path.Contains(m_nodeList[newY][newX]))
			{
				return null;
			}

			return m_nodeList[newY][newX];
		}

		return null;
	}

	public void Reset()
	{
		List<List<int>> data = Utils.ReadCSV(m_gameManager.GetCurrBoard());

		int m = data.Count;
		for (int i = 0; i < m; ++i)
		{
			int n = data[i].Count;
			for (int j = 0; j < n; ++j)
			{
				int y = m - i;

				if (data[i][j] >= 0)
				{
					m_nodeList[i][j].IsOfPlayer = data[i][j];
				}
			}
		}
	}

	public int GetScore(int color)
	{
		List<List<int>> winBoard = Utils.ReadCSV(m_gameManager.GetWinBoardList()[color - 1]);

		List<Node> nodeList = GetNodesOfColor(color);
		int score = 0;

		for (int i = 0; i < nodeList.Count; ++i)
		{
			score += winBoard[nodeList[i].Y][nodeList[i].X];
		}

		return score;
	}

	public bool CheckWin(int color)
	{
		// Debug.Log($"{color}, {score}");

		return GetScore(color) >= 50;
	}
}
