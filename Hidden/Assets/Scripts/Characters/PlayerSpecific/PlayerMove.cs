/* This is the script that controls player movement.
 * It access the character data singleton to refernce
 * player attributes (health, state, etc)
 */

using UnityEngine;
using System.Collections;
using App.Game.Utility;
using App.Game.Object;

namespace App.Game.Player
{
	public class PlayerMove : MonoBehaviour {

		/// <summary>
		/// Static reference to playerMove singleton
		/// </summary>
		public static PlayerMove playerMove;

		/// <summary>
		/// The collider resizer
		/// </summary>
		public ColliderResize resizer;

		/// <summary>
		/// The grappling hook
		/// </summary>
		public GameObject grapHook;

		/// <summary>
		/// The starting position of the grappling hook
		/// </summary>
		public Transform grapStart;

		public Vector2 grappleDirection = Vector2.up;

		public float maxGrappleDistance = 50f;
		public float distToLockCrawl = 0.5f;

		/// <summary>
		/// Access to the Characterdata singleton
		/// </summary>
		/// <value>The data.</value>
		private CharacterData data 
		{
			get { return CharacterData.charaData; }
		}

	#region Data

		private Vector2 moveVector;
		private bool facingRight;
		private float currentSpeed;

		private CreateProjectile proj;

		private bool prevGrapple;

		private bool isDesktop
		{
			get{ return (Application.isMobilePlatform) ? false : true; }
		}
		//jumping and falling 
		private float jumpForce
		{
			get { return CharacterData.charaData.jumpForce ; }
		}
		private float groundRadius
		{
			get { return CharacterData.charaData.gndRadius ; }
		}
		private Transform groundCheck
		{
			get { return CharacterData.charaData.gndCheck ; }
		}
		private LayerMask WhatIsGround
		{
			get { return CharacterData.charaData.gndMask ; }
		}
		private bool grounded
		{
			get { return CharacterData.charaData.gndBool ; }
		}

	#endregion

	#region Unity_Methods

		void Awake()
		{
			if(playerMove == null)
			{
				playerMove = this;
			}
			if(playerMove != this)
			{
				Destroy(gameObject);
			}
		}

		void Start()
		{
			if(grapHook == null)
				Debug.LogError("You need to include a reference to the GrappleHook object!");

			if(resizer == null)
				Debug.LogError("You need to include a reference to the ColliderResize script!");
		}

		void Update()
		{
			
		}

		void LateUpdate()
		{
			Move();

			Debug.Log("gndbool: " + data.gndBool);
		}

	#endregion

		private void Move()
		{
			if(data.getInputValue(inputValues.walk) != 0 || IntToBool(data.getInputValue(inputValues.jump)))
				NormalMove();

			else
			{
				crawl();
				crouch();
				grapple();
			}	

			if(data.getInputValue(inputValues.crawl) == 0 && data.getInputValue(inputValues.crouch) == 0)
				resizer.resizeCollider(inputValues.idle);
		}

		//normal movement: walk and jump
		void NormalMove ()
		{
			Debug.Log("Normal Move");

			if(data.h_axis > 0 && facingRight)
				Flip();
			if(data.h_axis < 0 && !facingRight)
				Flip();

			moveVector = new Vector2(data.h_axis * data.moveSpeedPerSecond, 0);
			data.transform.Translate(moveVector * Time.deltaTime);

			if(IntToBool(data.getInputValue(inputValues.jump)) && data.gndBool)
			{
					data.rgbody.AddForce(new Vector2(0, data.jumpForce));
			}
		}

		/// <summary>
		/// Returns 1 if grappling instance is not null
		/// </summary>
		void grapple()
		{
			//do grapple
			if(IntToBool(data.getInputValue(inputValues.grapple)) && !prevGrapple)
			{
				proj = new CreateProjectile(grapHook, grapStart.position, ((Vector2)grapStart.position)+(grappleDirection), data.moveSpeedPerSecond, 90f);
			} 

			//destroy grapple if let up
			else if(!IntToBool(data.getInputValue(inputValues.grapple)) || proj.projectile.distance > maxGrappleDistance)
			{
				if(proj != null)
					proj.deleteProjectile();
			}

			if(proj != null && proj.projectile != null)
			{
				//if the projectile is locked in position
				if(proj.projectile.locked)
				{
					Debug.Log("Attempting to transform");
					transform.Translate(grappleDirection * data.moveSpeedPerSecond * Time.deltaTime);

					if(CreateProjectile.distance((Vector2)transform.position, (Vector2)proj.projectile.transform.position) < distToLockCrawl)
					{
						data.crawl = 1;
						proj.deleteProjectile();
					}
				}
			}

			Debug.Log(IntToBool(data.getInputValue(inputValues.grapple)));
			prevGrapple = IntToBool(data.getInputValue(inputValues.grapple));
		}

		int crawl()
		{
			int theReturn = 0;

			if(IntToBool(data.getInputValue(inputValues.crawl)))
			{
				theReturn = 0;

				data.rgbody.constraints = RigidbodyConstraints2D.FreezePositionY;

				resizer.resizeCollider(inputValues.crawl);

				moveVector = new Vector2(data.h_axis * data.crawlSpeed, 0);
				data.transform.Translate(data.h_axis * data.crawlSpeed * Time.deltaTime, 0f, 0f, Space.World);

				if(Input.GetButton("Crouch"))
					theReturn = 0;
			}

			return theReturn;
		}
			
		int crouch()
		{
			int theReturn = data.crouch;

			resizer.resizeCollider(inputValues.crouch);

			return theReturn;
		}

	#region Input

		void OnMouseDown()
		{
			Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
			
		void Move(float value)
		{
			Debug.Log("Move Called");
			data.h_axis = value;
		}

	#endregion

	#region Other_Methods

		void Flip()
		{
			facingRight = !facingRight;
			//Vector3 theScale = data.player.transform.localScale;
			//theScale.x *= -1;
			//data.player.transform.localScale = theScale;
		}

		/// <summary>
		/// Greate than 0 is true, 0 or less is false.
		/// </summary>
		/// <returns><c>true</c>, if to bool was inted, <c>false</c> otherwise.</returns>
		/// <param name="i">The index.</param>
		private bool IntToBool(int i)
		{
			return (i > 0)? true : false;
		}
	#endregion
	}
}
