using UnityEngine;
using UnityEngine.UI;

public class UiController : Singleton<UiController>
{
    [Header("Count Components")]
    public Text molesMaleCountText;
    public Text molesFemaleCountText;
    public Text vegetablesCountText;
    [Header("Game session")]
    public MaskableGraphic sessionIndicator;
    public Text messageText;
    [Space(10)]
    public Color isPlayingColor;
    public Color isNotPlayingColor;

    protected override void InitializeFields()
    {
        molesMaleCountText.text = "0";
        molesFemaleCountText.text = "0";
        vegetablesCountText.text = "0";
    }

    private void Start()
    {
        MoleController.instance.updateMolesCount += UpdateMolesCount;
        GameController.instance.updateVegetablesCount += UpdateVegetablesCount;
        GameController.instance.updateGameSessionStatus += UpdateGameSessionStatus;
        GameController.instance.updateMessage += UpdateMessage;
    }

    private void UpdateMolesCount(int maleCount, int femaleCount)
    {
        molesMaleCountText.text = maleCount.ToString();
        molesFemaleCountText.text = femaleCount.ToString();
    }

    private void UpdateVegetablesCount(int vegetablesCount)
    {
        vegetablesCountText.text = vegetablesCount.ToString();
    }

    private void UpdateGameSessionStatus(bool isPlaying)
    {
        if (isPlaying) sessionIndicator.color = isPlayingColor;
        else sessionIndicator.color = isNotPlayingColor;
    }

    private void UpdateMessage(string value)
    {
        messageText.text = value;
    }
}
