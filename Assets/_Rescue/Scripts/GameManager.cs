using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ObstacleSpawner animalSpwaner;
    [SerializeField] ObstacleSpawner decoLeftSpwaner;
    [SerializeField] ObstacleSpawner decoRightSpwaner;
    [SerializeField] ObstacleSpawner propsSpawner;
    private void Start()
    {
        animalSpwaner.Respawn();
        decoLeftSpwaner.Respawn();
        decoRightSpwaner.Respawn();
        propsSpawner.Respawn();
    }
}
