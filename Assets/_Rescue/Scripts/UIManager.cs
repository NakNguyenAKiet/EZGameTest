using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button upgradeSpeedBtn;
    [SerializeField] Button playBtn;
    [SerializeField] Button backToMenuBtn;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI cashText;
    [SerializeField] TextMeshProUGUI upgradeCostText;
    [SerializeField] TextMeshProUGUI endGameTitle;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject endGame;
    [SerializeField] Camera menuCamera;
    private void Awake()
    {
        upgradeSpeedBtn.onClick.AddListener(OnClickUpgradeSpeed);
        playBtn.onClick.AddListener(OnClickPlay);
        backToMenuBtn.onClick.AddListener(OnClickBackToMenu);
    }
    private void Start()
    {
        MyGameEvent.Instance.OnGameOver += OnGameOver;
        MyGameEvent.Instance.OnCompleteLevel += OnCompleteLevel;
        UpdateUISpeed(MyGame.Instance.GameData.CurrentPlayerSpeed);
    }
    public void InitUIData(int cash, float playerSpeed, int upgradeCost)
    {
        upgradeCostText.text = upgradeCost.ToString();
        cashText.text = cash.ToString();
        UpdateUISpeed(playerSpeed);
    }
    void UpdateUISpeed(float speed)
    {
        speedText.text = "Speed "+speed.ToString();
    }
    void OnGameOver()
    {
        endGameTitle.text = "Game over";
        endGame.SetActive(true);
    }
    void OnCompleteLevel()
    {
        endGame.SetActive(true);
        endGameTitle.text = "Complete level "+ MyGame.Instance.GameData.GameLevel;
    }
    void OnClickBackToMenu()
    {
        menu.SetActive(true);
        menuCamera.gameObject.SetActive(true);
        endGame.SetActive(false);
        InitUIData(MyGame.Instance.GameData.Cash, MyGame.Instance.GameData.CurrentPlayerSpeed, MyGame.Instance.GetUpGradeCost());
    }
    void OnClickPlay()
    {
        menu.SetActive(false);
        menuCamera.gameObject.SetActive(false);
        MyGame.Instance.ResetGame();
    }
    void OnClickUpgradeSpeed()
    {
        MyGame.Instance.OnUpgradePlayerSpeed();
    }
    private void OnDestroy()
    {
        upgradeSpeedBtn.onClick.RemoveAllListeners();
        playBtn.onClick.RemoveAllListeners();
        MyGameEvent.Instance.OnGameOver -= OnGameOver;
        MyGameEvent.Instance.OnCompleteLevel -= OnCompleteLevel;
        backToMenuBtn.onClick.RemoveAllListeners();
    }
}
