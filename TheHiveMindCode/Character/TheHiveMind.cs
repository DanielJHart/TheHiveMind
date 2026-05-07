using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using TheHiveMind.TheHiveMindCode.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using TheHiveMind.TheHiveMindCode.Cards;
using TheHiveMind.TheHiveMindCode.Relics;

namespace TheHiveMind.TheHiveMindCode.Character;

public class TheHiveMind : PlaceholderCharacterModel
{
    public const string CharacterId = "TheHiveMind";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 70;
    
    protected override CharacterModel? UnlocksAfterRunAs => (CharacterModel) ModelDb.Character<Defect>();

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeSilent>(),
        ModelDb.Card<StrikeRegent>(),
        ModelDb.Card<StrikeNecrobinder>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendSilent>(),
        ModelDb.Card<DefendRegent>(),
        ModelDb.Card<DefendNecrobinder>(),
        ModelDb.Card<DefendDefect>(),
        ModelDb.Card<PickACard>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<UniversalMind>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<TheHiveMindCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheHiveMindRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheHiveMindPotionPool>();
    
    public override int BaseOrbSlotCount => 2;

    public TheHiveMind()
    {
        foreach (CardPoolModel pool in TheHiveMindHelper.GetTheHivePools())
        {
            foreach (CardModel card in pool.AllCards)
            {
                ModHelper.AddModelToPool(typeof(TheHiveMindCardPool), card.GetType());
            }
        }
    }

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_the_hive_mind.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_the_hive_mind.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_the_hive_mind_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_the_hive_mind.png".CharacterUiPath();
    public override string CustomArmPointingTexturePath => "multiplayer_hand_the_hive_mind_point.png".CharacterHandUiPath();
    public override string CustomArmRockTexturePath => "multiplayer_hand_the_hive_mind_rock.png".CharacterHandUiPath();
    public override string CustomArmPaperTexturePath => "multiplayer_hand_the_hive_mind_paper.png".CharacterHandUiPath();
    public override string CustomArmScissorsTexturePath => "multiplayer_hand_the_hive_mind_scissors.png".CharacterHandUiPath();
}