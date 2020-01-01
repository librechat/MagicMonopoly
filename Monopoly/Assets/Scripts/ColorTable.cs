using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTable
{    
    private const float maxColorLevel = 255.0f;
    
    public static List<Color> PlayerColors = new List<Color>() { 
        new Color(0x5B / maxColorLevel, 0xC0 / maxColorLevel, 0xEB / maxColorLevel),
        new Color(0xFD / maxColorLevel, 0xE7 / maxColorLevel, 0x4C / maxColorLevel),
        new Color(0x9B / maxColorLevel, 0xC5 / maxColorLevel, 0x3D / maxColorLevel),
        new Color(0xFF / maxColorLevel, 0x4A / maxColorLevel, 0x4D / maxColorLevel),
        new Color(0xFA / maxColorLevel, 0x79 / maxColorLevel, 0x21 / maxColorLevel)
    };
}
