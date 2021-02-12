using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GamesToGo.Common.Game;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Common.Graphics
{
    [Cached]
    public class TagSelectionContainer : Container, IHasCurrentValue<Tag>
    {
        public float TagSize => elementSize -5;
        private const double transform_duration = 125;
        private const Easing transform_easing = Easing.OutCubic;
        private readonly float elementSize;

        private FillFlowContainer<TagDropdown> flowContainer;

        private readonly BindableWithCurrent<Tag> current = new BindableWithCurrent<Tag>();
        private BasicScrollContainer scrollContainer;

        public override bool PropagatePositionalInputSubTree => !current.Disabled && base.PropagatePositionalInputSubTree;

        public Bindable<Tag> Current
        {
            get => current.Current;
            set => current.Current = value;
        }
        public TagSelectionContainer(float size)
        {
            elementSize = size;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Current = new Bindable<Tag>();

            Children = new Drawable[]
            {
                new Box
                {
                    Colour = FrameworkColour.BlueGreenDark,
                    RelativeSizeAxes = Axes.X,
                    Height = elementSize,
                },
                scrollContainer = new BasicScrollContainer(Direction.Horizontal)
                {
                    Name = "Scroll Tags",
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    ScrollbarVisible = false,
                    Masking = true,
                    Child = flowContainer = new FillFlowContainer<TagDropdown>
                    {
                        AutoSizeAxes = Axes.Both,
                        Padding = new MarginPadding((elementSize - TagSize) / 2),
                        Spacing = new Vector2((elementSize - TagSize) / 2),
                        Direction = FillDirection.Horizontal,
                    },
                },
            };

            scrollContainer.ScrollContent.RelativeSizeAxes = Axes.None;
            scrollContainer.ScrollContent.AutoSizeAxes = Axes.Both;

            var tags = Enum.GetValues(typeof(Tag)).Cast<Tag>();
            var organizedTags = new Dictionary<TagCategory, List<Tag>>();
            var notOrganizedTags = new List<Tag>();

            foreach (var tag in tags)
            {
                var tagCategory = tag.GetCategory();

                switch (tagCategory)
                {
                    case TagCategory.None:
                        notOrganizedTags.Add(tag);

                        break;
                    default:
                        if (!organizedTags.ContainsKey(tagCategory))
                            organizedTags.Add(tagCategory, new List<Tag>());
                        organizedTags[tagCategory].Add(tag);

                        break;
                }
            }

            foreach (var tagDropdown in organizedTags.Select(tagGroup => new TagDropdown(tagGroup.Key, tagGroup.Value)))
                prepareAndAddDropdown(tagDropdown);

            if (notOrganizedTags.Any())
                prepareAndAddDropdown(new TagDropdown(TagCategory.None, notOrganizedTags));


            current.BindValueChanged(selectWithValue);
            current.BindDisabledChanged(changeMenuEnabling, true);
        }

        private void prepareAndAddDropdown(TagDropdown tagDropdown)
        {
            tagDropdown.Current.BindValueChanged(_ =>
            {
                 current.Value = (current.Value & ~tagDropdown.AffectedTags) | tagDropdown.Current.Value;
            });
            tagDropdown.Visible.BindValueChanged(visible =>
            {
                if (!visible.NewValue)
                    return;
                foreach (var dropdown in flowContainer.Children)
                    if (dropdown.Visible.Value && dropdown != tagDropdown)
                        dropdown.Visible.Value = false;
            });
            flowContainer.Add(tagDropdown);
        }

        private void selectWithValue(ValueChangedEvent<Tag> values)
        {
            foreach (var dropdown in flowContainer)
            {
                dropdown.Current.Value = dropdown.AffectedTags & values.NewValue;
            }
        }

        private void changeMenuEnabling(bool disabled)
        {
            scrollContainer.FadeColour(disabled ? new Colour4(150, 150, 150, 255) : Colour4.White, transform_duration, transform_easing);

            if (!disabled) return;
            foreach(var dropdown in flowContainer)
            {
                dropdown.Visible.Value = false;
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            return current.Disabled || base.OnMouseDown(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            return current.Disabled || base.OnClick(e);
        }

        [Cached]
        private class TagDropdown : Container, IHasCurrentValue<Tag>
        {
            private readonly TagCategory category;
            private readonly IReadOnlyList<Tag> tags;

            public readonly Tag AffectedTags;

            private readonly BindableWithCurrent<Tag> current = new BindableWithCurrent<Tag>();
            private FillFlowContainer<TagContainer> fillFlow;
            private Container fillFlowParent;
            private Container headerContainer;
            private SpriteText countText;
            private Container countContainer;

            [Resolved]
            private TagSelectionContainer tagSelection { get; set; }

            public Bindable<Tag> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            public Bindable<bool> Visible { get; } = new Bindable<bool>();

            public float ButtonWidth => Math.Max(fillFlow.Children.Max(t => t.LayoutWidth), headerContainer.Width);

            public TagDropdown(TagCategory category, IEnumerable<Tag> tags)
            {
                this.category = category;
                this.tags = tags.ToList();

                foreach (var tag in this.tags)
                    AffectedTags |= tag;
            }


            [BackgroundDependencyLoader]
            private void load()
            {
                current.Current = new Bindable<Tag>();
                AutoSizeAxes = Axes.Both;

                Children = new Drawable[]
                {
                    new Container
                    {
                        Masking = true,
                        CornerRadius = 4,
                        RelativeSizeAxes = Axes.X,
                        Height = tagSelection.TagSize,
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.DarkRed,
                        },
                    },
                    fillFlowParent = new Container
                    {
                        Masking = true,
                        CornerRadius = 4,
                        Position = new Vector2(0, tagSelection.TagSize),
                        AutoSizeAxes = Axes.X,
                        Children = new Drawable[]
                        {
                            fillFlow = new FillFlowContainer<TagContainer>
                            {
                                AutoSizeAxes = Axes.Both,
                                Direction = FillDirection.Vertical,
                            },
                        },
                    },
                    headerContainer = new Container
                    {
                        Height = tagSelection.TagSize,
                        AutoSizeAxes = Axes.X,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Children = new Drawable[]
                        {
                            new FillFlowContainer
                            {
                                Direction = FillDirection.Horizontal,
                                RelativeSizeAxes = Axes.Y,
                                AutoSizeAxes = Axes.X,
                                Padding = new MarginPadding { Vertical = 2, Horizontal = 6 },
                                Spacing = new Vector2(4),
                                Children = new Drawable[]
                                {
                                    new SpriteIcon
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Size = new Vector2(0.85f),
                                        RelativeSizeAxes = Axes.Both,
                                        FillMode = FillMode.Fit,
                                        Icon = FontAwesome.Solid.Tags,
                                    },
                                    new SpriteText
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Text = category.GetDescription(),
                                        Font = new FontUsage(size: tagSelection.TagSize),
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        AutoSizeAxes = Axes.X,
                                        RelativeSizeAxes = Axes.Y,
                                        AutoSizeDuration = (float)transform_duration,
                                        AutoSizeEasing = transform_easing,
                                        Children = new Drawable[]
                                        {
                                            new CircularContainer
                                            {
                                                Masking = true,
                                                RelativeSizeAxes = Axes.Both,
                                                Child = new Box
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Colour = new Colour4(50, 50, 50, 255),
                                                },
                                            },
                                            countContainer = new Container
                                            {
                                                Alpha = 0,
                                                AutoSizeAxes = Axes.X,
                                                RelativeSizeAxes = Axes.Y,
                                                Anchor = Anchor.Centre,
                                                Origin = Anchor.Centre,
                                                Padding = new MarginPadding { Horizontal = 6 },
                                                Child = countText = new SpriteText
                                                {
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    Text = string.Empty,
                                                    Font = new FontUsage(size: tagSelection.TagSize - 4),
                                                },
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                };

                var tagContainers = tags.Select(t => new TagContainer(t)).ToImmutableList();

                foreach (var tagContainer in tagContainers)
                {
                    tagContainer.Selected.BindValueChanged(selected =>
                    {
                        if (selected.NewValue)
                            current.Value |= tagContainer.Value;
                        else
                            current.Value &= ~tagContainer.Value;
                    });
                }

                fillFlow.AddRange(tagContainers);

                current.BindValueChanged(values =>
                {
                    Tag deselected = (values.OldValue ^ values.NewValue) & values.OldValue;
                    Tag newSelected = (values.OldValue ^ values.NewValue) & ~values.OldValue;

                    foreach (var tagContainer in fillFlow)
                    {
                        if ((tagContainer.Value & deselected) > 0)
                        {
                            tagContainer.Selected.Value = false;
                        }

                        if ((tagContainer.Value & newSelected) > 0)
                        {
                            tagContainer.Selected.Value = true;
                        }
                    }

                    var tagCount = values.NewValue.GetSetFlags().Length;
                    countText.Text = tagCount.ToString();
                    countContainer.FadeTo(tagCount > 0 ? 1 : 0);
                });

                Visible.BindValueChanged(_ =>
                {
                    fillFlowParent.FinishTransforms();
                    fillFlowParent.ResizeHeightTo(Visible.Value ? fillFlow.Height : 0, transform_duration, Easing.OutCubic);
                });
            }

            protected override bool OnClick(ClickEvent e)
            {
                Visible.Value = !Visible.Value;
                return true;
            }
        }

        private class TagContainer : Button
        {
            public readonly Tag Value;

            private static readonly Colour4 background_hovered = new Colour4(75, 75, 75, 255);
            private static readonly Colour4 background_no_hover = new Colour4(50, 50, 50, 255);

            private Box backgroundBox;
            private Container iconContainer;
            private SpriteIcon activeIcon;
            private FillFlowContainer content;
            private Container layoutContainer;

            [Resolved]
            private TagSelectionContainer tagSelection { get; set; }

            [Resolved]
            private TagDropdown dropdown { get; set; }

            public Bindable<bool> Selected { get; } = new Bindable<bool>();

            public float LayoutWidth => content.Width;

            public override bool IsPresent => true;

            public TagContainer(Tag value)
            {
                Value = value;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.X;
                Height = tagSelection.TagSize;

                Children = new Drawable[]
                {
                    backgroundBox = new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                        Colour = new Colour4(50, 50, 50, 255),
                        Alpha = 0.8f,
                    },
                    iconContainer = new Container
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.Both,
                        FillMode = FillMode.Fit,
                        Margin = new MarginPadding { Horizontal = 5 },
                        Alpha = 0f,
                        Children = new Drawable[]
                        {
                            new SpriteIcon
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                Size = new Vector2(0.85f),
                                Icon = FontAwesome.Regular.Square,
                            },
                            activeIcon = new SpriteIcon
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Both,
                                Size = new Vector2(0.6f),
                                Icon = FontAwesome.Solid.Check,
                                Alpha = 0,
                            },
                        },
                    },
                    content = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.X,
                        RelativeSizeAxes = Axes.Y,
                        Padding = new MarginPadding { Horizontal = 5 },
                        Children = new Drawable[]
                        {
                            layoutContainer = new Container
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.Y,
                                Height = 1,
                                Width = 0,
                            },
                            new SpriteText
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Text = Value.GetDescription(),
                                Font = new FontUsage(size: tagSelection.TagSize),
                            },
                        },
                    },
                };

                Action = () => Selected.Value = !Selected.Value;

                Invalidate();

                Selected.BindValueChanged(s => updateVisibility());
            }

            protected override void Update()
            {
                base.Update();

                backgroundBox.Width = dropdown.ButtonWidth;
            }

            private void updateVisibility()
            {
                switch (Selected.Value)
                {
                    case true:
                        activeIcon.FadeIn(transform_duration, transform_easing);
                        iconContainer.FadeIn(transform_duration, transform_easing);
                        layoutContainer.ResizeWidthTo(tagSelection.TagSize + 3, transform_duration, transform_easing);
                        break;
                    case false when IsHovered:
                        activeIcon.FadeOut(transform_duration, transform_easing);
                        iconContainer.FadeIn(transform_duration, transform_easing);
                        layoutContainer.ResizeWidthTo(tagSelection.TagSize + 3, transform_duration, transform_easing);
                        break;
                    case false when !IsHovered:
                        activeIcon.FadeOut(transform_duration, transform_easing);
                        iconContainer.FadeOut(transform_duration, transform_easing);
                        layoutContainer.ResizeWidthTo(0, transform_duration, transform_easing);
                        break;
                }
            }

            protected override bool OnHover(HoverEvent e)
            {
                base.OnHover(e);

                updateVisibility();

                backgroundBox.FadeColour(background_hovered, transform_duration, transform_easing)
                    .FadeIn(transform_duration, transform_easing);
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                base.OnHoverLost(e);

                updateVisibility();

                backgroundBox.FadeColour(background_no_hover, transform_duration, transform_easing)
                    .FadeTo(0.8f, transform_duration, transform_easing);
            }
        }
    }
}
