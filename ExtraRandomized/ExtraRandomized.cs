using Il2Cpp;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppTMPro;
using System;
using Il2CppInterop.Runtime;

namespace ExtraRandomized;

[RegisterTypeInIl2Cpp]
public class ExtraRandomized : AdvancedMode
{
    public int score { get; set; }
    public int currentLevel { get; set; }
    public new int bestScore { get; set; }
    public int totalLevels { get; set; }
    public int evilKillBonus = 25;
    public int levelCompleteBonus = 50;
    public Il2CppSystem.Action action;
    public Il2CppSystem.Action action2;
    public Il2CppSystem.Action<Character> action3;
    public Il2CppSystem.Action action4;
    private int refreshCounter = 5;

    public override EGameMode bge()
    {
        return EGameMode.Standard;
    }

    public override void bgf()
    {
        GameplayEvents.OnRoundWon += action;
        GameplayEvents.OnDied += action2;
        GameplayEvents.OnCharacterKilled += action3;
        UIEvents.OnUIUpdate += action4;

        GameData.CurrentVillage = this.currentLevel;
    }

    public override GameMode bgg()
    {
        return SaveExRand.exRand;
    }

    public override void bgi()
    {
        GameplayEvents.OnRoundWon -= action;
        GameplayEvents.OnDied -= action2;
        GameplayEvents.OnCharacterKilled -= action3;
        UIEvents.OnUIUpdate -= action4;
    }

    public override int bgj()
    {
        return this.currentLevel;
    }
    public override int bgk()
    {
        return this.currentLevel;
    }

    public override void bgp()
    {
        this.bgi();
        score = 0;
        currentLevel = 0;
        SaveExRand.exRand = this;

        UIEvents.OnUIUpdate.Invoke();
    }

    public override void bgq()
    {
        int currentLevel = this.currentLevel;
        addScore(levelCompleteBonus + 5 * currentLevel);
        GameData.bfs();
        UIEvents.OnUIUpdate.Invoke();

        SaveExRand.exRand = this;
        this.refreshCounter--;
        if (this.refreshCounter <= 0)
        {
            this.refreshCounter = 5;
            SaveExRand.initExRand();
        }
    }

    public override void bgr(int score, int level)
    {
        if (this.currentLevel < level)
        {
            this.currentLevel = level;
        }
    }

    public override int bgt()
    {
        return this.score;
    }

    public override int bgv()
    {
        return this.currentLevel;
    }

    public override bool bgw()
    {
        return false;
    }

    public override string bgx()
    {
        int bestScore = this.bestScore;
        string str1 = string.Format("<size=24>Highest Score: <color=green>{0} </size></color><size=20>", bestScore);
        return str1;
    }

    public override string bgy()
    {
        int currentScore = this.score;
        string result = string.Format("<color=grey><size=20>Score: </color><color=green><size=24>{0}</color>", currentScore);
        return result;
    }

    public void bhb()
    {
        score = 0;
        currentLevel = 1;
        SaveExRand.initExRand();
        GameData.CurrentVillage = 0;

        SaveExRand.exRand = this;
    }

    public bool bhc(int mod = 0)
    {
        int currentLevel = this.currentLevel;
        return currentLevel <= mod + 5;
    }

    public void bhe(Character ch)
    {
        CharacterData dataRef = ch.dataRef;
        if (dataRef.type == ECharacterType.Minion || dataRef.type == ECharacterType.Demon)
        {
            addScore(evilKillBonus);
            UIEvents.OnUIUpdate.Invoke();
        }
    }
    public void editUI()
    {
        GameObject leftUI = GameObject.Find("Game/Gameplay/Content/Canvas/UI/Objectives_Left");
        TMP_Text textScore = leftUI.transform.FindChild("Objective (13)/Bg/Text (TMP)").gameObject.GetComponent<TMP_Text>();
        textScore.text = this.bgy();
        TMP_Text textVillage = leftUI.transform.FindChild("Objective (14) A/Bg/Text (TMP)").gameObject.GetComponent<TMP_Text>();
        textVillage.text = string.Format("<color=grey><size=20>Current Village: </color><color=white><size=24>{0}</color>", this.currentLevel);
    }
    private void addScore(int amt)
    {
        score += amt;
        if (score > bestScore)
        {
            bestScore = score;
        }
    }

    public override AscensionsData bgl()
    {
        return SaveExRand.dataER;
    }

    public override AscensionsData bgm()
    {
        return SaveExRand.dataER;
    }

    public override bool bgn()
    {
        return true;
    }

    public override int bgu()
    {
        int savedMaxStandardAscension = SavesGame.SavedMaxStandardAscension;
        return savedMaxStandardAscension;
    }
    public ExtraRandomized() : base(ClassInjector.DerivedConstructorPointer<ExtraRandomized>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action = new Action(bgq);
        action2 = new Action(bhb);
        action3 = new Action<Character>(bhe);
        action4 = new Action(editUI);
    }

    public ExtraRandomized(IntPtr ptr) : base(ptr)
    {
        action = new Action(bgq);
        action2 = new Action(bhb);
        action3 = new Action<Character>(bhe);
        action4 = new Action(editUI);
    }
}