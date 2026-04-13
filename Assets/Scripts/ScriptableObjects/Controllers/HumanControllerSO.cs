using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerControllers/HumanControllerSO")]
public class HumanControllerSO : PlayerControllerSO
{
	public override async UniTask BeginTurn(Board board, int color)
	{

	}
}
