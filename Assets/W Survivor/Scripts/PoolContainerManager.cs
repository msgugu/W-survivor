using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class PoolContainerManager : MonoBehaviour
{
    private List<Transform> _poolContainers;
    //public List<Transform> PoolContainers => _poolContainers;
    public string defaultContainerName;
    public int ContainerNum => _poolContainers.Count;

    private void Awake()
    {
        _poolContainers = new();
    }

    public Transform AddContainer()
    {
        Transform newContainer = new GameObject(defaultContainerName + ContainerNum).transform;
        newContainer.SetParent(this.transform);
        _poolContainers.Add(newContainer);

        return newContainer;
    }

    public Transform GetContainer(int index)
    {
        return _poolContainers[index];
    }
}
