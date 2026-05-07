using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using TheHiveMind.TheHiveMindCode.Character;
using TheHiveMind.TheHiveMindCode.Relics;

namespace TheHiveMind.TheHiveMindCode.Relics;

[Pool(typeof(TheHiveMindRelicPool))]
public class UniversalConscious() : UniversalMind
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override bool TryModifyRewardsLate(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != this.Owner)
            return false;
        foreach (CardReward cardReward in rewards.OfType<CardReward>())
            cardReward.CanReroll = true;
        return true;
    }
}