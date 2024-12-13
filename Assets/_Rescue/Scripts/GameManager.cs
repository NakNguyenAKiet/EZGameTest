using System;
using System.Collections;
using System.Collections.Generic;
using TTHUnityBase.Base.DesignPattern;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    [SerializeField] ObstacleSpawner animalSpwaner;
    [SerializeField] ObstacleSpawner decoLeftSpwaner;
    [SerializeField] ObstacleSpawner decoRightSpwaner;
    [SerializeField] ObstacleSpawner propsSpawner;
    [SerializeField] GameConfig gameConfig;

    [SerializeField] Transform endPos;
    [SerializeField] Tsunami tsunami;
    [SerializeField] UIManager uIManager;
    [SerializeField] Transform playerSpawnPos;
    [SerializeField] SimpleSampleCharacterControl playerControl;
    [SerializeField] PlayerInventories playerInventories;

    GameData gameData;
    string gameDataKey = "gameDataKey";

    private int pickedUpAnimalCount = 0;
    private int cashReward = 0;
    public GameConfig GameConfig { get => gameConfig;}
    public GameData GameData { get => gameData;}
    public int PickedUpAnimalCount { get => pickedUpAnimalCount;}
    public int CashReward { get => cashReward;}

    private void Awake()
    {
        LoadGameData();
    }
    private void Start()
    {
        uIManager.InitUIData(gameData.Cash, gameData.CurrentPlayerSpeed, GetUpGradeCost(), gameData.GameLevel);
        MyGameEvent.Instance.OnPickUpAnimal += OnPickUpAnimal;
        ResetGame();
    }
    public void ResetGame()
    {
        pickedUpAnimalCount = 0;
        playerControl.SetPlayerSpeed(0);
        tsunami.ResetPos();
        playerControl.transform.position = playerSpawnPos.position;
        endPos.position = new Vector3(endPos.position.x, endPos.position.y, GetCurrentEndGameDistance());
        playerInventories.ResetInventory();
    }
    public void PlayGame()
    {
        animalSpwaner.Respawn();
        decoLeftSpwaner.Respawn();
        decoRightSpwaner.Respawn();
        propsSpawner.Respawn();
        playerControl.SetPlayerSpeed(GameData.CurrentPlayerSpeed);
        tsunami.StartMove(GameData.CurrentTsunamiSpeedMultiplier);
        MyGameEvent.Instance.UpdateAnimalCount();
        playerInventories.ResetInventory();
    }

    #region EventListener
    public void OnGameOver()
    {
        cashReward = GameConfig.CashPerAnimal * pickedUpAnimalCount;
        GameData.Cash += cashReward;
        MyGameEvent.Instance.GameOver();
        SaveGameData();
    }
    public void OnCompleteLevel()
    {
        cashReward = GameConfig.CashPerAnimal * pickedUpAnimalCount;
        GameData.Cash += cashReward;
        GameData.NumberOfObstacle = GameConfig.NumberOfObstacle + GameData.GameLevel;
        GameData.CurrentAnimalSpeed = GameConfig.AnimalSpeed + (GameConfig.AnimalSpeed * GameConfig.AnimalSpeedMultiplier * GameData.GameLevel);
        GameData.CurrentTsunamiSpeedMultiplier = GameConfig.TsunamiSpeedMultiplier * GameData.GameLevel;

        GameData.GameLevel += 1;
        MyGameEvent.Instance.CompleteLevel();
        ResetGame();
        SaveGameData();
    }
    public void OnUpgradePlayerSpeed()
    {
        if (GameData.Cash < GetUpGradeCost()) return;

        gameData.Cash -= GetUpGradeCost();
        gameData.CurrentPlayerSpeed += GameConfig.PlayerSpeedMultiplier;
        gameData.CurrentSpeedUpgradeLevel += 1;
        uIManager.InitUIData(gameData.Cash, gameData.CurrentPlayerSpeed, GetUpGradeCost(), gameData.GameLevel);

        SaveGameData();
        MyGameEvent.Instance.UpgradeSpeed(gameData.CurrentPlayerSpeed);
    }
    void OnPickUpAnimal(AnimalObstacle animalObstacle)
    {
        pickedUpAnimalCount++;
        MyGameEvent.Instance.UpdateAnimalCount();
    }
    #endregion EventListener
    void LoadGameData()
    {
        if(PlayerPrefs.HasKey(gameDataKey))
        {
            string json = PlayerPrefs.GetString(gameDataKey);
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            gameData = new GameData()
            {
                CurrentPlayerSpeed = gameConfig.PlayerSpeed,
                CurrentAnimalSpeed = gameConfig.AnimalSpeed,
                CurrentTsunamiSpeedMultiplier = 0,
                GameLevel = 1,
                Cash = 0,
                NumberOfObstacle = gameConfig.NumberOfObstacle,
                CurrentSpeedUpgradeLevel = 0,
            };
            SaveGameData();
        }
    }
    public void SaveGameData()
    {
        string json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString(gameDataKey, json);
    }
    public int GetUpGradeCost()
    {
        int upgradeCost = GameConfig.UpgradeCost + (int)(GameConfig.UpgradeCost * GameData.CurrentSpeedUpgradeLevel * GameConfig.UpgradeCostMultiplier);
        return upgradeCost;
    }
    public float GetCurrentEndGameDistance()
    {
        if (GameData.GameLevel == 1) return gameConfig.EndgameDistance;

        //addition distance = 5
        return gameConfig.EndgameDistance + (GameConfig.EndgameDistance * GameConfig.TsunamiSpeedMultiplier * GameData.GameLevel) +5;
    }
    public void FreeCashTest()
    {
        gameData.Cash += 1000;
        uIManager.InitUIData(gameData.Cash, gameData.CurrentPlayerSpeed, GetUpGradeCost(), gameData.GameLevel);
        SaveGameData();
    }
}

public class MyGame : SingletonMonoBehaviour<GameManager> { }

[Serializable]
public class GameData
{
    public int GameLevel;
    public int NumberOfObstacle;
    public int Cash;
    public float CurrentPlayerSpeed;
    public int CurrentSpeedUpgradeLevel;
    public float CurrentAnimalSpeed;
    public float CurrentTsunamiSpeedMultiplier;
}
