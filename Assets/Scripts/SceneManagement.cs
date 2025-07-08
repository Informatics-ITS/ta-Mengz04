using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private UnityEvent modelMarkingEvent;
    [SerializeField] private UnityEvent menuEvent;
    [SerializeField] private UnityEvent quitEvent;
    public void ReturnModelMarking() {
		PlayerPrefs.DeleteKey("eyeLeft_x");
		PlayerPrefs.DeleteKey("eyeLeft_y");
		PlayerPrefs.DeleteKey("eyeLeft_z");

		PlayerPrefs.DeleteKey("eyeRight_x");
		PlayerPrefs.DeleteKey("eyeRight_y");
		PlayerPrefs.DeleteKey("eyeRight_z");

		PlayerPrefs.DeleteKey("chin_x");
		PlayerPrefs.DeleteKey("chin_y");
		PlayerPrefs.DeleteKey("chin_z");

		modelMarkingEvent.Invoke();
	}

	public void ReturnMainMenu() {
        PlayerPrefs.DeleteAll();
        menuEvent.Invoke();
	}

	public void QuitApplication() {
        PlayerPrefs.DeleteAll();
        quitEvent.Invoke();
    }
}
