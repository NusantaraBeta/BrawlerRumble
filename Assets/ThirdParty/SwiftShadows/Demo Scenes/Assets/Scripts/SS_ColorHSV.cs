﻿using System;
using UnityEngine;

/**
* A color in HSV space
*/

internal class SS_ColorHSV {
    private readonly float _a;
    private readonly float _h;
    private readonly float _s;
    private readonly float _v;

    /**
    * Construct without alpha (which defaults to 1)
    */

    public SS_ColorHSV(float h, float s, float v) {
        _h = h;
        _s = s;
        _v = v;
        _a = 1.0f;
    }

    /**
    * Construct with alpha
    */

    public SS_ColorHSV(float h, float s, float v, float a) {
        _h = h;
        _s = s;
        _v = v;
        _a = a;
    }

    /**
    * Create from an RGBA color object
    */

    public SS_ColorHSV(Color color) {
        float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);
        float max = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
        float delta = max - min;

        // value is our max color
        _v = max;

        // saturation is percent of max
        if (!Mathf.Approximately(max, 0)) {
            _s = delta / max;
        } else {
            // all colors are zero, no saturation and hue is undefined
            _s = 0;
            _h = -1;
            return;
        }

        // grayscale image if min and max are the same
        if (Mathf.Approximately(min, max)) {
            _v = max;
            _s = 0;
            _h = -1;
            return;
        }

        // hue depends which color is max (this creates a rainbow effect)
        // ReSharper disable CompareOfFloatsByEqualityOperator
        if (color.r == max) {
            _h = (color.g - color.b) / delta; // between yellow & magenta
        } else if (color.g == max) {
            _h = 2 + (color.b - color.r) / delta; // between cyan & yellow
        } else {
            _h = 4 + (color.r - color.g) / delta; // between magenta & cyan
        }
        // ReSharper restore CompareOfFloatsByEqualityOperator

        // turn hue into 0-360 degrees
        _h *= 60;
        if (_h < 0) {
            _h += 360;
        }
    }

    /**
    * Return an RGBA color object
    */

    public Color ToColor() {
        // no saturation, we can return the value across the board (grayscale)
        if (_s == 0) {
            return new Color(_v, _v, _v, _a);
        }

        // which chunk of the rainbow are we in?
        float sector = _h / 60;

        // split across the decimal (ie 3.87 into 3 and 0.87)
        var i = (int) Mathf.Floor(sector);
        float f = sector - i;

        float v = _v;
        float p = v * (1 - _s);
        float q = v * (1 - _s * f);
        float t = v * (1 - _s * (1 - f));

        // build our rgb color
        var color = new Color(0, 0, 0, _a);

        switch (i) {
            case 0:
                color.r = v;
                color.g = t;
                color.b = p;
                break;
            case 1:
                color.r = q;
                color.g = v;
                color.b = p;
                break;
            case 2:
                color.r = p;
                color.g = v;
                color.b = t;
                break;
            case 3:
                color.r = p;
                color.g = q;
                color.b = v;
                break;
            case 4:
                color.r = t;
                color.g = p;
                color.b = v;
                break;
            default:
                color.r = v;
                color.g = p;
                color.b = q;
                break;
        }

        return color;
    }

    /**
    * Format nicely
    */

    public override String ToString() {
        return String.Format("h: {0:0.00}, s: {1:0.00}, v: {2:0.00}, a: {3:0.00}", _h, _s, _v, _a);
    }

}