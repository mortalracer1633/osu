﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Screens.Play.HUD;
using osu.Game.Screens.Select;
using osu.Game.Tests.Beatmaps;
using osuTK;

namespace osu.Game.Tests.Visual.SongSelect
{
    [TestFixture]
    [System.ComponentModel.Description("PlaySongSelect leaderboard/details area")]
    public class TestSceneBeatmapDetailArea : OsuTestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(BeatmapDetails) };

        private ModDisplay modDisplay;

        [BackgroundDependencyLoader]
        private void load(OsuGameBase game, RulesetStore rulesets)
        {
            BeatmapDetailArea detailsArea;
            Add(detailsArea = new BeatmapDetailArea
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(550f, 450f),
            });

            Add(modDisplay = new ModDisplay
            {
                Anchor = Anchor.TopRight,
                Origin = Anchor.TopRight,
                AutoSizeAxes = Axes.Both,
                Position = new Vector2(0, 25),
            });

            modDisplay.Current.BindTo(SelectedMods);

            AddStep("all metrics", () => detailsArea.Beatmap = new TestWorkingBeatmap(new Beatmap
            {
                BeatmapInfo =
                {
                    BeatmapSet = new BeatmapSetInfo
                    {
                        Metrics = new BeatmapSetMetrics { Ratings = Enumerable.Range(0, 11).ToArray() }
                    },
                    Version = "All Metrics",
                    Metadata = new BeatmapMetadata
                    {
                        Source = "osu!lazer",
                        Tags = "this beatmap has all the metrics",
                    },
                    BaseDifficulty = new BeatmapDifficulty
                    {
                        CircleSize = 7,
                        DrainRate = 1,
                        OverallDifficulty = 5.7f,
                        ApproachRate = 3.5f,
                    },
                    StarDifficulty = 5.3f,
                    Metrics = new BeatmapMetrics
                    {
                        Fails = Enumerable.Range(1, 100).Select(i => i % 12 - 6).ToArray(),
                        Retries = Enumerable.Range(-2, 100).Select(i => i % 12 - 6).ToArray(),
                    },
                }
            }));

            AddStep("all except source", () => detailsArea.Beatmap = new TestWorkingBeatmap(new Beatmap
            {
                BeatmapInfo =
                {
                    BeatmapSet = new BeatmapSetInfo
                    {
                        Metrics = new BeatmapSetMetrics { Ratings = Enumerable.Range(0, 11).ToArray() }
                    },
                    Version = "All Metrics",
                    Metadata = new BeatmapMetadata
                    {
                        Tags = "this beatmap has all the metrics",
                    },
                    BaseDifficulty = new BeatmapDifficulty
                    {
                        CircleSize = 7,
                        DrainRate = 1,
                        OverallDifficulty = 5.7f,
                        ApproachRate = 3.5f,
                    },
                    StarDifficulty = 5.3f,
                    Metrics = new BeatmapMetrics
                    {
                        Fails = Enumerable.Range(1, 100).Select(i => i % 12 - 6).ToArray(),
                        Retries = Enumerable.Range(-2, 100).Select(i => i % 12 - 6).ToArray(),
                    },
                }
            }));

            AddStep("ratings", () => detailsArea.Beatmap = new TestWorkingBeatmap(new Beatmap
            {
                BeatmapInfo =
                {
                    BeatmapSet = new BeatmapSetInfo
                    {
                        Metrics = new BeatmapSetMetrics { Ratings = Enumerable.Range(0, 11).ToArray() }
                    },
                    Version = "Only Ratings",
                    Metadata = new BeatmapMetadata
                    {
                        Source = "osu!lazer",
                        Tags = "this beatmap has ratings metrics but not retries or fails",
                    },
                    BaseDifficulty = new BeatmapDifficulty
                    {
                        CircleSize = 6,
                        DrainRate = 9,
                        OverallDifficulty = 6,
                        ApproachRate = 6,
                    },
                    StarDifficulty = 4.8f
                }
            }));

            AddStep("fails+retries", () => detailsArea.Beatmap = new TestWorkingBeatmap(new Beatmap
            {
                BeatmapInfo =
                {
                    Version = "Only Retries and Fails",
                    Metadata = new BeatmapMetadata
                    {
                        Source = "osu!lazer",
                        Tags = "this beatmap has retries and fails but no ratings",
                    },
                    BaseDifficulty = new BeatmapDifficulty
                    {
                        CircleSize = 3.7f,
                        DrainRate = 6,
                        OverallDifficulty = 6,
                        ApproachRate = 7,
                    },
                    StarDifficulty = 2.91f,
                    Metrics = new BeatmapMetrics
                    {
                        Fails = Enumerable.Range(1, 100).Select(i => i % 12 - 6).ToArray(),
                        Retries = Enumerable.Range(-2, 100).Select(i => i % 12 - 6).ToArray(),
                    },
                }
            }));

            AddStep("null metrics", () => detailsArea.Beatmap = new TestWorkingBeatmap(new Beatmap
            {
                BeatmapInfo =
                {
                    Version = "No Metrics",
                    Metadata = new BeatmapMetadata
                    {
                        Source = "osu!lazer",
                        Tags = "this beatmap has no metrics",
                    },
                    BaseDifficulty = new BeatmapDifficulty
                    {
                        CircleSize = 5,
                        DrainRate = 5,
                        OverallDifficulty = 5.5f,
                        ApproachRate = 6.5f,
                    },
                    StarDifficulty = 1.97f,
                }
            }));

            AddStep("null beatmap", () => detailsArea.Beatmap = null);

            Ruleset ruleset = rulesets.AvailableRulesets.First().CreateInstance();

            AddStep("with EZ mod", () =>
            {
                detailsArea.Beatmap = new TestWorkingBeatmap(new Beatmap
                {
                    BeatmapInfo =
                    {
                        Version = "Has Easy Mod",
                        Metadata = new BeatmapMetadata
                        {
                            Source = "osu!lazer",
                            Tags = "this beatmap has the easy mod enabled",
                        },
                        BaseDifficulty = new BeatmapDifficulty
                        {
                            CircleSize = 3,
                            DrainRate = 3,
                            OverallDifficulty = 3,
                            ApproachRate = 3,
                        },
                        StarDifficulty = 1f,
                    }
                });

                SelectedMods.Value = new[] { ruleset.GetAllMods().First(m => m is ModEasy) };
            });

            AddStep("with HR mod", () =>
            {
                detailsArea.Beatmap = new TestWorkingBeatmap(new Beatmap
                {
                    BeatmapInfo =
                    {
                        Version = "Has Hard Rock Mod",
                        Metadata = new BeatmapMetadata
                        {
                            Source = "osu!lazer",
                            Tags = "this beatmap has the hard rock mod enabled",
                        },
                        BaseDifficulty = new BeatmapDifficulty
                        {
                            CircleSize = 3,
                            DrainRate = 3,
                            OverallDifficulty = 3,
                            ApproachRate = 3,
                        },
                        StarDifficulty = 1f,
                    }
                });

                SelectedMods.Value = new[] { ruleset.GetAllMods().First(m => m is ModHardRock) };
            });
        }
    }
}
