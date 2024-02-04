using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollector : MonoBehaviour
{
    [SerializeField]
    private GridManager gridManager;
    [SerializeField]
    private GemPoolManager gemPoolManager;
    
    [SerializeField]
    private Transform playerTrans;
    private WaitForSeconds collectCooldown;
    private WaitForFixedUpdate waitfixed;
    [SerializeField]
    private float collectRange;

    private float baseGemSpeed;
    
    private List<Gem> gemsInRange;
    public int gemNumsInRange;
    
    private void Start()
    {
        gemPoolManager = InGameManager.Instance.gemPoolManager;
        playerTrans = InGameManager.Instance.player.transform;
        collectCooldown = new WaitForSeconds(0.3f);
        waitfixed = new WaitForFixedUpdate();
        collectRange = 5.0f;
        baseGemSpeed = 1.0f;

        StartCoroutine(GemCollectRoutine());
    }

    IEnumerator GemCollectRoutine()
    {
        while (!InGameManager.Instance.isPaused)
        {
            Vector3 playerPos = playerTrans.position;
            gemsInRange = gridManager.GatherGems(playerPos, collectRange);
            gemNumsInRange = gemsInRange.Count;
            foreach (Gem gem in gemsInRange)
            {
                gridManager.RemoveFromCell(gem);
                StartCoroutine(GemMoveRoutine(playerTrans, gem));
            }
            yield return collectCooldown;
        }
    }

    IEnumerator GemMoveRoutine(Transform playerTrans, Gem gem)
    {
        bool isDone = true;
        float gemSpeed = baseGemSpeed;
        while (isDone)
        {
            Vector2 dir = (Vector2)playerTrans.position - gem.GemRigid.position;
            if (dir.sqrMagnitude <= 0.5f)
            {
                gemPoolManager.GemDespawn(gem);
                InGameManager.Instance.GetExp(gem.exp);
                isDone = false;
                yield break;
            }

            gemSpeed *= 1.1f;
            Vector2 newPos = dir * gemSpeed;

            gem.GemRigid.velocity = newPos;
            yield return waitfixed;
        }
    }
    
}
