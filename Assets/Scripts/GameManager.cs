using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagers : MonoBehaviour
{
    [SerializeField]
    private GamerOver goObj;
    private string winner;
    void OnPlayerDeath(GameObject gameObject)
    {
        foreach (Transform player in transform)
        {
            if(player.gameObject != gameObject)
            {
                winner= player.name;
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
