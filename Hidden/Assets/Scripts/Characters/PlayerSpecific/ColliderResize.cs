using UnityEngine;
using System.Collections;

namespace App.Game.Player
{
	public class ColliderResize : MonoBehaviour 
	{
		private CharacterData data { get { return CharacterData.charaData; } } 

		public Vector2 crouchResize;
		public Vector2 crawlResize;

		private Vector2 normalSize;

		void Start()
		{
			normalSize = transform.localScale;
			data.animChanged += new animValueChanged(resizeCollider);
		}

		void resizeCollider(animValues value)
		{
			if(value == animValues.idle)
			{
				transform.localScale = normalSize;
			}

			if(value == animValues.walk)
			{
				transform.localScale = normalSize;
			}

			if(value == animValues.crouch)
			{
				transform.localScale = crouchResize;
			}

			if(value == animValues.crawl)
			{
				transform.localScale = crawlResize;
			}
		}
	}
}