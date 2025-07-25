using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{

    public TextMeshProUGUI UserName;
    public TextMeshProUGUI Userbalance;
    private float gameAmount = 1;

    [SerializeField] private Slider amountSlider;
    [SerializeField] private TextMeshProUGUI amountText;

    private float[] gamesAmount = { 1f, 1.5f, 2f, 2.5f, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 15, 16, 18, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100 };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        amountSlider.minValue = 0;
        amountSlider.maxValue = gamesAmount.Length - 1;
        amountSlider.wholeNumbers = true;
        amountSlider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(amountSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnSliderValueChanged(float value)
    {
        int index = Mathf.RoundToInt(value);
        amountText.text = "$" + gamesAmount[index].ToString();
        gameAmount = gamesAmount[index];

    }
}
