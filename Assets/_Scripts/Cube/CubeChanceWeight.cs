using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "CubeChanceWeight")]
public class CubeChanceWeight : ScriptableObject
{
    public List<CubeWeightPair> cubeWeightList;

    public int WeightOfAllElements => cubeWeightList.Sum(x => x.Weight);

    [Serializable]
    public class CubeWeightPair
    {
        public ECube Cube;
        public int Weight;
    }
}