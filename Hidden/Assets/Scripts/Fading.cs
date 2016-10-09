
/*
 Name: Fading.cs
 C# Script
 Author: Brackeys. Modified heavily by Ethan Chapman @Eth0sire  for FLIP
 Date: ?.15 - 3..16
 
 Purpose:
 Fade Out a plane texture/color.
*/

using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 2f;
    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;
	private Color color = Color.white;

	void OnFlashScreen(Color _color)
	{
		color = _color;
    }

	void OnGUI()
	{
		// fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
		alpha += fadeDir * fadeSpeed * Time.deltaTime;

		Debug.Log("Before clamp: " + alpha);

		// force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
		alpha = Mathf.Clamp01(alpha);

		Debug.Log("Before clamp: " + alpha);
		// set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
		GUI.color = new Color(color.r, color.g, color.b, alpha);
		GUI.depth = drawDepth;                                                              // make the black texture render on top (drawn last)
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);       // draw the texture to fit the entire screen area
	}

    // sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    // OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes.
    void OnLevelWasLoaded()
    {
        // alpha = 1;		// use this if the alpha is not set to 1 by default
        BeginFade(-1);      // call the fade in function
    }
}
