using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {

    public GameObject lightObjectOn;
    public GameObject lightObjectOff;
    public GameObject lightShaft;
    public GameObject lightBloom;

    public AudioController AC;

    public float timeOn;
    public float timeOff;

    private bool lightOn;

	// Use this for initialization
	void Start () {
        AC = Camera.main.gameObject.GetComponent<AudioController>();
        lightObjectOn.SetActive(true);
        lightObjectOff.SetActive(false);
        lightBloom.SetActive(true);
        lightShaft.SetActive(true);
        lightOn = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (lightOn)
        {
            StartCoroutine(waitTimerOne(timeOn));
        } else
        {
            StartCoroutine(waitTimerTwo(timeOff));
        }

    }

    IEnumerator waitTimerOne(float time)
    {
        yield return new WaitForSeconds(time);
        print("light off");
        lightBloom.SetActive(false);
        lightShaft.SetActive(false);
        lightObjectOn.SetActive(false);
        lightObjectOff.SetActive(true);
        lightOn = false;
        yield return null;
    }

    IEnumerator waitTimerTwo(float time)
    {
        yield return new WaitForSeconds(time / 4);
        AC.LightFlicker();
        yield return new WaitForSeconds(time/2);
        print("light on");
        lightBloom.SetActive(true);
        lightShaft.SetActive(true);
        lightObjectOn.SetActive(true);
        lightObjectOff.SetActive(false);
        lightOn = true;
        yield return null;
    }

}
