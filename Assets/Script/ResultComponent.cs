using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultComponent : MonoBehaviour
{
    public TextMeshProUGUI TextResult;
    public TextMeshProUGUI TextDuration;
    public TextMeshProUGUI TextClothesCleaned;

    public Button BtnRestart;

    public void Setup(int Duration, int Score)
    {
        TextResult.text = Score >= 10 ? "WON" : "LOST";
        TextDuration.text = Duration + " seconds";
        TextClothesCleaned.text = Score + " Clothes Cleaned";

        BtnRestart.onClick.AddListener(OnBtnRestartClick);
        this.gameObject.SetActive(true);
    }

    public void OnBtnRestartClick()
    {
        GameController.instance.ClearAndRestart();
    }

    private void OnDisable()
    {
        BtnRestart.onClick.RemoveListener(OnBtnRestartClick);
    }
}
