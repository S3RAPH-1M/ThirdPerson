using System;
using EFT;
using UnityEngine;

namespace ThirdPerson {
	public static class Crosshair {
		public static void Draw() {
			if (TPPEntry.Instance.LocalPlayer.PointOfView == EPointOfView.FirstPerson) {
				return;
			}

			if (!(TPPEntry.Instance.LocalPlayer.HandsController is Player.FirearmController firearmcontroller)) {
				return;
			}

			if (!Physics.Raycast(new Ray(firearmcontroller.Fireport.position, firearmcontroller.WeaponDirection * 1f),
				    out RaycastHit Hit, Single.MaxValue, GClass2400.HitMask)) {
				return;
			}

			Vector3 HitPoint = CameraClass.Instance.Camera.WorldToScreenPoint(Hit.point);
			if (HitPoint.z < 0.01f) {
				return;
			}

			Vector2 pointA = new Vector2(HitPoint.x - TPPEntry.Instance.CrosshairGap.Value, Screen.height - HitPoint.y);
			Vector2 pointB =
				new Vector2(HitPoint.x - TPPEntry.Instance.CrosshairGap.Value - TPPEntry.Instance.CrosshairSize.Value,
					Screen.height - HitPoint.y);
			TPPUtils.DrawLine(pointA, pointB, TPPEntry.Instance.Crosshaircolor.Value,
				TPPEntry.Instance.CrosshairThickness.Value);

			pointA = new Vector2(HitPoint.x + TPPEntry.Instance.CrosshairGap.Value, Screen.height - HitPoint.y);
			pointB = new Vector2(
				HitPoint.x + TPPEntry.Instance.CrosshairGap.Value + TPPEntry.Instance.CrosshairSize.Value,
				Screen.height - HitPoint.y);
			TPPUtils.DrawLine(pointA, pointB, TPPEntry.Instance.Crosshaircolor.Value,
				TPPEntry.Instance.CrosshairThickness.Value);

			pointA = new Vector2(HitPoint.x, Screen.height - HitPoint.y - TPPEntry.Instance.CrosshairGap.Value);
			pointB = new Vector2(HitPoint.x,
				Screen.height - HitPoint.y - TPPEntry.Instance.CrosshairGap.Value -
				TPPEntry.Instance.CrosshairSize.Value);
			TPPUtils.DrawLine(pointA, pointB, TPPEntry.Instance.Crosshaircolor.Value,
				TPPEntry.Instance.CrosshairThickness.Value);

			pointA = new Vector2(HitPoint.x, Screen.height - HitPoint.y + TPPEntry.Instance.CrosshairGap.Value);
			pointB = new Vector2(HitPoint.x,
				Screen.height - HitPoint.y + TPPEntry.Instance.CrosshairGap.Value +
				TPPEntry.Instance.CrosshairSize.Value);
			TPPUtils.DrawLine(pointA, pointB, TPPEntry.Instance.Crosshaircolor.Value,
				TPPEntry.Instance.CrosshairThickness.Value);
		}
	}
}