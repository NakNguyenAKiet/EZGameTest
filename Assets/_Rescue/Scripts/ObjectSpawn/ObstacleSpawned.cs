using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawned : MonoBehaviour
{
    public virtual void OnSpwaned(Vector3 pos) 
    {
        SetPos(pos);
    }
    void SetPos(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }
}
