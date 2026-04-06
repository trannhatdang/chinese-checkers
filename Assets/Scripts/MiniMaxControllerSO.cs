using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerControllers/MinimaxControllerSO")]
public class MinimaxControllerSO : PlayerControllerSO
{
	[SerializeField] private int m_maxDepth = 3; // Keep this low for performance

	public async override void BeginTurn(Board board, GameManager gm, int color)
	{
		Node bestNode = null;
		List<Node> bestMove = null;
		float bestScore = float.NegativeInfinity;

		List<Node> myNodes = board.GetNodesOfColor(color);

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
				float score = Minimax(board, color, m_maxDepth - 1);

				// 3. Undo Move
				destination.IsOfPlayer = 0;
				node.IsOfPlayer = originalColor;

				if (score > bestScore && path.Count > 1)
				{
					bestScore = score;
					bestNode = node;
					bestMove = path;
				}
			}
		}

		// 4. Execute the best move found
		if (bestNode != null && bestMove != null)
		{
			if (color == 6)
			{
				// Debug.Log($"{bestMove[bestMove.Count - 1].X}, {bestMove[bestMove.Count - 1].Y}");
				Debug.Log($"{bestMove.Count}");
			}
			await board.ChangePosition(bestNode, bestMove);
		}

		// Wait for movement animation (simulated) then end turn
		await Task.Delay(1000);
		gm.NextTurn();
	}

	private float Minimax(Board board, int color, int depth)
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
				float score = Minimax(board, color, depth - 1);

				if (score > maxScore) maxScore = score;

				// Undo
				dest.IsOfPlayer = 0;
				node.IsOfPlayer = color;
			}
		}

		return maxScore == float.NegativeInfinity ? EvaluateBoard(board, color) : maxScore;
	}

	// A simple heuristic: The sum of distances of all pieces to the "goal"
	private float EvaluateBoard(Board board, int color)
	{
		float score = 0;
		List<Node> pieces = board.GetNodesOfColor(color);

		// Define your goal position based on the player's color
		Vector3 goalPos = GetGoalPosition(color);

		foreach (var p in pieces)
		{
			// We subtract distance because we want a higher score for being closer
			score -= Vector3.Distance(p.Position, goalPos);
		}

		return score;
	}

	private Vector3 GetGoalPosition(int color)
	{
		// Return the target coordinate for the specific player color
		// This should be the far corner of the Chinese Checkers star
		return new Vector3(0, 10, 0); // Placeholder
	}
}
