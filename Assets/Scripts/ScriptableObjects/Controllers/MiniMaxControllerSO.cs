using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerControllers/MinimaxControllerSO")]
public class MinimaxControllerSO : PlayerControllerSO
{
	[SerializeField] private int m_maxDepth = 5; // Keep this low for performance

	public async override UniTask BeginTurn(Board board, int color)
	{
		Node bestNode = null;
		List<Node> bestMove = null;
		float bestScore = float.NegativeInfinity;

		List<Node> myNodes = board.GetNodesOfColor(color);
		float alpha = float.NegativeInfinity;
		float beta = float.PositiveInfinity;

		// 1. Evaluate all possible moves for all my pieces
		foreach (Node node in myNodes)
		{
			List<List<Node>> possiblePaths = board.PossibleMoves(node);
			foreach (List<Node> path in possiblePaths)
			{
				// Simulate move (on the real board temporarily, then undo)
				Node destination = path[path.Count - 1];
				int originalColor = node.IsOfPlayer;

				// Perform "Virtual" Move
				node.IsOfPlayer = 0;
				destination.IsOfPlayer = originalColor;

				// 2. Recurse using Minimax
				float score = findMin(board, (color + 1) % 2, m_maxDepth - 1, alpha, beta);

				// 3. Undo Move
				destination.IsOfPlayer = 0;
				node.IsOfPlayer = originalColor;

				if (score > bestScore && path.Count > 1)
				{
					bestScore = score;
					bestNode = node;
					bestMove = path;
				}

				if (score > alpha)
				{
					alpha = score;
				}
			}
		}

		// 4. Execute the best move found
		if (bestNode != null && bestMove != null)
		{
			await board.ChangePosition(bestNode, bestMove);
		}
	}

	private float findMax(Board board, int color, int depth, float alpha, float beta)
	{
		// Base case: leaf node or max depth reached
		if (depth <= 0)
		{
			return EvaluateBoard(board, color);
		}

		float maxScore = float.NegativeInfinity;
		List<Node> myNodes = board.GetNodesOfColor(color);

		foreach (Node node in myNodes)
		{
			List<List<Node>> paths = board.PossibleMoves(node);
			foreach (List<Node> path in paths)
			{
				Node dest = path[path.Count - 1];

				// Virtual Move
				node.IsOfPlayer = 0;
				dest.IsOfPlayer = color;

				// Recurse (In Minimax, we always seek the MAX, even at the next depth)
				float score = findMin(board, (color + 1) % 2, depth - 1, alpha, beta);

				// Undo
				dest.IsOfPlayer = 0;
				node.IsOfPlayer = color;

				if (score > maxScore) maxScore = score;
				if (score > alpha) alpha = score;

				if (alpha > beta)
				{
					return maxScore == float.NegativeInfinity ? EvaluateBoard(board, color) : maxScore;
				}
			}
		}

		return maxScore == float.NegativeInfinity ? EvaluateBoard(board, color) : maxScore;
	}

	private float findMin(Board board, int color, int depth, float alpha, float beta)
	{
		// Base case: leaf node or max depth reached
		if (depth <= 0)
		{
			return EvaluateBoard(board, color);
		}

		float minScore = float.PositiveInfinity;
		List<Node> myNodes = board.GetNodesOfColor(color);

		foreach (Node node in myNodes)
		{
			List<List<Node>> paths = board.PossibleMoves(node);
			foreach (List<Node> path in paths)
			{
				Node dest = path[path.Count - 1];

				// Virtual Move
				node.IsOfPlayer = 0;
				dest.IsOfPlayer = color;

				// Recurse (In Minimax, we always seek the MAX, even at the next depth)
				float score = findMax(board, (color + 1) % 2, depth - 1, alpha, beta);
				// Undo
				dest.IsOfPlayer = 0;
				node.IsOfPlayer = color;

				if (score < minScore) minScore = score;
				if (score < beta) beta = score;

				if (alpha > beta)
				{
					return minScore == float.PositiveInfinity ? EvaluateBoard(board, color) : minScore;
				}
			}
		}

		return minScore == float.PositiveInfinity ? EvaluateBoard(board, color) : minScore;
	}

	// A simple heuristic: The sum of distances of all pieces to the "goal"
	private float EvaluateBoard(Board board, int color)
	{
		return board.GetScore(color);
	}
}
