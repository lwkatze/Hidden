using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{

    private AudioSource sourceMusic;
    private AudioSource sourceAmbient;
    private AudioSource sourceFX;

    public AudioClip musicLoop;
    public AudioClip musicEnd;
    public AudioClip dripLoop;
    public AudioClip locker;
    public AudioClip prisonerLines1;
    public AudioClip prisonerLines2;
    public AudioClip prisonerLines3;


    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();

        sourceMusic = audios[0]; //Volume & Pitch Control Music
        sourceAmbient = audios[1]; //Volume & Pitch Control Dripping / Ambient
        sourceFX = audios[2]; //Volume Control FX

        sourceFX.volume = 1f;
        sourceAmbient.volume = 1f;
        sourceMusic.volume = 0.2f;
        sourceMusic.pitch = 0.7f;
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
    }


    public void TrigEnter(GameObject obj)
    {
        if (obj.tag == "Enemy") //Enters enemy trigger space, which should be pretty large
        {
            //StartCoroutine(MusicFadeIn())
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
            //StartCoroutine(MusicFadeOut())
        }
    }

    public void GrappleLine()
    {

    }


}
