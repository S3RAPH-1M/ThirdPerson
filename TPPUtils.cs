using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThirdPerson
{
    //this code is from nexus4880.

    public static class TPPUtils
    {
        public static Texture2D lineTex;


        public static void MDrawLine(GameObject otherObject, Vector3 positionOne, Color col)
        {
            LineRenderer lr = otherObject.GetOrAddComponent<LineRenderer>();
            lr.enabled = false;
            lr.endColor = col;
            lr.startColor = col;
            lr.material.color = col;
            lr.endWidth = 0.02f;
            lr.startWidth = 0.02f;
            lr.SetPosition(0, positionOne);
            lr.SetPosition(1, otherObject.transform.position);
            lr.enabled = true;
        }

        public static void MRemoveLine(GameObject _object)
        {
            LineRenderer lr = _object.GetComponent<LineRenderer>();
            if (lr == null)
            {
                return;
            }

            lr.enabled = false;
        }

        public static void P(Vector2 Position, Color color, float thickness)
        {
            if (!lineTex) { lineTex = new Texture2D(1, 1); }
            float yOffset = Mathf.Ceil(thickness / 2f);
            Color savedColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(Position.x, Position.y - (float)yOffset, thickness, thickness), lineTex);
            GUI.color = savedColor;
        }

        public static void DrawCircle(Vector2 position, float r, Color color, float thickness)
        {
            const double PI = 3.1415926535;
            double i, angle, x1, y1;

            for (i = 0; i < 360; i += 0.1)
            {
                angle = i;
                x1 = r * Math.Cos(angle * PI / 180);
                y1 = r * Math.Sin(angle * PI / 180);
                P(new Vector2((float)(position.x + x1), (float)(position.y + y1)), color, thickness);
            }
        }
        public static void DrawBox(float x, float y, float w, float h, Color color)
        {
            DrawLine(new Vector2(x, y), new Vector2(x + w, y), color);
            DrawLine(new Vector2(x, y), new Vector2(x, y + h), color);
            DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color);
            DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color);
        }
        public static void DrawLine(Rect rect) { DrawLine(rect, GUI.contentColor, 1.0f); }
        public static void DrawLine(Rect rect, Color color) { DrawLine(rect, color, 1.0f); }
        public static void DrawLine(Rect rect, float width) { DrawLine(rect, GUI.contentColor, width); }
        public static void DrawLine(Rect rect, Color color, float width) { DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width); }
        public static void DrawLine(Vector2 pointA, Vector2 pointB) { DrawLine(pointA, pointB, GUI.contentColor, 1.0f); }
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color) { DrawLine(pointA, pointB, color, 1.0f); }
        public static void DrawLine(Vector2 pointA, Vector2 pointB, float width) { DrawLine(pointA, pointB, GUI.contentColor, width); }
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)

        {
            Matrix4x4 matrix = GUI.matrix;
            if (!lineTex) { lineTex = new Texture2D(1, 1); }
            Color savedColor = GUI.color;
            GUI.color = color;
            float angle = Vector3.Angle(pointB - pointA, Vector2.right);
            if (pointA.y > pointB.y) { angle = -angle; }
            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
            GUIUtility.RotateAroundPivot(angle, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);
            GUI.matrix = matrix;
            GUI.color = savedColor;
        }
    }
}
