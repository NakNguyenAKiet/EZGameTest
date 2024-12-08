using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Collections;
using static UnityEngine.UI.Image;

public class FieldOfView : MonoBehaviour
{
    private Mesh mesh;
    private float timeCheck = 0.2f;
    private List<Transform> visibleTargets = new List<Transform>();
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private MeshRenderer meshRender;

    [Range(0, 360)]
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 50f;
    private bool canSeeObjectInView;

    public bool CanSeeObjectInView => canSeeObjectInView;
    public List<Transform> VisibleTargets => visibleTargets;
    public float ViewDistance { get => viewDistance; set => viewDistance = value; }
    public float Fov { get => fov; set => fov = value; }
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        StartCoroutine(FOVRoutine());
        DrawFieldOfView();
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(timeCheck);

        while (true)
        {
            yield return wait;
            CheckObjectsInView();
            meshRender.enabled = canSeeObjectInView;
        }
    }

    private void DrawFieldOfView()
    {
        Vector3 origin = transform.position; // Lấy vị trí hiện tại của nhân vật
        int rayCount = 50;
        float angle = -fov / 2;  // Bắt đầu từ bên trái FOV
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero; // Gốc tọa độ cục bộ

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 direction = GetVectorFromAngle(angle);
            Vector3 vertex = origin + transform.rotation * direction * viewDistance;  // Xoay theo hướng của nhân vật

            vertices[vertexIndex] = transform.InverseTransformPoint(vertex);  // Chuyển về tọa độ local

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle += angleIncrease;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad));  // Vector trong mặt phẳng XZ
    }
    private List<AnimalObstacle> animalsInView = new List<AnimalObstacle>();
    private List<AnimalObstacle> lastAnimalsInView = new List<AnimalObstacle>();

    private void CheckObjectsInView()
    {
        animalsInView.Clear();

        // Tìm tất cả vật trong bán kính quan sát
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance, layerMask);

        foreach (Collider target in targetsInViewRadius)
        {
            AnimalObstacle animal = target.GetComponent<AnimalObstacle>();
            if (animal != null)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < fov / 2)
                {
                    // Vật chưa được thêm vào danh sách hiện tại
                    if (!animalsInView.Contains(animal))
                    {
                        animalsInView.Add(animal);

                        // Gọi sự kiện nếu vật vừa vào vùng nhìn
                        if (!lastAnimalsInView.Contains(animal))
                        {
                            animal.OnEnterPlayerView();
                        }
                    }
                }
            }
        }

        // Kiểm tra vật ra khỏi vùng nhìn
        foreach (var animal in lastAnimalsInView)
        {
            if (!animalsInView.Contains(animal))
            {
                animal.OnExitPlayerView();
            }
        }

        // Cập nhật danh sách cuối cùng
        lastAnimalsInView.Clear();
        lastAnimalsInView.AddRange(animalsInView);

        canSeeObjectInView = animalsInView.Count > 0;
    }


}
