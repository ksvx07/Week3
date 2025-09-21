using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit
{
    public string name;
    public int AttackPower;
    public int MaxHP;
    public int Life;

    public int CurrentHP { get; private set; }

    public Unit(string name, int attackPower, int maxHP, int life)
    {
        this.name = name;
        AttackPower = attackPower;
        MaxHP = maxHP;
        CurrentHP = maxHP;
        Life = life;
    }

    public void Attack(Unit target)
    {
        target.TakeDamage(AttackPower);
    }

    public void AttackAll(List<Unit> targets)
    {
        foreach (Unit target in targets)
        {
            target.TakeDamage(AttackPower / 2);
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP < 0)
            CurrentHP = 0;

        Debug.Log($"{name} took {damage} damage! (HP: {CurrentHP}/{MaxHP})");
    }
    public bool IsDead()
    {
        return CurrentHP <= 0;
    }

    public void Revive()
    {
        if (Life > 0)
        {
            Life--;
            CurrentHP = MaxHP;
        }
    }

    public Unit Clone()
    {
        Unit clone = new Unit(this.name, this.AttackPower, this.MaxHP, this.Life);
        clone.CurrentHP = this.CurrentHP; // 현재 체력까지 복사 (private set은 클래스 내부라 접근 가능)
        return clone;
    }
}
