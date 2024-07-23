using UnityEngine;

public class PensManager : MonoBehaviour
{
    [SerializeField] PenSkin[] penSkins;

    void OnEnable()
    {
        SetUpPenSkinsOnFirst();
    }

    void SetUpPenSkinsOnFirst()
    {
        int currentFillingPenIndex = PlayerData.TotalPenUnlocked;

        for (int i = 0; i < penSkins.Length; i++)
        {
            if (i == 0 || i <= currentFillingPenIndex)
            {
                penSkins[i].SetPenSkinState(PenSkinState.Filled);
                penSkins[i].UnusingPenSkin();
            }
            else if (i == currentFillingPenIndex + 1)
            {
                penSkins[i].SetPenSkinState(PenSkinState.Filling);
                float fillAmount = PlayerData.CurrentPenProgress;
                penSkins[i].SetPenFillCicle(fillAmount);
                penSkins[i].UnusingPenSkin();

                if (PlayerData.OpenLastPen == 1)
                {
                    penSkins[i].SetPenSkinState(PenSkinState.Filled);
                    penSkins[i].UnusingPenSkin();
                }
            }
            else
            {
                penSkins[i].SetPenSkinState(PenSkinState.Locked);
            }
        }

        int currentUsingPenIndex = PlayerData.UsingPen;
        penSkins[currentUsingPenIndex].SetPenSkinState(PenSkinState.Using);
        penSkins[currentUsingPenIndex].UsingPenSkin();
    }

    public void UsingPen(PenSkin penSkin)
    {
        AudioManager.Instance.ChooseColor();
        PlayerData.UsingPen = penSkin.ID;
        SetUpPenSkinsOnFirst();
        GameManager.Instance.SetPenSkin(penSkin.ID);
    }
}
