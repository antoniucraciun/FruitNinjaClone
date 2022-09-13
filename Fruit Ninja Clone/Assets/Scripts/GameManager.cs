using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public TMP_Text scoreText;
	public Image fadeImage;

    private int score;
	private Blade _blade;
	private Spawner _spawner;

	private void Awake()
	{
		Instance = this;
		_blade = FindObjectOfType<Blade>();
		_spawner = FindObjectOfType<Spawner>();
		NewGame();
	}

	public void NewGame()
	{
        score = 0;
        scoreText.text = score.ToString("N0");
		if (!_blade.isActiveAndEnabled)
			_blade.enabled = true;
		if (!_spawner.enabled)
			_spawner.enabled = true;

		var fruits = FindObjectsOfType<Fruit>();
		for (int i = 0; i < fruits.Length; i++)
		{
			Fruit item = fruits[i];
			Destroy(item.gameObject);
		}
		var bombs = FindObjectsOfType<Bomb>();
		for (int i = 0; i < bombs.Length; i++)
		{
			Bomb item = bombs[i];
			Destroy(item.gameObject);
		}
	}

    public void IncreaseScore(int amount)
	{
        score+= amount;
        scoreText.text = score.ToString("N0");
	}

	public void Explode()
	{
		_blade.enabled = false;
		_spawner.enabled = false;
		StartCoroutine(ExplodeSequence());
	}

	private IEnumerator ExplodeSequence()
	{
		float elapsedTime = 0f;
		float duration = 1f;

		Time.timeScale = 0.8f;

		while (elapsedTime < duration)
		{
			float t = Mathf.Clamp01(elapsedTime / duration);
			fadeImage.color = Color.Lerp(Color.clear, Color.white, t);
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(1f);
		NewGame();

		elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			float t = Mathf.Clamp01(elapsedTime / duration);
			fadeImage.color = Color.Lerp(Color.white, Color.clear, t);
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}
		Time.timeScale = 1f;
	}
}
