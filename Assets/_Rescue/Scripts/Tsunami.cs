using UnityEngine;

public class Tsunami : MonoBehaviour
{
    public float speed = 10f;
    public Transform hero;
    public Transform pointA;
    public Transform pointB;
    public float duration = 45f;

    private Rigidbody rb;
    private Vector3 velocity;
    private Vector3 originPos;
    private void Awake()
    {
        originPos = transform.position;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void ResetPos()
    {
        velocity = Vector3.zero;
        transform.position = originPos;
    }
    public void StartMove(float speedMultiply)
    {
        transform.position = originPos;
        velocity = (pointB.position - pointA.position) / duration;
        velocity += velocity*speedMultiply;
        transform.position = pointA.position;
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, pointB.position) > 0.1f)
        {
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }
        else
        {
            //Debug.Log("Game over");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MyGame.Instance.OnGameOver();
            velocity = Vector3.zero;
        }
    }
}