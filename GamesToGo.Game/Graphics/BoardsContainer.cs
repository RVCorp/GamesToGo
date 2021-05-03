using System.Collections.Generic;
using System.Linq;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Testing;

namespace GamesToGo.Game.Graphics
{
    [Cached]
    public class BoardsContainer : Container
    {
        private FillFlowContainer<BoardContainer> boardContainer;
        private BoardContainer current;
        

        private List<Board> boards;

        public List<Board> Boards
        {
            get => boards;
            set
            {
                boards = value;

            }
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;
            Child = boardContainer = new FillFlowContainer<BoardContainer>
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
            };
            if (Boards != null || Boards.Count != 0)
                populateBoards();
        }

        private void populateBoards()
        {


            foreach (var board in Boards)
            {
                boardContainer.Add(new BoardContainer(board));
            }
            foreach (var container in boardContainer)
            {
                container.Hide();
            }
            boardContainer.First().Show();
            current = boardContainer.First();
        }

        public void ChangeBoard(int id)
        {
            current.Hide();
            current = boardContainer.First(b => b.Board.TypeID == id);
            current.Show();
        }

        private class BoardContainer : Container
        {
            public readonly Board Board;
            private ContainedImage contained;
            private Container borderContainer;

            [Resolved]
            private Bindable<OnlineRoom> room { get; set; }

            public BoardContainer(Board board)
            {
                Board = board;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                RelativeSizeAxes = Axes.Both;
                Children = new Drawable[] 
                {
                    borderContainer = new Container
                    {
                        Masking = true,
                        CornerRadius = 10,
                        BorderThickness = 4f,
                        BorderColour = Colour4.White,
                        RelativeSizeAxes = Axes.Both,
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Transparent,
                            Alpha = 0.1f,
                        },
                    },
                    contained = new ContainedImage(true, 0)
                    {
                    RelativeSizeAxes = Axes.Both,
                    Texture = Board.Images.FirstOrDefault(),
                    ImageSize = Board.Size
                    }
                };
                contained.OverImageContent.Clear();
                
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                populateTiles();
                room.BindValueChanged(_ => updateTiles(room.Value.Boards.First(b => b.TypeID == Board.TypeID).Tiles));
            }

            private void updateTiles(List<OnlineTile> tiles)
            {
                var currentTiles = this.ChildrenOfType<TileContainer>().ToList();

                foreach (var tile in tiles)
                    currentTiles.First(t => t.Tile.TypeID == tile.TypeID).Model = tile;
            }

            private void populateTiles()
            {
                foreach (var tile in Board.Tiles)
                {
                    contained.OverImageContent.Add(new TileContainer(tile).With(c =>
                    {
                        c.Size = tile.Size / contained.ExpectedToRealSizeRatio;
                        c.Position = tile.Position / contained.ExpectedToRealSizeRatio;
                    }));
                }
            }
        }
    }
}
