using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LibraryLoader : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform scrollContent;
    [SerializeField] private string libraryPath;
    [SerializeField] private string targetPath;
    [SerializeField] private string[] directoryArr;
    [SerializeField] private List<GameObject> directoryTiles = new List<GameObject>();

    private void Awake() {
        libraryPath = Application.dataPath + "/Library/";
    }

    private void OnEnable() {
        ReloadDirectory();
    }

    public void ReloadDirectory(){
        foreach(GameObject item in directoryTiles){
            Destroy(item);
        }

        directoryArr = Directory.GetDirectories(libraryPath);
        foreach (string dir in directoryArr) {
            GameObject temp = Instantiate(tilePrefab, scrollContent);
            temp.GetComponent<LibraryTileData>().InitiateTile(dir, Path.GetFileName(dir));
            directoryTiles.Add(temp);
        }
    }


}
