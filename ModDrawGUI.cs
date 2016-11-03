using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

/*
        Use this for drawing the controlpanel .. debug windows too i guess
*/
public class ModDrawGUI : MonoBehaviour
{

    public static Vector2 DebugScrollPos;
    public static List<String> DebugStr = new List<string>();
    public static List<Color> DebugColor;
    public static GUIStyle ButtonStyle;
    public static GUIStyle FontStyle;
    public static bool bSetStyleOnce = false;
    public static Texture2D ColorTexture;
    public static float[] ColorFloats;
    public static HERO SelectedHero = null;
    public static Texture2D renderTexture = new Texture2D(170, 100);
   /* public static GameObject cameraObj = new GameObject();*/

    public static string[] TabStrings = new string[]
    {
        "Visual",
        "Player",
        "Misc",
        "Server",
        "Advanced"
    };

    public static string[] URLAddons = new string[]
    {
        "Horse: ",
        "Hair_Piece: ",
        "Eyes: ",
        "Glasses: ",
        "Face: ",
        "Body: ",
        "Costume: ",
        "Cape: ",
        "3DMG1: ",
        "3DMG2: ",
        "Smoke: ",
        "Hair: ",
        "BladeTrail: "
    };


    public static Rect[] Groups = new Rect[]
    {
        new Rect(20, 60, 105, 110),     //0
        new Rect(140, 60, 105, 110),    //1
        new Rect(20, 200, 225, 80),     //2
        new Rect(260, 60, 120, 220),    //3
        //
        new Rect(20, 60, 215, 180),     //4    // to keep from confusion i wont use [0] even though its already the exact one. Just easier 
        new Rect(250, 60, 105, 110),    //5    // this way if i look back at my source in the future and want to remember why i did it all.
    };

    public static void AddDebugLine(int TagTYPE, string str)
    {
       Color TagColor = new Color(0,0,0);
        string PreTag = string.Empty;

        switch(TagTYPE)
        {
            case 0: // Unknown RPCS / Unknown Events
                PreTag = "[RPC] ";
                TagColor = new Color(1.0f, 0.529f, 0.529f);
                break;

            case 1: // Warning / ModDetections
                PreTag = "[WARN] ";
                TagColor = new Color(1.0f, 1.0f, 0.529f);
                break;

            case 2: // Bans / Autobans 
                PreTag = "[BAN] ";
                TagColor = new Color(1.0f, 0.529f, 1.0f);
                break;

            case 3: // Information
                PreTag = "[INFO] ";
                TagColor = new Color(0.529f, 1.0f, 0.529f);
                break;

            default:
                PreTag = "[GENERAL] ";
                TagColor = Color.white;
                break;
        }

        str = String.Format("{0}{1}", PreTag, str);
        DebugColor.Add(TagColor);
        DebugStr.Add(str);
    }

    private static void GroupBox(string name, int xMax, int yMax)
    {
        Rendering.CustomCanvas.DrawLine(new Vector2(0, 12), new Vector2(10, 12), Color.white);
        Rendering.CustomCanvas.DrawLine(new Vector2(0, 12), new Vector2(0, yMax), Color.white);
        Rendering.CustomCanvas.DrawLine(new Vector2(xMax, 12), new Vector2(xMax, yMax), Color.white);
        Rendering.CustomCanvas.DrawLine(new Vector2(0, yMax), new Vector2(xMax, yMax), Color.white);
        Vector2 strBounds = PanelMain.TextBounds(name);
        Rendering.CustomCanvas.DrawLine(new Vector2(18 + strBounds.x, 12), new Vector2(xMax, 12), Color.white);
        GUI.Label(new Rect(14, 0, strBounds.x, strBounds.y), name, FontStyle);

    }

