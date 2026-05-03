using BaseLib.Abstracts;
using BaseLib.Extensions;
using TheHiveMind.TheHiveMindCode.Extensions;
using Godot;

namespace TheHiveMind.TheHiveMindCode.Powers;

public abstract class TheHiveMindPower : CustomPowerModel
{
    //Loads from TheHiveMind/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}