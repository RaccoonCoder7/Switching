//======= Copyright (c) Kandooz Studio, All rights reserved. ===============
//
// Purpose: Editor for hands animation controller, helps you set the values with  sliders
// and it's cool to write editors ¯\_(ツ)_/¯
//
//=============================================================================
using UnityEditor;
using UnityEngine;

namespace Kandooz.Burger
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AnimationController))]
    public class AnimationControllerEditor : Editor
    {
        AnimationController controller;

        private void OnEnable()
        {
            controller = (AnimationController)target;
            controller.animator = controller.GetComponent<Animator>();
            EditorApplication.update += Update;
        }
        private void OnDisable()
        {
            EditorApplication.update -= Update;

        }
        public override void OnInspectorGUI()
        {
            var staticPose = controller.StaticPose;
            var pose = controller.Pose;
            var grip = controller.Grip;
            var gripPercentage = controller.GripPercentage;
            var index = controller.Index;
            var indexPercentage = controller.IndexPercentage;
            var thumb = controller.Thumb;
            var thumbState = controller.ThumbState;
            var thumbPercentage = controller.ThumbPercentage;
            
            DrawDefaultInspector();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("staticPose"));
            if (controller.StaticPose)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pose"));
            }

            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("grip"));
                if (serializedObject.FindProperty("grip").boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("gripPercentage"));
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("index"));
                if (serializedObject.FindProperty("index").boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("indexPercentage"));
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("thumb"));
                if (serializedObject.FindProperty("thumb").boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("thumbPercentage"));
                }
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drivedByAnimation"));

            serializedObject.ApplyModifiedProperties();
            #region change check
            if (EditorApplication.isPlaying)
            {
                if (staticPose != controller.StaticPose)
                {
                    controller.StaticPose = controller.StaticPose;
                }
                if (pose != controller.Pose)
                {
                    controller.Pose = controller.Pose;
                }
                if (grip != controller.Grip)
                {
                    controller.Grip = controller.Grip;
                }
                if (gripPercentage != controller.GripPercentage)
                {
                    controller.GripPercentage = controller.GripPercentage;
                }
                if (index != controller.Index)
                {
                    controller.Index = controller.Index;
                }
                if (indexPercentage != controller.IndexPercentage)
                {
                    controller.IndexPercentage = controller.IndexPercentage;
                }
                if (thumb != controller.Thumb)
                {
                    controller.Thumb = controller.Thumb;
                }
                if (thumbState != controller.ThumbState)
                {
                    controller.ThumbState = controller.ThumbState;
                }
                if (thumbPercentage != controller.ThumbPercentage)
                {
                    controller.ThumbPercentage = controller.ThumbPercentage;
                }
            }
            else
            {
                controller.StaticPose = controller.StaticPose;
                controller.Pose = controller.Pose;
                controller.Grip = controller.Grip;
                controller.GripPercentage = controller.GripPercentage;
                controller.Index = controller.Index;
                controller.IndexPercentage = controller.IndexPercentage;
                controller.Thumb = controller.Thumb;
                controller.ThumbState = controller.ThumbState;
                controller.ThumbPercentage = controller.ThumbPercentage;


            }
            #endregion
        }
        private void Update()
        {

            //Debug.Log(delta);
            if (!EditorApplication.isPlaying )
            {
                controller.animator.Update(.01f);

                if (serializedObject.FindProperty("drivedByAnimation").boolValue)
                {
                    controller.StaticPose = controller.StaticPose;
                    controller.Pose = controller.Pose;
                    controller.Grip = controller.Grip;
                    controller.Index = controller.Index;
                    controller.Thumb = controller.Thumb;
                    controller.GripPercentage = controller.GripPercentage;
                    controller.IndexPercentage = controller.IndexPercentage;
                    controller.ThumbPercentage = controller.ThumbPercentage;
                    controller.ThumbState = controller.ThumbState;

                }
            }
        }
    }
}