    private static void DrawVisualsTab()
    {
        Texture2D ColorOpt = new Texture2D(1, 1);
        GUI.BeginGroup(Groups[0], "");
        GroupBox("Titans/PT", (int)Groups[0].width, (int)Groups[0].height);
        for (int i = 0; i <= 3; i++)
        {
            Vector2 Size = PanelMain.TextBounds(GlobalVars.GUIContents[i].text);
            GlobalVars.Bools[i] = GUI.Toggle(new Rect(5, 15 + (i * 20), Size.x + 16, Size.y + 2), GlobalVars.Bools[i], GlobalVars.GUIContents[i]);
            if (GUI.Button(new Rect(80, 18 + (i * 20), 20, 15), ""))
            {
                GlobalVars.Colors[i] = new Color(ColorFloats[0], ColorFloats[1], ColorFloats[2]);
            }
            ColorOpt.SetPixel(1, 1, new Color(GlobalVars.Colors[i][0], GlobalVars.Colors[i][1], GlobalVars.Colors[i][2], 1.0f));
            ColorOpt.Apply();

            GUI.DrawTexture(new Rect(80, 18 + (i * 20), 20, 15), ColorOpt);
        }
        GUI.EndGroup();


        GUI.BeginGroup(Groups[1], "");
        GroupBox("Player", (int)Groups[1].width, (int)Groups[1].height);
        for (int i = 4; i <= 7; i++)
        {
            Vector2 Size = PanelMain.TextBounds(GlobalVars.GUIContents[i].text);
            GlobalVars.Bools[i] = GUI.Toggle(new Rect(5, 15 + ((i - 4) * 20), Size.x + 16, Size.y + 2), GlobalVars.Bools[i], GlobalVars.GUIContents[i]);
            if (GUI.Button(new Rect(80, 18 + ((i - 4) * 20), 20, 15), ""))
            {
                GlobalVars.Colors[i] = new Color(ColorFloats[0], ColorFloats[1], ColorFloats[2]);
            }
            ColorOpt.SetPixel(1, 1, new Color(GlobalVars.Colors[i][0], GlobalVars.Colors[i][1], GlobalVars.Colors[i][2], 1.0f));
            ColorOpt.Apply();

            GUI.DrawTexture(new Rect(80, 18 + ((i - 4) * 20), 20, 15), ColorOpt);
        }
        GUI.EndGroup();

        GUI.BeginGroup(Groups[2], "");
        GroupBox("Misc", (int)Groups[2].width, (int)Groups[2].height);
        for (int i = 8; i <= 12; i++)
        {
            Vector2 Size = PanelMain.TextBounds(GlobalVars.GUIContents[i].text);
            if (i >= 11)
            {
                GlobalVars.Bools[i] = GUI.Toggle(new Rect(126, 15 + ((i - 11) * 20), Size.x + 16, Size.y + 2), GlobalVars.Bools[i], GlobalVars.GUIContents[i]);
                if (GUI.Button(new Rect(201, 18 + ((i - 11) * 20), 20, 15), ""))
                {
                    GlobalVars.Colors[i] = new Color(ColorFloats[0], ColorFloats[1], ColorFloats[2]);
                }
                ColorOpt.SetPixel(1, 1, new Color(GlobalVars.Colors[i][0], GlobalVars.Colors[i][1], GlobalVars.Colors[i][2], 1.0f));
                ColorOpt.Apply();
                GUI.DrawTexture(new Rect(201, 18 + ((i - 11) * 20), 20, 15), ColorOpt);
            }
            else
            {
                GlobalVars.Bools[i] = GUI.Toggle(new Rect(5, 15 + ((i - 8) * 20), Size.x + 16, Size.y + 2), GlobalVars.Bools[i], GlobalVars.GUIContents[i]);
                if (GUI.Button(new Rect(80, 18 + ((i - 8) * 20), 20, 15), ""))
                {
                    GlobalVars.Colors[i] = new Color(ColorFloats[0], ColorFloats[1], ColorFloats[2]);
                }
                ColorOpt.SetPixel(1, 1, new Color(GlobalVars.Colors[i][0], GlobalVars.Colors[i][1], GlobalVars.Colors[i][2], 1.0f));
                ColorOpt.Apply();

                GUI.DrawTexture(new Rect(80, 18 + ((i - 8) * 20), 20, 15), ColorOpt);
            }
        }
        GUI.EndGroup();

        GUI.BeginGroup(Groups[3], "");
        GroupBox("Colors", (int)Groups[3].width, (int)Groups[3].height);
        GUI.DrawTexture(new Rect(10, 20, 100, 30), ColorTexture);
        Vector2 PreviewSize = PanelMain.TextBounds("Preview");
        GUI.Label(new Rect(60 - (PreviewSize.x / 2), 32, PreviewSize.x, PreviewSize.y), "Preview", FontStyle);
        ColorFloats[0] = GUI.VerticalSlider(new Rect(20, 60, 12, 150), ColorFloats[0], 1.0f, 0.0f);
        ColorFloats[1] = GUI.VerticalSlider(new Rect(50, 60, 12, 150), ColorFloats[1], 1.0f, 0.0f);
        ColorFloats[2] = GUI.VerticalSlider(new Rect(80, 60, 12, 150), ColorFloats[2], 1.0f, 0.0f);
        ColorTexture.SetPixel(1, 1, new Color(ColorFloats[0], ColorFloats[1], ColorFloats[2], 1.0f));
        ColorTexture.Apply();

        GUI.EndGroup();
    }
    private static void DrawPlayerTab()
    {
        GUI.BeginGroup(Groups[4], "");
        GroupBox("Player", (int)Groups[4].width, (int)Groups[4].height);
        for (int i = 13; i <= 16; i++)
        {
            Vector2 Size = PanelMain.TextBounds(GlobalVars.GUIContents[i].text);
            GlobalVars.Bools[i] = GUI.Toggle(new Rect(5, 15 + ((i - 13) * 20), Size.x + 16, Size.y + 2), GlobalVars.Bools[i], GlobalVars.GUIContents[i]);
            GlobalVars.Floats[i - 13] = GUI.HorizontalSlider(new Rect(25 + Size.x, 22 + ((i - 13) * 20), 180 - Size.x, 12), GlobalVars.Floats[i - 13], GlobalVars.MinFloats[i - 13], GlobalVars.MaxFloats[i - 13]);

        }
        GUI.EndGroup();

        GUI.BeginGroup(Groups[5], "");
        GroupBox("Autosmite", (int)Groups[5].width, (int)Groups[5].height);
        for (int i = 17; i <= 20; i++)
        {
            Vector2 Size = PanelMain.TextBounds(GlobalVars.GUIContents[i].text);
            GlobalVars.Bools[i] = GUI.Toggle(new Rect(5, 15 + ((i - 17) * 20), Size.x + 16, Size.y + 2), GlobalVars.Bools[i], GlobalVars.GUIContents[i]);
        }
        GUI.EndGroup();
    }

