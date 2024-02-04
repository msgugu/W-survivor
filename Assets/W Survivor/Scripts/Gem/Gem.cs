using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int exp;
    private Rigidbody2D _gemRigid;

    public Rigidbody2D GemRigid
    {
        get { return _gemRigid;}
        set { _gemRigid = value; }
    }
    public void Awake()
    {
        exp = 1;
        _gemRigid = GetComponent<Rigidbody2D>();
    }
}
