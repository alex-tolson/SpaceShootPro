using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] 
    private TMP_Text _scoreText;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private TMP_Text _gameOverText;

    [SerializeField]
    private TMP_Text _restartText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("UIManager::GameManager is null");
        }
    }

    private void Update()
    {

    }

    public void UIScoreUpdate(int playerScore)
    {
        _scoreText.text = "Score: "+ playerScore;
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

    public void GameOverActions()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(ActivateGameOverTextRoutine());
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }
}
