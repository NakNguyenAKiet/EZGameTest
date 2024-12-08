using System;
using TTHUnityBase.Base.DesignPattern;

public class GameEventManager
{
    public event Action<AnimalObstacle> OnPickUpAnimal;
    public void PickUpAnimal(AnimalObstacle animal) => OnPickUpAnimal.Invoke(animal);
}

public class MyGameEvent: Singleton <GameEventManager>
{

}
