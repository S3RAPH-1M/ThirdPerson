using EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThirdPerson
{
    public static class Crosshair
    {
        public static float CrosshairSize = TPPEntry.Instance.CrosshairSize.Value;
        public static float CrosshairThickness = TPPEntry.Instance.CrosshairThickness.Value;
        public static float CrosshairGap = TPPEntry.Instance.CrosshairGap.Value;
        public static Color color = Color.cyan;

        public static void Draw()
        {
            if (TPPEntry.Instance.LocalPlayer.PointOfView == EPointOfView.FirstPerson)
            {
                return;
            }
            if (TPPEntry.Instance.LocalPlayer.HandsController == null)
            {
                return;
            }

            if (TPPEntry.Instance.LocalPlayer.HandsController is Player.FirearmController firearmcontroller)
            {

                Ray r = new Ray(firearmcontroller.Fireport.position, firearmcontroller.WeaponDirection * 1f);
                if (!Physics.Raycast(r, out RaycastHit Hit, float.MaxValue, GClass2394.HitMask))
                {
                    return;
                }
                Vector3 HitPoint = Camera.main.WorldToScreenPoint(Hit.point);
                if (HitPoint.z <= 0.01f)
                {
                    return;
                }

                Vector2 PointA = new Vector2(Mathf.Round(HitPoint.x - CrosshairGap), Mathf.Round(Screen.height - HitPoint.y));
                Vector2 PointB = new Vector2(Mathf.Round(HitPoint.x - CrosshairGap - CrosshairSize), Mathf.Round(Screen.height - HitPoint.y));
                TPPUtils.DrawLine(PointA, PointB, color, CrosshairThickness);

                PointA = new Vector2(Mathf.Round(HitPoint.x + CrosshairGap), Mathf.Round(Screen.height - HitPoint.y));
                PointB = new Vector2(Mathf.Round(HitPoint.x + CrosshairGap + CrosshairSize), Mathf.Round(Screen.height - HitPoint.y));
                TPPUtils.DrawLine(PointA, PointB, color, CrosshairThickness);

                PointA = new Vector2(Mathf.Round(HitPoint.x), Mathf.Round(Screen.height - HitPoint.y - CrosshairGap));
                PointB = new Vector2(Mathf.Round(HitPoint.x), Mathf.Round(Screen.height - HitPoint.y - CrosshairGap - CrosshairSize));
                TPPUtils.DrawLine(PointA, PointB, color, CrosshairThickness);

                PointA = new Vector2(Mathf.Round(HitPoint.x), Mathf.Round(Screen.height - HitPoint.y + CrosshairGap));
                PointB = new Vector2(Mathf.Round(HitPoint.x), Mathf.Round(Screen.height - HitPoint.y + CrosshairGap + CrosshairSize));
                TPPUtils.DrawLine(PointA, PointB, color, CrosshairThickness);
            }


        }
    }
}
