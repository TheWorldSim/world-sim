using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RatioChangeable
{
    short GetRatioAngle ();
    short GetRatioAngleStepSize ();
    void SetRatioAngle (short angle);
    float GetMinRatio ();
    void SetRatio(float ratio);
}

public class TransformsHelper
{
    public static bool Cos(RatioChangeable obj)
    {
        short ratio_angle = obj.GetRatioAngle();
        ratio_angle += obj.GetRatioAngleStepSize();
        if (ratio_angle > 360)
        {
            ratio_angle = 0;
        }
        obj.SetRatioAngle(ratio_angle);

        bool changed = false;
        // Premature optimisation?
        if (ratio_angle % 3 == 0)
        {
            float minRatio = obj.GetMinRatio();
            float ratio = ((Mathf.Cos(ratio_angle * 0.01745f) + 1f) / 2) * (1 - minRatio) + minRatio;
            obj.SetRatio(ratio);
            changed = true;
        }

        return changed;
    }

    short angle = 0;
    short _step_angle = 1;
    float _min_ratio = 0f;
    public float Ratio { get; private set; }

    public TransformsHelper(short step_angle, float min_ratio)
    {
        _step_angle = step_angle;
        _min_ratio = min_ratio;
        Ratio = 1f;
    }

    public bool Cos()
    {
        angle += _step_angle;
        if (angle > 360)
        {
            angle = 0;
        }

        bool changed = false;
        // Premature optimisation?
        if (angle % 3 == 0)
        {
            Ratio = ((Mathf.Cos(angle * 0.01745f) + 1f) / 2) * (1 - _min_ratio) + _min_ratio;
            changed = true;
        }

        return changed;
    }
}
