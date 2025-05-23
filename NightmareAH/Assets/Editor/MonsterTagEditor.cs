using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MonsterTag))]
public class MonsterTagEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MonsterTag tag = (MonsterTag)target;
        GameObject go = tag.gameObject;

        bool hasEntity = go.GetComponent<MonsterEntity>() != null;
        bool hasDraw = go.GetComponent<MonsterDrawLayer>() != null;
        bool hasAniCtlr = go.GetComponent<MonsterAnimController>() != null;
        bool hasColCtlr = go.GetComponent<MonsterColController>() != null;

        bool hasAnimator = go.GetComponent<Animator>() != null;
        bool hasRenderer = go.GetComponent<SpriteRenderer>() != null;
        bool hasCollider = go.GetComponent<Collider>() != null;

        if (!hasEntity || !hasAniCtlr || !hasDraw || !hasAnimator || !hasRenderer || !hasCollider ||!hasColCtlr)
        {
            EditorGUILayout.HelpBox("몬스터 구성 누락됨: 아래 버튼으로 자동 추가할 수 있습니다.", MessageType.Error);
        }
        else
        {
            EditorGUILayout.HelpBox("몬스터 구성 정상입니다.", MessageType.Info);
        }

        if (GUILayout.Button("누락된 구성 추가하는 버튼"))
        {
            if (!hasEntity) go.AddComponent<MonsterEntity>();
            if (!hasDraw) go.AddComponent<MonsterDrawLayer>();
            if (!hasAniCtlr) go.AddComponent<MonsterAnimController>();
            if (!hasColCtlr) go.AddComponent<MonsterColController>();

            if (!hasAnimator) go.AddComponent<Animator>();
            if (!hasRenderer) go.AddComponent<SpriteRenderer>();
            if (!hasCollider) go.AddComponent<Collider>();
        }
    }


}
