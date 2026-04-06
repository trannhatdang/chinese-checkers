using UnityEngine;

public abstract class PlayerControllerSO : ScriptableObject
{
	public string Name
	{
		get { return name; }
	}
	public abstract void BeginTurn(Board board, GameManager gm, int color);
}
