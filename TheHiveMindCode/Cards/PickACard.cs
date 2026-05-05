using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheHiveMind.TheHiveMindCode.Cards;
using TheHiveMind.TheHiveMindCode.Extensions;

namespace TheHiveMind.TheHiveMindCode.Cards;

public class PickACard() : TheHiveMindCard(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    private CardModel? _mockSelectedCard;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        PickACard pickACard = this;
        CardModel? card;
        if (pickACard._mockSelectedCard == null)
            card = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) TheHiveMindHelper.GetThreeHiveMindCards(pickACard.Owner), pickACard.Owner, true);
        else
            card = pickACard._mockSelectedCard;
        if (card == null)
            return;
        card.SetToFreeThisTurn();
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, true);
    }

    protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Exhaust);

    public void MockSelectedCard(CardModel card)
    {
        this.AssertMutable();
        this._mockSelectedCard = card;
    }
}