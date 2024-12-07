using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsObstacle : ObstacleSpawned
{
    public override void OnSpwaned(Vector3 pos)
    {
        base.OnSpwaned(pos);
        DoRandomRotate();
    }
    void DoRandomRotate()
    {
        float randomRotationX = Random.Range(0f, 360f);
        float randomRotationY = Random.Range(0f, 360f);
        float randomRotationZ = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(randomRotationX, randomRotationY, randomRotationZ);
    }
}
