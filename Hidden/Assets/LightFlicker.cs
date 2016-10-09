using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

    public GameObject lightObjectOn;
    public GameObject lightObjectOff;
    public GameObject lightShaft;
    public GameObject lightBloom;

    private bool lightOn;

	// Use this for initialization
	void Start () {
        //lightObjectOn.SetActive(true);
        lightBloom.SetActive(true);
        lightShaft.SetActive(true);
        lightOn = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (lightOn)
        {
            StartCoroutine(waitTimerOne(1.5f));
        } else
        {
            StartCoroutine(waitTimerTwo(1.5f));
        }

    }

    IEnumerator waitTimerOne(float time)
    {
        yield return new WaitForSeconds(time);
        print("light off");
        lightBloom.SetActive(false);
        lightShaft.SetActive(false);
        lightOn = false;
        yield return null;
    }

    IEnumerator waitTimerTwo(float time)
    {
        yield return new WaitForSeconds(time);
        print("light on");
        lightBloom.SetActive(true);
        lightShaft.SetActive(true);
        lightOn = true;
        yield return null;
    }

}
