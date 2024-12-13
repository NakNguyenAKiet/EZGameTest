using System;
using TTHUnityBase.Base.DesignPattern;

public class GameEventManager
{
    public event Action<AnimalObstacle> OnPickUpAnimal;
    public void PickUpAnimal(AnimalObstacle animal) => OnPickUpAnimal.Invoke(animal);

    public event Action<float> OnUpgradeSpeed;
    public void UpgradeSpeed(float speed) => OnUpgradeSpeed.Invoke(speed);

    public event Action OnCompleteLevel;
    public void CompleteLevel() => OnCompleteLevel.Invoke();

    public event Action OnGameOver;
    public void GameOver() => OnGameOver.Invoke();
    public event Action OnUpdateAnimalCount;
    public void UpdateAnimalCount() => OnUpdateAnimalCount.Invoke();
}

public class MyGameEvent: Singleton <GameEventManager>
{

}
