using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace TheHiveMind.TheHiveMindCode.Extensions;

public class TheHiveMindHelper
{
    public static List<CardPoolModel> GetTheHivePools()
    {
        return [ModelDb.CardPool<IroncladCardPool>(), ModelDb.CardPool<SilentCardPool>(), ModelDb.CardPool<RegentCardPool>(), ModelDb.CardPool<NecrobinderCardPool>(), ModelDb.CardPool<DefectCardPool>()];
    }

    public static List<CardModel> GetAllHiveCards(Player player, bool includeColorless = false)
    {
        List<CardModel> cards = new List<CardModel>();
        
        foreach (CardPoolModel cardPool in GetTheHivePools())
        {
            cards.AddRange(cardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).ToList());
        }

        if (includeColorless)
        {
            cards.AddRange(ModelDb.CardPool<ColorlessCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).ToList());
        }

        return cards;
    }

    private static CardModel? GetCardFromPool(CardPoolModel cardPool, Player player, CardRarityOddsType odds)
    {
        return CardFactory.CreateForReward(player,
            1, 
            new CardCreationOptions([cardPool], CardCreationSource.Other, odds)
                .WithFlags(CardCreationFlags.NoModifyHooks))
            .FirstOrDefault<CardCreationResult>()?
            .Card;
    }

    private static void AddCardToRewardsFromPool(CardPoolModel cardPool, Player player, CardRarityOddsType odds, ref List<CardModel> cards)
    {
        CardModel? card = GetCardFromPool(cardPool, player, odds);
        if (card != null)
        {
            cards.Add(card);
        }
    }
    
    public static List<CardModel> GetTheHiveRewards(Player player, CardRarityOddsType odds)
    {
        List<CardModel> cards = new List<CardModel>();

        foreach (CardPoolModel cardPool in GetTheHivePools())
        {
            AddCardToRewardsFromPool(cardPool, player, odds, ref cards);
        }
        
        return cards;
    }

    public static List<CardModel> GetFiveHiveMindCards(Player player)
    {
        // returns one card of each character
        List<CardModel> cards = new List<CardModel>();
        foreach (CardPoolModel cardPool in GetTheHivePools())
        {
            cards.Add(CardFactory.GetDistinctForCombat(player, cardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).ToList(), 1, player.RunState.Rng.CombatCardGeneration).First());
        }
        
        return cards;
    }

    public static List<CardModel> GetThreeHiveMindCards(Player player)
    {
        List<CardModel> result = GetFiveHiveMindCards(player).TakeRandom(3, player.RunState.Rng.CombatCardGeneration).ToList();
        return result;
    }

    public static List<CardModel> GetTheHiveShopCards(Player player)
    {
        List<CardModel> cards = new List<CardModel>();
        foreach (CardPoolModel cardPool in GetTheHivePools())
        {
            cards.Add(CardFactory.GetDistinctForCombat(player, cardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).ToList(), 1, player.RunState.Rng.CombatCardGeneration).First());
        }
        
        return cards;
    }
}