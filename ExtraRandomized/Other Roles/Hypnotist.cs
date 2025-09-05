using Il2Cpp;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;

namespace ExtraRandomized;

[RegisterTypeInIl2Cpp]
public class Hypnotist : Demon
{
    public override string Description
    {
        get
        {
            return "1 random Villager becomes an unknown Minion.";
        }
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Start)
        {
            return;
        }
        Characters instance = Characters.Instance;
        Gameplay gameplay = Gameplay.Instance;
        List<Character> characters = Gameplay.CurrentCharacters;
        List<Character> list1 = new List<Character>();
        foreach (Character character in characters)
        {
            list1.Add(character);
        }
        List<CharacterData> list2 = gameplay.mr();
        List<CharacterData> list3 = instance.hh(list2);
        List<CharacterData> list4 = instance.gw(list3, ECharacterType.Minion);
        if (list4.Count == 0)
        {
            return;
        }
        CharacterData randomData = list4[UnityEngine.Random.RandomRangeInt(0, list4.Count)];
        // gameplay.ml(ECharacterType.Minion, randomData);
        List<Character> list5 = instance.hi(list1);
        List<Character> list6 = instance.gs(list5, ECharacterType.Villager);
        if (list6.Count == 0)
        {
            return;
        }
        Character randomCharacter = list6[UnityEngine.Random.RandomRangeInt(0, list6.Count)];
        randomCharacter.statuses.fm(ECharacterStatus.MessedUpByEvil, charRef, null);
        randomCharacter.dv(randomData);
    }
    public Hypnotist() : base(ClassInjector.DerivedConstructorPointer<Hypnotist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Hypnotist(IntPtr ptr) : base(ptr)
    {
        
    }
}