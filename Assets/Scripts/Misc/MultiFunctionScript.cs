using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MultiFunctionScript : MonoBehaviour
{
    public List<string> sceneNames = new List<string>();
    public List<Material> colorTable = new List<Material>();

    private Material standardMaterial;
    // Start is called before the first frame update
    void Awake()
    {
        if(GetComponent<Renderer>() != null)
        {
            standardMaterial = GetComponent<Renderer>().material;
        }
    }

    public void SpawnPrefab(GameObject prefab)
    {
        GameObject gb = Instantiate(prefab, transform);
    }

    public void ChangeColorTo(int index)
    {
        if (index > colorTable.Count) return;
        GetComponent<Renderer>().material = colorTable[index];
    }

    public void ResetColor()
    {
        GetComponent<Renderer>().material = standardMaterial;
    }

    public void LoadSceneFromList(int index)
    {
        if (index > sceneNames.Count) return;
        SceneManager.LoadScene(sceneNames[index]);
    }
}
