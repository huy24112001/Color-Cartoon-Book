using UnityEngine;
using UnityEngine.UI;

public class PenSkin : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] PenSkinState penSkinState;
    [SerializeField] Button penButton;
    [SerializeField] Image penFillCircle;
    [SerializeField] Image penSkinBackground;
    [SerializeField] Sprite[] penSkinBackgroundSprites;
    [SerializeField] GameObject[] penStateHolders;

    public int ID { get => id; set => id = value; }

    public void SetPenFillCicle(float fillAmount)
    {
        penFillCircle.fillAmount = fillAmount;
    }

    public void SetPenSkinState(PenSkinState state)
    {
        penSkinState = state;

        switch (penSkinState)
        {
            case PenSkinState.Locked:
                penButton.interactable = false;
                penStateHolders[0].SetActive(true);
                penStateHolders[1].SetActive(false);
                penStateHolders[2].SetActive(false);
                penStateHolders[3].SetActive(false);
                break;
            case PenSkinState.Filling:
            penButton.interactable = false;
                penStateHolders[0].SetActive(false);
                penStateHolders[1].SetActive(true);
                penStateHolders[2].SetActive(false);
                penStateHolders[3].SetActive(false);
                break;
            case PenSkinState.Filled:
            penButton.interactable = true;
                penStateHolders[0].SetActive(false);
                penStateHolders[1].SetActive(false);
                penStateHolders[2].SetActive(true);
                penStateHolders[3].SetActive(false);
                break;
            case PenSkinState.Using:
            penButton.interactable = true;
                penStateHolders[0].SetActive(false);
                penStateHolders[1].SetActive(false);
                penStateHolders[2].SetActive(false);
                penStateHolders[3].SetActive(true);
                break;
        }
    }

    public void UsingPenSkin()
    {
        penSkinBackground.sprite = penSkinBackgroundSprites[1];
    }

    public void UnusingPenSkin()
    {
        penSkinBackground.sprite = penSkinBackgroundSprites[0];
    }
}
