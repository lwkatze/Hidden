#pragma strict

//Written by Ethan Chapman @Eth0sire October 8, 2015

 private var cameraObj : GameObject;
 public var specificVector : Vector3;
 public var smoothSpeed : float;

 
 function Start () { 
   cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
  

 }
 
 function Update () {

  	specificVector = Vector3(transform.position.x, transform.position.y, cameraObj.transform.position.z);
   	cameraObj.transform.position = Vector3.Lerp(cameraObj.transform.position, specificVector, smoothSpeed * Time.deltaTime);
   
   
 }