using BaseLib.Abstracts;
using BaseLib.Utils;
using TheHiveMind.TheHiveMindCode.Character;

namespace TheHiveMind.TheHiveMindCode.Potions;

[Pool(typeof(TheHiveMindPotionPool))]
public abstract class TheHiveMindPotion : CustomPotionModel;