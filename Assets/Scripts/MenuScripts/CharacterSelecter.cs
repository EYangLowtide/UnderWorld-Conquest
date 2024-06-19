using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelecter : MonoBehaviour
{
    public static CharacterSelecter instance;
    public PlayerScriptableObject playerData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }
    }
    
    public static PlayerScriptableObject GetData()
    {
        return instance.playerData;
    }

    public void SelectCharacter(PlayerScriptableObject player)
    {
        playerData = player;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
