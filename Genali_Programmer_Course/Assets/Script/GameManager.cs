using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI _ammoHUD;
    public TextMeshProUGUI _enemyKilledTxt;
    public int enemyKilled;
    [SerializeField] int enemyKilledWin;
    [SerializeField] GameObject WinUI;

    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        WinUI.SetActive(false);
    }

    void GameOver()
    {
        if (enemyKilled >= enemyKilledWin)
        {
            Time.timeScale = 0f;
            WinUI.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        GameOver();
        _enemyKilledTxt.text = "Killed : " + enemyKilled.ToString();
    }
}
