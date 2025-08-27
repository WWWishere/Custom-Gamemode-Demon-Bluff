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

[assembly: MelonInfo(typeof(ModMain), "ExtraRandomized", "0.1", "SS122")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace ExtraRandomized;

public class ModMain : MelonMod
{
    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<ExtraRandomized>();
    }

    public override void OnLateInitializeMelon()
    {
        Application.runInBackground = true;
        SaveExRand.leftUI = GameObject.Find("Game/Gameplay/Content/Canvas/UI/Objectives_Left");
        SaveExRand.objScore2 = SaveExRand.leftUI.transform.FindChild("Objective (13)").gameObject;
        SaveExRand.objCurrentVillage2 = SaveExRand.leftUI.transform.FindChild("Objective (14) A").gameObject;
    }

    public override void OnUpdate()
    {
        if (SaveExRand.charList.Length == 0)
        {
            SaveExRand.charList = Resources.FindObjectsOfTypeAll<CharacterData>();
            if (SaveExRand.charList.Length > 0)
            {
                SaveExRand.initExRand();
                SaveExRand.dataER.bbd();
            }
        }
        // Temp start Game option
        if (Input.GetKeyDown(KeyCode.M))
        {
            SaveExRand.initExRand();
            GameData.bfq(SaveExRand.exRand);
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
    }


}

public static class SaveExRand
{
    public static CharacterData[] charList = Array.Empty<CharacterData>();
    public static AscensionsData dataER = UnityEngine.Object.Instantiate(ProjectContext.Instance.gameData.advancedAscension);
    public static AscensionsData dataBase = UnityEngine.Object.Instantiate(ProjectContext.Instance.gameData.advancedAscension);
    public static ExtraRandomized exRand = new ExtraRandomized();
    public static List<int> useUnused = new List<int> { };
    public static List<int> poolUnused = new List<int> { 2, 5, 6, 9, 40, 43 };
    public static List<int> pool = new List<int>();
    public static List<int> poolEvil = new List<int>();
    public static GameObject leftUI;
    public static GameObject objScore2;
    public static GameObject objCurrentVillage2;

    public static void initExRand()
    {
        pool.Clear();
        poolEvil.Clear();
        for (int i = 0; i < charList.Length; i++)
        {
            if (!poolUnused.Contains(i) || useUnused.Contains(i))
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
            int[] roleCount6 = randRoleCount(6);
            int[] roleCount7 = randRoleCount(7);
            int[] roleCount8 = randRoleCount(8);
            int[] roleCount9 = randRoleCount(9);
            int[] roleCount10 = randRoleCount(10);
            int[] roleCount11 = randRoleCount(11);
            CharactersCount newCharCount6 = new CharactersCount(6, roleCount6[0], roleCount6[1], roleCount6[2], roleCount6[3]);
            CharactersCount newCharCount7 = new CharactersCount(7, roleCount7[0], roleCount7[1], roleCount7[2], roleCount7[3]);
            CharactersCount newCharCount8 = new CharactersCount(8, roleCount8[0], roleCount8[1], roleCount8[2], roleCount8[3]);
            CharactersCount newCharCount9 = new CharactersCount(9, roleCount9[0], roleCount9[1], roleCount9[2], roleCount9[3]);
            CharactersCount newCharCount10 = new CharactersCount(10, roleCount10[0], roleCount10[1], roleCount10[2], roleCount10[3]);
            CharactersCount newCharCount11 = new CharactersCount(11, roleCount11[0], roleCount11[1], roleCount11[2], roleCount11[3]);
            script.characterCounts = new Il2CppSystem.Collections.Generic.List<CharactersCount>();
            script.characterCounts.Add(newCharCount6);
            script.characterCounts.Add(newCharCount7);
            script.characterCounts.Add(newCharCount8);
            script.characterCounts.Add(newCharCount9);
            script.characterCounts.Add(newCharCount10);
            script.characterCounts.Add(newCharCount11);
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
}