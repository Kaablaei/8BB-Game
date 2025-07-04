using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } 

    public PlayerType playerType;
    public string PlayerName;
    public float PlayerMoney;
    public bool isPlayerHaveType;
    public string Token;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        playerType = PlayerType.None;
        isPlayerHaveType = false;   
    }
}

public enum PlayerType
{
    None,
    Solid,
    Stripes, 
    PotedAll,
}