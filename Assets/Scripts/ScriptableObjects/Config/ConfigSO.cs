using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ConfigSO")]
public class ConfigSO : ScriptableObject
{
	[Header("TwoPlayer")]
	[SerializeField] TextAsset m_twoPlayerBoard;

	[Header("FourPlayer")]
	[SerializeField] TextAsset m_fourPlayerBoard;

	[Header("SixPlayer")]
	[SerializeField] TextAsset m_sixPlayerBoard;
}
