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

    public override EGameMode bha()
    {
        return EGameMode.Standard;
    }

    public override void bhb()
    {
        GameplayEvents.OnRoundWon += action;
        GameplayEvents.OnDied += action2;
        GameplayEvents.OnCharacterKilled += action3;
        UIEvents.OnUIUpdate += action4;

        GameData.CurrentVillage = this.currentLevel;
    }

    public override GameMode bhc()
    {
        return SaveExRand.exRand;
    }

    public override void bhe()
    {
        GameplayEvents.OnRoundWon -= action;
        GameplayEvents.OnDied -= action2;
        GameplayEvents.OnCharacterKilled -= action3;
        UIEvents.OnUIUpdate -= action4;
    }

    public override int bhf()
    {
        return this.currentLevel;
    }
    public override int bhg()
    {
        return this.currentLevel;
    }

    public override void bhl()
    {
        this.bhe();
        score = 0;
        currentLevel = 0;
        SaveExRand.exRand = this;

        UIEvents.OnUIUpdate.Invoke();
    }

    public override void bhm()
    {
        int currentLevel = this.currentLevel;
        addScore(levelCompleteBonus + 5 * currentLevel);
        // GameData.bfs();
        UIEvents.OnUIUpdate.Invoke();

        SaveExRand.exRand = this;
        this.refreshCounter--;
        if (this.refreshCounter <= 0)
        {
            this.refreshCounter = 5;
            SaveExRand.initExRand();
        }
    }

    public override void bhn(int score, int level)
    {
        if (this.currentLevel < level)
        {
            this.currentLevel = level;
        }
    }

    public override int bho()
    {
        return this.score;
    }

    public override int bhq()
    {
        return this.currentLevel;
    }

    public override bool bhr()
    {
        return false;
    }

    public override string bhs()
    {
        int bestScore = this.bestScore;
        string str1 = string.Format("<size=24>Highest Score: <color=green>{0} </size></color><size=20>", bestScore);
        return str1;
    }

    public override string bht()
    {
        int currentScore = this.score;
        string result = string.Format("<color=grey><size=20>Score: </color><color=green><size=24>{0}</color>", currentScore);
        return result;
    }

    public void bhx()
    {
        score = 0;
        currentLevel = 1;
        SaveExRand.initExRand();
        GameData.CurrentVillage = 0;

        SaveExRand.exRand = this;
    }

    public bool bhy(int mod = 0)
    {
        int currentLevel = this.currentLevel;
        return currentLevel <= mod + 5;
    }

    public void bia(Character ch)
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
        GameObject objScore2 = SaveExRand.objScore2;
        GameObject objCurrentVillage2 = SaveExRand.objCurrentVillage2;
        if (objScore2 == null)
        {
            return;
        }
        TMP_Text textScore = objScore2.transform.FindChild("Bg/Text (TMP)").gameObject.GetComponent<TMP_Text>();
        TMP_Text textVillage = objCurrentVillage2.transform.FindChild("Bg/Text (TMP)").gameObject.GetComponent<TMP_Text>();
        if (textScore != null)
        {
            textScore.text = this.bht();
            textVillage.text = string.Format("<color=grey><size=20>Current Village: </color><color=white><size=24>{0}</color>", this.currentLevel);
        }
    }
    private void addScore(int amt)
    {
        score += amt;
        if (score > bestScore)
        {
            bestScore = score;
        }
    }

    public override AscensionsData bhh()
    {
        return SaveExRand.dataER;
    }

    public override AscensionsData bhi()
    {
        return SaveExRand.dataER;
    }

    public override bool bhj()
    {
        return true;
    }

    public override int bhp()
    {
        int savedMaxStandardAscension = SavesGame.SavedMaxStandardAscension;
        return savedMaxStandardAscension;
    }
    public ExtraRandomized() : base(ClassInjector.DerivedConstructorPointer<ExtraRandomized>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action = new Action(bhm);
        action2 = new Action(bhx);
        action3 = new Action<Character>(bia);
        action4 = new Action(editUI);
    }

    public ExtraRandomized(IntPtr ptr) : base(ptr)
    {
        action = new Action(bhm);
        action2 = new Action(bhx);
        action3 = new Action<Character>(bia);
        action4 = new Action(editUI);
    }
}