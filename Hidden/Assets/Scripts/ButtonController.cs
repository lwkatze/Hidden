using UnityEngine;
using System.Collections;
using App.Game.Utility;

public class ButtonController : MonoBehaviour {

    public GameObject[] rescuedPrisoners;

    public GameObject ButtonNormal;
    public GameObject ButtonPressed;

	void Start () {
        ButtonNormal.SetActive(true);
        ButtonPressed.SetActive(false);
    }
	
	void Update () {
	
	}


    void OnActuator() //On activated (app.game.utility)
    {



    }
}
