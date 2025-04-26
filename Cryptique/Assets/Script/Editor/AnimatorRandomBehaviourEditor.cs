using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(AnimatorRandomBehaviour))]
    public class AnimatorRandomBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AnimatorRandomBehaviour behaviour = (AnimatorRandomBehaviour)target;

            if (GUILayout.Button("Fill Random Triggers"))
            {
                FillRandomTriggers(behaviour);
            }
        }

        private void FillRandomTriggers(AnimatorRandomBehaviour behaviour)
        {
            // Essayer de retrouver l'AnimatorController contenant ce StateMachineBehaviour
            AnimatorController controller = FindAnimatorControllerContainingBehaviour(behaviour);

            if (controller == null)
            {
                Debug.LogWarning("AnimatorController not found for this behaviour.");
                return;
            }

            behaviour.randomTriggerNames = new List<string>();

            foreach (var parameter in controller.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger &&
                    parameter.name.Contains("Random"))
                {
                    behaviour.randomTriggerNames.Add(parameter.name);
                }
            }

            EditorUtility.SetDirty(behaviour);
            AssetDatabase.SaveAssets();
        }

        private AnimatorController FindAnimatorControllerContainingBehaviour(AnimatorRandomBehaviour behaviour)
        {
            // Chercher tous les AnimatorControllers dans le projet
            string[] guids = AssetDatabase.FindAssets("t:AnimatorController");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);

                foreach (var layer in controller.layers)
                {
                    if (SearchBehaviourInStateMachine(layer.stateMachine, behaviour))
                        return controller;
                }
            }

            return null;
        }

        private bool SearchBehaviourInStateMachine(AnimatorStateMachine stateMachine, AnimatorRandomBehaviour behaviour)
        {
            foreach (var state in stateMachine.states)
            {
                foreach (var b in state.state.behaviours)
                {
                    if (b == behaviour)
                        return true;
                }
            }

            foreach (var subStateMachine in stateMachine.stateMachines)
            {
                if (SearchBehaviourInStateMachine(subStateMachine.stateMachine, behaviour))
                    return true;
            }

            return false;
        }
    }
}
