using Il2Cpp;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;

namespace ExtraRandomized;

// [RegisterTypeInIl2Cpp]
public class Joker : Role
{
    public override string Description
    {
        get
        {
            return "If executed, all characters who match my disguise will die.";
        }
    }
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        MelonLogger.Msg("Joker bcs called: " + trigger);
    }
    public override CharacterData bcz(Character charRef)
    {
        Characters instance = Characters.Instance;
        Gameplay gameplay = Gameplay.Instance;
        List<CharacterData> uniquePool = instance.UniquePool;
        List<CharacterData> currentTown = gameplay.currentTownsfolks;
        foreach (CharacterData data in currentTown)
        {
            uniquePool.Add(data);
        }
        MelonLogger.Msg("Logging uniquePool:");
        foreach (CharacterData data1 in uniquePool)
        {
            MelonLogger.Msg(data1.name);
        }
        List<CharacterData> townPool = instance.gw(uniquePool, ECharacterType.Villager);
        MelonLogger.Msg("Logging townPool:");
        foreach (CharacterData data2 in townPool)
        {
            MelonLogger.Msg(data2.name);
        }
        CharacterData randomData = townPool[UnityEngine.Random.RandomRangeInt(0, townPool.Count)];
        return randomData;
    }
    public override void bcx(ETriggerPhase trigger, Character charRef)
    {
        
    }
    public override ActedInfo bcr(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public void executeDupe(Character charRef)
    {
        CharacterData bluff = charRef.bluff;
        if (bluff == null)
        {
            return;
        }
        int id = charRef.id;
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            if (character.dq() == bluff && character.id != id)
            {
                character.es();
            }
        }
    }

    public Joker() : base(ClassInjector.DerivedConstructorPointer<Joker>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Joker(IntPtr ptr) : base(ptr)
    {

    }
}