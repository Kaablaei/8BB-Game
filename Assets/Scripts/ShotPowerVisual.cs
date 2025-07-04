using UnityEngine;

public class ShotPowerVisual : MonoBehaviour
{
    [SerializeField] GameObject shotColorIndicator;

    public void setIndicatorColor(float cueYpos , float maxYpos)
    {
        float add = (Mathf.Abs((cueYpos - maxYpos)) * 45 + 80) / 255.0f;
        Color color = shotColorIndicator.GetComponent<SpriteRenderer>().color;
        color.r = add;
        color.g = add;
        color.b = add;

        shotColorIndicator.GetComponent<SpriteRenderer>().color = color;
    }


}
