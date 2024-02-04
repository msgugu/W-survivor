using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUtil
{
    private WaitForSeconds blinkterm = new WaitForSeconds(0.1f);
    private Color originColor = new Color(1, 1, 1, 1);
    private Color blinkColor = new Color(1, 1, 1, 0.5f);
    
    public IEnumerator BlinkRoutine(SpriteRenderer sprite)
    {
        while (true)
        {
            sprite.color = blinkColor;
            yield return blinkterm;
            sprite.color = originColor;  
            yield return blinkterm;
        }
    }
}
