using Il2Cpp;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;

namespace ExtraRandomized;

[RegisterTypeInIl2Cpp]
public class Psychic : Role
{
    public override string Description
    {
        get
        {
            return "Learn 1 Truthful character";
        }
    }

    public override ActedInfo bbw(Character charRef)
    {
        List<Character> characters = Gameplay.CurrentCharacters;
        System.Collections.Generic.List<Character> newList = new System.Collections.Generic.List<Character>();
        List<Character> selection = new List<Character>();
        Characters charInst = Characters.Instance;
        foreach (Character character in characters)
        {
            bool corrupted = false;
            bool evil = false;
            if (character.statuses != null)
            {
                corrupted = character.statuses.fn(ECharacterStatus.Corrupted) && character.dataRef.name != "Confessor";
                evil = character.bluff != null && !character.statuses.fn(ECharacterStatus.HealthyBluff);
            }
            if (!(corrupted || evil))
            {
                newList.Add(character);
            }
        }
        Character random = newList[UnityEngine.Random.RandomRangeInt(0, newList.Count)];
        string line = string.Format("#{0} is Truthful", random.id);
        selection.Add(random);
        ActedInfo actedInfo = new ActedInfo(line, selection);
        return actedInfo;
    }

    public override void bby(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            this.onActed.Invoke(this.bbw(charRef));
        }
    }

    public override void bcd(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            this.onActed.Invoke(this.bbx(charRef));
        }
    }

    public override ActedInfo bbx(Character charRef)
    {
        List<Character> characters = Gameplay.CurrentCharacters;
        System.Collections.Generic.List<Character> newList = new System.Collections.Generic.List<Character>();
        List<Character> selection = new List<Character>();
        Characters charInst = Characters.Instance;
        foreach (Character character in characters)
        {
            bool corrupted = false;
            bool evil = false;
            if (character.statuses != null)
            {
                corrupted = character.statuses.fn(ECharacterStatus.Corrupted) && character.dataRef.name != "Confessor";
                evil = character.bluff != null && !character.statuses.fn(ECharacterStatus.HealthyBluff);
            }
            if (corrupted || evil)
            {
                newList.Add(character);
            }
        }
        Character random = newList[UnityEngine.Random.RandomRangeInt(0, newList.Count)];
        string line = string.Format("#{0} is Truthful", random.id);
        selection.Add(random);
        ActedInfo actedInfo = new ActedInfo(line, selection);
        return actedInfo;
    }
    
    public Psychic() : base(ClassInjector.DerivedConstructorPointer<Psychic>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Psychic(IntPtr ptr) : base(ptr)
    {
        
    }
}