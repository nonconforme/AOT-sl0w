using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

public class ModDrawController : MonoBehaviour
{
    public static Rect[] GUIWindows;
    public static bool[] GUIToggles;
    public static Texture2D[] ToolTipTex;
    //public static Texture2D[] GUITextures;

    public static Texture2D LoadTextureFromFile(string Filename)
    {
        Texture2D tex = null; // no filename
        byte[] Data;
        string DataPath = String.Format(".\\{0}", Filename); // readallbytes just reads from the file. so a png will get converted to bytes and then loaded as an img later
        if (File.Exists(DataPath))
        {
            Data = File.ReadAllBytes(DataPath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(Data);
        }
        return tex;
    }


    public static void InitVars()
    {
        GUIWindows = new Rect[] 
        { 
            new Rect(0, Screen.height - 400, 400, 300), 
            new Rect(Screen.width - 400, Screen.height - 300, 400, 300)
        };

        GUIToggles = new bool[]
        { 
            false,
            false
        };

        ToolTipTex = new Texture2D[]
        {
            new Texture2D(1, 1),
            new Texture2D(1, 1)

        };

        ModDrawGUI.ColorFloats = new float[]
        {
            1.0f,
            1.0f,
            1.0f,
            1.0f
        };

        ToolTipTex[0].SetPixel(1, 1, new Color(0.784f, 0.784f, 0.490f, 1.0f));
        ToolTipTex[1].SetPixel(1, 1, new Color(1.0f, 1.0f, 0.627f, 1.0f));
        ToolTipTex[0].Apply();
        ToolTipTex[1].Apply();
//         GUITextures = new Texture2D[]
//         {
//             LoadTextureFromFile("ControlPanelMOD.DATA"),
//             LoadTextureFromFile("DebugConsoleMOD.DATA")
//         };

    }

    public static void HandleKeyPress()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Insert))
        {
            /*GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = true;*/
//             GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
//             GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
            Screen.showCursor = true;
            Screen.lockCursor = false;
            GUIToggles[0] = true;
        }

        if(UnityEngine.Input.GetKeyDown(KeyCode.Delete))
        {
           /* GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = false;*/
//             GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = false;
//             GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = false;
            Screen.showCursor = false;
            Screen.lockCursor = true;
            GUIToggles[0] = false;
        }

        //Does bValue = !bValue work in C# Will test l8r
    }

    public static void DrawToolTip()
    {
        Vector2 TooltipLength = PanelMain.TextBounds(GlobalVars.ToolTip);
        GUI.DrawTexture(new Rect(UnityEngine.Input.mousePosition.x + 10, UnityEngine.Input.mousePosition.y - 5, TooltipLength.x + 6, TooltipLength.y + 4), ToolTipTex[0]);
        GUI.DrawTexture(new Rect(UnityEngine.Input.mousePosition.x + 11, UnityEngine.Input.mousePosition.y - 4, TooltipLength.x + 4, TooltipLength.y + 2), ToolTipTex[1]);
        GUI.Label(new Rect(UnityEngine.Input.mousePosition.x + 12, UnityEngine.Input.mousePosition.y - 3, TooltipLength.x, TooltipLength.y), GlobalVars.ToolTip/*, ModDrawGUI.FontStyle*/);
    }

    void Start()
    {
//         InitVars();
//         base.gameObject.AddComponent<ModDrawGUI>();
    }

    void Awake()
    {
 
    }

    void OnGUI()
    {

//         UnityEngine.Object[] Ayylmao = GameObject.FindObjectsOfType<GameObject>();
// 
//         for (int i = 0; i < Ayylmao.Length; i++ )
//         {
//             GUI.Label(new Rect(500, 140 + (i * 20), 200, 24), Ayylmao[i].name);
//         }
            //         for (int i = 0; i < 20; i++ )
            //         {
            //             GUI.Label(new Rect(20, 140 + (i * 20), 200, 24), LayerMask.LayerToName(i));
            //         }
            //             LayerMask.LayerToName(0);

            //         if (Event.current.type == EventType.Repaint)
            //             GlobalVars.ToolTip = GUI.tooltip;
            // 
            //         if (Event.current.type == EventType.MouseUp)
            //             GlobalVars.ToolTip = GUI.tooltip = "";

        HandleKeyPress();
        if (GUIToggles[0])
        {
            GUIWindows[0] = GUI.Window(1, GUIWindows[0], ModDrawGUI.DrawGUI, "Control");
            GUIWindows[1] = GUI.Window(2, GUIWindows[1], ModDrawGUI.DrawSkinGUI, "SkinStealer");
        }
         if (UnityEngine.Input.GetKeyDown(KeyCode.Home))
         {
             Application.CaptureScreenshot(".\\ayylmao.png", 4);
         }
    }
    void Update()
    {
        //doubt ill use lmaaoooooooo
    }

}
