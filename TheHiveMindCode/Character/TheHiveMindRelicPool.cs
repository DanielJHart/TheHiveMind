using BaseLib.Abstracts;
using TheHiveMind.TheHiveMindCode.Extensions;
using Godot;

namespace TheHiveMind.TheHiveMindCode.Character;

public class TheHiveMindRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheHiveMind.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}