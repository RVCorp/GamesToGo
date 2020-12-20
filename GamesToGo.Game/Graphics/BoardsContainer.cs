using System.Collections.Generic;
using System.Linq;
using GamesToGo.Game.LocalGame.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    public class BoardsContainer : Container
    {
        private FillFlowContainer<BoardContainer> boardContainer;
        private BoardContainer current;

        public List<Board> Boards { get; set; }

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
        }

        public void PopulateBoards()
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

        private class BoardContainer : Container
        {
            public Board Board;
            private ContainedImage contained;

            public BoardContainer(Board Board)
            {
                this.Board = Board;
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
                    contained.OverImageContent.Add(getContainedImageFor(tile).With(c =>
                    {
                        c.Size = tile.Size / contained.ExpectedToRealSizeRatio;
                        c.Position = tile.Position / contained.ExpectedToRealSizeRatio;
                    }));
                }
            }

            private static ContainedImage getContainedImageFor(Tile tile)
            {
                Vector2 size = tile.Size;

                var created = new ContainedImage(true, 0)
                {
                    Texture = tile.Images.First(),
                    ImageSize = size,
                };

                created.Rotation = (int)tile.Orientation * -90;

                return created;
            }
        }
    }
}
