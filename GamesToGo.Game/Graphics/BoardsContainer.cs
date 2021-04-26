using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    [Cached]
    public class BoardsContainer : Container
    {
        private FillFlowContainer<BoardContainer> boardContainer;
        private BoardContainer current;
        private readonly Bindable<Tile> currentSelectedTile = new Bindable<Tile>();
        public IBindable<Tile> CurrentSelectedTile => currentSelectedTile;



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
            if(Boards != null)
                populateBoards();
        }

        private void populateBoards()
        {
            if (Boards.First().Size.X > Boards.First().Size.Y)
                boardContainer.Height = .75f;
            else
                boardContainer.Width = .75f;
            foreach (var board in Boards)
            {
                boardContainer.Add(new BoardContainer(board));
            }
            foreach(var container in boardContainer)
            {
                container.Hide();
            }
            boardContainer.First().Show();
            current = boardContainer.First();
        }

        public void ChangeBoard(int id)
        {
            current.Hide();
            current = boardContainer.First(b => b.Board.ID == id);
            current.Show();
        }

        public void SelectTile(Tile tile)
        {
            if (currentSelectedTile.Value == tile)
                currentSelectedTile.Value = null;
            else
                currentSelectedTile.Value = tile;
        }

        private class BoardContainer : Container
        {
            public readonly Board Board;
            private ContainedImage contained;

            public BoardContainer(Board board)
            {
                Board = board;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                RelativeSizeAxes = Axes.Both;
                Child = contained = new ContainedImage(true, 0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Texture = Board.Images.First(),
                    ImageSize = Board.Size
                };
                contained.OverImageContent.Clear();
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();
                populateTiles();
            }

            private void populateTiles()
            {
                foreach(var tile in Board.Tiles)
                {
                    contained.OverImageContent.Add(new TileContainer(tile, Board.ID).With(c =>
                    {
                        c.Size = tile.Size / contained.ExpectedToRealSizeRatio;
                        c.Position = tile.Position / contained.ExpectedToRealSizeRatio;
                    }));
                }
            }        
        }
    }
}
