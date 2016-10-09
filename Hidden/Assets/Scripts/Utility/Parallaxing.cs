using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;
    public Transform[] foregrounds;
    private float[] parallaxScales;
    private float[] parallaxScales2;
    public float smoothing;

    private Transform cam;
    private Vector3 previousCamPos;



	void Start () {
        cam = Camera.main.transform;

        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];
        parallaxScales2 = new float[foregrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
        for (int i = 0; i < foregrounds.Length; i++)
        {
            parallaxScales2[i] = foregrounds[i].position.z * -1;
        }
    }
	
	void LateUpdate () {
	    
        for(int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        for (int i = 0; i < foregrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales2[i];

            float foregroundTargetPosX = foregrounds[i].position.x + parallax;

            Vector3 foregroundTargetPos = new Vector3(foregroundTargetPosX, foregrounds[i].position.y, foregrounds[i].position.z);

            foregrounds[i].position = Vector3.Lerp(foregrounds[i].position, foregroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
	}
}
