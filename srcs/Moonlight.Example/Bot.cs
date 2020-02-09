using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moonlight.Clients;
using Moonlight.Game.Battle;
using Moonlight.Game.Entities;
using Moonlight.Game.Extensions;

namespace Moonlight.Example
{
    public class Bot
    {
        private readonly Client _client;

        private const int Radius = 10;
        public bool IsRunning { get; private set; }

        private Task _runningTask;

        public Bot(Client client)
        {
            _client = client;
        }

        public void Start(IEnumerable<Skill> skills)
        {
            if (IsRunning)
            {
                return;
            }
            
            IsRunning = true;
            _runningTask = Task.Run(async () =>
            {
                Character character = _client.Character;
                Skill basicAttack = character.Skills.FirstOrDefault();

                while (IsRunning)
                {
                    IEnumerable<Monster> allPii;
                    Skill skill;
                    
                    do
                    {
                        Monster pod;
                        do
                        {
                            pod = character.GetClosestMonsterInRadius(Constants.SoftPiiPodVnum, Radius);
                            if (pod == null)
                            {
                                await Task.Delay(100);
                            }
                        } 
                        while (pod == null);

                        await character.Attack(basicAttack, pod);
                        
                        allPii = character.GetClosestMonstersInRadius(Constants.SoftPiiVnum, Radius);
                        skill = skills.FirstOrDefault(x => !x.IsOnCooldown);

                        await Task.Delay(100);
                    } 
                    while (allPii.Count() < 10 || skill == null);

                    await character.Attack(skill, allPii.First());
                }
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}