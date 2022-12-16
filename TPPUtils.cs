using System;
using UnityEngine;

namespace ThirdPerson {
	public static class TPPUtils {
		public static Texture2D lineTex;

		public static void MDrawLine(GameObject otherObject, Vector3 positionOne, Color color) {
			LineRenderer lr = otherObject.GetOrAddComponent<LineRenderer>();
			lr.enabled = false;
			lr.endColor = color;
			lr.startColor = color;
			lr.material.color = color;
			lr.endWidth = 0.02f;
			lr.startWidth = 0.02f;
			lr.SetPosition(0, positionOne);
			lr.SetPosition(1, otherObject.transform.position);
			lr.enabled = true;
		}

		public static void MRemoveLine(GameObject _object) {
			LineRenderer lr = _object.GetComponent<LineRenderer>();
			if (lr == null) {
				return;
			}

			lr.enabled = false;
		}

		public static void P(Vector2 Position, Color color, Single thickness) {
			if (!lineTex) {
				lineTex = new Texture2D(1, 1);
			}

			Single yOffset = Mathf.Ceil(thickness / 2f);
			Color savedColor = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(new Rect(Position.x, Position.y - yOffset, thickness, thickness), lineTex);
			GUI.color = savedColor;
		}

		public static void DrawCircle(Vector2 position, Single r, Color color, Single thickness) {
			const Double PI = 3.1415926535;
			Double i;
			for (i = 0; i < 360; i += 0.1) {
				Double x1 = r * Math.Cos(i * PI / 180);
				Double y1 = r * Math.Sin(i * PI / 180);
				P(new Vector2((Single)(position.x + x1), (Single)(position.y + y1)), color, thickness);
			}
		}

		public static void DrawBox(Single x, Single y, Single w, Single h, Color color) {
			DrawLine(new Vector2(x, y), new Vector2(x + w, y), color);
			DrawLine(new Vector2(x, y), new Vector2(x, y + h), color);
			DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color);
			DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color);
		}

		public static void DrawLine(Rect rect) {
			DrawLine(rect, GUI.contentColor, 1.0f);
		}

		public static void DrawLine(Rect rect, Color color) {
			DrawLine(rect, color, 1.0f);
		}

		public static void DrawLine(Rect rect, Single width) {
			DrawLine(rect, GUI.contentColor, width);
		}

		public static void DrawLine(Rect rect, Color color, Single width) {
			DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width);
		}

		public static void DrawLine(Vector2 pointA, Vector2 pointB) {
			DrawLine(pointA, pointB, GUI.contentColor, 1.0f);
		}

		public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color) {
			DrawLine(pointA, pointB, color, 1.0f);
		}

		public static void DrawLine(Vector2 pointA, Vector2 pointB, Single width) {
			DrawLine(pointA, pointB, GUI.contentColor, width);
		}

		public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, Single width) {
			Matrix4x4 matrix = GUI.matrix;
			if (!lineTex) {
				lineTex = new Texture2D(1, 1);
			}

			Color savedColor = GUI.color;
			GUI.color = color;
			Single angle = Vector3.Angle(pointB - pointA, Vector2.right);
			if (pointA.y > pointB.y) {
				angle = -angle;
			}

			GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width),
				new Vector2(pointA.x, pointA.y + 0.5f));
			GUIUtility.RotateAroundPivot(angle, pointA);
			GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);
			GUI.matrix = matrix;
			GUI.color = savedColor;
		}
	}
}