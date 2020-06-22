using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_Mode
{
    Rotation,
    Scale,
    Move
}

public class UI_Tweens : MonoBehaviour
{
    public Enum_Mode mode = Enum_Mode.Move;

    [Header("시작 포지션")]
    public Vector3 Start_pos;

    [Header("끝 포지션")]
    public Vector3 End_pos;

    [Header("속도")]
    public float turnSpeed = 10;

    [Header("스케일 크기")]
    public float Scale_Val = 0.0f;

    [Header("시작 딜레이")]
    public float delay = 0.0f;

    [Header("루프 여부")]
    public bool Loop = false;

    private void Start()
    {
        switch (mode)
        {
            case Enum_Mode.Rotation:

                if (Loop)
                {
                    LeanTween.rotate(GetComponent<RectTransform>(), -360.0f, turnSpeed).setLoopClamp();

                }
                else
                {
                    LeanTween.rotate(GetComponent<RectTransform>(), -360.0f, turnSpeed);
                }
                break;

            case Enum_Mode.Scale:
             
                if (Loop)
                {
                    LeanTween.scale(GetComponent<RectTransform>(), Vector3.one * Scale_Val, turnSpeed)
                                     .setLoopPingPong();
                }
                else
                {
                    LeanTween.scale(GetComponent<RectTransform>(), Vector3.one * Scale_Val, turnSpeed);
                }

                break;
            case Enum_Mode.Move:

                transform.localPosition = Start_pos;

                StartCoroutine("Co_Move");
                break;
            default:
                break;
        }
    }

    IEnumerator Co_Move()
    {
        yield return new WaitForSeconds(delay);

        if (Loop)
        {
            LeanTween.move(GetComponent<RectTransform>(), End_pos, turnSpeed)
                             .setLoopClamp();
        }
        else
        {
            LeanTween.move(GetComponent<RectTransform>(), Vector3.one * Scale_Val, turnSpeed);
        }
    }
}
