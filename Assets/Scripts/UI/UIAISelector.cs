using UnityEngine;

public class UIAISelector : MonoBehaviour
{
	[SerializeField] Config m_config;
	[SerializeField] int index;

	public void SetAI(int controller)
	{
		m_config.SetAI(index, controller);
	}
}
