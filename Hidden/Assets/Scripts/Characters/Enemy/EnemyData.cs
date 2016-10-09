//data container for this enemy

using UnityEngine;
using System.Collections;

public class EnemyData : MonoBehaviour 
{
	public Animator anim;
	public Rigidbody2D rgbody;
	public float rayCastDist;
	public int walk { get { return anim.GetInteger("Walking"); } set { anim.SetInteger("Walking", value); } }

	void Start()
	{
		if(anim == null)
			anim = GetComponentInChildren<Animator>();

		if(rgbody == null)
			rgbody = GetComponent<Rigidbody2D>();
	}
}
