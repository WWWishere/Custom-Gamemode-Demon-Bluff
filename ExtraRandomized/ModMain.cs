using MelonLoader;
using UnityEngine;
using UnityEditor;
using System;
using System.ComponentModel;
using ExtraRandomized;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Runtime.ExceptionServices;
using System.Numerics;
using Il2CppInterop.Runtime.Runtime;
using System.Reflection;

[assembly: MelonInfo(typeof(ModMain), "ExtraRandomized", "0.3", "SS122")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace ExtraRandomized;

public class ModMain : MelonMod
{
    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<ExtraRandomized>();
        ClassInjector.RegisterTypeInIl2Cpp<Psychic>();
        ClassInjector.RegisterTypeInIl2Cpp<Purifier>();
        // ClassInjector.RegisterTypeInIl2Cpp<Hypnotist>();
    }

    public override void OnLateInitializeMelon()
    {
        Application.runInBackground = true;
        SaveExRand.allSprites = Resources.FindObjectsOfTypeAll<Sprite>();
        CharacterData psychic = SaveExRand.createCharData("Psychic", "2 1", ECharacterType.Villager,
        EAlignment.Good, new Psychic());
        psychic.bluffable = true;
        psychic.description = "Learn 1 Truthful character";
        psychic.flavorText = "Sees her friends' darkest secrets.\nUses it to know what food to get them.";
        psychic.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        SaveExRand.customCharList.Add(psychic);
        
        CharacterData purifier = SaveExRand.createCharData("Purifier", "evil", ECharacterType.Minion,
        EAlignment.Evil, new Purifier());
        purifier.bluffable = false;
        purifier.description = "Removes Corruption from adjacent characters.\n\nI Lie and Disguise.";
        purifier.flavorText = "No one dares to enter his lab.";
        purifier.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        purifier.tags.Add(ECharacterTag.Corrupt);
        SaveExRand.customCharList.Add(purifier);
        Characters.Instance.startGameActOrder = insertAfterAct("Alchemist", purifier);
        /*
        CharacterData hypno = SaveExRand.createCharData("Hypnotist", "evil", ECharacterType.Demon,
        EAlignment.Evil, new Hypnotist());
        hypno.bluffable = false;
        hypno.description = "<b>Game Start:</b>\nOne random Villager becomes an unknown Minion.\n\nI Lie and Disguise.";
        hypno.flavorText = "You look nice today. How about we go out and have a lunch together?";
        hypno.tags = new Il2CppSystem.Collections.Generic.List<ECharacterTag>();
        SaveExRand.customCharList.Add(hypno);
        Characters.Instance.startGameActOrder = insertAfterAct("Baa", hypno);
        */
        SaveExRand.leftUI = GameObject.Find("Game/Gameplay/Content/Canvas/UI/Objectives_Left");
        SaveExRand.objScore2 = SaveExRand.leftUI.transform.FindChild("Objective (13)").gameObject;
        SaveExRand.objCurrentVillage2 = SaveExRand.leftUI.transform.FindChild("Objective (14) A").gameObject;
        GameObject circle2 = SaveExRand.createCircle(2);
        SaveExRand.addToCharsPool(circle2.GetComponent<CharactersPool>());
        GameObject circle3 = SaveExRand.createCircle(3);
        SaveExRand.addToCharsPool(circle3.GetComponent<CharactersPool>());
        GameObject circle4 = SaveExRand.createCircle(4);
        SaveExRand.addToCharsPool(circle4.GetComponent<CharactersPool>());
        GameObject circle15 = SaveExRand.createCircle(15);
        SaveExRand.addToCharsPool(circle15.GetComponent<CharactersPool>());
        GameObject circle14 = SaveExRand.createCircle(14);
        SaveExRand.addToCharsPool(circle14.GetComponent<CharactersPool>());
        GameObject circle13 = SaveExRand.createCircle(13);
        SaveExRand.addToCharsPool(circle13.GetComponent<CharactersPool>());
        GameObject circle12 = SaveExRand.createCircle(12);
        SaveExRand.addToCharsPool(circle12.GetComponent<CharactersPool>());
    }

    public override void OnUpdate()
    {
        if (SaveExRand.charList.Length == 0)
        {
            SaveExRand.charList = Resources.FindObjectsOfTypeAll<CharacterData>();
            if (SaveExRand.charList.Length > 0)
            {
                SaveExRand.initExRand();
                SaveExRand.dataER.bbt();
            }
        }
        // Temp start Game option
        if (Input.GetKeyDown(KeyCode.M))
        {
            SaveExRand.initExRand();
            GameData.bgk(SaveExRand.exRand);
            SaveExRand.objScore2.GetComponent<EnableOnMode>().enabled = false;
            SaveExRand.objCurrentVillage2.GetComponent<EnableOnMode>().enabled = false;
            SaveExRand.objScore2.SetActive(true);
            SaveExRand.objCurrentVillage2.SetActive(true);
        }
        if (SaveExRand.objScore2 != null)
        {
            if (SaveExRand.objScore2.GetComponent<EnableOnMode>().enabled == false)
            {
                if (GameData.GameMode.GetType() != typeof(ExtraRandomized))
                {
                    SaveExRand.objScore2.GetComponent<EnableOnMode>().enabled = true;
                    SaveExRand.objCurrentVillage2.GetComponent<EnableOnMode>().enabled = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SaveExRand.limitPoolSize = !SaveExRand.limitPoolSize;
            string toggle = "on";
            if (SaveExRand.limitPoolSize)
            {
                toggle = "off";
            }
            LoggerInstance.Msg("Extra pool sizes toggled: " + toggle);
        }
    }
    public CharacterData[] insertAfterAct(string previous, CharacterData data)
    {
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        bool inserted = false;
        for (int i = 0; i < actSize; i++)
        {
            if (inserted)
            {
                newActList[i + 1] = actList[i];
            }
            else
            {
                newActList[i] = actList[i];
                if (actList[i].name == previous)
                {
                    newActList[i + 1] = data;
                    inserted = true;
                }
            }
        }
        if (!inserted)
        {
            LoggerInstance.Msg("");
        }
        return newActList;
    }
}

public static class SaveExRand
{
    public static Sprite[] allSprites = Array.Empty<Sprite>();
    public static CharacterData[] charList = Array.Empty<CharacterData>();
    public static List<CharacterData> customCharList = new List<CharacterData>();
    public static AscensionsData dataER = UnityEngine.Object.Instantiate(ProjectContext.Instance.gameData.advancedAscension);
    public static AscensionsData dataBase = UnityEngine.Object.Instantiate(ProjectContext.Instance.gameData.advancedAscension);
    public static ExtraRandomized exRand = new ExtraRandomized();
    public static List<string> useUnused = new List<string> { };
    public static List<string> poolUnused = new List<string> { "Mutant", "Wretch", "Marionette", "Puppet", "Saint", "Bounty Hunter" };
    public static List<int> pool = new List<int>();
    public static List<int> poolEvil = new List<int>();
    public static GameObject leftUI;
    public static GameObject objScore2;
    public static GameObject objCurrentVillage2;
    public static bool limitPoolSize = true;

    public static void initExRand()
    {
        pool.Clear();
        poolEvil.Clear();
        for (int i = 0; i < charList.Length; i++)
        {
            if (!poolUnused.Contains(charList[i].name) || useUnused.Contains(charList[i].name))
            {
                CharacterData data = charList[i];
                if (data.startingAlignment == EAlignment.Evil)
                {
                    poolEvil.Add(i);
                }
                pool.Add(i);
            }
        }
        AscensionsData ascensionsData = dataBase;
        dataER.possibleScriptsData = new CustomScriptData[9];
        int j = 0;
        foreach (CustomScriptData customScriptData in ascensionsData.possibleScriptsData)
        {
            CustomScriptData newData = UnityEngine.Object.Instantiate(customScriptData);
            ScriptInfo script = new ScriptInfo();
            script.startingTownsfolks = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            script.startingOutsiders = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            script.startingMinions = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            script.startingDemons = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (int roleNum in pool)
            {
                CharacterData roleData = charList[roleNum];
                switch (roleData.type)
                {
                    case ECharacterType.Villager:
                        script.startingTownsfolks.Add(roleData);
                        break;
                    case ECharacterType.Outcast:
                        script.startingOutsiders.Add(roleData);
                        break;
                    case ECharacterType.Minion:
                        script.startingMinions.Add(roleData);
                        break;
                    case ECharacterType.Demon:
                        script.startingDemons.Add(roleData);
                        break;
                }
            }
            script.characterCounts = new Il2CppSystem.Collections.Generic.List<CharactersCount>();
            int minSize = 6;
            int maxSize = 11;
            if (!limitPoolSize)
            {
                minSize = 2;
                maxSize = 15;
            }
            for (int m = minSize; m < maxSize + 1; m++)
            {
                int[] roleCounts = randRoleCount(m);
                CharactersCount newCharCount = new CharactersCount(m, roleCounts[0], roleCounts[1], roleCounts[2], roleCounts[3]);
                script.characterCounts.Add(newCharCount);
            }
            newData.scriptInfo = script;
            dataER.possibleScriptsData[j] = newData;
            j++;
        }
    }
    public static int[] randRoleCount(int count)
    {
        int[] counts = { count, 0, 0, 0 };
        List<int> tempPool = new List<int>(pool);
        List<int> tempPoolEvil = new List<int>(poolEvil);
        int evil = tempPool[UnityEngine.Random.RandomRangeInt(0, tempPoolEvil.Count)];
        if (charList[evil].type == ECharacterType.Minion)
        {
            counts[3]++;
        }
        else
        {
            counts[1]++;
        }
        tempPool.Remove(evil);
        counts[0]--;
        for (int i = 0; i < count - 1; i++)
        {
            int randomRole = tempPool[UnityEngine.Random.RandomRangeInt(0, tempPool.Count)];
            switch (charList[randomRole].type)
            {
                case ECharacterType.Outcast:
                    counts[2]++;
                    counts[0]--;
                    break;
                case ECharacterType.Minion:
                    counts[3]++;
                    counts[0]--;
                    break;
                case ECharacterType.Demon:
                    counts[1]++;
                    counts[0]--;
                    break;
                default:
                    break;
            }
            tempPool.Remove(randomRole);
        }
        return counts;
    }
    static GameObject circChar = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character");
    static GameObject circCharLeft = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (1)");
    static GameObject circCharRight = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (4)");
    static GameObject circCharDown = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (3)");
    public static GameObject createCircle(int size)
    {
        GameObject circle = new GameObject();
        circle.name = "Circle_" + size;
        circle.transform.SetParent(GameObject.Find("Game/Gameplay/Content/Canvas/Characters").transform);
        RectTransform circleRect = circle.AddComponent<RectTransform>();
        CharactersPool circle15Pool = circle.AddComponent<CharactersPool>();
        circle15Pool.characters = new Character[size];
        for (int i = 0; i < size; i++)
        {
            GameObject card;
            int rotation = 360 * i / size;
            if (rotation <= 30)
            {
                card = GameObject.Instantiate(circChar);
            }
            else if (rotation <= 149)
            {
                card = GameObject.Instantiate(circCharLeft);
                rotation += 300;
            }
            else if (rotation <= 210)
            {
                card = GameObject.Instantiate(circCharDown);
                rotation += 180;
            }
            else if (rotation <= 329)
            {
                card = GameObject.Instantiate(circCharRight);
                rotation += 120;
            }
            else
            {
                card = GameObject.Instantiate(circChar);
            }
            card.transform.SetParent(circle.transform);
            string cardname = "Character";
            if (i > 0)
            {
                cardname += " (" + i + ")";
            }
            card.name = cardname;
            Transform icon = card.transform.Find("Icon");
            card.transform.Rotate(0, 0, rotation);
            icon.Rotate(0, 0, 360 - rotation);
            circle15Pool.characters[i] = card.GetComponent<Character>();
        }
        circle.transform.position = new UnityEngine.Vector3(0f, 1f, 85.9444f);
        circle.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
        circle.SetActive(false);
        return circle;
    }
    public static void addToCharsPool(CharactersPool pool)
    {
        CharactersPool[] pools = Characters.Instance.characterPool;
        CharactersPool[] newPools = new CharactersPool[pools.Length + 1];
        for (int i = 0; i < pools.Length; i++)
        {
            newPools[i] = pools[i];
        }
        newPools[pools.Length] = pool;
        Characters.Instance.characterPool = newPools;
    }
    public static CharacterData? chString(string name)
    {
        foreach (CharacterData data in charList)
        {
            if (data.name == name)
            {
                return data;
            }
        }
        MelonLogger.Msg("Couldn't find CharacterData with name \"" + name + "\"!");
        return null;
    }
    public static Sprite? findSprite(string name)
    {
        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name == name)
            {
                return sprite;
            }
        }
        MelonLogger.Msg("Couldn't find Sprite with name \"" + name + "\"!");
        return null;
    }
    public static CharacterData createCharData(string name, string bgName, ECharacterType type, EAlignment alignment, Role role, bool picking = false, EAbilityUsage abilityUsage = EAbilityUsage.Once)
    {
        // unadded: tags, description, notes, bluffable
        CharacterData newData = new CharacterData();
        newData.name = name;
        newData.abilityUsage = abilityUsage;
        newData.backgroundArt = findSprite(bgName);
        newData.bundledCharacters = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        newData.canAppearIf = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        switch (type)
        {
            case ECharacterType.Villager:
                newData.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
                newData.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
                newData.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
                newData.color = new Color(1f, 0.935f, 0.7302f);
                break;
            case ECharacterType.Outcast:
                newData.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
                newData.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
                newData.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
                newData.color = new Color(0.9659f, 1f, 0.4472f);
                break;
            case ECharacterType.Minion:
                newData.artBgColor = new Color(1f, 0f, 0f);
                newData.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
                newData.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
                newData.color = new Color(0.8491f, 0.4555f, 0f);
                break;
            case ECharacterType.Demon:
                newData.artBgColor = new Color(1f, 0f, 0f);
                newData.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
                newData.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
                newData.color = new Color(1f, 0.3811f, 0.3811f);
                break;
        }
        newData.descriptionCHN = "";
        newData.descriptionPL = "";
        newData.hints = "";
        newData.ifLies = "";
        newData.notes = "";
        newData.picking = picking;
        newData.role = role;
        newData.skins = new Il2CppSystem.Collections.Generic.List<SkinData>();
        newData.startingAlignment = alignment;
        newData.type = type;
        return newData;
    }
}