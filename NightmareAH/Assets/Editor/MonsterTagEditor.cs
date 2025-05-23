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
            EditorGUILayout.HelpBox("���� ���� ������: �Ʒ� ��ư���� �ڵ� �߰��� �� �ֽ��ϴ�.", MessageType.Error);
        }
        else
        {
            EditorGUILayout.HelpBox("���� ���� �����Դϴ�.", MessageType.Info);
        }

        if (GUILayout.Button("������ ���� �߰��ϴ� ��ư"))
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
