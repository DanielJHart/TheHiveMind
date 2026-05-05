using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
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
    public override RelicRarity Rarity => RelicRarity.Starter;

    private List<CardPoolModel> pools = TheHiveMindHelper.GetTheHivePools();

    public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        if (player == this.Owner)
        {
            // We go through regular cards first.
            if (options.First().Pool.GetType() != typeof(ColorlessCardPool))
            {
                CardPoolModel pool = pools.TakeRandom(1, player.RunState.Rng.Shuffle).First();
                pools.Remove(pool);
            
                if (pools.Count == 0)
                {
                    pools = TheHiveMindHelper.GetTheHivePools();
                }
            
                return pool.GetUnlockedCards(player.UnlockState,  player.RunState.CardMultiplayerConstraint);
            }
            else
            {
                // Then we go through colorless.
            
                return base.ModifyMerchantCardPool(player, options);
            }   
        }
        
        return base.ModifyMerchantCardPool(player, options);
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