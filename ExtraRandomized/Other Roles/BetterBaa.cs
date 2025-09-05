using Il2Cpp;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;

namespace ExtraRandomized;

[RegisterTypeInIl2Cpp]
public class BetterBaa : Demon
{
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Gameplay gameplay = Gameplay.Instance;
            Characters instance = Characters.Instance;
            List<CharacterData> list1 = gameplay.mr();
            List<CharacterData> list2 = instance.hh(list1);
            List<CharacterData> list3 = instance.gw(list2, ECharacterType.Minion);
            if (list3.Count == 0)
            {
                return;
            }
            CharacterData randomData = list3[UnityEngine.Random.RandomRangeInt(0, list3.Count)];
            gameplay.ml(ECharacterType.Minion, randomData);
        }
    }
    public BetterBaa() : base(ClassInjector.DerivedConstructorPointer<BetterBaa>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public BetterBaa(IntPtr ptr) : base(ptr)
    {

    }
}