using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Sirenix.OdinInspector;
using System.Reflection;

// Created by Dor Ben Dor, agesonera@gmail.com
public class RagdollInitalizer : MonoBehaviour
{
    [MenuItem("CONTEXT/Animator/Create Ragdoll")]
    private static void GetRagdollScreen(MenuCommand command)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type ragdollWizard = null;
        ScriptableWizard wiz = null;

        foreach (var asmb in assemblies)
        {
            if (asmb.GetType("UnityEditor.RagdollBuilder") != null)
            {
                ragdollWizard = asmb.GetType("UnityEditor.RagdollBuilder");
                wiz = ScriptableWizard.DisplayWizard("Create Ragdoll", ragdollWizard);
                break;
            }
        }
        if (ragdollWizard != null)
        {
            var finfo = ragdollWizard.GetFields();
            var animator = command.context as Animator;

            foreach (var field in finfo)
            {
                switch (field.Name)
                {
                    case "pelvis":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.Hips));
                        break;
                    case "leftHips":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg));
                        break;
                    case "leftKnee":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg));
                        break;
                    case "leftFoot":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.LeftFoot));
                        break;
                    case "rightHips":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.RightUpperLeg));
                        break;
                    case "rightKnee":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg));
                        break;
                    case "rightFoot":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.RightFoot));
                        break;
                    case "leftArm":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.LeftUpperArm));
                        break;
                    case "leftElbow":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.LeftLowerArm));
                        break;
                    case "rightArm":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.RightUpperArm));
                        break;
                    case "rightElbow":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.RightLowerArm));
                        break;
                    case "middleSpine":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.Chest));
                        break;
                    case "head":
                        field.SetValue(wiz, animator.GetBoneTransform(HumanBodyBones.Head));
                        break;
                    default:
                        break;
                }
            }

            ragdollWizard.GetMethod("OnWizardUpdate", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(wiz, new object[0]);
        }
    }
}
