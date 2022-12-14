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
                if (!Physics.Raycast(r, out RaycastHit Hit, float.MaxValue, GClass2400.HitMask))
                {
                    return;
                }
                Vector3 HitPoint = Camera.main.WorldToScreenPoint(Hit.point);
                if (HitPoint.z <= 0.01f)
                {
                    return;
                }

                Vector2 PointA = new Vector2(Mathf.Round(HitPoint.x - TPPEntry.Instance.CrosshairGap.Value), Mathf.Round(Screen.height - HitPoint.y));
                Vector2 PointB = new Vector2(Mathf.Round(HitPoint.x - TPPEntry.Instance.CrosshairGap.Value - TPPEntry.Instance.CrosshairSize.Value), Mathf.Round(Screen.height - HitPoint.y));
                TPPUtils.DrawLine(PointA, PointB, TPPEntry.Instance.Crosshaircolor.Value, TPPEntry.Instance.CrosshairThickness.Value);

                PointA = new Vector2(Mathf.Round(HitPoint.x + TPPEntry.Instance.CrosshairGap.Value), Mathf.Round(Screen.height - HitPoint.y));
                PointB = new Vector2(Mathf.Round(HitPoint.x + TPPEntry.Instance.CrosshairGap.Value + TPPEntry.Instance.CrosshairSize.Value), Mathf.Round(Screen.height - HitPoint.y));
                TPPUtils.DrawLine(PointA, PointB, TPPEntry.Instance.Crosshaircolor.Value, TPPEntry.Instance.CrosshairThickness.Value);

                PointA = new Vector2(Mathf.Round(HitPoint.x), Mathf.Round(Screen.height - HitPoint.y - TPPEntry.Instance.CrosshairGap.Value));
                PointB = new Vector2(Mathf.Round(HitPoint.x), Mathf.Round(Screen.height - HitPoint.y - TPPEntry.Instance.CrosshairGap.Value - TPPEntry.Instance.CrosshairSize.Value));
                TPPUtils.DrawLine(PointA, PointB, TPPEntry.Instance.Crosshaircolor.Value, TPPEntry.Instance.CrosshairThickness.Value);

                PointA = new Vector2(Mathf.Round(HitPoint.x), Mathf.Round(Screen.height - HitPoint.y + TPPEntry.Instance.CrosshairGap.Value));
                PointB = new Vector2(Mathf.Round(HitPoint.x), Mathf.Round(Screen.height - HitPoint.y + TPPEntry.Instance.CrosshairGap.Value + TPPEntry.Instance.CrosshairSize.Value));
                TPPUtils.DrawLine(PointA, PointB, TPPEntry.Instance.Crosshaircolor.Value, TPPEntry.Instance.CrosshairThickness.Value);
            }


        }
    }
}
