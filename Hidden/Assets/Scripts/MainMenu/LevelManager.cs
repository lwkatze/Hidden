using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public void Update(){
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            Debug.Log("Key pressed");
            Application.LoadLevel(1);
        }
    }

}
