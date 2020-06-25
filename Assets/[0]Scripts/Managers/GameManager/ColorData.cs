using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorPaletteName
{
    PlayerColor = 0,

    AiColor = 100
}

[Serializable]
public class ColorPalette
{
    public ColorPaletteName name;
    public Color color;
}

[CreateAssetMenu(fileName = "ColorData", menuName = "Data/ColorData")]
public class ColorData : BaseInjectable
{
    public List<ColorPalette> _colorPalettes;

    public Color this[ColorPaletteName name] => _colorPalettes.Find(x => x.name == name).color;

}