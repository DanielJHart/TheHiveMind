using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
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

    private List<CardPoolModel> _pools = TheHiveMindHelper.GetTheHivePools();

    public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        if (player == this.Owner)
        {
            // We go through regular cards first.
            if (options.First().Pool.GetType() != typeof(ColorlessCardPool))
            {
                CardPoolModel pool = _pools.TakeRandom(1, player.RunState.Rng.Shuffle).First();
                _pools.Remove(pool);
            
                if (_pools.Count == 0)
                {
                    _pools = TheHiveMindHelper.GetTheHivePools();
                }
            
                return pool.GetUnlockedCards(player.UnlockState,  player.RunState.CardMultiplayerConstraint);
            }
        }
        
        return base.ModifyMerchantCardPool(player, options);
    }

    public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> cardRewardOptions, CardCreationOptions creationOptions)
    {
        if (player != this.Owner)
            return false;
        
        cardRewardOptions.Clear();
        List<CardModel> cards = TheHiveMindHelper.GetTheHiveRewards(player, creationOptions.RarityOdds);

        foreach (CardModel card in cards)
        {
            CardCreationResult cardCreationResult = new CardCreationResult(card);
            cardCreationResult.ModifyCard(card, this);
            cardRewardOptions.Add(cardCreationResult);
        }
        
        return true;
    }
}