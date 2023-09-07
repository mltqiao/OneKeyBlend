using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OneKeyBlend : MonoBehaviour
{
    public Transform transTargetObject;
    public Transform transSkinObject;
    public SkinnedMeshRenderer rendererSkinObject;
    
    public void NANAsBonesBlendMethod()
    {
        // 没有挂Target时
        if (!transTargetObject)
        {
            Debug.LogWarning("A TargetObject is needed.");
            return;
        }
        
        // 获取Target所有骨骼的Transform
        Transform[] transOriginBones = transTargetObject.GetComponentsInChildren<Transform>();
        
        // 没有挂Skin时
        if (!transSkinObject)
        {
            Debug.LogWarning("A SkinObject is needed.");
            return;
        }
        
        // 获取Skin所有骨骼的Transform
        Transform[] transSkinBones = transSkinObject.GetComponentsInChildren<Transform>();
        
        // 遍历Skin的骨骼Transform 在每个骨骼Transform时通过遍历Origin寻找对应的骨骼Transform
        foreach (var transSkinBone in transSkinBones)
        {
            // 存一个变化后的rootBone 用于对比是否改变（找到了Target上对应的name）
            Transform transSkinBoneAfter = null;
            foreach (var transOriginBone in transOriginBones)
            {
                // 当是同一个骨骼时
                if (transSkinBone.name == transOriginBone.name && !transSkinBone.GetComponent<SkinnedMeshRenderer>())
                {
                    // 把Skin的该骨骼移到Origin对应骨骼的同一个父级下 并重命名
                    transSkinBone.SetParent(transOriginBone);
                    transSkinBone.position = transOriginBone.position;
                    transSkinBone.eulerAngles = transOriginBone.eulerAngles;
                    transSkinBone.name += "(" +transSkinObject.name + ")";
                    transSkinBoneAfter = transOriginBone;
                    // 当找到Origin对应的骨骼Transform后不再继续寻找
                    break;
                }
            }
            if (transSkinBoneAfter == null)
            {
                Debug.LogWarning("Target bone : " + transSkinBone.name + " not found in TargetObject. It's still a child of '" + transSkinObject + "''s one bone (It maybe not a bone, it's normal, better check it).");
            }
        }
        
        // 删除除Transform外的Component
        Component[] allComponents = transSkinObject.GetComponents<Component>();
        foreach (var component in allComponents)
        {
            string componentName = component.GetType().Name;
            if (componentName != "Transform")
            {
                DestroyImmediate(component);
            }
        }
        // 把Skin的Renderer移到Origin的下
        transSkinObject.SetParent(transTargetObject);
        // 归零(强迫症犯了）
        transSkinObject.localPosition = Vector3.zero;
        transSkinObject.eulerAngles = transTargetObject.eulerAngles;
        // var skinObjectRenderers = transSkinObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        // foreach (var skinObjectRenderer in skinObjectRenderers)
        // {
        //     skinObjectRenderer.transform.SetParent(transTargetObject);
        //     skinObjectRenderer.transform.localPosition = Vector3.zero;
        //     skinObjectRenderer.transform.eulerAngles = transTargetObject.eulerAngles;
        // }
        Debug.Log("Move SkinObject to be a child of TargetObject");
        
        // 删除这个预制体
        DestroyImmediate(gameObject);
        
        Debug.Log("NANA's bones blend finished.");
    }

    public void AmesAllSkinnedMeshRendererInfoReplacementMethod()
    {
        // 没有挂Target时
        if (!transTargetObject)
        {
            Debug.LogWarning("A TargetObject is needed.");
            return;
        }
        // 获取Target的骨骼
        var transTargetBones = transTargetObject.GetComponentsInChildren<Transform>();
        
        // 没有挂Skin时
        if (!transSkinObject)
        {
            Debug.LogWarning("A SkinObject is needed.");
            return;
        }

        //获取Skin Object中的全部含有SkinnedMeshRenderer的List
        List<SkinnedMeshRenderer> skinSkinnedMeshRenderers = new List<SkinnedMeshRenderer>();
        for (int i = 0; i < transSkinObject.childCount; i++)
        {
            var skinSkinnedMeshRenderer = transSkinObject.GetChild(i).GetComponent<SkinnedMeshRenderer>();
            if (skinSkinnedMeshRenderer)
            {
                skinSkinnedMeshRenderers.Add(skinSkinnedMeshRenderer);
            }
        }

        // 对各SkinnedMeshRenderer的Object操作
        foreach (var currentSkinRenderer in skinSkinnedMeshRenderers)
        {
            // 存一个变化后的rootBone 用于对比是否改变（找到了Target上对应的name）
            Transform currentSkinRendererRootBoneAfter = null;
            // 在Target中寻找相同name的骨骼 用于替换根骨骼
            foreach (var transTargetBone in transTargetBones)
            {
                if (currentSkinRenderer.rootBone.name == transTargetBone.name)
                {
                    currentSkinRenderer.rootBone = transTargetBone;
                    currentSkinRendererRootBoneAfter = transTargetBone;
                    Debug.Log("rootBone :'" + currentSkinRenderer.name + ".rootBone' in '" + transSkinObject + "' has been successfully replaced!");
                    break;
                }
            }

            // 当遍历完Target的骨骼 但没找到对应的name时
            if (currentSkinRendererRootBoneAfter == null)
            {
                Debug.LogWarning("Can't find rootBone: '" + rendererSkinObject.rootBone.name + "' in '" + transTargetObject.name + "' while replacing '" + currentSkinRenderer.name + ".SkinnedMeshRenderer.rootBone'. Replacement process has been shut down.");
                return;
            }
            
            // 获取Skin Renderer的Bones
            Transform[] currentSkinRendererBones = currentSkinRenderer.bones;
        
            // 替换Skin Renderer的Bones
            // （防止两个物体的Bones信息顺序不一致，采取存Origin顺序的List的方式）
            List<Transform> tempCurrentSkinRendererBonesInfo = new List<Transform>();

            // 遍历Skin的Bones 在每个Bones时通过遍历Origin寻找对应的Bone
            foreach (var currentSkinRendererBone in currentSkinRendererBones)
            {
                // 存个变化后的skinRendererBone映射信息，用于判断是否找到了在Target上的映射
                Transform currentSkinRendererBoneAfter = null;
                foreach (var transTargetBone in transTargetBones)
                {
                    if (currentSkinRendererBone.name == transTargetBone.name)
                    {
                        // 按第一次遍历(Skin)的顺序存List
                        tempCurrentSkinRendererBonesInfo.Add(transTargetBone);
                        currentSkinRendererBoneAfter = transTargetBone;
                        // 找到对应的骨骼后不再往后找
                        break;
                    }
                }
                // 当遍历完Target的骨骼 但没找到对应的name时
                if (currentSkinRendererBoneAfter == null)
                {
                    Debug.LogWarning("Can't find bone: '" + currentSkinRendererBone.name + "' in '" + transTargetObject.name + "' while replacing '" + currentSkinRenderer.name + ".SkinnedMeshRenderer.bones'. Replacement process has been shut down.");
                    tempCurrentSkinRendererBonesInfo.Add(null);
                    return;
                }
            }
            // 将skin顺序的origin bones新数组赋值给skin
            currentSkinRenderer.bones = tempCurrentSkinRendererBonesInfo.ToArray();
            Debug.Log("bones :'" + currentSkinRenderer.name + ".SkinnedMeshRenderer.bones' have been successfully replaced!");
        }
        Debug.Log("ALL 'SkinnedMeshRenderer.rootBone' & 'SkinnedMeshRenderers.bones' in SkinObject : '" + transSkinObject.name + "' have been successfully replaced!");
        
        // 删除除Transform外的Component
        Component[] allComponents = transSkinObject.GetComponents<Component>();
        foreach (var component in allComponents)
        {
            string componentName = component.GetType().Name;
            if (componentName != "Transform")
            {
                DestroyImmediate(component);
            }
        }
        // 把Skin的Renderer移到Origin的下
        transSkinObject.SetParent(transTargetObject);
        // 归零(强迫症犯了）
        transSkinObject.localPosition = Vector3.zero;
        transSkinObject.eulerAngles = transTargetObject.eulerAngles;
        // 删除Skin的Bones
        DestroyImmediate(transSkinObject.GetChild(0).gameObject);
        Debug.Log("Move SkinObject to be a child of TargetObject");

        Debug.Log("Ame's ALL SkinnedMeshRenderers' information replacement finished.");
    }

    public void AmesOneSkinnedMeshRendererInfoReplacementMethod()
    {
        // 没有挂Target时
        if (!transTargetObject)
        {
            Debug.LogWarning("A TargetObject is needed.");
            return;
        }
        // 获取Target的骨骼
        var transTargetBones = transTargetObject.GetComponentsInChildren<Transform>();
        
        // 没有挂SkinnedMeshRenderer时
        if (!rendererSkinObject)
        {
            Debug.LogWarning("An object with SkinnedMeshRenderer component is needed.");
            return;
        }
        
        // 存一个变化后的rootBone 用于对比是否改变（找到了Target上对应的name）
        Transform skinRendererRootBoneAfter = null;
        
        // 处理SkinnedMeshRenderer
        foreach (var transTargetBone in transTargetBones)
        {
            if (rendererSkinObject.rootBone.name == transTargetBone.name)
            {
                rendererSkinObject.rootBone = transTargetBone;
                skinRendererRootBoneAfter = transTargetBone;
                Debug.Log("rootBone :'" + rendererSkinObject.name + ".rootBone' has been successfully replaced!");
                break;
            }
        }
        // 当遍历完Target的骨骼 但没找到对应的name时
        if (skinRendererRootBoneAfter == null)
        {
            Debug.LogWarning("Can't find rootBone: '" + rendererSkinObject.rootBone.name + "' in '" +transTargetObject.name  + "' while replacing '" + rendererSkinObject.name + ".SkinnedMeshRenderer.rootBone'. Replacement process has been shut down.");
            return;
        }
        
        // （防止两个物体的Bones信息顺序不一致，采取存Origin顺序的List的方式）
        List<Transform> tempSkinRendererBonesInfo = new List<Transform>();
        // 处理Bones
        foreach (var transSkinBone in rendererSkinObject.bones)
        {
            // 存个变化后的skinBone映射信息，用于判断是否找到了在Target上的映射
            Transform transSkinBoneAfter = null;
            foreach (var transTargetBone in transTargetBones)
            {
                if (transSkinBone.name == transTargetBone.name)
                {
                    tempSkinRendererBonesInfo.Add(transTargetBone);
                    transSkinBoneAfter = transTargetBone;
                    break;
                }
            }
            // 当遍历完Target的骨骼 但没找到对应的name时
            if (transSkinBoneAfter == null)
            {
                Debug.LogWarning("Can't find bone: '" + transSkinBone.name + "' in '" + transTargetObject.name + "' while replacing '" + rendererSkinObject.name + "SkinnedMeshRenderer.bones'. ★Force replacement process (may cause problems).★");
                tempSkinRendererBonesInfo.Add(null);
            }
        }
        // 将skin顺序的origin bones新数组赋值给skin
        rendererSkinObject.bones = tempSkinRendererBonesInfo.ToArray();
        Debug.Log("bones :'" + rendererSkinObject.name + ".SkinnedMeshRenderer.bones' have been successfully replaced!");
        
        // 把Skin的Renderer移到Origin的下
        rendererSkinObject.transform.SetParent(transTargetObject);
        // 归零(强迫症犯了）
        rendererSkinObject.transform.localPosition = Vector3.zero;
        rendererSkinObject.transform.eulerAngles = transTargetObject.eulerAngles;
        Debug.Log("Move SkinnedMeshRendererObject to be a child of TargetObject");
        
        Debug.Log("Ame's One SkinnedMeshRenderer's information replacement finished.");
    }
}
