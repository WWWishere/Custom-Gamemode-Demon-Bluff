using Il2Cpp;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;

namespace ExtraRandomized;

[RegisterTypeInIl2Cpp]
public class Cleric : Role
{
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override ActedInfo bcr(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            Health health = PlayerController.PlayerInfo.health;
            if (charRef.alignment == EAlignment.Evil)
            {
                health.jl(3);
                if (charRef.state != ECharacterState.Dead)
                {
                    charRef.en();
                    charRef.ep();
                }
            }
            else if (charRef.statuses.statuses.Contains(ECharacterStatus.Corrupted))
            {

            }
            else
            {
                health.jl(-3);
                int healthCount = health.value.jw();
                if (healthCount > 10)
                {
                    health.jl(healthCount - 10);
                } 
            }
            charRef.uses--;
            if (charRef.uses <= 0)
            {
                charRef.pickable.SetActive(false);
            }
        }
    }
    public Cleric() : base(ClassInjector.DerivedConstructorPointer<Cleric>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Cleric(IntPtr ptr) : base(ptr)
    {

    }
}