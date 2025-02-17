using System;
using System.Collections.Generic;
using System.Windows;
using Serilog;
using PaperInsight.UI;

namespace PaperInsight.Manager
{
    public class FlowManager
    {

        private static readonly Dictionary<Rounds , (string pdf, string content, string task)> _roundsChai = new()
        {
            { Rounds.Trial, (@"\Resources\Ku_2023.pdf", @"\Resources\Ku_2023.txt", "Trial round: Please familiarize yourself with the capabilities of the writing assistant and try out to write a summary covering the motivation, research question, method, results and contribution of the paper using the writing assistant. As this is the trail round, the summary will not be evaluated. Submit the summary when you are done.") },
            { Rounds.Round1, (@"\Resources\Nwagu_2023.pdf", @"\Resources\Nwagu_2023.txt", "Task: Please explore, understand and write a summary of the paper using the writing assistant. The summary should cover the motivation, research question, method, results and contribution of the paper. Submit the summary when you are done.") },
            { Rounds.Round2, (@"\Resources\Nwagu_2023.pdf", @"\Resources\Nwagu_2023.txt", "Task: Please explore, understand and write a summary of the paper using the writing assistant. The summary should cover the motivation, research question, method, results and contribution of the paper. Submit the summary when you are done.") },
        };

        private static readonly Dictionary<Rounds, (string pdf, string content, string task)> _roundsLotus = new()
        {
            { Rounds.Trial, (@"\Resources\Ku_2023.pdf", @"\Resources\Ku_2023.txt", "Trial round: Please familiarize yourself with the capabilities of the writing assistant and try out to write a summary covering the motivation, research question, method, results and contribution of the paper using the writing assistant. As this is the trail round, the summary will not be evaluated. Submit the summary when you are done.") },
            { Rounds.Round1, (@"\Resources\Chinareva_2020.pdf", @"\Resources\Chinareva_2020.txt", "Task: Please explore, understand and write a summary of the paper using the writing assistant. The summary should cover the motivation, research question, method, results and contribution of the paper. Submit the summary when you are done.") },
            { Rounds.Round2, (@"\Resources\Chinareva_2020.pdf", @"\Resources\Chinareva_2020.txt", "Task: Please explore, understand and write a summary of the paper using the writing assistant. The summary should cover the motivation, research question, method, results and contribution of the paper. Submit the summary when you are done.") },
        };

        private readonly State[] _gamePlan =
        {
            State.StartUp,
            State.Calibration,
            State.Introduction,
            State.IntroductionAssistant,

            State.TrialRound,
            State.PreQuestionaire,

            State.GetReadySummary,
            State.SummaryOne,
            State.Questionaire,

            //State.SummaryTwo,
            //State.QuestionaireTwo,

            State.PostQuestionaire,
            State.FinalPage,

        };

        //TODO: Input text as params
        //Main Paper: Chai Paper
        //private readonly Dictionary<State, Func<Window>> StateWindowDict = new()
        //{
        //    { State.StartUp, () => new StartUp() },
        //    { State.Introduction, () => new Introduction() },
        //    { State.IntroductionAssistant, () => new IntroductionAssistant()},
        //    { State.Calibration, () => new Calibration() },
        //    { State.PreQuestionaire, () => new PreQuestionaire() },
        //    { State.GetReadyLearning, () => new GetReady() },
        //    { State.TrialRound, () => new Editor(_roundsChai[Rounds.Trial])},
        //    { State.GetReadySummary, () => new GetReady() },
        //    { State.SummaryOne, () => new Editor(_roundsChai[Rounds.Round1]) },
        //    { State.SummaryTwo, () => new Editor(_roundsChai[Rounds.Round2]) },
        //    { State.Questionaire, () => new Questionaire("Chai") },
        //    { State.QuestionaireTwo, () => new Questionaire("Chai") },
        //    { State.PostQuestionaire, () => new PostQuestionaire() },
        //    { State.FinalPage, () => new Final() }
        //};

        //Main Paper: Lotus Paper
        private readonly Dictionary<State, Func<Window>> StateWindowDict = new()
        {
            { State.StartUp, () => new StartUp() },
            { State.Introduction, () => new Introduction() },
            { State.IntroductionAssistant, () => new IntroductionAssistant()},
            { State.Calibration, () => new Calibration() },
            { State.PreQuestionaire, () => new PreQuestionaire() },
            { State.GetReadyLearning, () => new GetReady() },
            { State.TrialRound, () => new Editor(_roundsLotus[Rounds.Trial])},
            { State.GetReadySummary, () => new GetReady() },
            { State.SummaryOne, () => new Editor(_roundsLotus[Rounds.Round1]) },
            { State.SummaryTwo, () => new Editor(_roundsLotus[Rounds.Round2]) },
            { State.Questionaire, () => new Questionaire("Lotus") },
            { State.QuestionaireTwo, () => new Questionaire("Lotus") },
            { State.PostQuestionaire, () => new PostQuestionaire() },
            { State.FinalPage, () => new Final() }
        };

        private int _currentStateIndex;

        public void Start()
        {
            _currentStateIndex = 0;
            ChangeState(_gamePlan[_currentStateIndex]);
        }

        private void ChangeState(State newState)
        {
            Log.Information($"Change state to: {newState}");
            switch (newState)
            {
                case State.Introduction:
                    ((App)Application.Current).StartLogging();
                    break;
                case State.FinalPage:
                    ((App)Application.Current).StopLogging();
                    break;
            }
            if (!(StateWindowDict.TryGetValue(newState, out Func<Window>? constructor) && constructor is not null)) throw new ArgumentOutOfRangeException($"Argument out of range {newState}");
            var window = constructor();
            window.Show();
        }

        public void Return()
        {
            _currentStateIndex++;
            if (_currentStateIndex >= _gamePlan.Length)
            {
                Application.Current.Shutdown();
                return;
            }
            ChangeState(_gamePlan[_currentStateIndex]);
        }
        //TODO: Return with values;
    }

    public enum State
    {
        StartUp,
        Calibration,
        Introduction,
        IntroductionAssistant,
        PreQuestionaire,
        GetReadyLearning,
        TrialRound,
        GetReadySummary,
        SummaryOne,
        SummaryTwo,
        Questionaire,
        QuestionaireTwo,
        PostQuestionaire,
        FinalPage,
    }

    public enum Rounds
    {
        Trial,
        Round1,
        Round2,
    }
}
