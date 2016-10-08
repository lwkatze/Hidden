//data container for this enemy

using UnityEngine;
using System.Collections;

public class EnemyData : MonoBehaviour 
{
	public Animator anim;
	public Rigidbody2D rgbody;

	void Start()
	{
		anim = GetComponentInChildren<Animator>();
	}
}
