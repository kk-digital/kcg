using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

[InitializeOnLoad]
public class FileExtensionGUI
{
    private static GUIStyle _style;
    private static readonly StringBuilder sb = new();
    private static string _selectedGuid;
    private static readonly HashSet<string> ShowExtExclude = new()
    {
        ".asmdef"
    };

    static FileExtensionGUI()
    {
        EditorApplication.projectWindowItemOnGUI += HandleOnGUI;
        Selection.selectionChanged += () =>
        {
            if (Selection.activeObject != null)
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out _selectedGuid, out long id);
        };

    }

    private static bool ValidString(string str)
    {
        return !string.IsNullOrEmpty(str) && str.Length > 7;
    }

    private static void HandleOnGUI(string guid, Rect selectionRect)
    {
        if (IsThumbnailsView)
            return;
        
        var path = AssetDatabase.GUIDToAssetPath(guid);
        if (0 >= path.Length)
            return;
        
        var attr = File.GetAttributes(path);

        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            return;

        var nameRaw = Path.GetFileNameWithoutExtension(path);
        var extRaw = Path.GetExtension(path);
        
        if (ShowExtExclude.Contains(extRaw))
            return;
        
        var selected = false;
        if (ValidString(guid) && ValidString(_selectedGuid))
            selected = string.Compare(guid, 0, _selectedGuid, 0, 6) == 0;


        sb.Clear().Append(extRaw);

        var ext = sb.ToString();

        _style ??= new GUIStyle(EditorStyles.label);

        _style.normal.textColor = selected ? new Color32(255, 255, 255, 255) : new Color32(220, 220, 220, 220);
        var extSize = _style.CalcSize(new GUIContent(ext));
        var nameSize = _style.CalcSize(new GUIContent(nameRaw));
        selectionRect.x += nameSize.x + (IsSingleColumnView ? 15 : 18);
        selectionRect.width = nameSize.x + 1 + extSize.x;
        

        var offsetRect = new Rect(selectionRect.position, selectionRect.size);
        EditorGUI.LabelField(offsetRect, ext, _style);
    }
    
    private static bool IsThumbnailsView {
        get {
            var projectWindow = GetProjectWindow();
            var gridSize = projectWindow.GetType().GetProperty("listAreaGridSize", BindingFlags.Instance | BindingFlags.Public);
            var columnsCount = (int) projectWindow.GetType().GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(projectWindow);
            return columnsCount == 1 && (float)gridSize.GetValue(projectWindow, null) > 16f;
        }
    }
    
    private static bool IsSingleColumnView {
        get {
            var projectWindow = GetProjectWindow();
            var columnsCount = (int) projectWindow.GetType().GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(projectWindow);
            return columnsCount == 0;
        }
    }
    
    private static EditorWindow GetProjectWindow() {
        if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == "Project") {
            return EditorWindow.focusedWindow;
        }

        return GetExistingWindowByName("Project");
    }

    private static EditorWindow GetExistingWindowByName(string name) {
        EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (EditorWindow item in windows) {
            if (item.titleContent.text == name) {
                return item;
            }
        }

        return default(EditorWindow);
    }
}