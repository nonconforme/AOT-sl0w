using Rendering;
using System;
using UnityEngine;

namespace Rendering
{
    public class CustomCanvas
    {
        private static float lastFrameTime = (float)Environment.TickCount;
        private static float framesPerSecond = 0.0f;
        private static float averageFramesPerSecond = 0.0f;
        private static float totalFramesPerSecond = 0.0f;
        private static Texture2D drawingTex = (Texture2D)null;
        private static Color lastTexColor;
        public static Material lineMaterial; // Private
        static CustomCanvas()
        {
        } // ayylmao
        private static Vector3 World2Screen(Vector3 Pos1)
        {
            Vector3 OutVec;
            Vector3 Pos1_ = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(Pos1);
            if (Pos1_.z > 0)
            {
                Pos1_.y = Screen.height - (Pos1_.y + 1f);
                OutVec = Pos1_;
                return OutVec;
            }
            return new Vector3(0, 0, 0);
        }
        public static void UpdateFPS()
        {
            float num = (float)Environment.TickCount;
            CustomCanvas.totalFramesPerSecond = (float)(((double)CustomCanvas.framesPerSecond + 0.100000001490116) / (((double)num - (double)CustomCanvas.lastFrameTime) / 1000.0));
            if ((double)num - (double)CustomCanvas.lastFrameTime > 1000.0)
            {
                CustomCanvas.lastFrameTime = num;
                CustomCanvas.framesPerSecond = 0.0f;
                CustomCanvas.averageFramesPerSecond = CustomCanvas.totalFramesPerSecond;
            }
            ++CustomCanvas.framesPerSecond;
        }
        public static void DrawString(Vector2 pos, Color color, CustomCanvas.TextFlags flags, string text)
        {
            bool center = (flags & CustomCanvas.TextFlags.TEXT_FLAG_CENTERED) == CustomCanvas.TextFlags.TEXT_FLAG_CENTERED;
            if ((flags & CustomCanvas.TextFlags.TEXT_FLAG_OUTLINED) == CustomCanvas.TextFlags.TEXT_FLAG_OUTLINED)
            {
                CustomCanvas.DrawStringInternal(pos + new Vector2(1f, 0.0f), Color.black, text, center);
                CustomCanvas.DrawStringInternal(pos + new Vector2(0.0f, 1f), Color.black, text, center);
                CustomCanvas.DrawStringInternal(pos + new Vector2(0.0f, -1f), Color.black, text, center);
            }
            if ((flags & CustomCanvas.TextFlags.TEXT_FLAG_DROPSHADOW) == CustomCanvas.TextFlags.TEXT_FLAG_DROPSHADOW)
                CustomCanvas.DrawStringInternal(pos + new Vector2(1f, 1f), Color.black, text, center);
            CustomCanvas.DrawStringInternal(pos, color, text, center);
        }
        public static Vector2 TextBounds(string text) // If i ever want to add more sized text options make it pass a size paam
        {
            return new GUIStyle(GUI.skin.label)
            {
                fontSize = 12 // <-- eww
            }.CalcSize(new GUIContent(text));
        }
        private static void DrawStringInternal(Vector2 pos, Color color, string text, bool center)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = color;
            style.fontSize = 12;
            style.font = GlobalVars.Segoe;
            //if (center)
                pos.x -= style.CalcSize(new GUIContent(text)).x / 2f;
            GUI.Label(new Rect((int)(pos.x /*- 132*/), (int)pos.y, 264f, 20f), text, style);
        }
        public static void DrawBox(Vector2 pos, Vector2 size, Color color)
        {
            if (!(bool)((UnityEngine.Object)CustomCanvas.drawingTex))
                CustomCanvas.drawingTex = new Texture2D(1, 1);
            if (color != CustomCanvas.lastTexColor)
            {
                CustomCanvas.drawingTex.SetPixel(0, 0, color);
                CustomCanvas.drawingTex.Apply();
                CustomCanvas.lastTexColor = color;
            }
            GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), CustomCanvas.drawingTex);
        }
        public static Vector2 CastVectorInt(Vector2 InVec)
        {
            Vector2 OutVec;
            OutVec.x = (int)InVec.x;
            OutVec.y = (int)InVec.y;
            return OutVec;
        }
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color)
        {
            if (!(bool)((UnityEngine.Object)CustomCanvas.lineMaterial))
            {
                CustomCanvas.lineMaterial = new Material("Shader \"Lines/Colored Blended\" {SubShader { Pass {   BindChannels { Bind \"Color\",color }   Blend SrcAlpha OneMinusSrcAlpha   ZWrite Off Cull Off Fog { Mode Off }} } }");
                CustomCanvas.lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                CustomCanvas.lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }
            CustomCanvas.lineMaterial.SetPass(0);
            GL.Begin(1);
            GL.Color(color);
            GL.Vertex3(pointA.x, pointA.y, 0.0f);
            GL.Vertex3(pointB.x, pointB.y, 0.0f);
            GL.End();
        }
        private static void World2ScreenLine(Vector3 Pos1, Vector3 Pos2, Color color)
        {
            Camera ClientCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            Vector3 Pos1_ = ClientCamera.WorldToScreenPoint(Pos1);
            Vector3 Pos2_ = ClientCamera.WorldToScreenPoint(Pos2);
            if (Pos1_.z > 0 && Pos2_.z > 0)
            {
                Pos1_.y = Screen.height - (Pos1_.y + 1f);
                Pos2_.y = Screen.height - (Pos2_.y + 1f);
//                 if(gVars.bOutlines)
//                 {
                    DrawLine(new Vector2((int)Pos1_.x - 1, (int)Pos1_.y), new Vector2((int)Pos2_.x, (int)Pos2_.y), Color.black);
                    DrawLine(new Vector2((int)Pos1_.x + 1, (int)Pos1_.y), new Vector2((int)Pos2_.x, (int)Pos2_.y), Color.black);
                    DrawLine(new Vector2((int)Pos1_.x, (int)Pos1_.y - 1), new Vector2((int)Pos2_.x, (int)Pos2_.y), Color.black);
                    DrawLine(new Vector2((int)Pos1_.x, (int)Pos1_.y + 1), new Vector2((int)Pos2_.x, (int)Pos2_.y), Color.black);
                /*}*/
                DrawLine(new Vector2((int)Pos1_.x, (int)Pos1_.y), new Vector2((int)Pos2_.x, (int)Pos2_.y), color);
            }
        }
        public static void DrawBounds(Vector3 Center, Vector3 Ext, Color color)
        {
            Vector3 Extents = Ext * 1.4f;
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x), color); // S - W Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x), color); // W - Y Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x), color); // S - V Corners
			World2ScreenLine(
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x), color); // V - Y Corners
			////////////////////////////////////////////////////////////////////////////
			/////ConnectorLines/////
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x),
				Center + (Vector3.back * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.right * Extents.x), color); // S - T Corners // WTF?
			World2ScreenLine(
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.right * Extents.x), color); // W - X Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.back * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.left * Extents.x), color); // V - U Corners
			World2ScreenLine(
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.left * Extents.x), color); // Y - Z Corners
			////////////////////////////////////////////////////////////////////////////
			/////LOWER RECT/////
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.left * Extents.x),
				Center + (Vector3.back * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.right * Extents.x), color); // T - X Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.right * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.right * Extents.x), color); // X - Z Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.left * Extents.x), color); // T - U Corners
			World2ScreenLine(
				Center + (Vector3.forward * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * (Extents.y / 1.4f)) + (Vector3.right * Extents.x), color); // U - Z Corners\
        } // dis has math to get a bit outside
        public static void DrawExactBounds(Vector3 Center, Vector3 Ext, Color color)
        {
            Vector3 Extents = Ext;
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x), color); // S - W Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x), color); // W - Y Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x), color); // S - V Corners
			World2ScreenLine(
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x), color); // V - Y Corners
			////////////////////////////////////////////////////////////////////////////
			/////ConnectorLines/////
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x),
				Center + (Vector3.back * Extents.z) + (Vector3.down * Extents.y) + (Vector3.right * Extents.x), color); // S - T Corners // WTF?
			World2ScreenLine(
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.right * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * Extents.y) + (Vector3.right * Extents.x), color); // W - X Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.back * Extents.z) + (Vector3.down * Extents.y) + (Vector3.left * Extents.x), color); // V - U Corners
			World2ScreenLine(
				Center + (Vector3.forward * Extents.z) + (Vector3.up * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * Extents.y) + (Vector3.left * Extents.x), color); // Y - Z Corners
			////////////////////////////////////////////////////////////////////////////
			/////LOWER RECT/////
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.down * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.back * Extents.z) + (Vector3.down * Extents.y) + (Vector3.right * Extents.x), color); // T - X Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.down * Extents.y) + (Vector3.right * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * Extents.y) + (Vector3.right * Extents.x), color); // X - Z Corners
			World2ScreenLine(
				Center + (Vector3.back * Extents.z) + (Vector3.down * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * Extents.y) + (Vector3.left * Extents.x), color); // T - U Corners
			World2ScreenLine(
				Center + (Vector3.forward * Extents.z) + (Vector3.down * Extents.y) + (Vector3.left * Extents.x),
				Center + (Vector3.forward * Extents.z) + (Vector3.down * Extents.y) + (Vector3.right * Extents.x), color); // U - Z Corners\
        } // dis is exact bounds
        public static void ESPBOX(Vector2 BoxTop, Vector2 BoxBottom, float Width, Color color) // this is 2D outline rect
        {
            CustomCanvas.DrawLine(new Vector2((int)(BoxTop.x - Width), (int)BoxTop.y), new Vector2((int)(BoxTop.x + Width - 1), (int)BoxTop.y), color);// TOP
            CustomCanvas.DrawLine(new Vector2((int)(BoxTop.x - Width), (int)BoxTop.y), new Vector2((int)(BoxTop.x - Width), (int)BoxBottom.y), color); // Left
            CustomCanvas.DrawLine(new Vector2((int)(BoxTop.x + Width), (int)BoxTop.y), new Vector2((int)(BoxTop.x + Width), (int)BoxBottom.y), color); // Right
            CustomCanvas.DrawLine(new Vector2((int)(BoxTop.x - Width), (int)BoxBottom.y), new Vector2((int)(BoxTop.x + Width), (int)BoxBottom.y), color);// Bottom
        }
        public static void ESPBOX_Outlined(Vector2 BoxTop, Vector2 BoxBottom, float Width, Color color)
        {
            BoxTop = CastVectorInt(BoxTop);
            BoxBottom = CastVectorInt(BoxBottom);
            Width = (int)Width;
            BoxTop.y -= 1;
            BoxBottom.y += 1;
            Width -= 1;
            CustomCanvas.ESPBOX(BoxTop, BoxBottom, Width, new Color(0, 0, 0));
            BoxTop.y += 2;
            BoxBottom.y -= 2;
            Width += 2;
            CustomCanvas.ESPBOX(BoxTop, BoxBottom, Width, new Color(0, 0, 0));
            BoxTop.y -= 1;
            BoxBottom.y += 1;
            Width -= 1;
            CustomCanvas.ESPBOX(BoxTop, BoxBottom, Width, color);
        }
        public static void DrawBoxOutlines(Vector2 position, Vector2 size, Color color)
        {
            CustomCanvas.DrawBox(new Vector2(position.x, position.y), new Vector2(size.x, position.y), color); // top
            CustomCanvas.DrawBox(new Vector2(position.x, position.y), new Vector2(position.x, size.y), color); // left
            CustomCanvas.DrawBox(new Vector2(size.x, position.y), new Vector2(size.x, size.y), color); // right
            CustomCanvas.DrawBox(new Vector2(position.x, size.y), new Vector2(size.x, size.y), color); // bottom
        }
        public static void DrawCrosshair()
        {
            float sx = Screen.width / 2 + 1f;
            float sy = Screen.height / 2 + 1f;
            CustomCanvas.DrawLine(new Vector2(sx, sy - 20f), new Vector2(sx, sy + 20f), Color.yellow);
            CustomCanvas.DrawLine(new Vector2(sx - 20f, sy), new Vector2(sx + 20f, sy), Color.yellow);
        }
        public static void HeliosBox() // ugly AA looking lock box.
        {
            CustomCanvas.HeliosBox((float)(Screen.width / 2), (float)(Screen.height / 2));
        }
        public static void HeliosBox(float sx, float sy)
        {
            sx += 1f;
            sy += 1f;
            Color color = new Color((float)byte.MaxValue, (float)byte.MaxValue, 0.0f);
            CustomCanvas.DrawLine(new Vector2(sx - 20f, sy - 20f), new Vector2(sx + 20f, sy - 20f), color);
            CustomCanvas.DrawLine(new Vector2(sx + 20f, sy - 20f), new Vector2(sx + 20f, sy + 20f), color);
            //CustomCanvas.DrawLine(new Vector2(sx, sy - 20f), new Vector2(sx, sy + 20f), color);
            //CustomCanvas.DrawLine(new Vector2(sx - 20f, sy), new Vector2(sx + 20f, sy), color);
            CustomCanvas.DrawLine(new Vector2(sx - 20f, sy + 20f), new Vector2(sx + 20f, sy + 20f), color);
            CustomCanvas.DrawLine(new Vector2(sx - 20f, sy - 20f), new Vector2(sx - 20f, sy + 20f), color);
        }
        [Flags]
        public enum TextFlags
        {
            TEXT_FLAG_NONE = 0,
            TEXT_FLAG_CENTERED = 1,
            TEXT_FLAG_OUTLINED = 2,
            TEXT_FLAG_DROPSHADOW = TEXT_FLAG_OUTLINED | TEXT_FLAG_CENTERED,
        }
    }
}