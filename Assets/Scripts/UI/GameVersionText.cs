using UnityEngine;
using UnityEngine.UI;

public class GameVersionText : MonoBehaviour
{
    [SerializeField] private Text txt;

    private void OnEnable()
    {
        txt.text = $"v{Application.version}";
    }
}
