using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KienGameManager : MonoBehaviour
{
    [SerializeField]
    private KienGameOver goObj;
    private string winner;
    void OnPlayerDeath(GameObject gameObject)
    {
        foreach (Transform player in transform)
        {
            if (player.gameObject != gameObject)
            {
                winner = player.name;
            }
        }
        goObj.Setup(winner);
    }

    void Start()
    {
        Player[] players = GetComponentsInChildren<Player>();
        foreach (Player player in players)
            player.OnDestroyed += OnPlayerDeath;
    }

}
