using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EzMvvm;
using EzMvvm.Command;
using Moonlight.Clients;
using Moonlight.Core.Enums;
using Moonlight.Game.Battle;

namespace Moonlight.Example.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public Client Client { get; }
        public Bot Bot { get; }

        public ObservableCollection<Skill> Skills { get; }
        public ObservableCollection<Skill> UsedSkills { get; }

        public ICommand AddSkillCommand { get; }
        public ICommand RemoveSkillCommand { get; }
        public ICommand RefreshSkillsCommand { get; }
        public ICommand StartStopCommand { get; }

        private Skill _selectedSkill;
        public Skill SelectedSkill
        {
            get => _selectedSkill;
            set => SetProperty(ref _selectedSkill, value);
        }

        private Skill _selectedUsedSkill;
        public Skill SelectedUsedSkill
        {
            get => _selectedUsedSkill;
            set => SetProperty(ref _selectedUsedSkill, value);
        }

        public MainViewModel(MoonlightAPI moonlight)
        {
            Client = moonlight.CreateLocalClient();
            Bot = new Bot(Client);
            
            Skills = new ObservableCollection<Skill>();
            UsedSkills = new ObservableCollection<Skill>();
            
            AddSkillCommand = new RelayCommand(OnAddSkillCommand);
            RemoveSkillCommand = new RelayCommand(OnRemoveSkillCommand);
            RefreshSkillsCommand = new RelayCommand(OnRefreshSkillsCommand);
            StartStopCommand = new RelayCommand(OnStartStopCommand);
        }

        private void OnStartStopCommand()
        {
            if (Bot.IsRunning)
            {
                Bot.Stop();
            }
            else
            {
                Bot.Start(new List<Skill>(UsedSkills));
            }
        }
        
        private void OnRefreshSkillsCommand()
        {
            Skills.Clear();
            UsedSkills.Clear();

            foreach (Skill skill in Client.Character.Skills.Where(x => x.HitType == HitType.ENEMIES_IN_ZONE || x.HitType == HitType.SPECIAL_AREA))
            {
                Skills.Add(skill);
            }
        }
        
        private void OnAddSkillCommand()
        {
            if (SelectedSkill == null)
            {
                return;
            }
            
            UsedSkills.Add(SelectedSkill);
            Skills.Remove(SelectedSkill);
        }

        private void OnRemoveSkillCommand()
        {
            if (SelectedUsedSkill == null)
            {
                return;
            }
            
            Skills.Add(SelectedUsedSkill);
            UsedSkills.Remove(SelectedUsedSkill);
        }
    }
}