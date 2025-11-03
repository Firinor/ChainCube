using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "LootBox", menuName = "StaticData/Loot/LootBox")]
public class LootBox : ScriptableObject
{
    public Loot[] Loot;

    public T GetRandomItem<T>() where T : ScriptableObject
    {
        if (Loot.Length <= 1)
            return (T)Loot[0].Item;

        int allWeight = 0;
        for (int i = 0; i < Loot.Length; i++)
        {
            allWeight += Loot[i].WeightOfChance;
        }

        int cursor = Random.Range(0, allWeight);

        int index = 0;
        while (cursor >= Loot[index].WeightOfChance)
        {
            cursor -= Loot[index].WeightOfChance;
            index++;
        }

        return (T)Loot[index].Item;
    }

    public List<T> GetRandomItems<T>(int count) where T : ScriptableObject
    {
        List<T> result = new();
        for (int i = 0; i < count; i++)
        {
            result.Add(GetRandomItem<T>());
        }
        return result;
    }
}

[Serializable]
public class Loot
{
    public ScriptableObject Item;
    public int WeightOfChance;
}