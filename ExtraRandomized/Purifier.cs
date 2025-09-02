using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using MelonLoader;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ExtraRandomized;

// [RegisterTypeInIl2Cpp]
public class Purifier : Minion
{
    public override string Description
    {
        get
        {
            return "Removes Corruption from adjacent characters.\n\nI Lie and Disguise.";;
        }
    }
    public AlchemistRuntimeData? bdd(Character charRef)
    {
        RuntimeCharacterData runtimeData = charRef.runtimeData;
        if (runtimeData == null)
        {
            return null;
        }
        return new AlchemistRuntimeData(runtimeData.Pointer);
    }
    private void bde(Character charRef)
    {
        MelonLogger.Msg("Checking Purifier at: " + charRef.id);
        List<Character> list = this.bdf(charRef);
        AlchemistRuntimeData alchemistRuntimeData = new AlchemistRuntimeData(list.Count);
        charRef.dj(new RuntimeCharacterData(alchemistRuntimeData.Pointer));
        foreach (Character character in list)
        {
            character.statuses.fn();
        }
        MelonLogger.Msg("Total corruptions found: " + list.Count);
    }
    public List<Character> bdf(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> list1 = CharactersHelper.tl(characters, charRef);
        List<Character> list2 = new List<Character>();
        Character[] neighbors = { list1[1], list1[list1.Count - 1] };
        foreach (Character character in neighbors)
        {
            MelonLogger.Msg("Checking neighbor at: " + character.id);
            if (character.statuses.statuses.Contains(ECharacterStatus.Corrupted))
            {
                list2.Add(character);
                MelonLogger.Msg(character.id + " is Corrupted!");
            }
        }
        return list2;
    }
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("");
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        MelonLogger.Msg("Purifier bcs call: " + trigger);
        if (trigger == ETriggerPhase.Start)
        {
            this.bde(charRef);
        }
    }
    public override void bcx(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            AlchemistRuntimeData alchemistRuntimeData = new AlchemistRuntimeData(0);
            charRef.dj(new RuntimeCharacterData(alchemistRuntimeData.Pointer));
        }
    }
    public Purifier() : base(ClassInjector.DerivedConstructorPointer<Purifier>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Purifier(IntPtr ptr) : base(ptr)
    {

    }
}
