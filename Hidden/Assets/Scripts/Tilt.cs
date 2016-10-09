using UnityEngine;
using System.Collections;

public class Tilt : MonoBehaviour {

    public Quaternion qTo;
    float tilt;

    void Start()
    {
        tilt = 10;
    }

    void Update()
    {
        qTo = Quaternion.Euler(0, 0, tilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, 3f * Time.deltaTime);
        GetNewTiltDir();
    }

    void GetNewTiltDir()
    {
        if (transform.rotation.z >= 0.08f || transform.rotation.z <= -0.08f)
        {
            tilt = tilt * -1f;
        }
    }
}
