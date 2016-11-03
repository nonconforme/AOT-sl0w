//Fixed With [DOGE]DEN aottg Sources fixer
//Doge Guardians FTW
//DEN is OP as fuck.
//Farewell Cowboy

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMain : MonoBehaviour
{
    public GameObject label_credits;
    public GameObject label_multi;
    public GameObject label_option;
    public GameObject label_single;
    private int lang = -1;
    public Rect changelogRect = new Rect(1, Screen.height - 300, 300, 300);
    public static bool bChangelogOnce = false;
    public static GUIStyle changelogstyle;
    public static GUIStyle changelogtextstyle;
    public static GUIStyle updatestyle;
    public static Vector2 ChangelogScroll, NotesScroll = Vector2.zero;
    public static string Changelog = "Loading...";
    public static string Notes = "Loading...";
    public static bool Change_Notes = false;


    private void OnEnable()
    {
    }

    private void showTxt()
    {
        if (this.lang != Language.type)
        {
            this.lang = Language.type;
            this.label_single.GetComponent<UILabel>().text = Language.btn_single[Language.type];
            this.label_multi.GetComponent<UILabel>().text = Language.btn_multiplayer[Language.type];
            this.label_option.GetComponent<UILabel>().text = Language.btn_option[Language.type];
            this.label_credits.GetComponent<UILabel>().text = Language.btn_credits[Language.type];
        }
    }

    public static Vector2 TextBounds(string text)
    {
        return new GUIStyle(GUI.skin.label)
        {
            font = GlobalVars.Segoe,
            fontSize = 12 // <-- eww

        }.CalcSize(new GUIContent(text));
    }

    IEnumerator CheckChangelog()
    {
        WWW WWWChangelog = new WWW("http://aotmod.sl0w.org/changelog.txt");
        yield return WWWChangelog;

        if (!string.IsNullOrEmpty(WWWChangelog.error))
        {
            UnityEngine.Debug.Log(WWWChangelog.error);
            ModDrawGUI.AddDebugLine(1, WWWChangelog.error);
        }
        Changelog = WWWChangelog.text;
    }

    IEnumerator CheckNotes()
    {
        WWW WWWNotes= new WWW("http://aotmod.sl0w.org/notes.txt");
        yield return WWWNotes;

        if (!string.IsNullOrEmpty(WWWNotes.error))
        {
            UnityEngine.Debug.Log(WWWNotes.error);
            ModDrawGUI.AddDebugLine(1, WWWNotes.error);
        }
        Notes = WWWNotes.text;
    }

    private void DrawChangelog(int WindowID)
    {
        if (!bChangelogOnce)
        {
            changelogstyle = new GUIStyle(GUI.skin.button);
            changelogstyle.hover.background = GlobalVars.sl0wTextures[3];
            changelogstyle.onHover.background = GlobalVars.sl0wTextures[3];
            changelogstyle.normal.background = GlobalVars.sl0wTextures[3];
            changelogstyle.onNormal.background = GlobalVars.sl0wTextures[3];
            changelogstyle.onActive.background = GlobalVars.sl0wTextures[3];
            changelogstyle.active.background = GlobalVars.sl0wTextures[3];
            changelogstyle.fontSize = 12;
            changelogstyle.font = GlobalVars.Segoe;

            changelogtextstyle = new GUIStyle(GUI.skin.label);
            changelogtextstyle.fontSize = 12;
            changelogtextstyle.font = GlobalVars.Segoe;
            bChangelogOnce = true;
        }

        GUI.DrawTexture(new Rect(0, 0, 300, 300), GlobalVars.sl0wTextures[2]);
        if(GUI.Button(new Rect(8, 8, 65, 22), "Changes", changelogstyle))
        {
            Change_Notes = false;
        }

        if(GUI.Button(new Rect(227, 8, 65, 22), "Notes", changelogstyle))
        {
            Change_Notes = true;
        }

        if (!Change_Notes)
        {
            ChangelogScroll = GUI.BeginScrollView(new Rect(8, 37, 296, 257), ChangelogScroll, new Rect(0, 0, 250, TextBounds(Changelog).y + 4));
            GUI.Label(new Rect(2, 2, 248, TextBounds(Changelog).y + 4), Changelog, changelogtextstyle);
            GUI.EndScrollView();
        } 
        else
        {
            NotesScroll = GUI.BeginScrollView(new Rect(8, 37, 296, 257), NotesScroll, new Rect(0, 0, 250, TextBounds(Notes).y + 4));
            GUI.Label(new Rect(2, 2, 248, TextBounds(Notes).y + 4), Notes, changelogtextstyle);
            GUI.EndScrollView();
        }
    }

    private void Start()
    {
        base.StartCoroutine(CheckChangelog());
        base.StartCoroutine(CheckNotes());
    }

    private void Update()
    {
        this.showTxt();
    }

    private void OnGUI()
    {
        changelogRect = GUI.Window(0, changelogRect, DrawChangelog, "changelog");
        if(!String.IsNullOrEmpty(FengGameManagerMKII.UpdateStatus))
        {
            updatestyle = new GUIStyle(GUI.skin.label);
            updatestyle.fontSize = 12;
            updatestyle.font = GlobalVars.Segoe;
            updatestyle.normal.textColor = FengGameManagerMKII.UpdateType == 0 ? new Color(1.0f, 1.0f, 1.0f) : FengGameManagerMKII.UpdateType == 1 ? new Color(1.0f, 1.0f, 0.529f) : FengGameManagerMKII.UpdateType == 2 ? new Color(1.0f, 0.529f, 0.529f) : new Color(0.529f, 1.0f, 0.529f);
            Vector2 msgCalc = TextBounds(FengGameManagerMKII.UpdateStatus);
            GUI.Label(new Rect(Screen.width - msgCalc.x - 5, Screen.height - msgCalc.y - 3, msgCalc.x + 5, msgCalc.y + 3), FengGameManagerMKII.UpdateStatus, updatestyle);
        }
    }
}

