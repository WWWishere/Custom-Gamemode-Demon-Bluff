using Il2Cpp;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;

namespace ExtraRandomized;

[RegisterTypeInIl2Cpp]
public class Baatender : Imp
{
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Start)
        {
            return;
        }
        Gameplay gameplay = Gameplay.Instance;
        Characters instance = Characters.Instance;
        List<CharacterData> list1 = gameplay.mr();
        List<CharacterData> list2 = instance.hh(list1);
        List<CharacterData> list3 = instance.gp(list2, ECharacterType.Outcast);
        if (list3.Count > 0)
        {
            CharacterData randomData = list3[UnityEngine.Random.RandomRangeInt(0, list3.Count)];
            gameplay.ml(ECharacterType.Outcast, randomData);
        }
        List<Character> list4 = new List<Character>();
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            list4.Add(character);
        }
        List<Character> list5 = instance.hi(list4);
        List<Character> list6 = instance.gs(list5, ECharacterType.Villager);
        if (list6.Count > 0)
        {
            CharacterData? drunkData = SaveExRand.chString("Drunk");
            Character randomCharacter = list6[UnityEngine.Random.RandomRangeInt(0, list6.Count)];
            if (drunkData != null)
            {
                randomCharacter.dv(drunkData);
            }
        }
    }
    public Baatender() : base(ClassInjector.DerivedConstructorPointer<Baatender>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Baatender(IntPtr ptr) : base(ptr)
    {

    }
}