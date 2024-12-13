using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AnimalObstacle : ObstacleSpawned
{
    public NavMeshAgent navMeshAgent;       // NavMeshAgent for character movement
    public Vector3 targetPoint;    // Center point (position A) for movement
    public float radius = 3f;        // Radius for the movement area
    public float moveInterval = 2f;  // Time interval between movements (2 seconds)
    private float timer; // Timer for tracking movement intervals
    [SerializeField] Slider slider;
    [SerializeField] ParticleSystem dirtParticle;

    void Start()
    {
        // Initialize the timer
        timer = moveInterval;
        slider.gameObject.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime; // Increase timer by the time passed since the last frame

        // Move the character every `moveInterval` seconds
        if (timer >= moveInterval)
        {
            MoveToRandomPoint(); // Move to a random point
            timer = 0f; // Reset the timer
        }

        if (isInView)
        {
            timeInView += Time.deltaTime;
            slider.value = timeInView / requiredTimeInView;
        }
        if (timeInView >= requiredTimeInView)
        {
            OnPlayerPickUp();
            timeInView = 0f; // Đặt lại thời gian theo dõi
        }
    }

    public void AllowTomove(bool isAllow)
    {
        navMeshAgent.enabled = isAllow;
    }

    #region OnPlayerView Handle

    private bool isInView = false;
    private float timeInView = 0f;
    private float requiredTimeInView = 1.4f;

    public void OnEnterPlayerView()
    {
        slider.gameObject.SetActive(true);

        Debug.Log($"{gameObject.name} đã vào vùng nhìn.");
        isInView = true;
        timeInView = 0f;
    }

    public void OnExitPlayerView()
    {
        slider.gameObject.SetActive(false);

        Debug.Log($"{gameObject.name} đã ra khỏi vùng nhìn.");
        isInView = false;
        timeInView = 0f; // Đặt lại thời gian theo dõi
    }

    public void OnPlayerPickUp()
    {
        dirtParticle.Stop();

        slider.gameObject.SetActive(false);

        Debug.Log($"{gameObject.name} đã được nhặt!");
        // Xử lý logic nhặt vật

        MyGameEvent.Instance.PickUpAnimal(this);
        AllowTomove(false);
    }
    #endregion OnPlayerView Handle

    void MoveToRandomPoint()
    {
        // Get a random point within the specified radius
        Vector3 randomPoint = GetRandomPointAround(targetPoint, radius);

        // If a valid point is found on the NavMesh
        if (randomPoint != Vector3.zero)
        {
            if(navMeshAgent.enabled)
            navMeshAgent.SetDestination(randomPoint); // Move to the point
        }
    }

    Vector3 GetRandomPointAround(Vector3 center, float radius)
    {
        // Generate a random point within a sphere of given radius
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center; // Adjust the point to be relative to the center

        // Check if the point is on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            return hit.position; // Return the valid point
        }

        return Vector3.zero; // Return (0,0,0) if no valid point is found
    }
    public override void OnSpwaned(Vector3 pos)
    {
        base.OnSpwaned(pos);
        targetPoint = pos;
        DoRotateY();
        AllowTomove(true);
        navMeshAgent.speed = MyGame.Instance.GameData.CurrentAnimalSpeed;
        dirtParticle.Play();
    }
    void DoRotateY()
    {
        float randomRotationY = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, randomRotationY, 0);
    }
}
