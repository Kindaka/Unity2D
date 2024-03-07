using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DucScenceGameOver : MonoBehaviour
{
    private TextMeshProUGUI winnerText;

    public void Setup(string winnerName)
    {
        winnerText = transform.Find("WinnerTxt").gameObject.GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(true);
        winnerText.text = winnerName.ToUpper() + " WIN";
    }
    public void Restart()
    {
        SceneManager.LoadScene("DucntScence");
    }
    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
