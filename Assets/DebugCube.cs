using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCube : MonoBehaviour
{
    [SerializeField] private Material red;
    [SerializeField] private Material yellow;
    [SerializeField] private Material green;
    private Renderer rend;
    // Start is called before the first frame update
    void Start(){
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    public void ChangeRed(){
        rend.material = red;
    }
    public void ChangeYellow(){
        rend.material = yellow;
    }
    public void ChangeGreen(){
        rend.material = green;
    }

}
