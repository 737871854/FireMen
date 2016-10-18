using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public class PlayerManager : MonoBehaviour
{
    protected Player[] players;

	// Use this for initialization
	public void Init () {
        players = new Player[GameConfig.GAME_CONFIG_PLAYER_COUNT];
        for (int index = 0; index < GameConfig.GAME_CONFIG_PLAYER_COUNT; ++index)
        {
            players[index] = new Player();
            players[index].Index = index;
        }
	}

    public void Reset()
    {
        for (int index = 0; index < GameConfig.GAME_CONFIG_PLAYER_COUNT; ++index)
        {
            players[index].Reset();
        }
    }
	
    public Player getPlayer(int index)
    {
        return players[index];
    }
}
