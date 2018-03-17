using UnityEngine;

namespace ArcadeJets
{
   public class StickInput : MonoBehaviour
   {
      [Tooltip("When true, pulls input from the player.")]
      public bool isPlayer = false;

      [SerializeField]
      private Vector3 stickInput;
      [SerializeField]
      private float throttle;

      public float Pitch
      {
         get { return stickInput.x; }
         set { stickInput.x = value; }
      }
      public float Roll
      {
         get { return stickInput.z; }
         set { stickInput.z = value; }
      }
      public float Yaw
      {
         get { return stickInput.y; }
         set { stickInput.y = value; }
      }
      /// <summary>
      /// Pitch, Yaw, and Roll represented as a Vector3, in that order.
      /// </summary>
      public Vector3 Combined
      {
         get { return stickInput; }
         set { stickInput = value; }
      }

      [SerializeField]
      public float Throttle
      {
         get { return throttle; }
         set { throttle = value; }
      }

      // Both of these are here only in case this package is imported as an asset.
      // These controls must be defined manually in the input manager upon import.
      private bool yawDefined = false;
      private bool throttleDefined = false;

      public const float ThrottleNeutral = 0.33f;
      public const float ThrottleMin = 0.1f;
      public const float ThrottleMax = 1f;
      public const float ThrottleSpeed = 2f;

      private void Awake()
      {
         try
         {
            Input.GetAxis("Yaw");
            yawDefined = true;
         }
         catch (System.ArgumentException e)
         {
            Debug.LogWarning(e);
            Debug.LogWarning(name + ": \"Yaw\" axis not defined in Input Manager. Rudder will not work!");
         }

         try
         {
            Input.GetButton("ThrottleUp");
            Input.GetButton("ThrottleDown");
            throttleDefined = true;
         }
         catch (System.ArgumentException e)
         {
            Debug.LogWarning(e);
            Debug.LogWarning(name + ": \"ThrottleUp\" or \"ThrottleDown\" buttons not defined in Input Manager. Throttle control will not work!");
         }
      }
      
      private void Update()
      {
         if (isPlayer)
         {
            Pitch = Input.GetAxis("Vertical");            
            Roll = -Input.GetAxis("Horizontal");

            // If this project is imported 
            if (yawDefined)
               Yaw = Input.GetAxis("Yaw");

            // Throttle works using buttons rather than an axis. When the throttle up is held,
            // the throttle moves towards to a max value. When throttle down is held, it goes
            // to an idle setting. When nothing is pressed, it returns to a neutral value.
            if (throttleDefined)
            {
               float target = ThrottleNeutral;
               if (Input.GetButton("ThrottleUp"))
                  target = ThrottleMax;
               else if (Input.GetButton("ThrottleDown"))
                  target = ThrottleMin;

               throttle = Mathf.MoveTowards(throttle, target, ThrottleSpeed * Time.deltaTime);
            }
         }
      }
   }
}