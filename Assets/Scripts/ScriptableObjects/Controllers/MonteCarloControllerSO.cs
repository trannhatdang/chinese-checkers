using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerControllers/MCTSControllerSO")]
public class MCTSControllerSO : PlayerControllerSO
{
	[SerializeField] int m_simulationsPerMove = 100;
	[SerializeField] int m_maxRolloutDepth = 20;

	public async override void BeginTurn(Board board, int color)
	{
		Node bestNode = null;
		List<Node> bestPath = null;
		float bestWinRate = -1f;

		List<Node> myNodes = board.GetNodesOfColor(color);

		// 1. SELECTION & EXPANSION
		// For every possible move I can make right now...
		foreach (Node node in myNodes)
		{
			List<List<Node>> possiblePaths = board.PossibleMoves(node);
			foreach (List<Node> path in possiblePaths)
			{
				int wins = 0;
				Node destination = path[path.Count - 1];

				// Perform initial move
				int startColor = node.IsOfPlayer;
				node.IsOfPlayer = 0;
				destination.IsOfPlayer = startColor;

				// 2. SIMULATION (Rollouts)
				for (int i = 0; i < m_simulationsPerMove; i++)
				{
					if (RunRandomRollout(board, color)) wins++;
				}

				// Backtrack initial move
				destination.IsOfPlayer = 0;
				node.IsOfPlayer = startColor;

				float winRate = (float)wins / m_simulationsPerMove;
				if (winRate > bestWinRate && path.Count > 1)
				{
					bestWinRate = winRate;
					bestNode = node;
					bestPath = path;
				}
			}
		}

		// 3. EXECUTION
		if (bestNode != null && bestPath != null)
		{
			await board.ChangePosition(bestNode, bestPath);
		}
	}

	private bool RunRandomRollout(Board board, int aiColor)
	{
		List<(Node start, Node end, int color)> history = new();
		bool aiWon = false;

		// Perform random moves for a set depth
		for (int d = 0; d < m_maxRolloutDepth; d++)
		{
			int turnColor = (aiColor + d) % 6; // Simple turn rotation simulation
			if (turnColor == 0) turnColor = 6;

			var move = GetRandomMove(board, turnColor);
			if (move.start == null) break;

			// Apply Move
			history.Add((move.start, move.end, move.start.IsOfPlayer));
			move.end.IsOfPlayer = move.start.IsOfPlayer;
			move.start.IsOfPlayer = 0;
		}

		// Evaluate board state at end of rollout
		aiWon = EvaluateWinner(board, aiColor);

		// BACKTRACK: Reverse all random moves to restore the board state
		for (int i = history.Count - 1; i >= 0; i--)
		{
			history[i].start.IsOfPlayer = history[i].color;
			history[i].end.IsOfPlayer = 0;
		}

		return aiWon;
	}

	private (Node start, Node end) GetRandomMove(Board board, int color)
	{
		var nodes = board.GetNodesOfColor(color);
		if (nodes.Count == 0) return (null, null);

		// Pick a random piece that has valid moves
		var shuffledNodes = nodes.OrderBy(x => Random.value).ToList();
		foreach (var node in shuffledNodes)
		{
			var moves = board.PossibleMoves(node);
			if (moves.Count > 1) // Count > 1 because index 0 is usually the node itself
			{
				var randomPath = moves[Random.Range(1, moves.Count)];
				return (node, randomPath[randomPath.Count - 1]);
			}
		}

		return (null, null);
	}

	private bool EvaluateWinner(Board board, int color)
	{
		return board.GetScore(color) > 50;
	}
}
