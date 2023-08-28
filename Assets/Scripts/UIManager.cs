using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _scoreText;

    [SerializeField] private Image _livesImg;

    [SerializeField] private Sprite[] _livesSprites;

    [SerializeField] private TMP_Text _gameOverText;

    [SerializeField] private TMP_Text _restartText;

    private SpawnManager _spawnManager;

    private GameManager _gameManager;

    [SerializeField] private TMP_Text _ammoCountText;
    [SerializeField] private TMP_Text _waveCountText;
    [SerializeField] private TMP_Text _waveTimeText;
    [SerializeField] private TMP_Text _finalWaveText;

    void Start()
    {
        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        //
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("UIManager::GameManager is null");
        }
        //
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager:: SpawnManager is null");
        }
    }

    public void UIScoreUpdate(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];
    }

    public IEnumerator ActivateGameOverTextRoutine()
    {
        while (_livesImg.sprite == _livesSprites[0])
        {
            _gameOverText.text = "GAME OVER";

            yield return new WaitForSeconds(.75f);

            _gameOverText.text = " ";

            yield return new WaitForSeconds(.75f);
        }
    }

    public void AmmoCountUpdate(int ammoCount)
    {
        _ammoCountText.text = "Ammo Count: " + ammoCount;
    }

    public void GameOverActions()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(ActivateGameOverTextRoutine());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator DisplayWaveInfoCorou(int Count, float Time)
    {
        _waveCountText.gameObject.SetActive(true);
        _waveTimeText.gameObject.SetActive(true);

        _waveCountText.text = "Wave: " + Count;
        _waveTimeText.text = "Time: " + Time;
        yield return new WaitForSeconds(3.0f);

        _waveCountText.gameObject.SetActive(false);
        _waveTimeText.gameObject.SetActive(false);
        _spawnManager.StartSpawning();
    }

    public void DisplayWaveInfo(int c, float t)
    {
        StartCoroutine(DisplayWaveInfoCorou(c, t));
    }
    public void FinalWaveUI()
    {
        StartCoroutine(DisplayFinalWaveInfoCorou());
    }

    IEnumerator DisplayFinalWaveInfoCorou()
    {
        _finalWaveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        _finalWaveText.gameObject.SetActive(false);
    }
}
