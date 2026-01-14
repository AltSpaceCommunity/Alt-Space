using Content.Server.RoundEnd;
using Content.Server.Voting.Managers;
using Content.Server.Chat.Systems;
using Content.Shared.CCVar;
using Content.Shared.Voting;
using Content.Shared.Chat;
using Content.Shared.GameTicking;
using Content.Shared.Administration;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;
using Robust.Shared.Localization;
using Robust.Shared.Console;
using Robust.Shared.Player;
using Robust.Server.Player;
using System.Collections.Generic;
using Content.Server.Voting;

namespace Content.Server.Administration.Systems
{
    public sealed class AutoRoundEndSystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _timing = default!;
        [Dependency] private readonly RoundEndSystem _roundEnd = default!;
        [Dependency] private readonly IVoteManager _voteManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;
        [Dependency] private readonly IConfigurationManager _cfg = default!;
        [Dependency] private readonly ChatSystem _chat = default!;
        [Dependency] private readonly SharedAudioSystem _audio = default!;

        private TimeSpan _nextVoteTime;
        private TimeSpan _hardEndTime;
        private TimeSpan? _delayedRestartTime;
        private bool _isInitialized;

        private const string AnnouncementSender = "???";
        private static readonly Color StartVoteColor = Color.Red;
        private static readonly Color SuccessVoteColor = Color.Green;
        private static readonly Color HardEndColor = Color.FromHex("#9500ff");

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);

            _cfg.OnValueChanged(CCVars.AutoVoteInterval, _ => ResetTimers());
            _cfg.OnValueChanged(CCVars.AutoHardEndThreshold, _ => ResetTimers());

            ResetTimers();
        }

        private void OnRoundRestart(RoundRestartCleanupEvent ev)
        {
            ResetTimers();
            _delayedRestartTime = null;

            var sound = new SoundPathSpecifier("/Audio/_Crescent/bois.ogg");
            var audioParams = new AudioParams().WithVolume(-3f);

            _audio.PlayGlobal(sound, Filter.Empty().AddAllPlayers(_playerManager), true, audioParams);
        }

        private void OnAutoVoteFinished(IVoteHandle vote, VoteFinishedEventArgs ev)
        {
            vote.OnFinished -= OnAutoVoteFinished;

            if (ev.Winner is string result && result == "Да")
            {
                PrepareFinalCountdown();
            }
        }

        private void ResetTimers()
        {
            var curTime = _timing.CurTime;

            var voteInterval = TimeSpan.FromHours(_cfg.GetCVar(CCVars.AutoVoteInterval));
            var hardEndThreshold = TimeSpan.FromHours(_cfg.GetCVar(CCVars.AutoHardEndThreshold));

            _nextVoteTime = curTime + voteInterval;
            _hardEndTime = curTime + hardEndThreshold;
            _isInitialized = true;
        }

        public override void Update(float frameTime)
        {
            if (!_isInitialized)
                return;

            var curTime = _timing.CurTime;

            if (_delayedRestartTime != null && curTime >= _delayedRestartTime)
            {
                _delayedRestartTime = null;
                _roundEnd.EndRound(TimeSpan.FromMinutes(2));
            }

            if (curTime >= _nextVoteTime)
            {
                TriggerAutoVote();
                var interval = TimeSpan.FromHours(_cfg.GetCVar(CCVars.AutoVoteInterval));
                _nextVoteTime = curTime + interval;
            }

            if (curTime >= _hardEndTime)
            {
                ForceRoundEnd();
                _hardEndTime = curTime + TimeSpan.FromMinutes(30);
            }
        }

        public void TriggerAutoVote()
        {
            if (_playerManager.PlayerCount == 0)
                return;

            var options = new VoteOptions
            {
                Title = "Прошло 3 часа, завершение смены?",
                Options =
                {
                    ("Да", "Да"),
                    ("Нет", "Нет")
                },
                Duration = TimeSpan.FromSeconds(60)
            };

            _voteManager.CreateVote(options);

            foreach (var vote in _voteManager.ActiveVotes)
            {
                if (vote.Title == options.Title)
                {
                    vote.OnFinished -= OnAutoVoteFinished;
                    vote.OnFinished += OnAutoVoteFinished;
                    break;
                }
            }

            var message = "Внимание! Вам стоит сделать верный выбор.";
            _chat.DispatchGlobalAnnouncement(
                message,
                sender: AnnouncementSender,
                colorOverride: StartVoteColor,
                announcementSound: null);
        }

        private void PrepareFinalCountdown()
        {
            _delayedRestartTime = _timing.CurTime + TimeSpan.FromMinutes(20);

            var sound = new SoundPathSpecifier("/Audio/_Crescent/Ambience/Servers/buzz2.ogg");
            _chat.DispatchGlobalAnnouncement(
                "Внимание! Зафиксирована временная аномалия в зоне Тайпана, коллапс неизбежен. Оставшееся время: 20 минут.",
                sender: AnnouncementSender,
                colorOverride: SuccessVoteColor,
                announcementSound: sound
            );
        }

        private void ForceRoundEnd()
        {
            _roundEnd.RequestRoundEnd(null, false);

            var sound = new SoundPathSpecifier("/Audio/_Crescent/Ambience/Servers/buzz3.ogg");
            _chat.DispatchGlobalAnnouncement(
                "Внимание! Зафиксирована временная аномалия в зоне Тайпана, коллапс неизбежен. Оставшееся время: 20 минут.",
                sender: AnnouncementSender,
                colorOverride: HardEndColor,
                announcementSound: sound
            );
        }
    }

    [AdminCommand(AdminFlags.Admin)]
    public sealed class VoteAutoEvacCommand : IConsoleCommand
    {
        public string Command => "voteautoevac";
        public string Description => "Запустить автоматическое голосование за завершение смены.";
        public string Help => "voteautoevac";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var system = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<AutoRoundEndSystem>();
            system.TriggerAutoVote();
            shell.WriteLine("Голосование запущено.");
        }
    }
}