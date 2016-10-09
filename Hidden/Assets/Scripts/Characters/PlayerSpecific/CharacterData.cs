using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Game.Player
{
	public delegate void animValueChanged (animValues value);

	public class CharacterData : MonoBehaviour 
	{
		public static CharacterData charaData;

		public event animValueChanged animChanged;

		#region Data

		public int playerSpeed { get; set; }

		public Transform player;
		public Transform gndCheck;

		/// <summary>
		/// Is the player touching the ground?
		/// </summary>
		/// <value><c>true</c> if gnd bool; otherwise, <c>false</c>.</value>
		[HideInInspector]public bool gndBool
		{
			get { return Physics2D.OverlapCircle(gndCheck.position, gndRadius, gndMask) ; }
		}

		/// <summary>
		/// Gives information on the raycast from distFromGnd
		/// </summary>
		[HideInInspector]public RaycastHit2D gndRayHit { get { return Physics2D.Raycast(player.position, Vector2.down, Mathf.Infinity, gndMask); } }

		/// <summary>
		/// The distance from ground, if nothing is hit then previous value returned again.
		/// </summary>
		/// <value>The dist from gnd.</value>
		public float distFromGnd
		{
			get { return (gndRayHit.collider != null) ? gndRayHit.distance : prev_distFromGnd; }
		}

		/// <summary>
		/// True if player is currently colliding, false if not.
		/// </summary>
		/// <value><c>true</c> if collide state; otherwise, <c>false</c>.</value>
		public bool collideState { get; set; }

		/// <summary>
		/// distFromGnd from previous frame
		/// </summary>
		private float prev_distFromGnd;

		/// <summary>
		/// The LayerMask of the ground
		/// </summary>
		public LayerMask gndMask;

		/// <summary>
		/// Radius of the collider checking if player is hitting ground
		/// </summary>
		public float gndRadius = 0.2f;

		[HideInInspector] public Vector3 vOnLand;
		public Rigidbody2D rgbody;
		public Animator anim;
		public Camera playerCam;
		public float jumpForce = 7200f;
		public float termVelocity = -60f;
		public float moveSpeed = 20f;
		public float moveSpeedPerSecond = 30f;
		public bool moveWithVelocity = false;

		[HideInInspector]public int walking 
		{ 
			get { return anim.GetInteger("Walking"); } 
			set { anim.SetInteger("Walking", value); } 
		}

		[HideInInspector]public bool grapple 
		{ 
			get { return anim.GetBool("Grapple"); } 
			set { anim.SetBool("Grapple", value); if(value == true) DispatchAnimChanged(animValues.grapple); else DispatchAnimChanged(animValues.idle); } 
		}
		[HideInInspector]public bool crouch 
		{ 
			get { return anim.GetBool("Crouch"); } 
			set { anim.SetBool("Crouch", value); if(value == true) DispatchAnimChanged(animValues.crouch); else DispatchAnimChanged(animValues.idle); } 
		}
		[HideInInspector]public bool crawl 
		{ 
			get { return anim.GetBool("Crawl"); } 
			set { anim.SetBool("Crawl", value); if(value == true) DispatchAnimChanged(animValues.crawl); DispatchAnimChanged(animValues.idle); } 
		}
		[HideInInspector]public bool interact;

		#endregion

		private void setValues()
		{
			
		}

		#region Unity_Methods
		void Awake()
		{
			if(charaData == null)
			{
				charaData = this;
			}
			if(charaData != this)
			{
				Destroy(gameObject);
			}
		}

		void Start()
		{
			setValues();
		}

		void Update()
		{
			prev_distFromGnd = distFromGnd;
			//Debug.Log("YVelocity: " + rgbody.velocity.y);
		}

		void OnCollisionEnter()
		{
			collideState = true;
		}

		void OnCollisionExit()
		{
			collideState = false;
		}

		#endregion

		void DispatchAnimChanged(animValues value)
		{
			if(animChanged != null)
			{
				animChanged(value);
			}
		}
	}

	public enum animValues
	{
		idle,
		walk, 
		grapple, 
		crouch,
		crawl
	}
}