using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{

    private AudioSource sourceMusic;
    private AudioSource sourceAmbient;
    private AudioSource sourceFX;

    //Music
    public AudioClip musicLoop;
    public AudioClip musicEnd;

    //Ambient
    public AudioClip dripLoop;

    //FX
    public AudioClip locker;
    public AudioClip lightFlicker;

    //Voice
    public AudioClip prisonerLines1;
    public AudioClip prisonerLines2;
    public AudioClip prisonerLines3;

    bool inEnemyRange;

    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();

        sourceMusic = audios[0]; //Volume & Pitch Control Music
        sourceAmbient = audios[1]; //Volume & Pitch Control Dripping / Ambient
        sourceFX = audios[2]; //Volume Control FX

        sourceFX.volume = 0.2f;
        sourceAmbient.volume = 1f;
        sourceMusic.volume = 0.05f;
        sourceMusic.pitch = 0.4f;
    }

    void Update()
    {
        if (!sourceAmbient.isPlaying)
        {
            sourceAmbient.PlayOneShot(dripLoop);
        }
        if (!sourceMusic.isPlaying)
        {
            sourceMusic.PlayOneShot(musicLoop);
        }
        if (inEnemyRange)
        {
            sourceAmbient.volume = Mathf.Lerp(sourceAmbient.volume, 0.1f, Time.deltaTime * 4f);
            sourceMusic.volume = Mathf.Lerp(sourceMusic.volume, 0.6f, Time.deltaTime * 4f);
            sourceMusic.pitch = Mathf.Lerp(sourceMusic.pitch, 0.75f, Time.deltaTime * 2f);

        } else
        {
            sourceAmbient.volume = Mathf.Lerp(sourceAmbient.volume, 1f, Time.deltaTime * 4f);
            sourceMusic.volume = Mathf.Lerp(sourceMusic.volume, 0.05f, Time.deltaTime * 4f);
            sourceMusic.pitch = Mathf.Lerp(sourceMusic.pitch, 0.4f, Time.deltaTime * 2f);
        }
    }


    public void TrigEnter(GameObject obj)
    {
        if (obj.tag == "Enemy") //Enters enemy trigger space, which should be pretty large
        {
            inEnemyRange = true;
            //sourceAmbient.volume = Mathf.lerp(audio.volume, 0, time.deltatime); Dontdestroyonload(game object);
        }
        else if (obj.tag == "Prisoner")
        {
            int randNum = Random.Range(1, 4);
            switch (randNum)
            {
                case 1:
                    sourceFX.PlayOneShot(prisonerLines1);
                    break;
                case 2:
                    sourceFX.PlayOneShot(prisonerLines2);
                    break;
                case 3:
                    sourceFX.PlayOneShot(prisonerLines3);
                    break;
            }
        }
        else if (obj.tag == "lol")
        {
            //pass
        }
        else if (obj.tag == "tt")
        {

        }
    }

    public void TrigExit(GameObject obj)
    {
        if (obj.tag == "Enemy") //Exits enemy trigger space, which should be pretty large
        {
            inEnemyRange = false;
        }
    }

    public void GrappleLine()
    {

    }

    public void GrappleHit()
    {

    }

    public void LockerSound()
    {

    }

    public void TaserShot()
    {

    }


}
