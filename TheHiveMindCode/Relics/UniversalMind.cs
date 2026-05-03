using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using TheHiveMind.TheHiveMindCode.Character;
using TheHiveMind.TheHiveMindCode.Extensions;

namespace TheHiveMind.TheHiveMindCode.Relics;

[Pool(typeof(TheHiveMindRelicPool))]
public class UniversalMind() : TheHiveMindRelic
{
    private int _merchantCardCount = 0;
    
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        _merchantCardCount += 1;

        switch (_merchantCardCount)
        {
            case 1:
                return ModelDb.CardPool<IroncladCardPool>().GetUnlockedCards(player.UnlockState,  player.RunState.CardMultiplayerConstraint);
            case 2:
                return ModelDb.CardPool<SilentCardPool>().GetUnlockedCards(player.UnlockState,  player.RunState.CardMultiplayerConstraint);
            case 3:
                return ModelDb.CardPool<RegentCardPool>().GetUnlockedCards(player.UnlockState,  player.RunState.CardMultiplayerConstraint);
            case 4:
                return ModelDb.CardPool<NecrobinderCardPool>().GetUnlockedCards(player.UnlockState,  player.RunState.CardMultiplayerConstraint);
            case 5:
                return ModelDb.CardPool<DefectCardPool>().GetUnlockedCards(player.UnlockState,  player.RunState.CardMultiplayerConstraint);
            case 6:
                return base.ModifyMerchantCardPool(player, options);
            case 7:
                _merchantCardCount = 0;
                return base.ModifyMerchantCardPool(player, options);
            default:
                return base.ModifyMerchantCardPool(player, options);
        }
    }
    
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != this.Owner || room == null)
            return false;

        switch (room.RoomType)
        {
            case RoomType.Monster:
            case RoomType.Elite:
            case RoomType.Boss:
                // Remove current card award.
                CardReward? cardReward = rewards.First(x => x is CardReward ) as CardReward;

                if (cardReward != null)
                {
                    rewards.Remove(cardReward);
                }

                CardRarityOddsType odds = CardRarityOddsType.RegularEncounter;

                if (room.RoomType == RoomType.Elite)
                {
                    odds = CardRarityOddsType.EliteEncounter;
                }
                else if (room.RoomType == RoomType.Boss)
                {
                    odds = CardRarityOddsType.BossEncounter;
                }
                
                List<CardModel> cards = TheHiveMindHelper.GetTheHiveRewards(player, odds);
                rewards.Add((Reward)new CardReward(cards, CardCreationSource.Other, player));
                return true;
            default:
                base.TryModifyRewards(player, rewards, room);
                break;
        }

        return true;
    }
}