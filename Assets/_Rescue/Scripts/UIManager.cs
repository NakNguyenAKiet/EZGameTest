using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button upgradeSpeedBtn;
    [SerializeField] Button playBtn;
    [SerializeField] Button backToMenuBtn;
    [SerializeField] TextMeshProUGUI speedText, endGameTitle, cashText, upgradeCostText, levelText, animalCountText, cashRewardText;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject endGame;
    [SerializeField] GameObject menuCamera;
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
        MyGameEvent.Instance.OnUpdateAnimalCount += UpdateUIAnimalCount;
        UpdateUISpeed(MyGame.Instance.GameData.CurrentPlayerSpeed);
    }
    public void InitUIData(int cash, float playerSpeed, int upgradeCost, int level)
    {
        upgradeCostText.text = "$"+upgradeCost.ToString();
        levelText.text = "LEVEL " + level.ToString();
        cashText.text =  cash.ToString();
        UpdateUISpeed(playerSpeed);
    }
    void UpdateUISpeed(float speed)
    {
        speedText.text = "Speed "+ Math.Round(speed, 2).ToString();
    }
    void UpdateUIAnimalCount()
    {
        animalCountText.text = $"{MyGame.Instance.PickedUpAnimalCount}/{MyGame.Instance.GameData.NumberOfObstacle}";
    }
    void UpdateUICashReward()
    {
        cashRewardText.text = $"Reward: {MyGame.Instance.CashReward}$";
    }
    void OnGameOver()
    {
        endGameTitle.text = "Game over";
        endGame.SetActive(true);
        UpdateUICashReward();
    }
    void OnCompleteLevel()
    {
        endGameTitle.text = "Complete level "+ (MyGame.Instance.GameData.GameLevel -1);
        endGame.SetActive(true);
        UpdateUICashReward();
    }
    void OnClickBackToMenu()
    {
        menu.SetActive(true);
        menuCamera.gameObject.SetActive(true);
        endGame.SetActive(false);
        InitUIData(MyGame.Instance.GameData.Cash, MyGame.Instance.GameData.CurrentPlayerSpeed, MyGame.Instance.GetUpGradeCost(), MyGame.Instance.GameData.GameLevel);
        MyGame.Instance.ResetGame();
    }
    async void OnClickPlay()
    {
        menu.SetActive(false);
        MyGame.Instance.PlayGame();

        await Task.Delay(1000);
        menuCamera.gameObject.SetActive(false);
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
