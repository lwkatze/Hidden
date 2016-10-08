#pragma strict

//Written by Ethan Chapman @Eth0sire October 8, 2015 for FLIP

 private var cameraObj : GameObject;
 public var specificVector : Vector3;
 public var smoothSpeed : float;
// var menu : GameObject;
// var pm : PauseMenu;
 
 function Start () { 
   cameraObj = GameObject.FindGameObjectWithTag("CamHolder");
  // menu = GameObject.FindGameObjectWithTag("coinsSysMaster");
  // pm = menu.GetComponent(PauseMenu);

 }
 
 function Update () {
	//if (pm.isDead == false) {
  		specificVector = Vector3(transform.position.x, transform.position.y, cameraObj.transform.position.z);
   		cameraObj.transform.position = Vector3.Lerp(cameraObj.transform.position, specificVector, smoothSpeed * Time.deltaTime);
   		//}
   
 }