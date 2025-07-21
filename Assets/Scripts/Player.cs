using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } 

    public PlayerType playerType;
    public string myName = "aLI";
    public string myOpponentname;

    public float myBalance;

    public bool isPlayerHaveType;

    public string Token;

    public string Gamebanamce;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

public enum PlayerType
{
    None,
    Solid,
    Stripes, 
    PotedAll,
}