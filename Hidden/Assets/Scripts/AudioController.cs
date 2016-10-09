using UnityEngine;
using System.Collections;
using App.Game.Player;

public class AudioController : MonoBehaviour
{
    private CharacterData data { get { return CharacterData.charaData; } }
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
    public AudioClip grappleLine;
    public AudioClip grappleHit;
    public AudioClip lightFlicker1;
    public AudioClip lightFlicker2;
    public AudioClip lightFlicker3;


    //Voice
    public AudioClip prisonerLines1;
    public AudioClip prisonerLines2;
    public AudioClip prisonerLines3;

    bool inEnemyRange;
    bool inLightRange;

    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();

        sourceMusic = audios[0]; //Volume & Pitch Control Music
        sourceAmbient = audios[1]; //Volume & Pitch Control Dripping / Ambient
        sourceFX = audios[2]; //Volume Control FX

        sourceFX.volume = 0.5f;
        sourceAmbient.volume = 1f;
        sourceMusic.volume = 0.05f;
        sourceMusic.pitch = 0.4f;
    }

    void Update()
    {
        if (data.grapple > 0)
        {
            GrappleLine();
        }

        if(data.grappleLocked)
        {
            GrappleHit();
        }
        if (data.interact > 0)
        {
            LockerSound();
        }

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

        }
        else
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
        else if (obj.tag == "Light")
        {
            inLightRange = true;
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
        else if (obj.tag == "Light")
        {
            inLightRange = false;
        }
    }

    public void GrappleLine()
    {
        if (!sourceFX.isPlaying)
        {
            sourceFX.PlayOneShot(grappleLine);
        }
    }

    public void GrappleHit()
    {
        if (!sourceFX.isPlaying)
        {
            sourceFX.PlayOneShot(grappleHit);
        }
    }

    public void LockerSound()
    {
        if (!sourceFX.isPlaying)
        {
            sourceFX.volume = 1f;
            sourceFX.PlayOneShot(locker);
            sourceFX.volume = 0.5f;
        }
    }

    public void TaserShot()
    {

    }

    public void LightFlicker()
    {
        if (inLightRange)
        {
            sourceFX.PlayOneShot(lightFlicker3);
        }

    }
}
