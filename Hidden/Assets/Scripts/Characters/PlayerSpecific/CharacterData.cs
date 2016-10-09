using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Game.Player
{
	public delegate void animValueChanged (inputValues value);

	public class CharacterData : MonoBehaviour 
	{
		public static CharacterData charaData;

		public event animValueChanged animChanged;

		#region Data

		public int playerSpeed { get; set; }

		public Transform player;
		public Transform gndCheck;

		public List<Vector3> spawnPoints = new List<Vector3>();
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
		public float gndRadius = 0.1f;

		[HideInInspector] public Vector3 vOnLand;
		public Rigidbody2D rgbody;
		public Animator anim;
		public Camera playerCam;
		public float jumpForce = 7200f;
		public float termVelocity = -60f;
		public float crawlSpeed = 10f;
		public float moveSpeed = 20f;
		public float moveSpeedPerSecond = 30f;
		public bool moveWithVelocity = false;

		[HideInInspector] public float h_axis;

		public int interact 
		{ 
			get { return inputHierarchy[inputValues.interact]; } 
			set { inputHierarchy[inputValues.interact] = value; } 
		}

		public int crawl
		{ 
			get { return inputHierarchy[inputValues.crawl]; } 
			set 
			{ 
				inputHierarchy[inputValues.crawl] = value; 
				anim.SetBool("Crawl", (value > 0)? true : false);
			} 
		}

		public int crouch
		{ 
			get { return inputHierarchy[inputValues.crouch]; } 
			set 
			{ 
				inputHierarchy[inputValues.crouch] = value; 
				anim.SetBool("Crouch", (value > 0)? true : false);
			}
		}

		public int grapple
		{ 
			get { return inputHierarchy[inputValues.grapple]; } 
			set 
			{ 
				inputHierarchy[inputValues.grapple] = value; 
				anim.SetBool("Grapple", (value > 0)? true : false);
			} 
		}

		public int jump
		{ 
			get { return inputHierarchy[inputValues.jump]; } 
			set 
			{ 
				inputHierarchy[inputValues.jump] = value; 
			} 
		}

		public int walk
		{ 
			get { return inputHierarchy[inputValues.walk]; } 
			set 
			{ 
				inputHierarchy[inputValues.walk] = value; 
				anim.SetInteger("Walking", value);
			} 
		}

		[HideInInspector]private int animWalking 
		{ 
			get { return anim.GetInteger("Walking"); } 
			set { anim.SetInteger("Walking", value); } 
		}
		[HideInInspector]private bool animGrapple 
		{ 
			get { return anim.GetBool("Grapple"); } 
			set { anim.SetBool("Grapple", value); } 
		}
		[HideInInspector]public bool animCrouch 
		{ 
			get { return anim.GetBool("Crouch"); } 
			set { anim.SetBool("Crouch", value); } 
		}
		[HideInInspector]public bool animCrawl 
		{ 
			get { return anim.GetBool("Crawl"); } 
			set { anim.SetBool("Crawl", value); } 
		}

		#endregion

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

			/*List<inputValues> list = new List<inputValues>(inputHierarchy.Keys);
			foreach(inputValues values in list)
			{
				Debug.Log("Value: " + values.ToString());
			}*/
		}

		void Update()
		{
			prev_distFromGnd = distFromGnd;
			KeyboardUpdate();
			Debug.Log("AnimWalking: " + animWalking);
		}

		#endregion

		private Dictionary<inputValues, int> inputHierarchy = new Dictionary<inputValues, int>()
		{
			{inputValues.interact, 0},
			{inputValues.crawl, 0},
			{inputValues.grapple, 0},
			{inputValues.jump, 0},
			{inputValues.crouch, 0},
			{inputValues.walk, 0},
			{inputValues.idle, 0}
		};

		public int getInputValue(inputValues value)
		{
			int theReturn = 0;

			List<inputValues> keys = new List<inputValues>(inputHierarchy.Keys);

			for(int i = 0; i < keys.Count; i++)
			{
				//either crawl or interact takes priority if they are true
				if(inputHierarchy[inputValues.crawl] == 1 || inputHierarchy[inputValues.interact] == 1)
				{
					theReturn = (value == inputValues.crawl || value == inputValues.interact)? 1 : 0;
					break;
				}

				//break if a item with higher priority is true
				if(inputHierarchy[keys[i]] == 1 && keys[i] != value)
				{
					theReturn = 0;
					break;
				}

				//break when element is found
				if(keys[i] == value)
				{
					theReturn = inputHierarchy[value];
					break;
				}
			}

			return theReturn;
		}

		void KeyboardUpdate()
		{
			/*Debug.Log("Grapple: " + getInputValue(inputValues.grapple));
			Debug.Log("Walk: " + getInputValue(inputValues.walk));
			Debug.Log("Crouch: " + getInputValue(inputValues.crouch));
			Debug.Log("Jump: " + getInputValue(inputValues.jump));*/

			h_axis = Input.GetAxis("Horizontal");

			anim.SetInteger("CrawlWalk", (h_axis > 0)? 1 : ((h_axis < 0)? -1 : 0));

			if(Input.GetButtonUp("Horizontal"))
			{
				h_axis = 0;
			}
			if(Input.GetButton("Interact"))
			{
				interact = (interact > 0)? 0 : 1;	//toggle interact
				if(interact > 0)
					beneathToZero(inputValues.interact);
				return;
			}
			if(crawl > 0)
			{
				beneathToZero(inputValues.crawl);
				return;
			}
			if(Input.GetButton("Grapple"))
			{
				grapple = 1;
				beneathToZero(inputValues.grapple);
				return;
			}

			if(Input.GetButtonUp("Grapple"))
			{
				grapple = 0;
			}

			if(Input.GetButtonDown("Jump"))
			{
				jump = 1;
				beneathToZero(inputValues.jump);
				return;
			}
			if(!Input.GetButtonDown("Jump"))
			{
				jump = 0;
			}

			if(Input.GetButton("Crouch"))
			{
				crouch = 1;
				beneathToZero(inputValues.crouch);
				return;
			}

			if(Input.GetButtonUp("Crouch"))
			{
				crouch = 0;
			}
				
			walk = (h_axis > 0)? 1 : ((h_axis < 0)? -1 : 0);
		}

		//Set all elements lower than the given element to zero
		private void beneathToZero(inputValues value)
		{
			List<inputValues> keys = new List<inputValues>(inputHierarchy.Keys);

			for(int i = keys.IndexOf(value) + 1; i < keys.Count; i++)
			{
				Debug.Log(keys[i].ToString() + " is " + inputHierarchy[keys[i]]);
				inputHierarchy[keys[i]] = 0;
			}

			walk = inputHierarchy[inputValues.walk];
			crouch = inputHierarchy[inputValues.crouch];
			crawl = inputHierarchy[inputValues.crawl];
			grapple = inputHierarchy[inputValues.grapple];
			jump = inputHierarchy[inputValues.jump];
			interact = inputHierarchy[inputValues.interact];
		}
	}


	public enum inputValues
	{
		interact,
		crawl,
		grapple,
		jump,
		crouch,
		walk, 
		idle,
	}
}