using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions; // lets see wut we use furst b4 we remove shits :D
using UnityEngine;

public class GlobalVars
{
    public static Shader BloomShader;
    public static Shader BlurFlaresShader;
    public static Shader LensFlareCreate;
    public static Shader BrightPassFilter;
    public static bool[] Bools;
    public static int[] Ints;
    public static float[] Floats;
    public static float[] FloatTimers;
    public static float[] MaxFloats;
    public static float[] MinFloats;
    public static Color[] Colors;
    public static string[] Strings;
    public static GUIContent[] GUIContents;
    public static Texture2D[] sl0wTextures;
    public static Material OutlineShader;
    public static AssetBundle mapBundle;
    public static Font Segoe;
    public static string ToolTip;
    public static bool bOnce = false;

    public static void InitVars()
    {
        ToolTip = string.Empty;
        Bools = new bool[]
        {
            /*      Visuals tab     */
            /*      VisualTitans 0-3    */
            false,  //box
            false,  //name
            false,  //id
            false,  //shaders
            /*      VisualPlayers 4-7   */
            false,  //box
            false,  //name
            false,  //guild
            false,  //id
            /*      Misc Visuals 8-12   */
            false,  //localtime
            false,  //resolution
            false,  //fps
            false,  //ping
            false,  //crosshair
            /*         ////         */

            /*      Player tab      */
            /*      Player      13-16   */
            false,  // Speed Modifier
            false,  // Superjump
            false,  // Force anim
            false,  // No Clip

            /*      Player Smite 17-20  */
            false,  // Autosmite
            false,  // Synced Kill Only // This is going to be fucking annoying to log timings. 500FPS recordings of AOT incoming LE OLE 660 and 4790k to the rescue
            true,   // Sync DMG to Min[RC]
            false,  // Ultra-Legit toggle (Lets add some better height checks and maybe even predict flightpath before doing attacks) Le ole 1 Deag

            /*      Player Move 21-22   */
            false,  // Onkey Acceleration
            false,  // Teleport // lets make an entire group dedicated to teleport options. IE: mouseraycast, titannape, safetyport, playerport etc.

            //misc
            false,  // insult generator
            false, // saitama
            false, // crash
            false, // upsidedown
            false, // reverse
            false, // fag white rapscene talk
            false, // logger
            false // autoignore


        };

            Colors = new Color[]
        {
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f),
            new Color(1.0f, 1.0f, 1.0f)
        };

            GUIContents = new GUIContent[]
        { 
            
            new GUIContent("3D Box","Display the bounds of the titan."),
            new GUIContent("Name","Display the name of the titan/pt."),
            new GUIContent("ID","Display the ID of the pt."),
            new GUIContent("Shaders","Display outlines on the titans."),
            new GUIContent("3D Box","Display the bounds of the player."),
            new GUIContent("Name","Display the name of the player."),
            new GUIContent("Guild","Display the guild of the player."),
            new GUIContent("ID","Display the ID of the player."),
            new GUIContent("LocalTime","Display the localtime of your machine."),
            new GUIContent("Resolution","Display the resolution of your gamewindow."),
            new GUIContent("FPS","Display your frames per second."),
            new GUIContent("Ping","Display the latency of your connection."),
            new GUIContent("Crosshair","Display a secondary crosshair on screen."),
            new GUIContent("Speed","Set your characters air speed acceleration."),
            new GUIContent("Jump","Set your characters jump height."),
            new GUIContent("Force Anim","Force the animation of your player."),
            new GUIContent("No Clip","Disable the collision of your player and fly around the map."),
            new GUIContent("Autosmite","Automatically attack titans."),
            new GUIContent("AnimSync","Only attack the titan if the animation is synced."),
            new GUIContent("MinDMG","Only attack if the minimum damge for RCDMG or healthmodes are met."),
            new GUIContent("UltraLegit","Add more checks and calculations before attempting to attack."),
            new GUIContent("HotkeyACL","On keypress accelerate your player faster."),
            new GUIContent("Teleport","Move the player to a desired location."),
            new GUIContent("Insult Generator","Generate random insults to chat"),
            new GUIContent("Saitama Mode","Punch titans!"),
            new GUIContent("Crash Selected","Crashes selected server based on matching words in the title"),
            new GUIContent("UpsideDown","nigga u wronkwayp"),
            new GUIContent("Reverse text","won sdruwkcab niklat u aggin"),
            new GUIContent("Fag talk","TVLK LIKX V FVGGOT"),
            new GUIContent("Logging","Log network traffic to file"),
            new GUIContent("Auto Ignore","Automatically ignore users for violations"),

            //new GUIContent("Crosshair","Display a secondary crosshair on screen."),

        };

            Ints = new int[]
        {
            0 // ActiveTab
        };

            FloatTimers = new float[]
        {
            Environment.TickCount,
            Environment.TickCount,
            150.0f
        };

            Floats = new float[]
        {
            1.0f,
            1.0f,
            0.0f,
            1.0f,
            150.0f,
            1.0f,
            1.4f
        };

            MaxFloats = new float[]
        {
            3.0f,
            20.0f,
            6.0f, // we need to find some fun animations. horsetwerk cant be the only one :) maybe an armin twirl...
            1000.0f,
            30000
 
        };

            MinFloats = new float[]
        {
            1.0f,
            1.0f,
            0.0f, // we need to find some fun animations. horsetwerk cant be the only one :) maybe an armin twirl...
            40.0f,
            150.0f
        };
            Strings = new string[]
        {
            "Visual",
            "Player",
            "Misc",
            "Server",
            "Advanced"
        };

    }

}
