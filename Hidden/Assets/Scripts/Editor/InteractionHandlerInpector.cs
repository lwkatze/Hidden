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
	
		private bool expandTag = false;
		private bool expandName = false;

		public override void OnInspectorGUI()
		{
			expandTag = EditorGUILayout.Foldout(expandTag, "Tags to check");

			if(expandTag)
			{
				for(int i = 0; i < handler.checkTags.Count; i++)
				{
					handler.checkTags[i] = EditorGUILayout.TagField(handler.checkTags[i], GUILayout.Width(100f));
				}

				GUILayout.BeginHorizontal();

				if(GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonLength))
					handler.checkTags.Add("");

				if(GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonLength))
					handler.checkTags.RemoveAt(handler.checkTags.Count - 1);

				GUILayout.EndHorizontal();
			}

			expandName = EditorGUILayout.Foldout(expandName, "Names to check");

			if(expandName)
			{
				for(int i = 0; i < handler.checkNames.Count; i++)
				{
					handler.checkNames[i] = EditorGUILayout.TextField(handler.checkNames[i], GUILayout.Width(100f));
				}

				GUILayout.BeginHorizontal();

				if(GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonLength))
					handler.checkNames.Add("");

				if(GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonLength))
					handler.checkNames.RemoveAt(handler.checkNames.Count - 1);

				GUILayout.EndHorizontal();
			}

		}
	}
}
