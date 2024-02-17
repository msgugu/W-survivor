using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    [SerializeField]
    public Component[] skills;

    //스킬을 반환하는 메소드를 선언합니다.
    public ISkill GetSkill(int index)
    {
        // skills 배열에서 해당 인덱스의 Component를 가져와 ISkill로 변환한 후 반환합니다.
        // 만약 해당 Component가 ISkill 인터페이스를 구현하지 않았다면 null이 반환됩니다.
        return skills[index] as ISkill;
    }
 }
