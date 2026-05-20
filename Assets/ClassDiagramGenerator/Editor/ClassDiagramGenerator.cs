using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ClassDiagramGenerator
{
    public class ClassDiagramGenerator : EditorWindow
    {
        private enum ExportFormat
        {
            PlantUMLFile,
            PlantUML_URL
        }
        
        private const string DocumentationUrl = "https://julestools.gitbook.io/julestools-docs/documentation/tools/class-diagram-generator";

        private ExportFormat _exportFormat = ExportFormat.PlantUMLFile;
        private string _lastDiagramURL = "";
        private string _outputPath = "Assets/ClassDiagramGeneratorOutput/ClassDiagram.puml";
        private string _status = "Ready.";
        private Vector2 _outerScroll;
        private bool _includeAssociations = true;

        private ScriptSelectionManager _scriptManager = new();
        private Vector2 _scriptScroll;
        private string _scriptSearch = "";
        private bool _scriptsScanned;
        private DefaultAsset _scanTarget;

        private Texture2D _bgTex;
        private const float BG_OVERLAY_ALPHA = 0.26f;

        private static readonly Color COL_BG = new(0.12f, 0.12f, 0.14f, 1f);
        private static readonly Color COL_PANEL = new(0.16f, 0.17f, 0.20f, 1f);
        private static readonly Color COL_PANEL_2 = new(0.20f, 0.21f, 0.24f, 1f);
        private static readonly Color COL_BORDER = new(0.30f, 0.32f, 0.36f, 1f);
        private static readonly Color COL_TEXT = new(0.94f, 0.94f, 0.97f, 1f);
        private static readonly Color COL_TEXT_SUB = new(0.78f, 0.80f, 0.84f, 1f);
        private static readonly Color COL_ROW_EVEN = new(1f, 1f, 1f, 0.03f);

        private static readonly Color ACCENT_VIOLET = new(0.43f, 0.34f, 0.68f, 1f);
        private static readonly Color ACCENT_BLUE = new(0.36f, 0.62f, 0.97f, 1f);
        private static readonly Color ACCENT_V_HOVER = new(0.54f, 0.45f, 0.80f, 1f);
        private static readonly Color ACCENT_B_HOVER = new(0.52f, 0.74f, 1.00f, 1f);

        private Texture2D _texPanel, _texPanel2;
        private GUIStyle _titleStyle, _subtitleStyle, _cardHeaderStyle, _cardBodyStyle, _dirStyle;

        private Texture2D GetHeaderIcon() => Resources.Load<Texture2D>("Icon 160x160 - Diagram Generator");

        [MenuItem("Tools/Diagram Generator")]
        private static void ShowWindow()
        {
            var w = GetWindow<ClassDiagramGenerator>("Diagram Generator");
            w.minSize = new Vector2(640, 460);
        }

        private void OnEnable()
        {
            _bgTex = Resources.Load<Texture2D>("settings_bg");
            if (_bgTex == null)
                _bgTex = Resources.Load<Texture2D>("inspector_bg");

            TryAutoScan();
        }

        private void TryAutoScan()
        {
            string[] preferredFolders =
            {
                "Assets/Scripts",
                "Assets/_Project",
                "Assets"
            };

            foreach (string folder in preferredFolders)
            {
                if (!AssetDatabase.IsValidFolder(folder))
                    continue;

                _scriptManager.Scan(folder);

                if (_scriptManager.Scripts.Count > 0)
                {
                    _scriptsScanned = true;
                    _status = $"Scanned: {_scriptManager.Scripts.Count} files under '{folder}'.";
                    return;
                }
            }

            _scriptsScanned = false;
            _status = "Select a folder or drop .cs files to begin.";
        }

        private void EnsureThemeAssets()
        {
            if (Event.current == null)
                return;

            if (_texPanel == null) _texPanel = MakeTex(2, 2, COL_PANEL);
            if (_texPanel2 == null) _texPanel2 = MakeTex(2, 2, COL_PANEL_2);

            if (_titleStyle == null)
            {
                _titleStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 14,
                    richText = true
                };
                _titleStyle.normal.textColor = COL_TEXT;
            }

            if (_subtitleStyle == null)
            {
                _subtitleStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    alignment = TextAnchor.MiddleLeft
                };
                _subtitleStyle.normal.textColor = COL_TEXT_SUB;
            }

            if (_cardHeaderStyle == null)
            {
                _cardHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 12
                };
                _cardHeaderStyle.normal.textColor = COL_TEXT;
            }

            if (_cardBodyStyle == null)
            {
                _cardBodyStyle = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(12, 12, 10, 12),
                    margin = new RectOffset(0, 0, 0, 0)
                };
                _cardBodyStyle.normal.background = _texPanel2;
            }

            if (_dirStyle == null)
            {
                _dirStyle = new GUIStyle(EditorStyles.miniLabel);
                _dirStyle.normal.textColor = new Color(0.76f, 0.78f, 0.82f, 1f);
            }
        }

        private void OnGUI()
        {
            EnsureThemeAssets();

            DrawBackground();
            DrawHeader();
            DrawToolbar();

            _outerScroll = EditorGUILayout.BeginScrollView(_outerScroll);
            DrawCard("Selection", DrawSelectionCard);
            DrawCard("Export Options", DrawExportCard);
            DrawCard("Advanced", DrawAdvancedCard);
            EditorGUILayout.EndScrollView();

            DrawFooter();
        }

        private void DrawBackground()
        {
            var r = new Rect(0, 0, position.width, position.height);
            if (_bgTex != null)
            {
                GUI.DrawTexture(r, _bgTex, ScaleMode.ScaleAndCrop, true);
                EditorGUI.DrawRect(r, new Color(0, 0, 0, BG_OVERLAY_ALPHA));
            }
            else
            {
                EditorGUI.DrawRect(r, COL_BG);
            }
        }

        private void DrawHeader()
        {
            GUILayout.Space(6);
            var icon = GetHeaderIcon();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (icon != null)
                    GUILayout.Label(icon, GUILayout.Width(40), GUILayout.Height(40));

                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Space(2);
                    EditorGUILayout.LabelField("Class Diagram Generator", _titleStyle);
                    EditorGUILayout.LabelField("Generate UML (PlantUML) from your C# scripts", _subtitleStyle);
                }
            }

            var line = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(line, COL_BORDER);
            GUILayout.Space(2);
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                DrawFormatTabs();
                GUILayout.FlexibleSpace();

                if (ToolbarSecondary("Docs"))
                    OpenDocumentation();

                GUILayout.Space(4);

                if (ToolbarPrimary("Generate"))
                    GenerateDiagram(_exportFormat);
            }

            GUILayout.Space(4);
        }
        
        private bool ToolbarSecondary(string label)
        {
            var r = GUILayoutUtility.GetRect(new GUIContent(label), EditorStyles.toolbarButton, GUILayout.Width(72));
            bool hover = r.Contains(Event.current.mousePosition);

            EditorGUI.DrawRect(r, hover ? COL_PANEL_2 : COL_PANEL);

            var s = new GUIStyle(EditorStyles.toolbarButton)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            s.normal.textColor = COL_TEXT;

            return GUI.Button(r, label, s);
        }
        
        private void OpenDocumentation()
        {
            if (string.IsNullOrWhiteSpace(DocumentationUrl))
            {
                EditorUtility.DisplayDialog("Documentation", "Documentation URL is empty.", "OK");
                return;
            }

            Application.OpenURL(DocumentationUrl);
        }

        private void DrawFormatTabs()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                for (int i = 0; i < 2; i++)
                {
                    bool active = (int)_exportFormat == i;
                    string label = i == 0 ? "File (.puml)" : "URL";

                    var content = new GUIContent(label);
                    var r = GUILayoutUtility.GetRect(content, EditorStyles.toolbarButton, GUILayout.Width(110), GUILayout.Height(20));

                    EditorGUI.DrawRect(r, active ? ACCENT_BLUE : new Color(0, 0, 0, 0f));

                    bool hover = r.Contains(Event.current.mousePosition);
                    if (active)
                    {
                        var underline = new Rect(r.x, r.yMax - 2, r.width, 2);
                        EditorGUI.DrawRect(underline, hover ? ACCENT_B_HOVER : ACCENT_VIOLET);
                    }
                    else if (hover)
                    {
                        var outline = new Rect(r.x, r.yMax - 1, r.width, 1);
                        EditorGUI.DrawRect(outline, COL_BORDER);
                    }

                    var style = new GUIStyle(EditorStyles.toolbarButton)
                    {
                        fontStyle = active ? FontStyle.Bold : FontStyle.Normal,
                        alignment = TextAnchor.MiddleCenter
                    };
                    style.normal.textColor = active ? Color.white : COL_TEXT_SUB;

                    if (GUI.Button(r, content, style))
                        _exportFormat = (ExportFormat)i;

                    GUILayout.Space(2);
                }
            }
        }

        private bool ToolbarPrimary(string label)
        {
            var r = GUILayoutUtility.GetRect(new GUIContent(label), EditorStyles.toolbarButton, GUILayout.Width(92));
            bool hover = r.Contains(Event.current.mousePosition);
            EditorGUI.DrawRect(r, hover ? ACCENT_B_HOVER : ACCENT_BLUE);

            var s = new GUIStyle(EditorStyles.toolbarButton) { fontStyle = FontStyle.Bold };
            s.normal.textColor = Color.white;
            return GUI.Button(r, label, s);
        }

        private void DrawCard(string title, Action body)
        {
            var header = GUILayoutUtility.GetRect(1, 24, GUILayout.ExpandWidth(true));
            if (Event.current.type == EventType.Repaint)
                GUI.DrawTexture(header, _texPanel);

            GUI.Label(new Rect(header.x + 10, header.y, header.width - 20, header.height), title, _cardHeaderStyle);
            EditorGUI.DrawRect(new Rect(header.x, header.yMax - 1, header.width, 1), COL_BORDER);

            EditorGUILayout.BeginVertical(_cardBodyStyle);
            try
            {
                body?.Invoke();
            }
            finally
            {
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(10);
        }

        private void DrawSelectionCard()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                _scanTarget = (DefaultAsset)EditorGUILayout.ObjectField(
                    new GUIContent("Folder / .cs", "Choose a folder or a .cs file"),
                    _scanTarget,
                    typeof(DefaultAsset),
                    false,
                    GUILayout.ExpandWidth(true),
                    GUILayout.MinWidth(200));

                GUILayout.Space(6);

                if (SecondaryButton("Scan", 20))
                    ScanSelectionTarget();

                if (MiniButton("Scan Assets", 88))
                    ScanFolder("Assets");

                GUILayout.FlexibleSpace();
            }

            GUILayout.Space(6);

            var dz = GUILayoutUtility.GetRect(1, 64, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(dz, COL_PANEL);
            EditorGUI.DrawRect(new Rect(dz.x, dz.y, dz.width, 1), COL_BORDER);
            EditorGUI.DrawRect(new Rect(dz.x, dz.yMax - 1, dz.width, 1), COL_BORDER);

            var dzLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel) { fontSize = 11 };
            dzLabel.normal.textColor = COL_TEXT_SUB;
            GUI.Label(dz, "Drop a folder to scan it • Drop .cs files to add them", dzLabel);

            HandleDragAndDrop(dz);

            GUILayout.Space(8);

            using (new EditorGUILayout.HorizontalScope())
            {
                _scriptSearch = EditorGUILayout.TextField(new GUIContent("Search", "Filter by file name"), _scriptSearch);

                if (MiniButton("All", 56))
                    foreach (var s in _scriptManager.Scripts)
                        s.IsSelected = true;

                if (MiniButton("None", 56))
                    foreach (var s in _scriptManager.Scripts)
                        s.IsSelected = false;

                if (MiniButton("Invert", 64))
                    for (int i = 0; i < _scriptManager.Scripts.Count; i++)
                        _scriptManager.Scripts[i].IsSelected = !_scriptManager.Scripts[i].IsSelected;
            }

            GUILayout.Space(4);

            using (new EditorGUILayout.HorizontalScope())
            {
                int total = _scriptManager.Scripts.Count;
                int selected = _scriptManager.Scripts.Count(s => s.IsSelected);

                GUILayout.Label($"Total: {total}", EditorStyles.miniBoldLabel, GUILayout.Width(90));
                GUILayout.Label($"Selected: {selected}", EditorStyles.miniBoldLabel, GUILayout.Width(110));
                GUILayout.FlexibleSpace();
            }

            var filtered = string.IsNullOrWhiteSpace(_scriptSearch)
                ? _scriptManager.Scripts
                : _scriptManager.Scripts
                    .Where(s => s.FileName.IndexOf(_scriptSearch, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

            _scriptScroll = EditorGUILayout.BeginScrollView(_scriptScroll, GUILayout.Height(240));

            if (filtered.Count == 0)
            {
                EditorGUILayout.HelpBox("Use Scan or drop content above to populate the list.", MessageType.Info);
            }
            else
            {
                for (int i = 0; i < filtered.Count; i++)
                {
                    var s = filtered[i];
                    var row = GUILayoutUtility.GetRect(1, 20, GUILayout.ExpandWidth(true));

                    if (i % 2 == 0)
                        EditorGUI.DrawRect(row, COL_ROW_EVEN);

                    var tRect = new Rect(row.x + 6, row.y + 2, 20, row.height - 2);
                    s.IsSelected = EditorGUI.Toggle(tRect, s.IsSelected);

                    var nameRect = new Rect(tRect.xMax + 4, row.y + 1, 320, row.height - 2);
                    var nameLabel = new GUIStyle(EditorStyles.label);
                    nameLabel.normal.textColor = COL_TEXT;
                    EditorGUI.LabelField(nameRect, s.FileName, nameLabel);

                    var dirRect = new Rect(nameRect.xMax + 8, row.y + 1, row.width - nameRect.width - 40, row.height - 2);
                    if (!string.IsNullOrEmpty(s.Path))
                    {
                        EditorGUIUtility.AddCursorRect(dirRect, MouseCursor.Link);
                        EditorGUI.LabelField(dirRect, s.Path, _dirStyle);

                        if (Event.current.type == EventType.MouseDown && dirRect.Contains(Event.current.mousePosition))
                        {
                            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s.Path);
                            if (obj != null)
                            {
                                Selection.activeObject = obj;
                                EditorGUIUtility.PingObject(obj);
                            }
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawExportCard()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Output (.puml)", GUILayout.Width(120));
                _outputPath = EditorGUILayout.TextField(_outputPath);

                if (MiniButton("…", 28))
                {
                    string path = EditorUtility.SaveFilePanelInProject(
                        "Save PlantUML file",
                        "ClassDiagram",
                        "puml",
                        "Choose where to save the PlantUML file.");

                    if (!string.IsNullOrEmpty(path))
                        _outputPath = path;
                }
            }

            GUILayout.Space(8);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (PrimaryButton("🛠️  Generate Diagram", 42))
                    GenerateDiagram(_exportFormat);
                GUILayout.FlexibleSpace();
            }

            if (_exportFormat == ExportFormat.PlantUML_URL && !string.IsNullOrEmpty(_lastDiagramURL))
            {
                GUILayout.Space(6);
                EditorGUILayout.SelectableLabel(_lastDiagramURL, EditorStyles.textField, GUILayout.Height(20));

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (SecondaryButton("📋 Copy URL"))
                        EditorGUIUtility.systemCopyBuffer = _lastDiagramURL;
                    GUILayout.FlexibleSpace();
                }
            }
        }

        private void DrawAdvancedCard()
        {
            _includeAssociations = EditorGUILayout.ToggleLeft(
                new GUIContent("Include associations (fields/parameters of other classes)"),
                _includeAssociations);

            GUILayout.Space(2);
        }

        private void DrawFooter()
        {
            var line = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(line, COL_BORDER);

            var box = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12
            };
            box.normal.textColor = COL_TEXT;

            var r = GUILayoutUtility.GetRect(1, 32, GUILayout.ExpandWidth(true));
            if (Event.current.type == EventType.Repaint)
                GUI.DrawTexture(r, _texPanel);

            GUI.Label(r, string.IsNullOrEmpty(_status) ? "Ready." : _status, box);

            var sig = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                alignment = TextAnchor.LowerCenter,
                fontStyle = FontStyle.Italic
            };
            sig.normal.textColor = COL_TEXT_SUB;

            GUILayout.Label("© 2026 ClassDiagramGenerator • 2.1.0", sig);
        }

        private bool PrimaryButton(string label, float height)
        {
            var rect = GUILayoutUtility.GetRect(
                new GUIContent(label),
                EditorStyles.miniButton,
                GUILayout.Height(height),
                GUILayout.MinWidth(240),
                GUILayout.ExpandWidth(false));

            bool hover = rect.Contains(Event.current.mousePosition);

            var top = new Rect(rect.x, rect.y, rect.width, Mathf.Round(rect.height * 0.5f));
            EditorGUI.DrawRect(top, hover ? ACCENT_B_HOVER : ACCENT_BLUE);

            var bot = new Rect(rect.x, rect.y + top.height, rect.width, rect.height - top.height);
            EditorGUI.DrawRect(bot, hover ? ACCENT_V_HOVER : ACCENT_VIOLET);

            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), COL_BORDER);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 1, rect.width, 1), COL_BORDER);

            var style = new GUIStyle(GUI.skin.button)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            style.normal.textColor = Color.white;

            return GUI.Button(rect, label, style);
        }

        private bool SecondaryButton(string label, float height = 28f)
        {
            var rect = GUILayoutUtility.GetRect(
                new GUIContent(label),
                EditorStyles.miniButton,
                GUILayout.Height(height),
                GUILayout.MaxWidth(160));

            EditorGUI.DrawRect(rect, COL_PANEL_2);

            var style = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter
            };
            style.normal.textColor = COL_TEXT;

            return GUI.Button(rect, label, style);
        }

        private bool MiniButton(string label, float width)
        {
            var rect = GUILayoutUtility.GetRect(
                new GUIContent(label),
                EditorStyles.miniButton,
                GUILayout.Width(width));

            EditorGUI.DrawRect(rect, COL_PANEL_2);

            var s = new GUIStyle(EditorStyles.miniButton);
            s.normal.textColor = COL_TEXT;

            return GUI.Button(rect, label, s);
        }

        private void HandleDragAndDrop(Rect dropArea)
        {
            var e = Event.current;

            if (e.type != EventType.DragUpdated && e.type != EventType.DragPerform)
                return;

            if (!dropArea.Contains(e.mousePosition))
                return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (e.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                if (DragAndDrop.paths.Length == 1 && AssetDatabase.IsValidFolder(DragAndDrop.paths[0]))
                {
                    ScanFolder(DragAndDrop.paths[0]);
                }
                else
                {
                    var toAdd = new List<string>();

                    foreach (string p in DragAndDrop.paths)
                    {
                        if (AssetDatabase.IsValidFolder(p))
                        {
                            string[] guids = AssetDatabase.FindAssets("t:MonoScript", new[] { p });
                            foreach (string guid in guids)
                            {
                                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                                if (assetPath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                                    toAdd.Add(assetPath);
                            }
                        }
                        else if (p.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                        {
                            toAdd.Add(NormalizeAssetPath(p));
                        }
                    }

                    AddCsFiles(toAdd);
                }
            }

            e.Use();
        }

        private void ScanSelectionTarget()
        {
            if (_scanTarget == null)
            {
                EditorUtility.DisplayDialog("Scan", "Select a folder or a .cs file first.", "OK");
                return;
            }

            string path = AssetDatabase.GetAssetPath(_scanTarget);

            if (string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog("Scan", "Invalid asset.", "OK");
                return;
            }

            if (AssetDatabase.IsValidFolder(path))
            {
                ScanFolder(path);
            }
            else if (path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            {
                AddCsFiles(new[] { path });
            }
            else
            {
                EditorUtility.DisplayDialog("Scan", "Please select a folder or a .cs file.", "OK");
            }

            Repaint();
        }

        private void ScanFolder(string folder)
        {
            _scriptManager.Scan(folder);
            _scriptsScanned = _scriptManager.Scripts.Count > 0;
            _status = _scriptsScanned
                ? $"Scanned: {_scriptManager.Scripts.Count} files under '{folder}'."
                : $"No C# scripts found under '{folder}'.";
        }

        private void AddCsFiles(IEnumerable<string> paths)
        {
            int before = _scriptManager.Scripts.Count;
            _scriptManager.AddFiles(paths);
            int added = _scriptManager.Scripts.Count - before;

            if (added > 0)
            {
                _scriptsScanned = true;
                _status = $"Added {added} file(s) to the list.";
            }
            else
            {
                _status = "No new .cs files added.";
            }

            Repaint();
        }

        private static Texture2D MakeTex(int w, int h, Color c)
        {
            var tex = new Texture2D(w, h);
            var pix = new Color[w * h];
            for (int i = 0; i < pix.Length; i++) pix[i] = c;
            tex.SetPixels(pix);
            tex.Apply();
            return tex;
        }

        private static string NormalizeAssetPath(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? string.Empty : path.Replace('\\', '/');
        }

        private void GenerateDiagram(ExportFormat format)
        {
            if (!_scriptsScanned || _scriptManager.Scripts.Count == 0)
            {
                _status = "❌ No scripts scanned or added. Use Scan or drop files.";
                EditorUtility.DisplayDialog("Error", "No scripts have been scanned.", "OK");
                return;
            }

            var selectedScripts = _scriptManager.GetSelected();
            if (selectedScripts.Count == 0)
            {
                _status = "❗ No script selected!";
                EditorUtility.DisplayDialog("Error", "Select at least one script to include in the diagram.", "OK");
                return;
            }

            try
            {
                var parser = new CSharpParser();
                var umlClasses = new List<UmlClass>();
                var parseWarnings = new List<string>();

                foreach (var script in selectedScripts)
                {
                    if (!File.Exists(script.Path))
                    {
                        parseWarnings.Add($"Missing file: {script.Path}");
                        continue;
                    }

                    string content = File.ReadAllText(script.Path, Encoding.UTF8);
                    var parsed = parser.ParseClasses(content);

                    if (parsed.Count == 0)
                        parseWarnings.Add($"No class/interface found in: {script.Path}");

                    umlClasses.AddRange(parsed);
                }

                if (umlClasses.Count == 0)
                {
                    _status = "❗ No classes detected. Check your scripts or parser logic.";
                    EditorUtility.DisplayDialog(
                        "Nothing generated",
                        "No class or interface could be parsed from the selected scripts.\nCheck the selected files and parser coverage.",
                        "OK");
                    return;
                }

                string plantuml = PlantUmlGenerator.GeneratePlantUml(umlClasses, _includeAssociations);

                if (format == ExportFormat.PlantUMLFile)
                {
                    EnsureOutputDirectoryExists(_outputPath);
                    File.WriteAllText(_outputPath, plantuml, new UTF8Encoding(false));
                    AssetDatabase.Refresh();

                    _status = $"✅ Diagram generated: {_outputPath}  •  Classes: {umlClasses.Count}";
                    EditorUtility.DisplayDialog(
                        "Done!",
                        $"Diagram generated:\n{_outputPath}\nClasses: {umlClasses.Count}" +
                        (parseWarnings.Count > 0 ? $"\nWarnings: {parseWarnings.Count} (check Console or status)." : ""),
                        "OK");
                }
                else
                {
                    _lastDiagramURL = PlantUMLTextToUrl(plantuml);
                    _status = $"✅ Diagram URL generated. Classes: {umlClasses.Count}";
                    EditorUtility.DisplayDialog("Done!", "URL generated below.\nCopy-paste it in your browser.", "OK");
                }

                if (parseWarnings.Count > 0)
                    Debug.LogWarning("[ClassDiagramGenerator]\n" + string.Join("\n", parseWarnings));
            }
            catch (Exception ex)
            {
                _status = $"❌ Generation failed: {ex.Message}";
                Debug.LogException(ex);
                EditorUtility.DisplayDialog("Generation failed", ex.Message, "OK");
            }
        }

        private static void EnsureOutputDirectoryExists(string assetFilePath)
        {
            if (string.IsNullOrWhiteSpace(assetFilePath))
                throw new InvalidOperationException("Output path is empty.");

            if (!assetFilePath.StartsWith("Assets/", StringComparison.Ordinal) && assetFilePath != "Assets")
                throw new InvalidOperationException("Output path must stay inside the Unity project under 'Assets/'.");

            string folder = Path.GetDirectoryName(assetFilePath)?.Replace('\\', '/');
            if (string.IsNullOrWhiteSpace(folder))
                throw new InvalidOperationException("Invalid output folder.");

            CreateAssetFolderRecursive(folder);
        }

        private static void CreateAssetFolderRecursive(string assetFolder)
        {
            if (AssetDatabase.IsValidFolder(assetFolder))
                return;

            string[] parts = assetFolder.Split('/');
            if (parts.Length == 0 || parts[0] != "Assets")
                throw new InvalidOperationException($"Invalid Unity asset folder: {assetFolder}");

            string current = "Assets";
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);

                current = next;
            }
        }

        public static string PlantUMLTextToUrl(string uml)
        {
            byte[] data = Encoding.UTF8.GetBytes(uml);

            using var ms = new MemoryStream();
            using (var ds = new System.IO.Compression.DeflateStream(
                       ms,
                       System.IO.Compression.CompressionLevel.Optimal,
                       true))
            {
                ds.Write(data, 0, data.Length);
            }

            var deflated = ms.ToArray();
            string encoded = PlantUmlBase64Encode(deflated);
            return "https://www.plantuml.com/plantuml/uml/" + encoded;
        }

        private static readonly string _encode = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_";

        public static string PlantUmlBase64Encode(byte[] data)
        {
            var sb = new StringBuilder();
            int curr = 0;
            int bits = 0;

            foreach (byte b in data)
            {
                curr = (curr << 8) | b;
                bits += 8;

                while (bits >= 6)
                {
                    bits -= 6;
                    sb.Append(_encode[(curr >> bits) & 0x3F]);
                }
            }

            if (bits > 0)
                sb.Append(_encode[(curr << (6 - bits)) & 0x3F]);

            return sb.ToString();
        }

        public class UmlClass
        {
            public string Name;
            public string BaseClass;
            public List<string> Interfaces = new();
            public bool IsAbstract;
            public bool IsInterface;
            public List<UmlField> Fields = new();
            public List<UmlProperty> Properties = new();
            public List<UmlMethod> Methods = new();
            public string Summary;
        }

        public class UmlField
        {
            public string Name;
            public string Type;
            public string Visibility;
        }

        public class UmlProperty
        {
            public string Name;
            public string Type;
            public string Visibility;
        }

        public class UmlMethod
        {
            public string Name;
            public string ReturnType;
            public string Visibility;
            public List<UmlParameter> Parameters = new();
        }

        public class UmlParameter
        {
            public string Name;
            public string Type;
        }

        private static List<string> SafeSplitBaseTypes(string input)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(input)) return result;

            var sb = new StringBuilder();
            int depth = 0;

            foreach (char c in input)
            {
                if (c == '<')
                {
                    depth++;
                    sb.Append(c);
                }
                else if (c == '>')
                {
                    depth = Math.Max(0, depth - 1);
                    sb.Append(c);
                }
                else if (c == ',' && depth == 0)
                {
                    result.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
                result.Add(sb.ToString().Trim());

            return result;
        }

        private static string StripGenerics(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return typeName;

            int i = typeName.IndexOf('<');
            return i >= 0 ? typeName.Substring(0, i) : typeName;
        }

        public class CSharpParser
        {
            private static readonly Regex TypeHeaderRegex = new(
                @"(?<summary>(?:\s*///.*\r?\n)*)\s*(?<modifiers>(?:(?:public|private|protected|internal|abstract|sealed|static|partial)\s+)*)\b(?<kind>class|interface|struct)\s+(?<name>[A-Za-z_]\w*)(?<generics>\s*<[^>{;]+>)?\s*(?:\:\s*(?<bases>[^{\r\n]+))?\s*\{",
                RegexOptions.Multiline | RegexOptions.Compiled);

            private static readonly Regex FieldRegex = new(
                @"^\s*(?<visibility>public|private|protected|internal)\s+(?:(?:static|readonly|const|volatile|new)\s+)*(?<type>[A-Za-z_][\w<>\[\],\.\?]+)\s+(?<name>[A-Za-z_]\w*)\s*(?:=\s*[^;]+)?;",
                RegexOptions.Multiline | RegexOptions.Compiled);

            private static readonly Regex PropertyRegex = new(
                @"^\s*(?<visibility>public|private|protected|internal)\s+(?:(?:static|virtual|abstract|override|sealed|new)\s+)*(?<type>[A-Za-z_][\w<>\[\],\.\?]+)\s+(?<name>[A-Za-z_]\w*)\s*\{\s*(?:(?:public|private|protected|internal)\s+)?get\s*;\s*(?:(?:public|private|protected|internal)\s+)?set\s*;\s*\}",
                RegexOptions.Multiline | RegexOptions.Compiled);

            private static readonly Regex MethodRegex = new(
                @"^\s*(?<visibility>public|private|protected|internal)\s+(?:(?:static|virtual|abstract|override|sealed|async|new|partial)\s+)*(?<returnType>[A-Za-z_][\w<>\[\],\.\?]+)\s+(?<name>[A-Za-z_]\w*)\s*\((?<params>[^\)]*)\)\s*(?:where[^{]+)?(?=\{|;)",
                RegexOptions.Multiline | RegexOptions.Compiled);

            public List<UmlClass> ParseClasses(string content)
            {
                var classes = new List<UmlClass>();
                if (string.IsNullOrWhiteSpace(content))
                    return classes;

                foreach (Match match in TypeHeaderRegex.Matches(content))
                {
                    string kind = match.Groups["kind"].Value;
                    bool isInterface = kind == "interface";

                    string className = match.Groups["name"].Value;
                    string summary = CleanSummary(match.Groups["summary"].Value);

                    string baseClass = null;
                    List<string> interfaces = new();

                    if (match.Groups["bases"].Success)
                    {
                        var bases = SafeSplitBaseTypes(match.Groups["bases"].Value);
                        if (bases.Count > 0)
                        {
                            if (!isInterface)
                            {
                                baseClass = bases[0];
                                if (bases.Count > 1)
                                    interfaces.AddRange(bases.Skip(1));
                            }
                            else
                            {
                                interfaces.AddRange(bases);
                            }
                        }
                    }

                    int bodyOpenBraceIndex = match.Index + match.Length - 1;
                    int bodyCloseBraceIndex = FindMatchingBrace(content, bodyOpenBraceIndex);

                    if (bodyCloseBraceIndex <= bodyOpenBraceIndex)
                        continue;

                    string body = content.Substring(bodyOpenBraceIndex + 1, bodyCloseBraceIndex - bodyOpenBraceIndex - 1);

                    var c = new UmlClass
                    {
                        Name = className,
                        BaseClass = baseClass,
                        Interfaces = interfaces,
                        IsAbstract = match.Groups["modifiers"].Value.Contains("abstract"),
                        IsInterface = isInterface,
                        Summary = summary
                    };

                    ParseMembers(body, c);
                    classes.Add(c);
                }

                return classes;
            }

            private static void ParseMembers(string body, UmlClass umlClass)
            {
                string topLevel = StripNestedTypeBodies(body);

                foreach (Match f in FieldRegex.Matches(topLevel))
                {
                    umlClass.Fields.Add(new UmlField
                    {
                        Visibility = GetVisibilitySymbol(f.Groups["visibility"].Value),
                        Type = f.Groups["type"].Value,
                        Name = f.Groups["name"].Value
                    });
                }

                foreach (Match p in PropertyRegex.Matches(topLevel))
                {
                    umlClass.Properties.Add(new UmlProperty
                    {
                        Visibility = GetVisibilitySymbol(p.Groups["visibility"].Value),
                        Type = p.Groups["type"].Value,
                        Name = p.Groups["name"].Value
                    });
                }

                foreach (Match m in MethodRegex.Matches(topLevel))
                {
                    if (umlClass.Name == m.Groups["name"].Value)
                        continue;

                    umlClass.Methods.Add(new UmlMethod
                    {
                        Visibility = GetVisibilitySymbol(m.Groups["visibility"].Value),
                        ReturnType = string.IsNullOrWhiteSpace(m.Groups["returnType"].Value) ? "void" : m.Groups["returnType"].Value,
                        Name = m.Groups["name"].Value,
                        Parameters = ParseParameters(m.Groups["params"].Value)
                    });
                }
            }

            private static string StripNestedTypeBodies(string body)
            {
                var sb = new StringBuilder(body.Length);
                int depth = 0;

                foreach (char c in body)
                {
                    if (c == '{')
                    {
                        depth++;
                        sb.Append('\n');
                    }
                    else if (c == '}')
                    {
                        depth = Math.Max(0, depth - 1);
                        sb.Append('\n');
                    }
                    else
                    {
                        if (depth == 0)
                            sb.Append(c);
                        else if (c == '\n' || c == '\r')
                            sb.Append(c);
                        else
                            sb.Append(' ');
                    }
                }

                return sb.ToString();
            }

            private static int FindMatchingBrace(string text, int openBraceIndex)
            {
                if (openBraceIndex < 0 || openBraceIndex >= text.Length || text[openBraceIndex] != '{')
                    return -1;

                int depth = 0;
                bool inString = false;
                bool inChar = false;
                bool inLineComment = false;
                bool inBlockComment = false;

                for (int i = openBraceIndex; i < text.Length; i++)
                {
                    char c = text[i];
                    char next = i + 1 < text.Length ? text[i + 1] : '\0';
                    char prev = i > 0 ? text[i - 1] : '\0';

                    if (inLineComment)
                    {
                        if (c == '\n')
                            inLineComment = false;
                        continue;
                    }

                    if (inBlockComment)
                    {
                        if (prev == '*' && c == '/')
                            inBlockComment = false;
                        continue;
                    }

                    if (!inString && !inChar)
                    {
                        if (c == '/' && next == '/')
                        {
                            inLineComment = true;
                            i++;
                            continue;
                        }

                        if (c == '/' && next == '*')
                        {
                            inBlockComment = true;
                            i++;
                            continue;
                        }
                    }

                    if (!inChar && c == '"' && prev != '\\')
                    {
                        inString = !inString;
                        continue;
                    }

                    if (!inString && c == '\'' && prev != '\\')
                    {
                        inChar = !inChar;
                        continue;
                    }

                    if (inString || inChar)
                        continue;

                    if (c == '{')
                    {
                        depth++;
                    }
                    else if (c == '}')
                    {
                        depth--;
                        if (depth == 0)
                            return i;
                    }
                }

                return -1;
            }

            private static string CleanSummary(string raw)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    return string.Empty;

                string[] lines = raw
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim())
                    .Select(l => l.StartsWith("///") ? l.Substring(3).Trim() : l)
                    .ToArray();

                return string.Join(" ", lines).Trim();
            }

            private static string GetVisibilitySymbol(string kw)
            {
                if (kw.Contains("public")) return "+";
                if (kw.Contains("private")) return "-";
                if (kw.Contains("protected")) return "#";
                if (kw.Contains("internal")) return "~";
                return "";
            }

            private static List<UmlParameter> ParseParameters(string raw)
            {
                var list = new List<UmlParameter>();
                if (string.IsNullOrWhiteSpace(raw))
                    return list;

                foreach (string param in SplitParameters(raw))
                {
                    string cleaned = param.Trim();
                    if (string.IsNullOrWhiteSpace(cleaned))
                        continue;

                    string[] tokens = cleaned
                        .Replace("ref ", "")
                        .Replace("out ", "")
                        .Replace("in ", "")
                        .Replace("params ", "")
                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (tokens.Length < 2)
                        continue;

                    string name = tokens[^1];
                    string type = string.Join(" ", tokens.Take(tokens.Length - 1));

                    if (name.Contains("="))
                        name = name.Split('=')[0].Trim();

                    list.Add(new UmlParameter
                    {
                        Type = type.Trim(),
                        Name = name.Trim()
                    });
                }

                return list;
            }

            private static List<string> SplitParameters(string raw)
            {
                var result = new List<string>();
                if (string.IsNullOrWhiteSpace(raw))
                    return result;

                var sb = new StringBuilder();
                int genericDepth = 0;
                int parenDepth = 0;
                int bracketDepth = 0;

                foreach (char c in raw)
                {
                    switch (c)
                    {
                        case '<': genericDepth++; sb.Append(c); break;
                        case '>': genericDepth = Math.Max(0, genericDepth - 1); sb.Append(c); break;
                        case '(': parenDepth++; sb.Append(c); break;
                        case ')': parenDepth = Math.Max(0, parenDepth - 1); sb.Append(c); break;
                        case '[': bracketDepth++; sb.Append(c); break;
                        case ']': bracketDepth = Math.Max(0, bracketDepth - 1); sb.Append(c); break;
                        case ',':
                            if (genericDepth == 0 && parenDepth == 0 && bracketDepth == 0)
                            {
                                result.Add(sb.ToString());
                                sb.Clear();
                            }
                            else
                            {
                                sb.Append(c);
                            }
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }

                if (sb.Length > 0)
                    result.Add(sb.ToString());

                return result;
            }
        }

        public static class PlantUmlGenerator
        {
            public static string GeneratePlantUml(List<UmlClass> classes, bool includeAssociations)
            {
                var sb = new StringBuilder();
                sb.AppendLine("@startuml");

                foreach (var c in classes)
                {
                    string typeKeyword = c.IsInterface ? "interface" : "class";
                    string stereotype = !c.IsInterface && c.IsAbstract ? " <<abstract>>" : string.Empty;

                    sb.AppendLine($"{typeKeyword} {c.Name}{stereotype} {{");

                    foreach (var f in c.Fields)
                        sb.AppendLine($"    {f.Visibility} {f.Name} : {f.Type}");

                    foreach (var p in c.Properties)
                        sb.AppendLine($"    {p.Visibility} {p.Name} : {p.Type} {{ get; set; }}");

                    foreach (var m in c.Methods)
                    {
                        string plist = string.Join(", ", m.Parameters.Select(p => $"{p.Name} : {p.Type}"));
                        sb.AppendLine($"    {m.Visibility} {m.Name}({plist}) : {m.ReturnType}");
                    }

                    sb.AppendLine("}");

                    if (!string.IsNullOrWhiteSpace(c.Summary))
                        sb.AppendLine($"' {c.Name}: {c.Summary}");
                }

                foreach (var c in classes)
                {
                    if (!string.IsNullOrWhiteSpace(c.BaseClass))
                        sb.AppendLine($"{StripGenerics(c.BaseClass)} <|-- {c.Name}");

                    foreach (string iface in c.Interfaces)
                        sb.AppendLine($"{StripGenerics(iface)} <|.. {c.Name}");
                }

                if (includeAssociations)
                {
                    var names = new HashSet<string>(classes.Select(cl => StripGenerics(cl.Name)));
                    var added = new HashSet<string>();

                    foreach (var c in classes)
                    {
                        string className = StripGenerics(c.Name);

                        foreach (var f in c.Fields)
                        {
                            string typeName = StripGenerics(f.Type);
                            if (names.Contains(typeName) && typeName != className && added.Add($"{className}-{typeName}-f"))
                                sb.AppendLine($"{className} --> {typeName} : field");
                        }

                        foreach (var p in c.Properties)
                        {
                            string typeName = StripGenerics(p.Type);
                            if (names.Contains(typeName) && typeName != className && added.Add($"{className}-{typeName}-p"))
                                sb.AppendLine($"{className} --> {typeName} : property");
                        }

                        foreach (var m in c.Methods)
                        {
                            foreach (var param in m.Parameters)
                            {
                                string typeName = StripGenerics(param.Type);
                                if (names.Contains(typeName) && typeName != className && added.Add($"{className}-{typeName}-a"))
                                    sb.AppendLine($"{className} --> {typeName} : parameter");
                            }
                        }
                    }
                }

                sb.AppendLine("@enduml");
                return sb.ToString();
            }
        }
    }
}