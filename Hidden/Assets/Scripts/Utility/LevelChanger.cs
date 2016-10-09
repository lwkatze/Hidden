using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour 
{
	public static LevelChanger lvlChanger;

	ScreenFader fadeScr;

	void Awake()
	{
		if(lvlChanger == null)
		{
			lvlChanger = this;
		}
		if(lvlChanger != this)
		{
			Destroy(gameObject);
		}

		fadeScr = GameObject.FindObjectOfType<ScreenFader>();
	}

	void SceneLoad(sceneLoadType type)
	{
		int index = 0;

		if(type == sceneLoadType.reload)
			index = SceneManager.GetActiveScene().buildIndex;

		else if(type == sceneLoadType.nextScene)
			index = SceneManager.GetActiveScene().buildIndex + 1; 

		else if(type == sceneLoadType.lastScene)
			index = SceneManager.GetActiveScene().buildIndex - 1;

		else if(type == sceneLoadType.Menu)
			index = SceneManager.GetSceneByName("MainMenu").buildIndex;

		fadeScr.EndScene(index);
	}

	void SceneLoad(int index)
	{
		fadeScr.EndScene(index);
	}

	void SceneLoad(string name)
	{
		fadeScr.EndScene(SceneManager.GetSceneByName(name).buildIndex);
	}
}

public enum sceneLoadType {reload, nextScene, lastScene, Menu, GameOver}