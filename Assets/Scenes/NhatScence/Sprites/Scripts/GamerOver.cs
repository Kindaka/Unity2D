using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamerOver : MonoBehaviour
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
        SceneManager.LoadScene("NhatSceneTerrian");
    }
    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
