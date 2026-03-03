using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Create BallObjectSO", fileName = "BallObjectSO", order = 0)]
public class BallObjectSO : ScriptableObject
{
    public List<BallData> BallDatas;

    public BallData GetBallData(BallType ballType)
    {
        return BallDatas.First(x => x.BallType == ballType);
    }
}

[Serializable]
public struct BallData
{
    public BallType BallType;
    public int Capacity;
    public GameObject Prefab;
}