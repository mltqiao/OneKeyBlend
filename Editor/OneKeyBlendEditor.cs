using UnityEditor;
using UnityEngine;

public class OneKeyBlendEditor : EditorWindow
{
    private static readonly string instantiatedPrefabName = "One Key Blend";
    [MenuItem("Ame雨/Instantiate One Key Blend Prefab")]
    public static void InstantiateBindingComponentPrefab()
    {
        // 当场景中已有该预制体时
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in gameObjects)
        {
            if (obj.name == instantiatedPrefabName)
            {
                obj.GetComponent<OneKeyBlend>().transTargetObject = null;
                obj.GetComponent<OneKeyBlend>().transSkinObject = null;
                Debug.Log("There is already a prefab in this scene, data on its script have been removed.");
                return;
            }
        }
        
        // 实例化预制体
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/OneKeyBlend/Resources/Prefabs/One Key Blend.prefab");
        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab);
            instance.name = instantiatedPrefabName;
            Debug.Log("Prefab is instantiated.");
        }
        else
        {
            Debug.LogError("Prefab not found. Make sure to specify the correct path: 'Assets/OneKeyBlend/Resources/Prefabs/One Key Blend.prefab'");
        }
    }
    
    [MenuItem("Ame雨/Plug-in Instruction")]
    public static void OpenInstruction()
    {
        
    }
}