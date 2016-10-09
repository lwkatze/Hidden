using UnityEngine;
using System.Collections;

namespace App.Game.Utility
{
	public enum actuatorType { on_off_sensor, toggle, three_state };

	public class GenericActuator : MonoBehaviour 
	{
		public actuatorType actType;
		public Transform[] targets;
		public string[] names;
		public string[] tags;
		public int initialState;

		protected int state;

		protected virtual void OnTriggerEnter2D(Collider2D trig)
		{
			if(names != null)
			{
				foreach(string str in names)
				{
					if(trig.name == str)
					{
						if(actType == actuatorType.on_off_sensor)
						{
							updateState();
							trigDispatch();
						}
					}
				}
			}

			if(tags != null)
			{
				foreach(string str in tags)
				{
					if(trig.tag == str)
					{
						if(actType == actuatorType.on_off_sensor)
						{
							updateState();
							trigDispatch();
						}
					}
				}
			}
		}

		protected virtual void OnTriggerExit2D(Collider2D trig)
		{
			if(names != null)
			{
				foreach(string str in names)
				{
					if(trig.name == str)
					{
						updateState();
						trigDispatch();
					}
				}
			}

			if(tags != null)
			{
				foreach(string str in tags)
				{
					if(trig.name == str)
					{
						updateState();
						trigDispatch();
					}
				}
			}
		}

		protected virtual void updateState()
		{
			//switch states
			if(actType == actuatorType.on_off_sensor)
				state = (state > 0)? 0 : 1;

			//toggle states
			else if(actType == actuatorType.toggle)
				state = (state > 0)? 0 : 1;

			else if(actType == actuatorType.three_state)
			{
				if(state < 2)
					state += 1;
				else 
					state = 0;
			}
		}

		protected virtual void trigDispatch()
		{
			if(targets != null)
			{
				foreach(Transform trans in targets)
				{
					trans.SendMessage("OnActuator", new ActuatorArgs(gameObject, actType, state), SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public struct ActuatorArgs
	{
		public GameObject sender;
		public actuatorType type;
		public int state;

		public ActuatorArgs(GameObject _sender, actuatorType _type, int _state)
		{
			sender = _sender;
			type = _type;
			state = _state;
		}
	}
}
