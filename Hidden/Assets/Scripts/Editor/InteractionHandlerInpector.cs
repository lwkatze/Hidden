using UnityEngine;
using System.Collections;
using UnityEditor;
using App.Game.Utility;

namespace CustomInspectors
{
	[CustomEditor(typeof(InteractionHandler))]
	public class InteractionHandlerInspector : Editor 
	{
		private InteractionHandler handler { get { return target as InteractionHandler; } }	
		private static GUILayoutOption miniButtonLength = GUILayout.Width(18f);
		
		private bool colExpandTag = false;
		private bool colExpandName = false;

		private bool trigExpandTag = false;
		private bool trigExpandName = false;

		private bool rayExpandTag = false;
		private bool rayExpandName = false;

		public override void OnInspectorGUI()
		{
			if(handler.colCheckNames.Count > 0)
				Debug.Log("First element in handler col list: " + handler.colCheckNames[0]);
			colExpandTag = EditorGUILayout.Foldout(colExpandTag, "Tags to check on collision");

			if(colExpandTag)
			{
				for(int i = 0; i < handler.colCheckTags.Count; i++)
				{
					handler.colCheckTags[i] = EditorGUILayout.TagField(handler.colCheckTags[i], GUILayout.Width(100f));
				}

				GUILayout.BeginHorizontal();

				if(GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonLength))
					handler.colCheckTags.Add("");

				if(GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonLength))
					handler.colCheckTags.RemoveAt(handler.colCheckTags.Count - 1);

				GUILayout.EndHorizontal();
			}

			colExpandName = EditorGUILayout.Foldout(colExpandName, "Names to check on collision");

			if(colExpandName)
			{
				for(int i = 0; i < handler.colCheckNames.Count; i++)
				{
					handler.colCheckNames[i] = EditorGUILayout.TextField(handler.colCheckNames[i], GUILayout.Width(100f));
				}

				GUILayout.BeginHorizontal();

				if(GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonLength))
					handler.colCheckNames.Add("");

				if(GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonLength))
					handler.colCheckNames.RemoveAt(handler.colCheckNames.Count - 1);

				GUILayout.EndHorizontal();
			}

			trigExpandTag = EditorGUILayout.Foldout(trigExpandTag, "Tags to check on trigger");

			if(trigExpandTag)
			{
				for(int i = 0; i < handler.trigCheckTags.Count; i++)
				{
					handler.trigCheckTags[i] = EditorGUILayout.TagField(handler.trigCheckTags[i], GUILayout.Width(100f));
				}

				GUILayout.BeginHorizontal();

				if(GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonLength))
					handler.trigCheckTags.Add("");

				if(GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonLength))
					handler.trigCheckTags.RemoveAt(handler.trigCheckTags.Count - 1);

				GUILayout.EndHorizontal();
			}

			trigExpandName = EditorGUILayout.Foldout(trigExpandName, "Names to check on trigger");

			if(trigExpandName)
			{
				for(int i = 0; i < handler.trigCheckNames.Count; i++)
				{
					handler.trigCheckNames[i] = EditorGUILayout.TextField(handler.trigCheckNames[i], GUILayout.Width(100f));
				}

				GUILayout.BeginHorizontal();

				if(GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonLength))
					handler.trigCheckNames.Add("");

				if(GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonLength))
					handler.trigCheckNames.RemoveAt(handler.trigCheckNames.Count - 1);

				GUILayout.EndHorizontal();
			}

			rayExpandTag = EditorGUILayout.Foldout(rayExpandTag, "Tags to check on ray cast hit");

			if(rayExpandTag)
			{
				for(int i = 0; i < handler.rayCheckTags.Count; i++)
				{
					handler.rayCheckTags[i] = EditorGUILayout.TagField(handler.rayCheckTags[i], GUILayout.Width(100f));
				}

				GUILayout.BeginHorizontal();

				if(GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonLength))
					handler.rayCheckTags.Add("");

				if(GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonLength))
					handler.rayCheckTags.RemoveAt(handler.rayCheckTags.Count - 1);

				GUILayout.EndHorizontal();
			}

			rayExpandName = EditorGUILayout.Foldout(rayExpandName, "Names to check on ray cast hit");

			if(rayExpandName)
			{
				for(int i = 0; i < handler.rayCheckNames.Count; i++)
				{
					handler.rayCheckNames[i] = EditorGUILayout.TextField(handler.rayCheckNames[i], GUILayout.Width(100f));
				}

				GUILayout.BeginHorizontal();

				if(GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonLength))
					handler.rayCheckNames.Add("");

				if(GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonLength))
					handler.rayCheckNames.RemoveAt(handler.rayCheckNames.Count - 1);

				GUILayout.EndHorizontal();
			}
			EditorUtility.SetDirty(handler);
		}
	}
}
