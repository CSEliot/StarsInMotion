using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
	

	public void ChangeToScene(int sceneToChange) {
		Application.LoadLevel (sceneToChange);
	}

    public void ChangeToScene(string sceneToChange)
    {
        Application.LoadLevel(sceneToChange);
    }
}
