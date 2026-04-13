using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerControllerListSO")]
public class PlayerControllerListSO : ScriptableObject
{
	[SerializeField] PlayerControllerSO m_humanController;
	[SerializeField] PlayerControllerSO m_MCTSController;
	[SerializeField] PlayerControllerSO m_miniMaxController;
	[SerializeField] PlayerControllerSO m_maxiMaxController;

	[SerializeField] List<PlayerControllerSO> m_machineLearningAIControllerList;

	public PlayerControllerSO HumanController
	{
		get { return m_humanController; }
	}

	public PlayerControllerSO MCTSController
	{
		get { return m_MCTSController; }
	}

	public PlayerControllerSO MiniMaxController
	{
		get { return m_miniMaxController; }
	}

	public PlayerControllerSO MaxiMaxController
	{
		get { return m_maxiMaxController; }
	}

	public List<PlayerControllerSO> MachineLearningAIControllerList
	{
		get { return m_machineLearningAIControllerList; }
	}
}
