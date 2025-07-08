using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LibraryTileData : MonoBehaviour
{
    private string targetPath {get; set;}
    private string keyword = "SelectedPath";
    [SerializeField] private TextMeshProUGUI folderName;

    public void InitiateTile(string path, string folderName){
        SetTargetPath(path);
        SetFolderName(folderName);
    }

    public void SetTargetPath(string path){
        this.targetPath = path;
    }
    public void SetFolderName(string name){
        this.folderName.text = name;
    }

    public void SelectTargetPath(){
        if(targetPath == null) return;
        PlayerPrefs.SetString(keyword, this.targetPath);

        SceneManager.LoadScene("Model Marking", LoadSceneMode.Single);
    }
}
