using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System.Collections;
using static UnityEngine.UI.Image;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;

    [Range(0, 360)]
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 50f;
    public bool canSeeObjectInView;

    public List<Transform> visibleTargets = new List<Transform>();

    public float ViewDistance { get => viewDistance; set => viewDistance = value; }
    public float Fov { get => fov; set => fov = value; }
    public Transform CurrentTarget;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        StartCoroutine(FOVRoutine());
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            CheckObjectsInView();

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


    private float GetStartingAngle()
    {
        return transform.eulerAngles.y + Fov / 2f;
    }
    private void CheckObjectsInView()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance, layerMask);

        foreach (Collider target in targetsInViewRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

            // Kiểm tra nếu góc giữa hướng nhân vật và vật trong khoảng fov / 2
            if (Vector3.Angle(transform.forward, directionToTarget) < fov / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

                Debug.Log($"Vật trong vùng nhìn: {targetTransform.name}");
                CurrentTarget = targetTransform;
                if(!visibleTargets.Contains(CurrentTarget))
                visibleTargets.Add( targetTransform );
            }
        }
    }
}
