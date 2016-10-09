using UnityEngine;
using System.Collections;

namespace App.Game.Object
{
	public delegate void ProjectileEvent(ProjectileArgs args);

	public class Projectile : MonoBehaviour 
	{
		public event ProjectileEvent projectileEvent;

		public Transform sprite;
		public Vector2 destination;
		public float angleAddition;
		public float speedPerSecond;
	
		protected Vector2 initPos;

		[HideInInspector]public float angle;
		protected bool move = false;

		protected virtual void Start()
		{
			//get sprite transform
			if(sprite == null)
			{
				for(int i = 0; i < transform.childCount; i++)
				{
					if(transform.GetChild(i).GetComponent<SpriteRenderer>())
					{
						sprite = transform.GetChild(i);
						break;
					}
				}
			}
			
			initPos = transform.position;
		}

		protected virtual void OnCollision2D(Collision2D col)
		{
			stop();
		}

		protected virtual void Update()
		{
			Debug.Log("Move: " + move);
			if(move)
				translatePosition();

			if(transform.position.y - initPos.y >= 10)
				stop();
		}

		public virtual void go()
		{
			Debug.Log("GO");
			move = true;
		}

		public virtual void stop()
		{
			move = false;
		}

		public virtual void setEulerAngles(Vector3 theAngle)
		{
			transform.eulerAngles = theAngle;
		}

		public virtual void translatePosition()
		{
			transform.Translate(new Vector2(Time.deltaTime * speedPerSecond * Mathf.Cos(angle), Time.deltaTime * speedPerSecond * Mathf.Sin(angle)));
		}

		public void ProjSetToLocalVar(CreateProjectile inst)
		{
			destination = inst.destination;
			angleAddition = inst.angleAddition;
			speedPerSecond = inst.speedPerSecond;
		}

		public void Dispatch()
		{

		}
	}

	public class CreateProjectile
	{
		public GameObject gameObject;
		public Projectile projectile;
		public Vector2 destination;
		public float angleAddition;
		public float speedPerSecond;

		public CreateProjectile(GameObject projectilePrefab, Vector2 startPos, Vector2 _destination, float _speedPS, float _angleAddition = 0)
		{
			gameObject = projectilePrefab;
			destination = _destination;
			speedPerSecond = _speedPS;
			angleAddition = _angleAddition;

			makeProjectile(startPos);
		}

		void makeProjectile(Vector2 startPos)
		{
			gameObject = (GameObject)MonoBehaviour.Instantiate(gameObject, startPos, Quaternion.identity);
			projectile = gameObject.GetComponent<Projectile>();
			projectile.setEulerAngles(new Vector3(0f, 0f, calcAngle(startPos, destination) * Mathf.Rad2Deg + angleAddition));
			projectile.ProjSetToLocalVar(this);
			projectile.go();
		}

		/// <summary>
		/// Delete this projectile
		/// </summary>
		public void deleteProjectile()
		{
			MonoBehaviour.Destroy(gameObject);
		}

		/// <summary>
		/// Gives angle between two vectors. X axis is zero.
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="v1">V1.</param>
		/// <param name="v2">V2.</param>
		public float calcAngle(Vector2 v1, Vector2 v2)
		{
			if(distVector(v1, v2).x == 0)
				return (v1.y < v2.y)? Mathf.PI/2f : Mathf.PI*1.5f;
			else
			{
				float ratio = distVector(v1, v2).y/distVector(v1, v2).x;


				Debug.Log("V1: " + v1 + "  V2: " + v2);
				return Mathf.Atan2(distVector(v1, v2).y, distVector(v1, v2).x) * Mathf.PI;
			}
		}
			
		private Vector2 distVector(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v2.x - v1.x, v2.y - v2.y);
		}
	}

	public enum ProjectileArgs
	{
		go, 
		stop,
		die
	}
}