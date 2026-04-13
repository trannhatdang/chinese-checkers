using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerControllers/MLControllerSO")]
public class MLControllerSO : PlayerControllerSO
{
	// These "Weights" are what a Machine Learning model would "learn" over time
	[Header("Model Weights (The Brain)")]
	[SerializeField] float w_ForwardProgress = 1.5f;  // Priority: Move toward goal
	[SerializeField] float w_Centrality = 0.5f;       // Priority: Stay near center line
	[SerializeField] float w_JumpPotential = 0.8f;    // Priority: Land where more jumps are possible
	[SerializeField] float w_HomeBaseExit = 2.0f;     // Priority: Get out of the starting area ASAP

	public float Fitness = 0;

	public async override UniTask BeginTurn(Board board, int color)
	{
		List<Node> myNodes = board.GetNodesOfColor(color);

		Node bestStartNode = null;
		List<Node> bestPath = null;
		float highestInferenceScore = float.NegativeInfinity;

		foreach (Node node in myNodes)
		{
			List<List<Node>> possiblePaths = board.PossibleMoves(node);
			foreach (List<Node> path in possiblePaths)
			{
				if (path.Count <= 1) continue;

				// Inference: The model "predicts" the value of this state
				float score = InferMoveValue(node, path, board, color);

				if (score > highestInferenceScore && path.Count > 1)
				{
					highestInferenceScore = score;
					bestStartNode = node;
					bestPath = path;
				}
			}
		}

		if (bestStartNode != null && bestPath != null)
		{
			Fitness = board.GetScore(color);
			await board.ChangePosition(bestStartNode, bestPath);
		}
	}

	/// <summary>
	/// This is the "Forward Pass" of a simple linear neural network.
	/// Score = (Feature_1 * Weight_1) + (Feature_2 * Weight_2) ...
	/// </summary>
	private float InferMoveValue(Node start, List<Node> path, Board board, int color)
	{
		Node destination = path[path.Count - 1];
		Vector3 goalPos = GetGoalPosition(color);

		// Feature 1: Progress (Distance closed to goal)
		float initialDist = Vector3.Distance(start.Position, goalPos);
		float finalDist = Vector3.Distance(destination.Position, goalPos);
		float progress = initialDist - finalDist;

		// Feature 2: Centrality (Staying near the x=0 axis to avoid getting stuck in corners)
		float centrality = 1.0f / (Mathf.Abs(destination.X - 9) + 1);

		// Feature 3: Chain potential (How many moves could I make from the NEW spot next turn?)
		// We temporarily simulate the move to check
		int oldColor = destination.IsOfPlayer;
		destination.IsOfPlayer = color;
		start.IsOfPlayer = 0;

		int nextMoveCount = board.PossibleMoves(destination).Count;
		destination.IsOfPlayer = oldColor;
		start.IsOfPlayer = color;

		// Final Linear Equation
		return (progress * w_ForwardProgress) +
		       (centrality * w_Centrality) +
		       (nextMoveCount * w_JumpPotential);
	}

	private Vector3 GetGoalPosition(int color)
	{
		// Logic to return the target corner based on player index
		return new Vector3(0, 8, 0);
	}

	public void Mutate(float mutationRate)
	{
		w_ForwardProgress += Random.Range(-mutationRate, mutationRate);
		w_Centrality += Random.Range(-mutationRate, mutationRate);
		w_JumpPotential += Random.Range(-mutationRate, mutationRate);
		w_HomeBaseExit += Random.Range(-mutationRate, mutationRate);
	}

	public void RandomizeWeights()
	{
		w_ForwardProgress = Random.Range(-2f, 2f);
		w_Centrality = Random.Range(-2f, 2f);
		w_JumpPotential = Random.Range(-2f, 2f);
		w_HomeBaseExit = Random.Range(-2f, 2f);
	}

	public void Copy(MLControllerSO cont)
	{
		w_ForwardProgress = cont.w_ForwardProgress;
		w_Centrality = cont.w_Centrality;
		w_JumpPotential = cont.w_JumpPotential;
		w_HomeBaseExit = cont.w_HomeBaseExit;
	}
}
