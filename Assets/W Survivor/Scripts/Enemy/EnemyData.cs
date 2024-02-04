using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public Vector3 enemyScale { get; set; }
    public SpriteRenderer enemySpriteRenderer { get; set; }
    public RuntimeAnimatorController enemyAnimatorController { get; set; }
    public CapsuleCollider2D enemyCollider { get; set; }
    
    public EnemyData(Vector3 scale ,SpriteRenderer spriteRenderer, RuntimeAnimatorController animatorController, CapsuleCollider2D collider)
    {
        enemyScale = scale;
        enemySpriteRenderer = spriteRenderer;
        enemyAnimatorController = animatorController;
        enemyCollider = collider;
    }
}
