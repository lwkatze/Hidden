using UnityEngine;
using System.Collections;

public class AudioBridge : MonoBehaviour
{

    public AudioController AC;

    void Start()
    {
        AC = Camera.main.gameObject.GetComponent<AudioController>();
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        AC.TrigEnter(trig.gameObject);
    }

    void OnTriggerExit2D(Collider2D trig)
    {
        AC.TrigExit(trig.gameObject);
    }
}
