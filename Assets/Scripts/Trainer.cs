using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;


public class Trainer : MonoBehaviour
{
	[SerializeField] MLControllerSO m_baseTemplate;
	[SerializeField] GameManager m_gameManager;
	// [SerializeField] int m_populationSize = 12; // Must be divisible by 6 (for full games)

	[SerializeField] List<MLControllerSO> m_population;
	private int m_generation = 0;

	void Start()
	{
		// Step 1: Create Generation 0 with random weights

	}

	public void EvolvePopulation()
	{
		// Sort by a Fitness variable (you should add 'public float Fitness' to your SO)
		var winners = m_population.OrderByDescending(x => x.Fitness).ToList();

		for (int i = 3; i < 6; ++i)
		{
			winners[i].Copy(winners[i - 3]);
			winners[i].Mutate(0.1f);
		}

		m_population = winners;
		m_generation++;

		for (int i = 0; i < 6; ++i)
		{
			m_gameManager.SetAI(i, winners[i]);
			winners[i].Fitness = 0;
		}

		m_gameManager.StartGame(EvolvePopulation);
	}

	public void SimulateGames()
	{
		// Here you would tell your GameManager to run games using m_population
		// and assign Fitness = (1 / turnsTaken) or (distanceCovered).
		for (int i = 0; i < 6; ++i)
		{
			m_gameManager.SetPlayerAI(i, i + 3);
		}
		m_gameManager.StartGame(EvolvePopulation);
	}

	void RandomizeWeights()
	{
		for (int i = 0; i < m_population.Count; i++)
		{
			m_population[i].RandomizeWeights();
		}


	}

	// void OnGUI()
	// {
	// 	if (GUI.Button(new Rect(10, 240, 150, 110), "Start Trainning"))
	// 	{
	// 		// RandomizeWeights();
	// 		SimulateGames();
	// 		Time.timeScale = 100.0f;
	// 	}
	// }
}
