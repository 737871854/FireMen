using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using Need.Mx;

public class RankManager : MonoBehaviour {

    public class RankItem
    {
        public int id;
        public string name;
        public int value;
    }

    private List<RankItem> topN;

	// Use this for initialization
	void Start () {
        LoadTopN();
	}

    void LoadTopN()
    {
        topN = new List<RankItem>();
        for (int index = 0; index < GameConfig.GAME_CONFIG_MAX_RANK_ITEM; ++index)
        {
            string rankName     = "GAME_CONFIG_RANK_ITEM" + index;
            string defaultValue = 0 + ",AAA,1000";
            RankItem item       = new RankItem();
            string value        = PlayerPrefs.GetString(rankName, defaultValue);
            string[] values     = value.Split(',');
            if (values.Length == 3)
            {
                item.id    = index;
                item.name  = values[1];
                item.value = values[1].ToInt();
                topN.Add(item);
            }
        }

        if (topN.Count != GameConfig.GAME_CONFIG_MAX_RANK_ITEM)
        {
            topN = new List<RankItem>();
        }
    }

    public void SaveTopN(int index, bool all)
    {
        if (all && topN.Count == GameConfig.GAME_CONFIG_MAX_RANK_ITEM)
        {
            for (int count = 0; count < GameConfig.GAME_CONFIG_MAX_RANK_ITEM; ++count)
            {
                RankItem item   = topN[count];
                string rankName = "GAME_CONFIG_RANK_ITEM" + count;
                string value    = count + "," + item.name + "," + item.value;
                PlayerPrefs.SetString(rankName, value);
            }
        }

        if (index >= topN.Count)
        {
            RankItem item = topN[index];
            string rankName = "GAME_CONFIG_RANK_ITEM" + index;
            string value = index + "," + item.name + "," + item.value;
            PlayerPrefs.SetString(rankName, value);
        }

        PlayerPrefs.Save();
    }
	
    public void ChangeScore(int index, string name, int value)
    {
        if (topN.Count != GameConfig.GAME_CONFIG_MAX_RANK_ITEM)
        {
            return;
        }

        for (int count = 0; count < GameConfig.GAME_CONFIG_MAX_RANK_ITEM; ++count)
        {
            RankItem item = topN[count];
            if(item.value < value)
            {
                RankItem newItem = new RankItem();
                newItem.id    = index;
                newItem.name  = name.Substring(0,3);
                newItem.value = value;
                topN.Insert(count, newItem);
                topN.RemoveAt(GameConfig.GAME_CONFIG_MAX_RANK_ITEM);
                SaveTopN(-1, true);
                break;
            }
        }
    }
    
    public RankItem GetItem(int index)
    {
        if (topN.Count != GameConfig.GAME_CONFIG_MAX_RANK_ITEM)
        {
            return null;
        }
        if (index < 0 || index >= topN.Count)
        {
            return null;
        }
        return topN[index];
    }
}
