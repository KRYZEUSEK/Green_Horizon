using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace Cards {
    [CustomEditor(typeof(Card))]
    public class CardEditor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            SerializedProperty cardTitle = serializedObject.FindProperty("_title");
            SerializedProperty cardDescription = serializedObject.FindProperty("_description");
            SerializedProperty cardImage = serializedObject.FindProperty("_image");
            SerializedProperty cardDecisions = serializedObject.FindProperty("_decisions");

            cardTitle.stringValue = TextField(
                new GUIContent("Title"),
                cardTitle.stringValue
            );

            cardDescription.stringValue = TextArea(
                cardDescription.stringValue,
                GUILayout.Height(30), // Set initial height
                GUILayout.ExpandHeight(true) // Allow resizing
            );

            ObjectField(
                cardImage, 
                typeof(Sprite), 
                new GUIContent("Image")
            );

            for (int i = 0; i < cardDecisions.arraySize; i++) {
                SerializedProperty decision = cardDecisions.GetArrayElementAtIndex(i);
                SerializedProperty decisionDescription = decision.FindPropertyRelative("_description");
                SerializedProperty decisionBudget = decision.FindPropertyRelative("_budget");
                SerializedProperty decisionSatisfaction = decision.FindPropertyRelative("_satisfaction");
                SerializedProperty decisionInfrastructure = decision.FindPropertyRelative("_infrastructure");
                SerializedProperty decisionOrder = decision.FindPropertyRelative("_order");
                SerializedProperty decisionEnvironment = decision.FindPropertyRelative("_environment");
                SerializedProperty decisionEffectsDuration = decision.FindPropertyRelative("_effectsDuration");

                LabelField($"Decision {i + 1}.");

                decisionDescription.stringValue = TextArea(
                    decisionDescription.stringValue,
                    GUILayout.Height(30),
                    GUILayout.ExpandHeight(true)
                );

                decisionBudget.intValue = IntField(
                    new GUIContent("BUD"),
                    decisionBudget.intValue
                );

                decisionSatisfaction.intValue = IntField(
                    new GUIContent("SAT"),
                    decisionSatisfaction.intValue
                );

                decisionInfrastructure.intValue = IntField(
                    new GUIContent("INF"),
                    decisionInfrastructure.intValue
                );

                decisionOrder.intValue = IntField(
                    new GUIContent("ORD"),
                    decisionOrder.intValue
                );

                decisionEnvironment.intValue = IntField(
                    new GUIContent("ENV"),
                    decisionEnvironment.intValue
                );

                decisionEffectsDuration.intValue = IntField(
                    new GUIContent("Effects duration"),
                    decisionEffectsDuration.intValue
                );
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}