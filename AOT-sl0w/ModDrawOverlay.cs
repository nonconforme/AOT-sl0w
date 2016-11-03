using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Rendering;
using UnityEngine;

/*
           Use this for drawing my overlay in general. 
*/
public class ModDrawOverlay : MonoBehaviour
{
    public static GUIStyle MiscStyle;
    public static float accum;
    public static float frequency;
    public static int frames;
    public static int nbDecimal;
    public static string sFPS;
    IEnumerator FPS()
    {
        while (true)
        {
            float fps = accum / frames;
            sFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
            sFPS = String.Format("FPS: {0}", fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10)));
            accum = 0.0F;
            frames = 0;
            yield return new WaitForSeconds(frequency);
        }
    }

    public static void DrawLabel(Vector3 point, string label, Color color, int yOffset)
    {
        Vector3? vector = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(point);
        if (vector.HasValue)
        {
            Vector3 value = vector.Value;
            if (value.z > 0f)
            {
                Vector2 vector2 = GUIUtility.ScreenToGUIPoint(value);
                vector2.y = Screen.height - (vector2.y + yOffset);// 1f
                CustomCanvas.DrawString(vector2, color, CustomCanvas.TextFlags.TEXT_FLAG_OUTLINED, label);
            }
        }
    }
//     public static string RemoveTags(string name)
//     {
//         string PlayerName = name;
//         if (name.Contains("[") && name.Length >= 8)
//         {
//             for (PlayerName = name;
//                 PlayerName.Contains("[") && PlayerName.Substring(PlayerName.IndexOf("[") + 7, 1) == "]";
//                 PlayerName = PlayerName.Replace(PlayerName.Substring(PlayerName.IndexOf("["), 8), ""))
//             {
//                 if (PlayerName.Contains("[-]"))
//                 {
//                     PlayerName = PlayerName.Replace("[-]", "");
//                 }
//             }
//         }
// 
//             if (PlayerName.Contains("GUEST"))
//                 PlayerName = PlayerName.Replace(PlayerName, "[G]");
// 
//         return PlayerName;
//     }

    public static string RemoveTags(string text)
    {
        if (text.Contains("]"))
        {
            text = text.Replace("]", "");
        }
        bool flag2 = false;
        while (text.Contains("[") && !flag2)
        {
            int index = text.IndexOf("[");
            if (text.Length >= (index + 7))
            {
                string str = text.Substring(index + 1, 6);
                text = text.Remove(index, 7);
                int length = text.Length;
                if (text.Contains("["))
                {
                    length = text.IndexOf("[");
                }
                /*text = text.Insert(length, "</color>");*/
            }
            else
            {
                flag2 = true;
            }
        }
        if (flag2)
        {
            return string.Empty;
        }
        return text;
    }
    public static void DrawTitans()
    {
        IEnumerator zEnum = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().getTitans().GetEnumerator();
        while (zEnum.MoveNext())
        {
            TITAN aTitan = zEnum.Current as TITAN;

            if (!aTitan.hasDie)
            {
                foreach (Collider collider in aTitan.GetComponentsInChildren<Collider>())
                {
                    if (collider.name == "AABB")
                    {
//                         if (HERO.MyPlayer.SimpleVisible(aTitan, HERO.MyPlayer)) // fuck the vischecks for now. They work but i dont want to add 2 colors =_=
//                         {
                            if (GlobalVars.Bools[0])
                                CustomCanvas.DrawBounds(collider.bounds.center, collider.bounds.extents, GlobalVars.Colors[0]);

                            // Why the fuck didnt i just fucking set the color in an else instead of literally duping my fucking code what the fuck was i thinking..... Ahh fuck it for now
                            if((int)aTitan.photonView.owner.customProperties[PhotonPlayerProperty.isTitan] == 2) // 1 is human - 2 is titan
                            {
                                if (GlobalVars.Bools[1])
                                    DrawLabel(aTitan.transform.position, RemoveTags((string)aTitan.photonView.owner.customProperties[PhotonPlayerProperty.name]), GlobalVars.Colors[1], 0);

                                if (GlobalVars.Bools[2])
                                    DrawLabel(aTitan.transform.position, aTitan.photonView.ownerId.ToString(), GlobalVars.Colors[1], 15);
                            }
                            else
                            {
                                if (GlobalVars.Bools[1])
                                    DrawLabel(aTitan.transform.position, aTitan.name, GlobalVars.Colors[1], 0);
                            }
//                        }
//                         else
//                         {
//                             if (GlobalVars.Bools[0])
//                                 CustomCanvas.DrawBounds(collider.bounds.center, collider.bounds.extents, new Color(0.207f, 0.556f, 1.0f, 1.0f));
// 
//                             if(aTitan.nonAI)
//                             {
//                                 if (GlobalVars.Bools[1])
//                                     DrawLabel(aTitan.transform.position, (string)aTitan.photonView.owner.customProperties[PhotonPlayerProperty.name], new Color(0.207f, 0.556f, 1.0f, 1.0f), 0);
//                             }
//                             else
//                             {
//                                 if (GlobalVars.Bools[1])
//                                     DrawLabel(aTitan.transform.position, (string)aTitan.name, new Color(0.207f, 0.556f, 1.0f, 1.0f), 0);
//                             }

//                        }
                    }
                }
            }
        }
    }
    public static void DrawHumans()
    {
         IEnumerator zEnum = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().getPlayers().GetEnumerator();
         while (zEnum.MoveNext())
         {
             HERO aHERO = zEnum.Current as HERO;
             if (!aHERO ||
                !aHERO.photonView)
                 continue;
             if (!aHERO.photonView.owner.isLocal)
             {
                 foreach (Collider collider in aHERO.GetComponentsInChildren<Collider>()) // humans
                 {
                     if (collider.name == "Controller_Body")
                     {
                         Vector3 Headpos = new Vector3(0, collider.bounds.extents.y, 0);
                         if (GlobalVars.Bools[4])
                             CustomCanvas.DrawBounds(collider.bounds.center, collider.bounds.extents, GlobalVars.Colors[4]);

                         if (aHERO.photonView != null)
                         {
                             if (aHERO.photonView.owner != null)
                             {

                                 if (GlobalVars.Bools[5] || GlobalVars.Bools[6])
                                    aHERO.myNetWorkName.GetComponent<UILabel>().text = string.Empty;

                                 string Playername = RemoveTags((string)aHERO.photonView.owner.customProperties[PhotonPlayerProperty.name]);
                                 string Guildname = RemoveTags((string)aHERO.photonView.owner.customProperties[PhotonPlayerProperty.guildName]);
                                 if(!GlobalVars.Bools[5] && !GlobalVars.Bools[6] && aHERO.myNetWorkName.GetComponent<UILabel>().text == string.Empty)
                                 {
                                     if (Guildname != string.Empty)
                                    {
                                        aHERO.myNetWorkName.GetComponent<UILabel>().text = "[FFFF00]" + Guildname + "\n[FFFFFF]" + Playername;
                                    }
                                    else
                                    {
                                        aHERO.myNetWorkName.GetComponent<UILabel>().text = Playername;
                                    }
                                 }


                                 if (GlobalVars.Bools[5])
                                    DrawLabel(collider.bounds.center + Headpos, Playername, GlobalVars.Colors[5], +20);

                                 if (GlobalVars.Bools[6])
                                     DrawLabel(collider.bounds.center + Headpos, Guildname, GlobalVars.Colors[6], GlobalVars.Bools[5] ? +35 : +20);

                                 //wtf am i doing here...... uhhhhh... k
                                 if (GlobalVars.Bools[7])
                                     DrawLabel(collider.bounds.center + Headpos, aHERO.photonView.ownerId.ToString(), GlobalVars.Colors[7],
                                         GlobalVars.Bools[5] ? GlobalVars.Bools[6] ? Guildname != string.Empty ? +50 : +35 : +35 : GlobalVars.Bools[6] ? Guildname != string.Empty ? +35 : +20 : +20);
                                //7
                                //6
                                //5 
                                
                                 
                                 if (Playername.ToUpper().Contains("KITHARA")) // she changed name to margarite or sm shit so find new 1 Q~Q
                                     DrawLabel(collider.bounds.center, "The Waifu", new Color(0.705f, 0.490f, 1.0f, 1.0f), +35);

                                 if (aHERO.photonView.owner.name != "")
                                     DrawLabel(collider.bounds.center + Headpos, aHERO.photonView.owner.name, GlobalVars.Colors[7], +35);

                                 if (GlobalVars.Bools[4] || GlobalVars.Bools[5] || GlobalVars.Bools[6] || GlobalVars.Bools[7])
                                 {
                                     string strTags = string.Empty;
                                     if (aHERO.photonView.owner.customProperties.ContainsKey("RCteam"))
                                     {
                                         strTags = strTags + "[RC] ";
                                     }
                                     if (aHERO.photonView.owner.customProperties.ContainsKey("NRC"))
                                     {
                                         strTags = strTags + "[NRC] ";
                                     }
                                     if (aHERO.photonView.owner.customProperties.ContainsKey("Nathan"))
                                     {
                                         strTags = strTags + "[Nathan] ";
                                     }
                                     if (aHERO.photonView.owner.customProperties.ContainsKey("KageNoKishi"))
                                     {
                                         strTags = strTags + "[KnK] ";
                                     }
                                     if (aHERO.photonView.owner.customProperties.ContainsKey("not null"))
                                     {
                                         strTags = strTags + "[ECMod] ";
                                     }

                                     if (strTags != string.Empty)
                                         DrawLabel(collider.bounds.center - Headpos, strTags, new Color(0.0f, 0.7f, 1.0f, 1.0f), 0);
                                 }
                             }
                         }
                     }
                 }
             }
         }
    }
    public static void DrawMiscOverlay()
    {
        MiscStyle = new GUIStyle(GUI.skin.label);
        MiscStyle.font = GlobalVars.Segoe;
        MiscStyle.fontSize = 12;

        // Only reason atm im not just forlooping a single func is beyond me... i think the layering math that way would hurt my hed tho XDDDD
        if(GlobalVars.Bools[8])
        {
            MiscStyle.normal.textColor = GlobalVars.Colors[8];
            string LocalTime = DateTime.Now.ToString();
            Vector2 TxtBounds = PanelMain.TextBounds(LocalTime);
            GUI.Label(new Rect(Screen.width - 4 - TxtBounds.x, Screen.height - 4 - TxtBounds.y, TxtBounds.x, TxtBounds.y), LocalTime, MiscStyle);
        }

        if (GlobalVars.Bools[9])
        {
            MiscStyle.normal.textColor = GlobalVars.Colors[9];
            string ResolutionString = String.Format("({0}x{1})", Screen.width, Screen.height);
            Vector2 TxtBounds = PanelMain.TextBounds(ResolutionString);
            GUI.Label(new Rect(Screen.width - 4 - TxtBounds.x, Screen.height - 4 - TxtBounds.y - 
                (GlobalVars.Bools[8] ? 18 : 0), TxtBounds.x, TxtBounds.y), ResolutionString, MiscStyle);
        }

        if (GlobalVars.Bools[11])
        {
            MiscStyle.normal.textColor = GlobalVars.Colors[11];
            string PingString = String.Format("Ping: {0}", PhotonNetwork.GetPing());
            Vector2 TxtBounds = PanelMain.TextBounds(PingString);
            GUI.Label(new Rect(Screen.width - 4 - TxtBounds.x, Screen.height - 4 - TxtBounds.y -
                (GlobalVars.Bools[8] ? GlobalVars.Bools[9] ? 36 : 18 : GlobalVars.Bools[9] ? 18 : 0), 
                TxtBounds.x, TxtBounds.y), PingString, MiscStyle);
        }

        if (GlobalVars.Bools[10])
        {
            if(sFPS != string.Empty)
            {
                MiscStyle.normal.textColor = GlobalVars.Colors[10];
                Vector2 TxtBounds = PanelMain.TextBounds(sFPS);
                GUI.Label(new Rect(Screen.width - 4 - TxtBounds.x, Screen.height - 4 - TxtBounds.y -
                    (GlobalVars.Bools[8] 
                    ?
                        GlobalVars.Bools[9]
                        ?
                            GlobalVars.Bools[11]
                            ? 
                                54 
                            :
                                36 
                       :
                        GlobalVars.Bools[11]
                        ?
                            36
                        :
                            18 
                    :
                    GlobalVars.Bools[9]
                    ?
                        GlobalVars.Bools[11]
                        ?
                            36
                        :
                            18
                    :
                        GlobalVars.Bools[11]
                        ?
                            18
                        :
                            0
                      ),
                    TxtBounds.x, TxtBounds.y), sFPS, MiscStyle);
            }
        }
    }

    void Start()
    {
        frequency = 0.5F;
        accum = 0.0f;
        nbDecimal = 0;
        frames = 0;
        sFPS = string.Empty;
        base.StartCoroutine(FPS());

    }

    void Awake() // i wonder if its best to just remove dis cuz i dont see myself using it much
    {}

    void OnGUI()
    {
            FengGameManagerMKII FengGameMGR = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
            if (FengGameMGR.gameStart)
            {
                DrawTitans();
                DrawHumans();
                DrawMiscOverlay();
            }
    }
    void Update()
    {
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
    }
}
