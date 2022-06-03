using TMPro;
using UnityEngine;

public class ScoreboardElementController : MonoBehaviour
{
    [SerializeField] private TMP_Text nicknameTMP;
    [SerializeField] private TMP_Text scoreTMP;
    [SerializeField] private TMP_Text numberTMP;

    public void SetNicknameColor()
    {
        nicknameTMP.color = new Color(0, 1, 0.8688135f);
    }
    
    public void SetPlayerScore(int number, string nickname, int score)
    {
        numberTMP.text = number.ToString();
        SetNumberColor(number);
        nicknameTMP.text = nickname;
        scoreTMP.text = score.ToString();
    }

    private void SetNumberColor(int number)
    {
        numberTMP.color = number switch
        {
            1 => new Color(1, 0.9f, 0),
            2 => new Color(0.73f, 0.73f, 0.73f),
            3 => new Color(0.773f, 0.429f, 0.13f),
            _ => Color.white
        };
    }
}
