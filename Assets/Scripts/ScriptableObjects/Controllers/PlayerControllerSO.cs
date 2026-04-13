using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class PlayerControllerSO : ScriptableObject
{
	[SerializeField] string m_customName = "";
	public string Name
	{
		get
		{
			if (m_customName == "")
			{
				return name;
			}

			return m_customName;
		}
	}

	public abstract UniTask BeginTurn(Board board, int color);
}
