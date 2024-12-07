using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoObstacle : ObstacleSpawned
{
    public override void OnSpwaned(Vector3 pos)
    {
        base.OnSpwaned(pos);
        DoRotateY();
    }
    void DoRotateY()
    {
        float randomRotationY = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, randomRotationY, 0);
    }
}
