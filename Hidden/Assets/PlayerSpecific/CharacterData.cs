using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Game.Player
{
public class CharacterData : MonoBehaviour 
	{
		
		public static CharacterData charaData;

		#region Data

		public Transform player;
		public Transform spawn;
		public List<Transform> spawns;
		public Transform gndCheck;
		public Transform startPlat;

		[HideInInspector] public Transform lastPlat;
		[HideInInspector] public Transform currentPlat;

		/// <summary>
		/// Is the player touching the ground?
		/// </summary>
		/// <value><c>true</c> if gnd bool; otherwise, <c>false</c>.</value>
		[HideInInspector]public bool gndBool
		{
			get { return Physics2D.OverlapArea(gndCheck.position, new Vector2(gndCheck.position.x + 1f, gndCheck.position.y + 1f), gndMask) ; }
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
		/// distFromGnd from previous frame
		/// </summary>
		private float prev_distFromGnd;

		/// <summary>
		/// This gives the y velocity scaled 1 to -1. 1 means it's yvelocity is positive, 0 means 0, 0 to -1 is percentage of terminal Velocity.
		/// </summary>
		[HideInInspector]public float yVelocityScaled
		{
			get{ return (rgbody.velocity.y > 0) ? 1f : -1 * rgbody.velocity.y/termVelocity;}
		}

		/// <summary>
		/// Set the terminal Velocity of player
		/// </summary>
		public float termVelocity = -60f;

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
		public float moveSpeed = 20f;
		public float score;
		public float dist_death = 30f;
		public bool collideState;
		public bool prevcollideState;
		public bool obstaclehit = false;

		/// <summary>
		/// Battery's recharge rate per second
		/// </summary>
		/// <value>The b percent power.</value>
		public float b_Recharge = 10f;

		/// <summary>
		/// Battery's Capacity
		/// </summary>
		public float b_MaxPower = 100f;
		public float b_Power;
		public float b_PercentPower
		{
			get{ return b_Power/b_MaxPower ; }
		}
			
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
			currentPlat = startPlat;
			b_Power = b_MaxPower;
			setValues();
		}

		void Update()
		{
			if(lastPlat.position.y - player.position.y >= dist_death)
			{
				Reload();
			}
			battery();

			prev_distFromGnd = distFromGnd;
			//Debug.Log("YVelocity: " + rgbody.velocity.y);
		}

		void LateUpdate()
		{
			prevcollideState = collideState;
		}
			
		void OnCollisionEnter2D(Collision2D col)
		{
			collideState = true;
			if(currentPlat != null)
				lastPlat = currentPlat;
			
			currentPlat = col.transform;

			if(col.transform.gameObject.layer == gndMask)
			{
				vOnLand = rgbody.velocity;
			}
		}

		void OnCollisionExit2D(Collision2D col)
		{
			collideState = false;
		}

		#endregion

		#region Methods

		public void Reload()
		{
			
		}

		private void resetValues()
		{

		}

		/// <summary>
		/// Subtracts the power and returns a float determining the effect.
		/// </summary>
		/// <returns>Percentage of effect 0 - 1.</returns>
		/// <param name="amount">Amount.</param>
		public float subtractPower(float amount)
		{
			float rtnValue = 1;
			if(b_Power >= amount)
			{
				rtnValue = 1;
				b_Power -= amount;
			}
			else if(b_Power < amount)
			{
				rtnValue = b_Power/amount;
				b_Power = 0;
			}
			return rtnValue;
		}

		private void battery()
		{
			if(b_Power < b_MaxPower)
				b_Power += b_Recharge * Time.deltaTime;
		}

		#endregion

	}
}