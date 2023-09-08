using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(OneKeyBlend))]
public class OneKeyBlendInspector : Editor
{
    private OneKeyBlend _oneKeyBlend;
    public override void OnInspectorGUI()
    {
        _oneKeyBlend = (OneKeyBlend)target;
        DrawTargetObjectTransformField();
        DrawSkinsObjectTransformField();
        DrawClassicBlendButton();
        DrawAllSetsBonesBlendButton();
        DrawOneSkinObjectTransformField();
        DrawOneSetBonesBlendButton();
    }

    private void DrawTargetObjectTransformField()
    {
        EditorGUI.BeginChangeCheck();
        Transform transTargetObjectField = EditorGUILayout.ObjectField("目标", _oneKeyBlend.transTargetObject, typeof(Transform), true) as Transform;
        if (EditorGUI.EndChangeCheck())
        {
            _oneKeyBlend.transTargetObject = transTargetObjectField;
        }
    }
    
    private void DrawSkinsObjectTransformField()
    {
        EditorGUI.BeginChangeCheck();
        Transform transSourceObjectField = EditorGUILayout.ObjectField("来源（整体）", _oneKeyBlend.transSkinObject, typeof(Transform), true) as Transform;
        if (EditorGUI.EndChangeCheck())
        {
            _oneKeyBlend.transSkinObject = transSourceObjectField;
        }
    }
    
    private void DrawClassicBlendButton()
    {
        if (GUILayout.Button("一键移动骨骼（经典）"))
        {
            // 在按钮被点击时触发的函数
            _oneKeyBlend.NANAsBonesBlendMethod();
        }
    }
    
    private void DrawAllSetsBonesBlendButton()
    {
        if (GUILayout.Button("一键替换Renderer.Bones（整体）"))
        {
            // 在按钮被点击时触发的函数
            _oneKeyBlend.AmesAllSkinnedMeshRendererInfoReplacementMethod();
        }
    }
    
    private void DrawOneSkinObjectTransformField()
    {
        EditorGUI.BeginChangeCheck();
        SkinnedMeshRenderer rendererSkinObjectField = EditorGUILayout.ObjectField("来源（单个）", _oneKeyBlend.rendererSkinObject, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
        if (EditorGUI.EndChangeCheck())
        {
            _oneKeyBlend.rendererSkinObject = rendererSkinObjectField;
        }
    }
    
    private void DrawOneSetBonesBlendButton()
    {
        if (GUILayout.Button("一键替换Renderer.Bones（单个、强制）"))
        {
            // 在按钮被点击时触发的函数
            _oneKeyBlend.AmesOneSkinnedMeshRendererInfoReplacementMethod();
        }
    }
}
