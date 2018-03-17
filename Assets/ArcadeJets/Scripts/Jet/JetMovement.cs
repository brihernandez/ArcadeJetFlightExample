using UnityEngine;

namespace ArcadeJets
{
   [RequireComponent(typeof(Rigidbody))]
   public class JetMovement : MonoBehaviour
   {
      public StickInput input;

      [Tooltip("How powerfully the plane can maneuver in each axis.\n\nX: Pitch\nY: Yaw\nZ: Roll")]
      public Vector3 turnTorques = new Vector3(60.0f, 10.0f, 90.0f);
      public float bankTorque = 5.0f;
      public float maxThrust = 3000.0f;
      public float acceleration = 10.0f;
      public float brakeDrag = 5.0f;

      private Rigidbody rigid;

      private float throttleTrue = StickInput.ThrottleNeutral;

      // Heavy things often require big numbers. It's nice to keep this multiplier on the
      // same scale as your mass to keep numbers small and manageable. For example, if your
      // game has mass in the hundreds, then use 100. If thousands, then 1000, etc.
      private const float FORCE_MULT = 100.0f;

      public Rigidbody Rigidbody { get { return rigid; } }
      public StickInput StickInput { get { return input; } }

      private void Awake()
      {
         rigid = GetComponent<Rigidbody>();
      }

      private void Start()
      {
         if (input == null)
            Debug.LogWarning(name + ": JetMovement has no input assigned!");
      }

      private void FixedUpdate()
      {
         // When the throttle goes below neutral, apply increased acceleration to slow down faster.
         float throttleTarget = GetTargetThrottle();
         float brakePower = brakeDrag * Mathf.InverseLerp(StickInput.ThrottleNeutral, StickInput.ThrottleMin, throttleTarget);
         float brakeAccel = brakeDrag * brakePower;

         // Throttle has to move slowly so that the plane still accelerates slowly using high
         // drag physics. Without them, the plane would change speed almost instantly.
         throttleTrue = Mathf.MoveTowards(throttleTrue, throttleTarget, ((acceleration + brakeAccel) / FORCE_MULT) * Time.deltaTime);

         // Apply forces to the plane.
         rigid.AddRelativeForce(Vector3.forward * maxThrust * throttleTrue * FORCE_MULT, ForceMode.Force);
         rigid.AddRelativeTorque(MultiplyByComponent(GetStickInput(), turnTorques) * FORCE_MULT, ForceMode.Force);

         // Apply magic forces when the plane is banked because it feels good. The principle
         // is that the plane rotates in the direction you're banked. The more banked you are
         // (up to a max of 90 degrees) the more it magically turns in that direction.

         // This is a weird vector trick where I use the Y value of the plane's right to
         // determine how banked it is. A value of -1/1 implies the plane is flying sideways
         // It also automatically takes care of cases where the plane is flying straight up
         // or down because in those situations your right would have negligible Y value.
         float bankFactor = -transform.right.y;
         rigid.AddRelativeTorque(Vector3.up * bankFactor * bankTorque * FORCE_MULT, ForceMode.Force);
      }

      private Vector3 GetStickInput()
      {
         if (input != null)
            return input.Combined;
         else
            return Vector3.zero;
      }

      private float GetTargetThrottle()
      {
         if (input != null)
            return input.Throttle;
         else
            return StickInput.ThrottleNeutral;
      }

      private Vector3 MultiplyByComponent(Vector3 a, Vector3 b)
      {
         Vector3 retVec = a;
         retVec.x *= b.x;
         retVec.y *= b.y;
         retVec.z *= b.z;
         return retVec;
      }
   }
}