using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class GemPoolManager : PoolingManager
{
    public List<IPool<Component>> GemPools => Pools;

    public int maxGemNum = 500;
    public int currGemNum = 0;
    public Gem[] gemPrfabs;
    public Transform gemContainer;
    
    protected override void Awake()
    {
        base.Awake();
        AddPool<Gem>(gemPrfabs[0], maxGemNum, gemContainer, false);
    }

    public Gem GemSpawn(Vector3 pos)
    {
        Gem gem = GetFromPool<Gem>(0, pos);
        if (gem == null)
            return null;
        gridManager.AddToCell<Gem>(gem); //redundant call of gem.transform
        currGemNum++;
        
        return gem;
    }

    public void GemDespawn(Gem gemClone)
    {
        //gridManager.RemoveFromCell<Gem>(gemClone);
        ReturnPool<Gem>(0, gemClone);
        currGemNum--;
    }
    
}
