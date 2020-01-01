using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpellTable))]
public class SpellTableEditor : GameDataTableBaseEditor<SpellData>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(CircleTable))]
public class CircleTableEditor : GameDataTableBaseEditor<CircleData>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(ActionTable))]
public class ActionTableEditor : GameDataTableBaseEditor<ActionData>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(GridTypeTable))]
public class GridTypeTableEditor : GameDataTableBaseEditor<GridTypeData>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : GameDataTableBaseEditor<GridData>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    protected override void SetParameters()
    {
        base.SetParameters();

        ImportToggle.GoogleSheet = false;
        ImportToggle.Excel = false;
        ExportToggle.GoogleSheet = false;
        ExportToggle.Excel = false;
    }
}
