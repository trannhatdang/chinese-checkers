using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class PlayerControllerSO : ScriptableObject
{
	public string Name
	{
		get { return name; }
	}
	public abstract UniTask BeginTurn(Board board, int color);
}
