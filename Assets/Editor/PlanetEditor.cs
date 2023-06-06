using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetGenerator))]
public class PlanetEditor : Editor
{
    PlanetGenerator planet;
    Editor shapeEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if(GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.Shapesettings, planet.OnShapeSettingsUpdate, ref planet.ShapeSettingsFoldout, ref shapeEditor);
    }

    private void OnEnable()
    {
        planet = (PlanetGenerator)target;
    }

    private void DrawSettingsEditor(Object settings, System.Action callback, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if(check.changed)
                    {
                        callback?.Invoke();
                    }
                }
            }
        }
    }
}
