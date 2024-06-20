using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObjects : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier {  get => multiplier; private set => multiplier = value; }

    [SerializeField] //only mod in editor 
    int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; //what and object becomes when leveled up
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    Sprite icon; //dont mod
    public Sprite Icon { get => icon; private set => icon = value; }
}
