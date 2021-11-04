using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[SerializeField]
	private int[] sceneIds;

	private Scene[] _scenes;

	private void Start()
	{
		for (int id = 0; id < sceneIds.Length; id++)
		{
			SceneManager.LoadSceneAsync(sceneIds[id], LoadSceneMode.Additive);
		}
	}
}
