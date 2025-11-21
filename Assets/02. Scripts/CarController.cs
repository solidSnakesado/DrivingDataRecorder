using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public  float       maxSteerAngle   = 30f;          // 최대 조향 각도 (도 단위)
    public  float       accleration     = 15f;          // 가속도 (전진 하는 가속도)
    public  float       breakStrength   = 20f;          // 브레이크
    public  float       maxSpeed        = 15f;          // 최대 속도

    private Rigidbody   rb              = null;
    private Vector3     horizontalVel   = Vector3.zero;
    private Vector3     vHorizontal     = Vector3.zero;
    private Vector3     vVertical       = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 입력값 받기
        float   steering    = InputUtility.GetSteering();        // -1 ~ 1 (좌 ~ 우)
        float   throttle    = InputUtility.GetThrottle();        // -1 ~ 1 (전 ~ 후)
        float   isBraking   = InputUtility.GetBrake();           // 브레이크

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed)
            {
                steering -= 1;
            }
        }

        // 회전 처리
        float steeringAngle = steering * maxSteerAngle;         // 회전 각도

        if (steeringAngle != 0)
        {
            transform.Rotate(0f, steeringAngle * Time.fixedDeltaTime, 0f);
        }
        

        // 전진 / 후진 값 계산
        Vector3 forward = transform.forward * throttle * accleration;

        if (forward != Vector3.zero)
        {
            // 현재 속도가 최대 속도보다 작을때만 가속
            horizontalVel.x = rb.linearVelocity.x;
            horizontalVel.z = rb.linearVelocity.z;
            horizontalVel.y = 0f;

            if (horizontalVel.magnitude < maxSpeed)
            {
                rb.AddForce(forward , ForceMode.Acceleration);
            }
        }

        // 브레이크 처리 (속도감소)
        if (isBraking == 1)
        {
            // 수평 속도만 감속
            Vector3 v = rb.linearVelocity;
            vHorizontal.x = v.x;
            vHorizontal.z = v.z;
            vHorizontal.y = 0;

            vVertical.x = 0;
            vVertical.z = 0;
            vVertical.y = v.y;

            vHorizontal = Vector3.MoveTowards(vHorizontal, Vector3.zero, breakStrength * Time.fixedDeltaTime);

            rb.linearVelocity = vHorizontal + vVertical;
        }
    }
}
