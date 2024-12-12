using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Game Settings")]
    public float PlayerSpeed = 1f;
    public int UpgradeCost = 100;
    public float PlayerSpeedMultiplier = 0.05f;
    public float UpgradeCostMultiplier = 2f;

    public float TsunamiSpeed = 10f;
    public float TsunamiSpeedMultiplier = 0.15f;

    public float AnimalSpeed = 1.5f;
    public float AnimalSpeedMultiplier = 0.15f;

    public float EndgameDistance = 40;
    public int NumberOfObstacle = 5;
    public int CashPerAnimal = 10;
}
