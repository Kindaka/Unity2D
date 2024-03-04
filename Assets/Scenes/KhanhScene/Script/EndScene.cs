using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI winnerText;

    public void Setup(string winnerName)
    {
        winnerText = transform.Find("WinnerTxt").gameObject.GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(true);
        winnerText.text = winnerName.ToUpper() + " WIN";
    }
    public void Restart()
    {
        SceneManager.LoadScene("KhanhScene");
    }
}
