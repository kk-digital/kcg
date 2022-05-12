editor-settings.md

--- Editor Applications

Unity Version
- 2021.3 LTS

Microsoft Visual Studios 2022

Sublime Text 2

--- MicroSoft Visual Studios 2022 Settings

To hide .meta files created by unity, to avoid clutter

Excluding Unity .meta files from showing up in editor
1> In left hand file list in the "solution explorer", hit show all files
2> Go to "Global Settings", then go into VSWorkSpaceSettings.json
3> Add

  "ExcludedItems": [
    ".git",
    ".vs",
    ".vscode",
    "**/.DS_Store",
    "**/*.meta",
  ],

4> You can see all files by clicking "Show all files" or unclick to hide useless files.

You may want to hide 
- *.property files
- *.sublime-project
- *.sublime-workspace


---

1> All fonts should be Monokai

2> Dark Theme
- for Unity
- for Microsoft Visual Studios 2022
- for Sublime Text 2 (default)

Monokai Theme
- https://marketplace.visualstudio.com/items?itemName=idex.vsthemepack

One Monokai theme
- https://marketplace.visualstudio.com/items?itemName=azemoh.onemonokai

Monokai-night
- https://marketplace.visualstudio.com/items?itemName=LukLinhart.monokainight

Monokai-dark
- https://marketplace.visualstudio.com/items?itemName=AlfredoCarrazco.monokaivs2019

Monokai-pro
- https://monokai.pro/vscode

--- Fonts

consolas

---

Todo:
- unity extention for changing unity editor font from arial

Unity Extensions
- extention to show files for .md files in Assets folder (done)
- extention to change font in unity editor

--- Changing Unity Font

TODO: Research how to set per application system font

https://answers.unity.com/questions/56067/what-fonts-does-the-unity-interface-use.html

you can check by doing:
- Assets > Create > GUI Skin

The Unity Editor just uses the same system font as all other applications.
- If you're on Windows, this is Segoe UI

