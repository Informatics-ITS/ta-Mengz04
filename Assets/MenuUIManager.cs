using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour{
    [SerializeField] private GameObject[] UIWindows;

    private void Start(){
        ChangeWindow("CT Scroll View");
    }
    public void ChangeWindow(string windowName){
        foreach (GameObject window in UIWindows){
            if(window.name == windowName){
                window.SetActive(true);
            }else{
                window.SetActive(false);
            }
        }
    }
 
}
