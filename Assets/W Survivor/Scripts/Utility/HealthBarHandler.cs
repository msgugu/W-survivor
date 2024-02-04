using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject bar;
    public Enums.CharacterType characterType;
    
    private Transform _barTransform;
    private Vector3 _tempVec3;
    private SpriteRenderer[] _barSprites;
    private int _spritesNum;

    private void Awake()
    {
        _tempVec3 = new Vector3(1,1,1);
    }

    private void Start()
    {
        _barTransform = bar.GetComponent<Transform>();
        _barSprites = GetComponentsInChildren<SpriteRenderer>();
        _spritesNum = _barSprites.Length;
        SetHealthBar(1);
    }

    public void SetHealthBar(float ratio)
    {
        if (ratio < 0) ratio = 0;
        _tempVec3.x = ratio;

        if (characterType == Enums.CharacterType.Player)
        {
            PlayerHealthBar(ratio);
        }
        else if (characterType == Enums.CharacterType.Enemy)
        {
            EnemyHealthBar(ratio);
        }

    }

    private void PlayerHealthBar(float ratio)
    {
        _barTransform.localScale = _tempVec3;
    }
    
    private void EnemyHealthBar(float ratio)
    {
        if (Mathf.Approximately(ratio, 0) || Mathf.Approximately(ratio, 1))
        {
            HealthBarOff();
        }
        else
        {
            HealthBarOn();
            _barTransform.localScale = _tempVec3;
        }
    }
    

    private void HealthBarOn()
    {
        for (int i=0; i<_spritesNum; i++)
        {
            _barSprites[i].enabled = true;
        }
    }

    private void HealthBarOff()
    {
        for (int i=0; i<_spritesNum; i++)
        {
            _barSprites[i].enabled = false;
        }
    }
    
    
}
