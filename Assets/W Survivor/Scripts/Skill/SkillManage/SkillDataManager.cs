using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    [SerializeField]
    public Component[] skills;

    //��ų�� ��ȯ�ϴ� �޼ҵ带 �����մϴ�.
    public ISkill GetSkill(int index)
    {
        // skills �迭���� �ش� �ε����� Component�� ������ ISkill�� ��ȯ�� �� ��ȯ�մϴ�.
        // ���� �ش� Component�� ISkill �������̽��� �������� �ʾҴٸ� null�� ��ȯ�˴ϴ�.
        return skills[index] as ISkill;
    }
 }
