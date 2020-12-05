using System.Linq;
using GamesToGo.Editor.Graphics;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Project.Actions;
using GamesToGo.Editor.Project.Arguments;
using GamesToGo.Editor.Project.Elements;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace GamesToGo.Tests.Visual
{
    public class TestSceneArgumentDescriptor : TestScene
    {
        [Cached]
        private WorkingProject workingProject;

        [Cached]
        private ArgumentSelectorOverlay selectorOverlay;
        private FillFlowContainer fillFlow;
        private EventAction tileAction;
        private EventAction tokenAction;
        private EventAction numberAction;
        private EventAction privacyAction;

        [Resolved]
        private Storage store { get; set; }

        public TestSceneArgumentDescriptor()
        {
            workingProject = WorkingProject.Parse(null, null, null, null);
            selectorOverlay = new ArgumentSelectorOverlay();
            var tile1 = new Tile();
            tile1.Name.Value = "Tile 1";
            workingProject.AddElement(tile1);
            var card1 = new Card();
            card1.Name.Value = "Card 1";
            workingProject.AddElement(card1);
            var board1 = new Board();
            board1.Name.Value = "Board 1";
            workingProject.AddElement(board1);
            var token1 = new Token();
            token1.Name.Value = "Token 1";
            workingProject.AddElement(token1);
            var board2 = new Board();
            board2.Name.Value = "Board 2";
            workingProject.AddElement(board2);
            var token2 = new Token();
            token2.Name.Value = "Token 2";
            workingProject.AddElement(token2);
            var card2 = new Card();
            card2.Name.Value = "Card 2";
            workingProject.AddElement(card2);
            var tile2 = new Tile();
            tile2.Name.Value = "Tile 2";
            workingProject.AddElement(tile2);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = new Colour4(106, 100, 104, 255),
            });
            Add(fillFlow = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
            Add(selectorOverlay);

            tileAction = addCardToTile();
            fillFlow.Add(new ActionDescriptor(tileAction));
            tokenAction = playerWithTokensWins();
            fillFlow.Add(new ActionDescriptor(tokenAction));
            numberAction = delayGameBySeconds();
            fillFlow.Add(new ActionDescriptor(numberAction));
            privacyAction = changeTokenPrivacy();
            fillFlow.Add(new ActionDescriptor(privacyAction));

        }

        [TestCase]
        public void TestToken()
        {
            AddStep("Set token result as null", () =>
                ((IHasResult)tokenAction.Arguments[0].Value.Arguments[0].Value).Result.Value = null);

            AddStep("Set token result as Token 1",
                () => ((IHasResult)tokenAction.Arguments[0].Value.Arguments[0].Value).Result.Value = (int?)workingProject.ProjectTokens.First().ID);

            AddStep("Set token result as null", () =>
                ((IHasResult)tokenAction.Arguments[0].Value.Arguments[0].Value).Result.Value = null);

            AddStep("Set token result as Token 2",
                () => ((IHasResult)tokenAction.Arguments[0].Value.Arguments[0].Value).Result.Value = (int?)workingProject.ProjectTokens.Skip(1).First().ID);
        }

        [TestCase]
        public void TestCard()
        {
            AddStep("Set tile result as null", () =>
                ((IHasResult)tileAction.Arguments[0].Value).Result.Value = null);

            AddStep("Set tile result as Card 1",
                () => ((IHasResult)tileAction.Arguments[0].Value).Result.Value = (int?)workingProject.ProjectCards.First().ID);

            AddStep("Set tile result as null", () =>
                ((IHasResult)tileAction.Arguments[0].Value).Result.Value = null);

            AddStep("Set tile result as Card 2",
                () => ((IHasResult)tileAction.Arguments[0].Value).Result.Value = (int?)workingProject.ProjectCards.Skip(1).First().ID);
        }

        [TestCase]
        public void TestTile()
        {
            AddStep("Set tile result as null", () =>
                ((IHasResult)tileAction.Arguments[1].Value).Result.Value = null);

            AddStep("Set tile result as Tile 1",
                () => ((IHasResult)tileAction.Arguments[1].Value).Result.Value = (int?)workingProject.ProjectTiles.First().ID);

            AddStep("Set tile result as null", () =>
                ((IHasResult)tileAction.Arguments[1].Value).Result.Value = null);

            AddStep("Set tile result as Tile 2",
                () => ((IHasResult)tileAction.Arguments[1].Value).Result.Value = (int?)workingProject.ProjectTiles.Skip(1).First().ID);
        }

        [TestCase]
        public void TestNumber()
        {
            AddStep("Set number result as 1", () =>
                ((IHasResult)numberAction.Arguments[0].Value).Result.Value = (int?)1);

            AddStep("Set number result as 10", () =>
                ((IHasResult)numberAction.Arguments[0].Value).Result.Value = (int?)10);

            AddStep("Set number result as 99999", () =>
                ((IHasResult)numberAction.Arguments[0].Value).Result.Value = (int?)99999);

            AddStep("Set number result as 5", () =>
                ((IHasResult)numberAction.Arguments[0].Value).Result.Value = (int?)5);
        }

        [TestCase]
        public void TestPrivacy()
        {

        }

        private EventAction changeTokenPrivacy()
        {
            EventAction action = new ChangeTokenPrivacyAction();
            action.Arguments[1].Value = new PrivacyTypeArgument();

            return action;
        }

        private EventAction playerWithTokensWins()
        {
            EventAction action = new PlayerWinsAction();
            action.Arguments[0].Value = new PlayerWithTokenArgument();
            action.Arguments[0].Value.Arguments[0].Value = new TokenTypeArgument();

            return action;
        }

        private EventAction addCardToTile()
        {
            EventAction action = new AddCardToTileAction();
            action.Arguments[0].Value = new CardTypeArgument();
            action.Arguments[1].Value = new TileTypeArgument();

            return action;
        }

        private EventAction delayGameBySeconds()
        {
            EventAction action = new DelayGameAction();
            action.Arguments[0].Value = new NumberArgument();

            return action;
        }
    }
}
