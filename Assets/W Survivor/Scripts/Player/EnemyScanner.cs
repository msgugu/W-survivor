using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScan : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;
    
    private float _timer;

    private void FixedUpdate()
    {
        if (InGameManager.Instance.isPaused)
            return;
        
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform currNearest = null;
        Transform currTarget = null;
        Vector3 playerPosition = transform.position;
        float diff = 100;
        
        for (int i = 0; i < targets.Length; i++)
        {
            currTarget = targets[i].transform;
            Vector3 targetPosition = currTarget.position;
            float currDiff = Vector3.Distance(playerPosition, targetPosition);
            if (currDiff < diff)
            {
                diff = currDiff;
                currNearest = currTarget;
            }
        }

        return currNearest;
    }
    
}
