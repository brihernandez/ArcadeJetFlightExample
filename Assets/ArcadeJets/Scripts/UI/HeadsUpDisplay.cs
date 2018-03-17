using UnityEngine;
using UnityEngine.UI;

namespace ArcadeJets
{
   public class HeadsUpDisplay : MonoBehaviour
   {
      public Rigidbody plane;

      public Image fpm;
      public Image cross;
      public Text speed;
      public Text altitude;
      public Text throttle;

      const float kProjectionDistance = 500.0f;

      // Use this for initialization
      void Start()
      {
         if (plane == null)
            Debug.LogWarning(name + ": HeadsUpDisplay has no reference plane to pull information form!");
         if (fpm == null)
            Debug.LogWarning(name + ": HeadsUpDisplay has no flight path marker to position!");
         if (cross == null)
            Debug.LogWarning(name + ": HeadsUpDisplay has no cross to position!");
         if (speed == null)
            Debug.LogWarning(name + ": HeadsUpDisplay has no speed text to write to!");
         if (altitude == null)
            Debug.LogWarning(name + ": HeadsUpDisplay has no altitude text to write to!");
         if (throttle == null)
            Debug.LogWarning(name + ": HeadsUpDisplay has no throttle text to write to!");
      }

      // Update is called once per frame
      void Update()
      {
         if (plane != null)
         {
            Vector3 pos = Vector3.zero;

            if (cross != null)
            {
               // Put the cross some meters in front of the plane. This way the cross and FPM line up
               // correctly when there is zero angle of attack.
               pos = Camera.main.WorldToScreenPoint(plane.transform.position + (plane.transform.forward.normalized * kProjectionDistance));
               //pos.z = 0.0f;
               cross.transform.position = pos;
            }

            if (fpm != null)
            {
               // Put the cross some meters in front of the plane. This way the cross and FPM line up
               // correctly when there is zero angle of attack.
               pos = Camera.main.WorldToScreenPoint(plane.transform.position + (plane.velocity.normalized * kProjectionDistance));
               //pos.z = 0.0f;
               fpm.transform.position = pos;
            }

            if (speed != null)
            {
               // Speed is measured in m/s.
               speed.text = plane.velocity.magnitude.ToString("0");
            }

            if (altitude != null)
            {
               // Altitude is just how high above the origin they are.
               altitude.text = plane.transform.position.y.ToString("0");
            }

            if (throttle != null)
            {
               // Don't do this in a real game please.
               StickInput stick = plane.GetComponent<StickInput>();
               if (stick != null)
                  throttle.text = (stick.Throttle * 100f).ToString("000");
               else
                  throttle.text = StickInput.ThrottleNeutral.ToString("000");
            }
         }
      }
   }
}
