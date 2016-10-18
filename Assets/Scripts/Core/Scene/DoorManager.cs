using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using Need.Mx;

public class DoorManager : SingletonBehaviour<DoorManager>
{

    public enum StateType
    {
        Unknown = 0,
        Opened  = 1,
        Cloesd  = 2,
        Opening = 3,
        Closing = 4
    }

    public enum MoveType
    {
        Unknown    = 0,
        UpDown     = 1,
        LeftRight  = 2,

    }

    public class Door
    {
        public GameObject parent;
        public GameObject left;
        public GameObject right;
        public StateType state;
        public MoveType type;
        public float time;
    }

    public GameObject[] upDownPrefabsList;
    public GameObject[] leftRightPrefabsList;

    protected Dictionary<string, Door> prefabsDict;
    protected List<string> openedList;
    protected List<string> openingList;
    protected List<string> closingList;
    protected Vector3 leftOpenRotateEnd;
    protected Vector3 rightOpenRotateEnd;
	protected Vector3 leftOpenRotateEnd1;
	protected Vector3 rightOpenRotateEnd1;

	// Use this for initialization
	void Start () 
    {
	    prefabsDict = new Dictionary<string, Door>();
        leftOpenRotateEnd = new Vector3(0, 90, 0);
        rightOpenRotateEnd = new Vector3(0, -90, 0);
		leftOpenRotateEnd1 = new Vector3(0, 0, 0);
		rightOpenRotateEnd1 = new Vector3(0, 0, 0);
        openingList = new List<string>();
        closingList = new List<string>();
        openedList  = new List<string>();

        for (int index = 0; index < leftRightPrefabsList.Length; ++index)
        {
            Door door = new Door();
            door.parent = leftRightPrefabsList[index];
            door.right  = door.parent.transform.FindChild("Right").gameObject;
            door.left   = door.parent.transform.FindChild("Left").gameObject;
            door.state  = StateType.Cloesd;
            door.time   = 0.0f;
            door.type = MoveType.LeftRight;
            prefabsDict.Add(door.parent.name, door);
        }

        for (int index = 0; index < upDownPrefabsList.Length; ++index)
        {
            Door door = new Door();
            door.parent = upDownPrefabsList[index];
            door.left   = door.parent.transform.FindChild("Left").gameObject;
            door.right  = door.parent.transform.FindChild("Head").gameObject;
            door.state  = StateType.Cloesd;
            door.time   = 0.0f;
            door.type   = MoveType.UpDown;
            prefabsDict.Add(door.parent.name, door);
        }
       
	}

    public void UpdateDoor()
    {
        for (int index = 0; index < openedList.Count; ++index)
        {
            string name = openedList[index];
            Door go = prefabsDict[name];
            if (go.state != StateType.Opened)
            {
                continue;
            }
            go.time -= Time.deltaTime;
            if (go.time <= 0.0f)
            {
                CloseDoor(name);
                openedList.RemoveAt(index);
                return;
            }
        }

        //foreach (KeyValuePair<string, Door> element in prefabsDict)
        //{
        //    if (element.Value.state != StateType.Opened)
        //    {
        //        continue;
        //    }

        //    element.Value.time -= Time.deltaTime;
        //    if (element.Value.time <= 0.0f)
        //    {
        //        CloseDoor(element.Key);
        //    }
        //}
    }

    public bool OpenDoor(string name)
    {
        if (prefabsDict.ContainsKey(name) == false)
        {
            return false;
        }

        Door go = prefabsDict[name];
        if (go == null)
        {
            return false;
        }

        if (go.state == StateType.Closing)
        {
            return false;
        }

        if (go.state == StateType.Opening)
        {
            return true;
        }

        if (go.state == StateType.Opened)
        {
            go.time = GameConfig.GAME_CONFIG_CLOSE_DOOR_TIME;
        }

        if (go.type == MoveType.LeftRight)
        {
            Tweener twLeft  = go.left.transform.DOLocalRotate(leftOpenRotateEnd, 1.0f);
            Tweener twRight = go.right.transform.DOLocalRotate(rightOpenRotateEnd, 1.0f);
            twLeft.OnComplete(OnOpened);
            openingList.Add(name);
            go.state = StateType.Opening;
        }
        else if (go.type == MoveType.UpDown)
        {
            Tweener twLeft = go.left.transform.DOLocalMoveY(go.right.transform.localPosition.y, 1.0f);
            twLeft.OnComplete(OnOpened);
            openingList.Add(name);
            go.state = StateType.Opening;
        }

        return true;
    }

    public bool CloseDoor(string name)
    {
        if (prefabsDict.ContainsKey(name) == false)
        {
            return false;
        }

        Door go = prefabsDict[name];
        if (go == null)
        {
            return false;
        }

        if (go.state != StateType.Opened)
        {
            return false;
        }

        if (go.type == MoveType.LeftRight)
        {
			Tweener twLeft = go.left.transform.DOLocalRotate(leftOpenRotateEnd1, 1.0f);
			Tweener twRight = go.right.transform.DOLocalRotate(rightOpenRotateEnd1, 1.0f);
            twLeft.OnComplete(OnClosed);
            closingList.Add(name);
            go.state = StateType.Closing;
        }
        else if (go.type == MoveType.UpDown)
        {
            Tweener twLeft = go.left.transform.DOLocalMoveY(0, 1.0f);
            twLeft.OnComplete(OnClosed);
            closingList.Add(name);
            go.state = StateType.Closing;
        }

        return true;
    }

    void OnOpened()
    {
        if (openingList.Count > 0)
        {
            string name = openingList[0];
            Door go  = prefabsDict[name];
            go.state = StateType.Opened;
            go.time = GameConfig.GAME_CONFIG_CLOSE_DOOR_TIME;
            openingList.Remove(name);
            openedList.Add(name);
        }
    }

    void OnClosed()
    {
        if (closingList.Count > 0)
        {
            string name = closingList[0];
            Door go  = prefabsDict[name];
            go.state = StateType.Cloesd;
            closingList.Remove(name);
        }
    }
}
