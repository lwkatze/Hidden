using UnityEngine;
using System.Collections;

public delegate void gameEvent(gameEventTest gameEvent, int index = 0);

public class GlobalEventHandler : MonoBehaviour 
{

}

public enum gameEventTest
{
	start, 
	respawn, 
	gameOver
}