using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleSpawned : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _models;

    public virtual void OnSpwaned(Vector3 pos) 
    {
        SetPos(pos);
        RandomModel();
    }
    protected virtual void RandomModel()
    {
        if (_models.Count == 0) return;
        DeActiveAllModel();
        _models[Random.RandomRange(0, _models.Count - 1)].SetActive(true);
    }
    private void DeActiveAllModel()
    {
        foreach (GameObject model in _models)
        {
            model.SetActive(false);
        }
    }
    void SetPos(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }
}
