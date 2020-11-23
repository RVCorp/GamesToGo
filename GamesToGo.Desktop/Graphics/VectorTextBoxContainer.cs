using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class VectorTextBoxContainer : Container, IHasCurrentValue<Vector2>
    {
        private LabeledElement<NumericTextBox, float> textX;

        private string stringX;

        public string TextX
        {
            get => stringX;
            set
            {
                stringX = value;

                if (textX != null)
                    textX.Text = value;
            }
        }

        private LabeledElement<NumericTextBox, float> textY;
        private string stringY;

        public string TextY
        {
            get => textY?.Text;
            set
            {
                stringY = value;

                if (textY != null)
                    textY.Text = value;
            }
        }

        private readonly int textLength;
        private readonly bool zeroAllowed;

        public Bindable<Vector2> Current { get; set; } = new Bindable<Vector2>();

        public VectorTextBoxContainer(int length, bool allowZero)
        {
            textLength = length;
            zeroAllowed = allowZero;

            Current.BindValueChanged(vectorChange =>
            {
                validateData(vectorChange.NewValue);
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;

            Child = new GridContainer
            {
                AutoSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                    new Dimension(GridSizeMode.Absolute, 15),
                    new Dimension(GridSizeMode.AutoSize),
                },
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        textX = new LabeledElement<NumericTextBox, float>
                        {
                            Text = stringX,
                            Element = new NumericTextBox(zeroAllowed)
                            {
                                LengthLimit = textLength,
                                Size = new Vector2(75, 35),
                            },
                        },
                    },
                    new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                    },
                    new Drawable[]
                    {
                        textY = new LabeledElement<NumericTextBox, float>
                        {
                            Text = stringY,
                            Element = new NumericTextBox(zeroAllowed)
                            {
                                LengthLimit = textLength,
                                Size = new Vector2(75, 35),
                            },
                        },
                    },
                },
            };

            textX.Current.BindValueChanged(vectorChange => validateData(new Vector2(vectorChange.NewValue, Current.Value.Y)));
            textY.Current.BindValueChanged(vectorChange => validateData(new Vector2(Current.Value.X, vectorChange.NewValue)));
        }

        private void validateData(Vector2 newValue)
        {
            Vector2 realChange = new Vector2(newValue.X, newValue.Y);

            if (newValue.X == 0 && !zeroAllowed)
                realChange.X = Current.Value.X;

            if (newValue.Y == 0 && !zeroAllowed)
                realChange.Y = Current.Value.Y;

            Current.Value = realChange;

            textX.Current.Value = realChange.X;
            textY.Current.Value = realChange.Y;
        }
    }
}
