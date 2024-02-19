
using UnityEngine;
using Redcode.Pools;

public interface ISkill
{
    int SkillID { get; set; }
    int PoolNum { get; set; }
    int[] PoolIndexes { get; set; }
    Bullet[] SkillBullets { get; set; }
    Transform[] BulletContainers { get; set; }
    BulletPoolManager BulletPoolM { get; set; }
    
    float Damage { get; set; }
    int Count { get; set; }
    float Speed { get; set; }
    int Pierce { get; set; }
    float Cooldown { get; set; }
    float Duration {  get; set; }
    
    public void InitSkill();
    public void UseSkill();
    public void UpgradeSkill();
}
