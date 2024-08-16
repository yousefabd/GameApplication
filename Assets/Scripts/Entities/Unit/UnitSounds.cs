using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSounds : MonoBehaviour
{
    [SerializeField] AudioClip[] attackSounds;
    [SerializeField] Unit unit;

    private void Start()
    {
        if(unit is Soldier)
        {
            (unit as Soldier).OnNormalAttack += UnitSounds_OnAttack;
            (unit as Soldier).OnRangedAttack += UnitSounds_OnAttack;
        }
    }

    private void UnitSounds_OnAttack(Vector3 obj)
    {
        AudioSource.PlayClipAtPoint(attackSounds[Random.Range(0, attackSounds.Length)], unit.transform.position, 1f);
        Debug.Log("sound");
    }
}
