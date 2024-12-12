using System;
using System.Collections;
using System.Collections.Generic;
using TTHUnityBase.Base.DesignPattern;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    GameData gameData;
    string gameDataKey = "gameDataKey";

    private int pickedUpAnimalCount = 0;
    public GameConfig GameConfig { get => gameConfig;}
    public GameData GameData { get => gameData;}

    private void Awake()
    {
        LoadGameData();
    }
    private void Start()
    {
        uIManager.InitUIData(gameData.Cash, gameData.CurrentPlayerSpeed, GetUpGradeCost());
    }
    public void ResetGame()
    {
        animalSpwaner.Respawn();
        decoLeftSpwaner.Respawn();
        decoRightSpwaner.Respawn();
        propsSpawner.Respawn();

        pickedUpAnimalCount = 0;
        tsunami.StartMove(GameData.CurrentTsunamiSpeedMultiplier);
        playerControl.SetPlayerSpeed(GameData.CurrentPlayerSpeed);
        playerControl.transform.position = playerSpawnPos.position;
        endPos.position = new Vector3(endPos.position.x, endPos.position.y, GetCurrentEndGameDistance());
    }
    public void OnGameOver()
    {
        GameData.Cash += GameConfig.CashPerAnimal * pickedUpAnimalCount;

        MyGameEvent.Instance.GameOver();
        SaveGameData();
    }
    public void OnCompleteLevel()
    {
        GameData.GameLevel += 1;
        GameData.Cash += GameConfig.CashPerAnimal * pickedUpAnimalCount;
        GameData.NumberOfObstacle = GameConfig.NumberOfObstacle + GameData.GameLevel;
        GameData.CurrentAnimalSpeed = GameConfig.AnimalSpeed + (GameConfig.AnimalSpeed * GameConfig.AnimalSpeedMultiplier * GameData.GameLevel);
        GameData.CurrentTsunamiSpeedMultiplier = GameConfig.TsunamiSpeedMultiplier * GameData.GameLevel;

        MyGameEvent.Instance.CompleteLevel();

        SaveGameData();
    }
    public void OnUpgradePlayerSpeed()
    {
        if (GameData.Cash < GetUpGradeCost()) return;

        gameData.Cash -= GetUpGradeCost();
        gameData.CurrentPlayerSpeed += GameConfig.PlayerSpeedMultiplier;
        gameData.CurrentSpeedUpgradeLevel += 1;
        uIManager.InitUIData(gameData.Cash, gameData.CurrentPlayerSpeed, GetUpGradeCost());

        SaveGameData();
        MyGameEvent.Instance.UpgradeSpeed(gameData.CurrentPlayerSpeed);
    }
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

        return gameConfig.EndgameDistance + (GameConfig.EndgameDistance * GameConfig.TsunamiSpeedMultiplier * GameData.GameLevel);
    }

    private void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameData.Cash += 1000000;
        }
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
