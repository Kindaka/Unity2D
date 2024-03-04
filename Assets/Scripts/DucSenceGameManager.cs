using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DucSenceGameManager : MonoBehaviour
{
    [SerializeField]
    private DucScenceGameOver goObj;
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
        PlayerScence[] players = GetComponentsInChildren<PlayerScence>();
        foreach (PlayerScence player in players)
            player.OnDestroyed += OnPlayerDeath;
    }
}
