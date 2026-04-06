using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	[SerializeField] TMP_Text m_winnerText;
	[SerializeField] GameObject m_pauseButton;
	[SerializeField] GameObject m_pauseScreen;

	public void Pause()
	{
		m_pauseButton.SetActive(false);
		m_pauseScreen.SetActive(true);
	}

	public void UnPause()
	{
		m_pauseButton.SetActive(true);
		m_pauseScreen.SetActive(false);
	}

	public void Winner(int color, string name)
	{
		m_winnerText.transform.parent.gameObject.SetActive(true);
		m_winnerText.text = $"Player({name}) {color} won!";
		m_winnerText.transform.parent.transform.DOMove(new Vector3(640, 360, 0), 0.5f);
	}
}