    private static void DrawMiscTab()
    {
        GUI.BeginGroup(Groups[4], "");
        GroupBox("Misc", (int)Groups[4].width, (int)Groups[4].height + 10);
        for (int i = 23; i <= 30; i++)
        {
            Vector2 Size = PanelMain.TextBounds(GlobalVars.GUIContents[i].text);
            GlobalVars.Bools[i] = GUI.Toggle(new Rect(5, 15 + ((i - 23) * 20), Size.x + 16, Size.y + 2), GlobalVars.Bools[i], GlobalVars.GUIContents[i]);
            if(i == 23)
                GlobalVars.FloatTimers[2] = GUI.HorizontalSlider(new Rect(25 + Size.x, 22 + ((i - 23) * 20), 180 - Size.x, 12), GlobalVars.FloatTimers[2], GlobalVars.MinFloats[i - 19], GlobalVars.MaxFloats[i - 19]);
        
            if(i == 30)
            {
                if(GUI.Button(new Rect(95, 18 + ((i - 23) * 20), 40, 16), "clear"))
                {
                    FengGameManagerMKII.instance.InternalIgnore.Clear();
                    object[] parameters = new object[] { "<color=#FFFFFF>[</color><b><color=#A5D2FF>sl0wm0d</color></b><color=#FFFFFF>]</color><b><color=#FFD2A5>Ignorelist has been cleared.</color></b>", string.Empty };
                    FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
                }
            }
        }
        GUI.EndGroup();
    }

