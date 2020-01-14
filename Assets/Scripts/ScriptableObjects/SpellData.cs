
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SanityBlade/Spell")]
public class SpellData : ScriptableObject
{
    public float Damage;
    public float Speed;
    public float FireRate;
    public Sprite Sprite;
}

