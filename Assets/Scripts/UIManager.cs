using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMP_Text _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UIScoreUpdate(int playerScore)
    {
        _scoreText.text = "Score: "+ playerScore;
    }
}
