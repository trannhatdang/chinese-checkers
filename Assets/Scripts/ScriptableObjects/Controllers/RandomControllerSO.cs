using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerControllers/RandomControllerSO")]
public class RandomControllerSO : PlayerControllerSO
{
	public async override UniTask BeginTurn(Board board, int color)
	{
		// 1. Get all pieces belonging to this player
		List<Node> myNodes = board.GetNodesOfColor(color);

		// 2. Filter to find only nodes that actually have valid moves
		// We use a list of pairs (StartingNode, Path)
		List<(Node startNode, List<Node> path)> allLegalMoves = new();

		foreach (Node node in myNodes)
		{
			List<List<Node>> paths = board.PossibleMoves(node);

			// PossibleMoves usually returns the starting node itself as a path of length 1.
			// We only want paths that actually move the piece.
			foreach (List<Node> path in paths)
			{
				if (path.Count > 1)
				{
					allLegalMoves.Add((node, path));
				}
			}
		}

		// 3. Pick a move at random
		if (allLegalMoves.Count > 0)
		{
			int randomIndex = Random.Range(0, allLegalMoves.Count);
			var selectedMove = allLegalMoves[randomIndex];

			// 4. Execute the move on the board
			// This triggers the DOTween animation in your Board script
			await board.ChangePosition(selectedMove.startNode, selectedMove.path);
		}
		else
		{
			Debug.LogWarning($"Player {color} has no legal moves!");
			await Task.Delay(500);
		}
	}
}