    public static void DrawGUI(int WindowID)
    {

//         if (GlobalVars.ToolTip != "")
//             ModDrawController.DrawToolTip();

        int Reapply = GUI.skin.button.fontSize;
        Font ReapplyFont = GUI.skin.button.font;
        GUI.skin.button.font = GlobalVars.Segoe;
        GUI.skin.button.fontSize = 12;

        GUI.skin.toggle.font = GlobalVars.Segoe;
        GUI.skin.toggle.fontSize = 12;
        if (!bSetStyleOnce)
        {
            FontStyle = new GUIStyle(GUI.skin.label);
            FontStyle.fontSize = 12;
            FontStyle.font = GlobalVars.Segoe;
            GUI.skin.button.hover.background = GlobalVars.sl0wTextures[3];
            GUI.skin.button.onHover.background = GlobalVars.sl0wTextures[3];
            GUI.skin.button.normal.background = GlobalVars.sl0wTextures[3];
            GUI.skin.button.onNormal.background = GlobalVars.sl0wTextures[3];
            GUI.skin.button.onActive.background = GlobalVars.sl0wTextures[3];
            GUI.skin.button.active.background = GlobalVars.sl0wTextures[3];
            bSetStyleOnce = true;
        }

        GUI.DrawTexture(new Rect(0, 0, 400, 300), GlobalVars.sl0wTextures[0]);
        GlobalVars.Ints[0] = GUI.SelectionGrid(new Rect(8, 35, 384, 20), GlobalVars.Ints[0], TabStrings, 5/*, ButtonStyle*/);

        switch(GlobalVars.Ints[0])
        {
            case 0:
                DrawVisualsTab(); // doesnt unity purge a lot of private shit? idk we will find out. most objects i used i recall were purged.
                break;
            case 1:
                DrawPlayerTab();
                break;
            case 2:
                DrawMiscTab();
                break;


        }

        GUI.skin.toggle.font = ReapplyFont;
        GUI.skin.toggle.fontSize = Reapply;
        GUI.skin.button.font = ReapplyFont;
        GUI.skin.button.fontSize = Reapply;

        GUI.DragWindow(new Rect(0, 0, 300, 35));
    }

    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "");
    }

    public static void DrawSkinGUI(int WindowID)
    {
        int Reapply = GUI.skin.button.fontSize;
        Font ReapplyFont = GUI.skin.button.font;
        GUI.skin.button.font = GlobalVars.Segoe;
        GUI.skin.button.fontSize = 12;

        GUI.skin.toggle.font = GlobalVars.Segoe;
        GUI.skin.toggle.fontSize = 12;

        int Additions = 0;
        GUI.DrawTexture(new Rect(0, 0, 400, 300), GlobalVars.sl0wTextures[1]);
        IEnumerator zEnum = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().getPlayers().GetEnumerator();
        DebugScrollPos = GUI.BeginScrollView(new Rect(9, 45, 218, 235), DebugScrollPos, new Rect(0, 0, 170, (20 * 18) + 4)); // add real number via a loop later  //218 248
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        while (zEnum.MoveNext())
        {
            HERO aHERO = zEnum.Current as HERO;
            if (!aHERO ||
               !aHERO.photonView)
                continue;

            if (!aHERO.photonView.owner.isLocal)
            {
                if(aHERO.bHasSkin && aHERO.skinURL != string.Empty)
                {
                    string Playername = ModDrawOverlay.RemoveTags((string)aHERO.photonView.owner.customProperties[PhotonPlayerProperty.name]);
                    if (GUI.Button(new Rect(4, 25 * (Additions) + 4, 130, 20), Playername))
                        SelectedHero = aHERO;
                    Additions++;
                }
            }
        }
        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUI.EndScrollView();

        if(SelectedHero) // 240 90
        {
            string PlayerName = ModDrawOverlay.RemoveTags((string)SelectedHero.photonView.owner.customProperties[PhotonPlayerProperty.name]);

           if(PlayerName.Length > 25 )
           {
              PlayerName = PlayerName.Remove(25);
           }

            string Label = string.Format("{0}'s Setup", PlayerName);
            Vector2 LabelBounds = PanelMain.TextBounds(Label);
            GUI.Label(new Rect(240, 90, LabelBounds.x, LabelBounds.y), Label, FontStyle);
            GUI.Label(new Rect(250, 105, 100, 20), string.Format("SEX: {0}", (SelectedHero.setup.myCostume.sex == SEX.MALE ? "MALE" : "FEMALE")), FontStyle);
            GUI.Label(new Rect(250, 117, 100, 20), string.Format("EYES: {0}", SelectedHero.setup.myCostume.eye_texture_id), FontStyle);
            GUI.Label(new Rect(250, 129, 100, 20), string.Format("MOUTH: {0}", SelectedHero.setup.myCostume.beard_texture_id), FontStyle);
            GUI.Label(new Rect(250, 141, 100, 20), string.Format("GLASSES: {0}", SelectedHero.setup.myCostume.glass_texture_id), FontStyle);
            GUI.Label(new Rect(250, 153, 100, 20), string.Format("HAIR: {0}", SelectedHero.setup.myCostume.hairInfo.hair), FontStyle);
            GUI.Label(new Rect(250, 165, 100, 20), string.Format("CAPE: {0}", (SelectedHero.setup.myCostume.cape ? "YES" : "NO")), FontStyle);
            GUI.Label(new Rect(250, 177, 100, 20), string.Format("COSTUME: {0}", SelectedHero.setup.myCostume.costumeId), FontStyle);

            if(GUI.Button(new Rect(330, 275, 65, 22), "Dumpskin", PanelMain.changelogstyle))
            {
                string[] Urls = SelectedHero.skinURL.Split(new char[] { ',' });
                string FullText = string.Format("Username: {0}\n\n", PlayerName);

                for(int i = 0; i < URLAddons.Length; i++)
                {
                    FullText = string.Format("{0}{1}{2}\n", FullText, URLAddons[i], Urls[i]);
                }


                string folderDir = RemoveSpecialCharacters(PlayerName);
                if(folderDir == string.Empty)
                    folderDir = "DefaultFallback";

                folderDir = string.Format(".\\{0}", folderDir);
                if(!System.IO.Directory.Exists(folderDir))
                {
                    System.IO.Directory.CreateDirectory(folderDir);
                    System.IO.File.WriteAllText(string.Format("{0}\\{1}.txt", folderDir, 1), FullText);
                }
                else
                {
                    int ExistingNum = 1;
                    while(System.IO.File.Exists(string.Format("{0}\\{1}.txt",folderDir, ExistingNum)))
                    {
                        ExistingNum++;
                    }
                    System.IO.File.WriteAllText(string.Format("{0}\\{1}.txt", folderDir, ExistingNum), FullText);
                }
            }

            if(GUI.Button(new Rect(255, 275, 65, 22), "Clone", PanelMain.changelogstyle))
            {

            }

            
//               if(Event.current.type == EventType.Repaint)
//               {
//                   guiCamera.Render();
//               }

            //GUI.DrawTexture(new Rect(228, 42, 170, 100), renderTexture);

        }
        GUI.skin.toggle.font = ReapplyFont;
        GUI.skin.toggle.fontSize = Reapply;
        GUI.skin.button.font = ReapplyFont;
        GUI.skin.button.fontSize = Reapply;

        GUI.DragWindow(new Rect(0, 0, 300, 35));
    }


    IEnumerator DownloadAsets()
    {
        WWW www = new WWW("http://aotmod.sl0w.org/asset.unity3d");
        yield return www;
        System.IO.File.WriteAllBytes(Application.dataPath + "/sl0wAssets.unity3d", www.bytes);
        FengGameManagerMKII.DownloadAsset = false;
    }

    IEnumerator DownloadModule()
    {
        WWW www = new WWW("http://aotmod.sl0w.org/Assembly-CSharp.dll");
        yield return www;
        System.IO.File.WriteAllBytes(Application.dataPath + "/Managed/Assembly-CSharp.dll", www.bytes);
        FengGameManagerMKII.DownloadDLL = false;
    }

    IEnumerator CheckVersion()
    {
        WWW WWWUpdate = new WWW("http://aotmod.sl0w.org/version.txt");
        yield return WWWUpdate;

        if (!string.IsNullOrEmpty(WWWUpdate.error))
        {
            UnityEngine.Debug.Log(WWWUpdate.error);
            ModDrawGUI.AddDebugLine(1, WWWUpdate.error);

        }

        string[] UpdateString = WWWUpdate.text.Split(new char[] { ' ' });
        if (FengGameManagerMKII.ModVersion != UpdateString[0])
        {
            FengGameManagerMKII.DownloadDLL = true;
            //ModDrawGUI.AddDebugLine(1, "Downloading new version...");
            FengGameManagerMKII.UpdateType = 2;
            FengGameManagerMKII.UpdateStatus = "Downloading new version...";
            FengGameManagerMKII.HasToDownload = true;
            UnityEngine.Debug.Log(WWWUpdate.text);
        }
        else
        {
            FengGameManagerMKII.UpdateType = 3;
            FengGameManagerMKII.UpdateStatus = "Version is current...";
            //ModDrawGUI.AddDebugLine(3, "Version is current...");
        }

        if (UpdateString[1] == "1" && FengGameManagerMKII.DownloadDLL == true)
            FengGameManagerMKII.DownloadAsset = true;

        if (FengGameManagerMKII.DownloadDLL == true)
            base.StartCoroutine(DownloadModule());

        if (FengGameManagerMKII.DownloadAsset == true)
            base.StartCoroutine(DownloadAsets());
    }

    IEnumerator WaitLoad()
    {
        yield return new WaitForSeconds(2.0f);
        FengGameManagerMKII.UpdateType = 1;
        FengGameManagerMKII.UpdateStatus = "Checking version...";
        base.StartCoroutine(CheckVersion());

    }

    public static Texture2D RTImage(Camera cam)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D image = new Texture2D(100, 100);
        image.ReadPixels(new Rect(0, 0, 100, 100), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;
        return image;
    }

    void Start()
    {
//         cameraObj.AddComponent<Camera>();
//         cameraObj.GetComponent<Camera>().enabled = false;
        /*guiCamera = GameObject.Find("MainCamera").GetComponent<Camera>();*/
        DebugScrollPos = Vector2.zero;
        DebugColor = new List<Color>();
//         AddDebugLine(5, "Welcome to sl0wm0d " + FengGameManagerMKII.ModVersion);
//         base.StartCoroutine(WaitLoad());
        ColorTexture = new Texture2D(1, 1);
        ColorTexture.SetPixel(1, 1, new Color(0.0f, 0.0f, 1.0f, 1.0f));
        ColorTexture.Apply();
    }

    void Awake()
    {

    }

    void OnGUI()
    {

    }

    void Update()
    {
        if(SelectedHero != null)
        {

//             cameraObj.transform.LookAt(SelectedHero.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform.position);
//             cameraObj.transform.RotateAround(SelectedHero.transform.position, new Vector3(0.0f, 0.5f, 0.0f), 30 * Time.deltaTime);
        }
    }

}
